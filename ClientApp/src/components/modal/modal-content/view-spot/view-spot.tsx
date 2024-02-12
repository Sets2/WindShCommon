import React, {
  FC,
  memo,
  useEffect,
  useState,
  useCallback,
  SyntheticEvent,
  ChangeEvent,
} from "react";
import Moment from "moment";
import { Button, Col, Form, Row } from "react-bootstrap";
import {
  useAppSelector,
  useAppDispatch,
} from "../../../../hooks/use-app-dispatch";

import {
  fetchSpot,
  fetchSpotIamHere,
  fetchUploadSpotPhoto,
} from "../../../../services/reducers/thunks/spot";
import { v4 as uuidv4 } from "uuid";

import styles from "./view-spot.module.css";
import { useFormAndValidation } from "../../../../hooks/use-form-and-validation";
import { fetchWeather } from "../../../../services/reducers/thunks";
import { URL_API } from "../../../../services/utils/constant";

type TViewSpotContentModalProps = {
  spotId: string;
};

const ViewSpotContentModal: FC<TViewSpotContentModalProps> = ({ spotId }) => {
  const dispatch = useAppDispatch();

  const { values, handleChange, setValues } = useFormAndValidation();

  const { loggedIn, user } = useAppSelector((state) => state.user);
  const { spot, error, loading } = useAppSelector((state) => state.spot);
  const weatherLoading = useAppSelector((state) => state.weather.loading);
  const weatherData = useAppSelector((state) => state.weather.data);
  const weatherError = useAppSelector((state) => state.weather.error);
  const [currentImageId, setCurrentImageId] = useState("");

  useEffect(() => {
    dispatch(fetchSpot(spotId));
  }, [dispatch, spotId]);

  useEffect(() => {
    if (!loading && spot) {
      dispatch(fetchWeather(spot.latitude, spot.longitude));
      setCurrentImageId(spot.photos?.[0]?.id ?? "");
    }
  }, [dispatch, spot, loading]);

  const handleFileChange = useCallback(
    (e: ChangeEvent<HTMLInputElement>) => {
      if (e.target.files) {
        dispatch(fetchUploadSpotPhoto(e.target.files[0], spotId));
      }
    },
    [dispatch, spotId]
  );

  const handleButtonIamHere = useCallback(
    (e: SyntheticEvent) => {
      dispatch(
        fetchSpotIamHere({ userId: user!.id, spotId, comment: values.about })
      );
      setValues({ about: "" });
    },
    [dispatch, values, setValues, spotId, user]
  );

  const handleImageClick = useCallback(
    (e: any) => {
      setCurrentImageId(e.target.getAttribute("data-id"));
    },
    [setCurrentImageId]
  );

  if (error) {
    return (
      <div className={`${styles.main} red`}>Ошибка загрузки спота: {error}</div>
    );
  }
  return (
    <div className={styles.main}>
      {loading ? (
        <div>Загрузка...</div>
      ) : (
        <>
          <Row>
            <Col md="6" className="position-relative">
              <Form.Group className="mb-1">
                {weatherLoading ? (
                  <div>Загрузка...</div>
                ) : (
                  <div className={styles.weather}>
                    <img
                      src={weatherData?.condition.icon}
                      alt={weatherData?.condition.text}
                      title={weatherData?.condition.text}
                    ></img>
                    <p title="Температура">{weatherData?.avgtemp_c} °C</p>
                    <p title="Влажность">💧 {weatherData?.avghumidity} %</p>
                    <p title="Видимость">🌫️ {weatherData?.avgvis_km} км</p>
                    <p title="Скорость ветра">
                      💨{" "}
                      {Number((weatherData?.maxwind_kph ?? 0) * 0.28).toFixed(
                        0
                      )}{" "}
                      м/с
                    </p>
                  </div>
                )}
                {weatherError && (
                  <div>Ошибка при получении погоды: {weatherError}</div>
                )}
              </Form.Group>
              <Form.Group className="mb-3">
                <div className={styles.current_image}>
                  <img
                    src={`${URL_API}/SpotPhoto/photo?id=${currentImageId}`}
                    alt=""
                  />
                </div>
                <div className={styles.div_photos}>
                  {spot?.photos?.map((item) => (
                    <div
                      key={uuidv4()}
                      style={{
                        backgroundImage:
                          "url(" +
                          `${URL_API}/SpotPhoto/photo?id=${item.id}` +
                          ")",
                        backgroundSize: "cover",
                        backgroundRepeat: "no-repeat",
                      }}
                      onClick={handleImageClick}
                      data-id={item.id}
                    >
                      {/* <img
                        src={`${URL_API}/SpotPhoto/photo?id=${item.id}`}
                        alt=""
                        
                      /> */}
                    </div>
                  ))}
                </div>
              </Form.Group>
              {loggedIn && (
                <div className={styles.upload}>
                  <input
                    type="file"
                    id="fileloader"
                    onChange={handleFileChange}
                  />
                  <label htmlFor="fileloader" className="btn btn-secondary">
                    Загрузить новое фото
                  </label>
                </div>
              )}
            </Col>
            <Col md="6">
              <h3>
                {spot?.name}
                <span className={styles.sub_title}>{spot?.activityName}</span>
              </h3>
              <p className={styles.description}>{spot?.note}</p>
              <div>
                <h6>Как это было на споте</h6>

                <div className={styles.comments}>
                  {!spot?.userSpots?.length ?? 0 === 0 ? (
                    <span>Пока еще нет отзывов, станьте первым!!!</span>
                  ) : (
                    spot?.userSpots?.map((item) => (
                      <div key={uuidv4()}>
                        <div>
                          <span className="fw-bold me-2">{item.userName}</span>
                          <span>
                            {Moment(item.createDataTime).format("D.MM.YYYY")}
                          </span>
                        </div>
                        <div className={styles.comment_text}>
                          {item.comment}
                        </div>
                      </div>
                    ))
                  )}
                </div>
                {loggedIn && (
                  <Form.Group className={styles.i_am_here}>
                    <textarea
                      name="about"
                      placeholder="Раскажите немного о том как тебе спот..."
                      value={values.about || ""}
                      onChange={handleChange}
                      className="form-control"
                    />
                    <Button
                      variant="secondary"
                      type="button"
                      className="float-end"
                      onClick={handleButtonIamHere}
                    >
                      Я тут был
                    </Button>
                  </Form.Group>
                )}
              </div>
            </Col>
          </Row>
        </>
      )}
    </div>
  );
};

export default memo(ViewSpotContentModal);
