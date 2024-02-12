import { Map, Placemark, YMaps } from "@pbe/react-yandex-maps";
import {
  FC,
  useState,
  useEffect,
  useCallback,
  SyntheticEvent,
  ChangeEvent,
  useMemo,
} from "react";
import { Form, Button, FloatingLabel, Row, Col } from "react-bootstrap";
import { useParams } from "react-router";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";
import { useFormAndValidation } from "../../hooks/use-form-and-validation";
import {
  fetchSpot,
  fetchUpdateSpot,
  fetchUploadSpotPhoto,
} from "../../services/reducers/thunks/spot";
import { URL_API } from "../../services/utils/constant";
import styles from "./spot-edit.module.css";
import { deletePhoto } from "../../services/utils/api";

const SpotEdit: FC = () => {
  let { id } = useParams();
  const dispatch = useAppDispatch();
  const { loading, error, spot } = useAppSelector((state) => state.spot);
  const activities = useAppSelector((state) => state.activities.items);
  const { values, setValues, isChange, setIsChange, handleChange } =
    useFormAndValidation();
  const [isNewRecord, setIsNewRecord] = useState(true);

  useEffect(() => {
    setIsNewRecord(id === "new" ? true : false);
  }, [id]);

  useEffect(() => {
    setValues({
      id: spot ? spot.id : "",
      name: spot ? spot.name : "",
      activityId: spot ? spot.activityId : "",
      note: spot ? spot.note : "",
      latitude: spot ? spot.latitude.toString() : "",
      longitude: spot ? spot.longitude.toString() : "",
      isActive: (spot ? spot.isActive : false).toString(),
    });
  }, [spot, isNewRecord, setValues]);

  useEffect(() => {
    if ((!loading && !error && !spot && id) || (spot?.id !== id && id)) {
      if (!isNewRecord) {
        dispatch(fetchSpot(id));
      }
    }
  }, [id, loading, spot, error, dispatch, isNewRecord]);

  const handleChangeCheckbox = (e: ChangeEvent<any>) => {
    const { name, checked } = e.target;
    setValues({ ...values, [name]: checked.toString() });
    setIsChange(true);
  };

  const handleOnSubmit = useCallback(
    async (e: SyntheticEvent) => {
      e.preventDefault();
      dispatch(
        fetchUpdateSpot({
          id: values.id!,
          name: values.name!,
          placeId: values.placeId,
          activityId: values.activityId!,
          note: values.note!,
          isActive: values.isActive === "true" ? true : false,
          latitude: Number(values.latitude),
          longitude: Number(values.longitude),
        })
      );
      setIsChange(false);
    },
    [dispatch, values, setIsChange]
  );

  const [coordinates, setCoordinates] = useState<Array<number>>([]);
  const mapData = useMemo(() => {
    return {
      center: [spot?.latitude ?? 0, spot?.longitude ?? 0],
      zoom: 7,
    };
  }, [spot]);
  const onClickMap = useCallback(
    (e: any) => {
      console.log(e.get("coords"));
      setCoordinates(e.get("coords"));
      setIsChange(true);
    },
    [setCoordinates, setIsChange]
  );
  useEffect(() => {
    setValues((prevState) => ({
      ...prevState,
      latitude: coordinates[0]?.toString(),
      longitude: coordinates[1]?.toString(),
    }));
  }, [coordinates, setValues]);
  const handleFileChange = useCallback(
    (e: ChangeEvent<HTMLInputElement>) => {
      if (e.target.files) {
        dispatch(fetchUploadSpotPhoto(e.target.files[0], id ?? ""));
      }
    },
    [dispatch, id]
  );

  const handlePhotoDelete = useCallback(
    async (e: any) => {
      e.preventDefault();
      await deletePhoto(e.target.getAttribute("data-id"))
        .then((data) => {
          id && dispatch(fetchSpot(id));
        })
        .catch((ex) => {
          console.error(ex);
        });
    },
    [dispatch, id]
  );

  return (
    <div>
      <Row>
        <Col md="6">
          <h2>Редактирование спота</h2>
          <Form className={styles.form}>
            <input type="hidden" id="id" value={spot?.id} />
            <input type="hidden" id="placeId" value={spot?.placeId} />
            <Row>
              <Col>
                <FloatingLabel
                  controlId="floatingInput"
                  label="Имя"
                  className="mb-3"
                >
                  <Form.Control
                    type="text"
                    value={values.name || ""}
                    placeholder="Название спота"
                    name="name"
                    onChange={handleChange}
                  />
                </FloatingLabel>

                <Row>
                  <Col md="6">
                    <FloatingLabel
                      className="mb-3"
                      controlId="latitude"
                      label="Широта"
                    >
                      <Form.Control
                        type="text"
                        value={values.latitude || ""}
                        placeholder="Широта"
                        name="latitude"
                        onChange={handleChange}
                      />
                    </FloatingLabel>
                  </Col>
                  <Col md="6">
                    <FloatingLabel
                      className="mb-3"
                      controlId="longitude"
                      label="Долгота"
                    >
                      <Form.Control
                        type="text"
                        value={values.longitude || ""}
                        placeholder="Долгота"
                        name="longitude"
                        onChange={handleChange}
                      />
                    </FloatingLabel>
                  </Col>
                </Row>
                <div className={styles.map}>
                  <YMaps>
                    <Map
                      width={"100%"}
                      height={"100%"}
                      defaultState={mapData}
                      onClick={onClickMap}
                    >
                      {coordinates.length > 0 ? (
                        <Placemark geometry={coordinates} />
                      ) : (
                        <Placemark
                          geometry={[spot?.latitude, spot?.longitude]}
                        />
                      )}
                    </Map>
                  </YMaps>
                </div>
              </Col>
              <Col>
                <FloatingLabel
                  className="mb-3"
                  controlId="activityId"
                  label="Активность"
                >
                  <Form.Select name="activityId" onChange={handleChange}>
                    {activities &&
                      activities.map((item) => (
                        <option key={item.id} value="item.id">
                          {item.name}
                        </option>
                      ))}
                  </Form.Select>
                </FloatingLabel>
                <FloatingLabel
                  className="mb-3"
                  controlId="note"
                  label="Описание"
                >
                  <Form.Control
                    type="text"
                    as="textarea"
                    rows={3}
                    placeholder="Введите описание"
                    value={values.note || ""}
                    name="note"
                    onChange={handleChange}
                    style={{ height: "475px" }}
                  />
                </FloatingLabel>
              </Col>
            </Row>

            <Form.Check
              type="checkbox"
              id="isActive"
              name="isActive"
              label="Активен"
              checked={values.isActive === "true" ? true : false}
              onChange={handleChangeCheckbox}
            />

            <Button
              className="btn-warning"
              variant="primary"
              type="submit"
              onClick={handleOnSubmit}
              disabled={!isChange ? true : false}
            >
              {loading ? "Сохранение..." : "Сохранить"}
            </Button>
          </Form>
        </Col>
        <Col md="6" className="ps-5">
          <h2>Галерея</h2>
          <div className={styles.div_photos}>
            {spot?.photos?.map((item) => (
              <div key={item.id}>
                <img src={`${URL_API}/SpotPhoto/photo?id=${item.id}`} alt="" />
                <div
                  className={styles.delete_button}
                  role="button"
                  data-id={item.id}
                  onClick={handlePhotoDelete}
                >
                  удалить
                </div>
              </div>
            ))}
          </div>
          <div className={styles.upload}>
            <input type="file" id="fileloader" onChange={handleFileChange} />
            <label htmlFor="fileloader" className="btn btn-secondary">
              Загрузить новое фото
            </label>
          </div>
        </Col>
      </Row>

      {error && <p>Ошибка при загрузке спота: {error}</p>}
    </div>
  );
};

export default SpotEdit;
