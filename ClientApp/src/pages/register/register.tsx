import { FC, useCallback, useEffect, SyntheticEvent, memo } from "react";
import { Row, Col } from "react-bootstrap";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";

import { useFormAndValidation } from "../../hooks/use-form-and-validation";
import { userClear } from "../../services/reducers/slices";
import { fetchRegisterUser } from "../../services/reducers/thunks";

import styles from "./register.module.css";

const RegisterPage: FC = () => {
  const dispatch = useAppDispatch();

  const { values, handleChange } = useFormAndValidation();
  const { loading, error } = useAppSelector((state) => state.user);

  useEffect(() => {
    dispatch(userClear());
  }, [dispatch]);

  const handlOnsubmin = useCallback(
    async (e: SyntheticEvent) => {
      e.preventDefault();
      dispatch(fetchRegisterUser(values.name, values.email, values.password));
    },
    [dispatch, values]
  );

  return (
    <Row className="">
      <Col md="8" className={styles.bg}></Col>
      <Col md="4" className="mt-5 position-relative">
        <h2 className="ms-3 me-3 mb-2">Регистрация</h2>
        <form className="ms-3 me-3 mt-5" onSubmit={handlOnsubmin}>
          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label>Эл. почта</Form.Label>
            <input
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
            <Form.Label>Имя</Form.Label>
            <input
              name="name"
              type="text"
              placeholder="Имя"
              value={values.name || ""}
              onChange={handleChange}
              className="form-control"
            />
          </Form.Group>
          <Form.Group className="mb-3" controlId="formBasicPassword">
            <Form.Label>Пароль</Form.Label>
            <input
              name="password"
              type="password"
              placeholder="Пароль"
              value={values.password || ""}
              onChange={handleChange}
              className="form-control"
            />
          </Form.Group>
          <Form.Group>
            <Button
              variant="warning"
              type="submit"
              className="float-end"
              disabled={loading ? true : false}
            >
              {loading ? "Ожидание..." : "Зарегистрироваться"}
            </Button>
          </Form.Group>
          {error && <div className={`${styles.error} red mt-6`}>{error}</div>}
        </form>
      </Col>
    </Row>
  );
};

export default memo(RegisterPage);
