import {
  FC,
  useCallback,
  useEffect,
  SyntheticEvent,
  memo,
  useState,
  ChangeEvent,
} from "react";
import { Row, Col } from "react-bootstrap";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";

import { useFormAndValidation } from "../../hooks/use-form-and-validation";
import {
  fetchSetInfoUser,
  fetchUploadUserPhoto,
} from "../../services/reducers/thunks";
import { getPhoto } from "../../services/utils/api";

import styles from "./profile.module.css";

const ProfilePage: FC = () => {
  const dispatch = useAppDispatch();

  const [imgSrc, setImgSrc] = useState("");
  const [firstLoadFlag, setFirstLoadFlag] = useState(true);
  const { values, setValues, handleChange } = useFormAndValidation();
  const { loading, error, user } = useAppSelector((state) => state.user);

  useEffect(() => {
    if (user !== null && firstLoadFlag) {
      setValues({
        userName: user.userName,
        email: user.email,
        fio: user.fio ?? "",
        age: user.age?.toString() ?? "",
        about: user.about ?? "",
      });
      setFirstLoadFlag(false);
    }
  }, [user, firstLoadFlag, setValues]);

  const handlOnsubmin = useCallback(
    async (e: SyntheticEvent) => {
      e.preventDefault();
      dispatch(
        fetchSetInfoUser({
          id: user!.id,
          email: values.email,
          fio: values.fio,
          userName: values.userName,
          age: parseInt(values.age),
          about: values.about,
        })
      );
    },
    [dispatch, values, user]
  );

  const handlCansel = useCallback(
    async (e: SyntheticEvent) => {
      if (user !== null) {
        setValues({
          userName: user.userName,
          email: user.email,
          fio: user.fio ?? "",
          age: user.age?.toString() ?? "",
          about: user.about ?? "",
        });
      }
    },
    [user, setValues]
  );

  useEffect(() => {
    if (user?.fotoFileName && user?.fotoFileName.length > 0)
        getPhoto("UserWind/photo?id=" + user?.id).then((data) =>
        setImgSrc(data?.data ?? "")
      );
  }, [user, setImgSrc]);

  const handleFileChange = useCallback(
    (e: ChangeEvent<HTMLInputElement>) => {
      if (e.target.files) {
        dispatch(fetchUploadUserPhoto(e.target.files[0], user!.id));
      }
    },
    [dispatch]
  );
  return (
    <Row>
      <Col md="6" className="mt-5">
        <div className={styles.div_img}>
          <img src={imgSrc} alt="" />
        </div>
        <div className={styles.upload}>
          <input type="file" id="fileloader" onChange={handleFileChange} />
          <label htmlFor="fileloader" className="btn btn-secondary">
            Загрузить новый аватар
          </label>
        </div>
      </Col>
      <Col md="6" className="mt-5 position-relative">
        <h2>Профиль</h2>
        <form className="ms-3 me-3 mt-5" onSubmit={handlOnsubmin}>
          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label>Эл. почта</Form.Label>
            <input
              required={true}
              name="email"
              type="email"
              placeholder="E-mail"
              value={values.email || ""}
              onChange={handleChange}
              className="form-control"
            />
            <Form.Text className="text-muted">
              Мы никогда не передадим вашу электронную почту кому-либо еще.
            </Form.Text>
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Имя пользователя</Form.Label>
            <input
              required={true}
              name="userName"
              type="text"
              placeholder="Имя пользователя"
              value={values.userName || ""}
              onChange={handleChange}
              className="form-control"
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>ФИО</Form.Label>
            <input
              name="fio"
              type="text"
              placeholder="ФИО"
              value={values.fio || ""}
              onChange={handleChange}
              className="form-control"
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Возвраст</Form.Label>
            <input
              name="age"
              type="number"
              placeholder="Возвраст"
              value={values.age || ""}
              onChange={handleChange}
              className="form-control"
            />
          </Form.Group>
          <Form.Group className="mb-3">
            <Form.Label>Немного о себе</Form.Label>
            <textarea
              name="about"
              placeholder="Раскажите немного о себе..."
              value={values.about || ""}
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
        </form>
      </Col>
    </Row>
  );
};

export default memo(ProfilePage);
