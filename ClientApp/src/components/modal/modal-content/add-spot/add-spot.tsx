import React, {
  FC,
  memo,
  useEffect,
  useState,
  useCallback,
  useMemo,
  SyntheticEvent,
} from "react";
import { Map, Placemark, YMaps } from "@pbe/react-yandex-maps";
import { Button, Col, Form, Row } from "react-bootstrap";
import {
  useAppSelector,
  useAppDispatch,
} from "../../../../hooks/use-app-dispatch";
import { useFormAndValidation } from "../../../../hooks/use-form-and-validation";

import styles from "./add-spot.module.css";
import { closeModal } from "../../../../services/reducers/slices";
import { fetchAddSpot } from "../../../../services/reducers/thunks/spot";

const AddSpotContentModal: FC = () => {
  const dispatch = useAppDispatch();
  const { values, handleChange, setValues } = useFormAndValidation();
  const { error, loading } = useAppSelector((state) => state.spotAdd);
  const activities = useAppSelector((state) => state.activities.items);
  const [coordinates, setCoordinates] = useState<Array<number>>([]);
  const mapData = useMemo(() => {
    return {
      center: [44.67097258486721, 37.67861302627597],
      zoom: 7,
    };
  }, []);
  const onClickMap = useCallback(
    (e: any) => {
      console.log(e.get("coords"));
      setCoordinates(e.get("coords"));
    },
    [setCoordinates]
  );

  useEffect(() => {
    setValues({
      ...values,
      latitude: coordinates[0]?.toString(),
      longitude: coordinates[1]?.toString(),
    });
  }, [coordinates, setValues]);

  useEffect(() => {
    setValues({
      ...values,
      activityId: activities[0].id,
    });
  }, [activities, setValues]);

  const handlOnsubmin = useCallback(
    async (e: SyntheticEvent) => {
      e.preventDefault();
      dispatch(
        fetchAddSpot({
          name: values.name,
          activityId: values.activityId,
          note: values.note,
          latitude: coordinates[0],
          longitude: coordinates[1],
        })
      );
    },
    [dispatch, values, coordinates]
  );

  const handlCansel = useCallback(() => dispatch(closeModal()), [dispatch]);

  return (
    <form className="ms-3 me-3 mt-5" onSubmit={handlOnsubmin}>
      <Row className={styles.main}>
        <Col md="7">
          <Form.Group className="mb-3">
            <Form.Label>Выберите место на карте</Form.Label>
            <div className={styles.div_coordinates}>
              <input
                name="latitude"
                type="text"
                placeholder="Широта"
                value={values.latitude}
                onChange={handleChange}
                className="form-control"
              />
              <input
                name="longitude"
                type="text"
                placeholder="Долгота"
                value={values.longitude}
                onChange={handleChange}
                className="form-control"
              />
            </div>
          </Form.Group>
          <div className={styles.map}>
            <YMaps>
              <Map
                width={"100%"}
                height={"100%"}
                defaultState={mapData}
                onClick={onClickMap}
              >
                {coordinates && <Placemark geometry={coordinates} />}
              </Map>
            </YMaps>
          </div>
        </Col>
        <Col md="5">
          <Form.Group className="mb-3">
            <Form.Label>Название</Form.Label>
            <input
              name="name"
              type="text"
              placeholder="Название"
              value={values.name}
              onChange={handleChange}
              className="form-control"
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Активность</Form.Label>
            <Form.Select
              name="activityId"
              value={values.activityId}
              onChange={handleChange}
            >
              {activities &&
                activities.map((item) => (
                  <option key={item.id} value={item.id}>
                    {item.name}
                  </option>
                ))}
            </Form.Select>
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Описание</Form.Label>
            <textarea
              name="note"
              rows={5}
              placeholder="Описание"
              value={values.note}
              onChange={handleChange}
              className="form-control"
            />
          </Form.Group>
          <Form.Group>
            <Button
              variant="warning"
              type="submit"
              className="me-3"
              disabled={loading ? true : false}
            >
              {loading ? "Ожидание..." : "Сохранить"}
            </Button>
            <Button
              variant="secondary"
              type="button"
              disabled={loading ? true : false}
              onClick={handlCansel}
            >
              Отмена
            </Button>
          </Form.Group>
          {error && <div className={`${styles.error} red mt-2`}>{error}</div>}
        </Col>
      </Row>
    </form>
  );
};

export default memo(AddSpotContentModal);
