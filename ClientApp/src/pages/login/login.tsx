import { FC, useCallback, useEffect, SyntheticEvent, memo } from "react";
import { Col, Row } from "react-bootstrap";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import { Link } from "react-router-dom";

import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";
import { useFormAndValidation } from "../../hooks/use-form-and-validation";
import { userClear } from "../../services/reducers/slices";
import { fetchLoginUser } from "../../services/reducers/thunks";

import styles from "./login.module.css";

const LoginPage: FC = () => {
  const dispatch = useAppDispatch();

  const { values, handleChange } = useFormAndValidation();
  const { loading, error } = useAppSelector((state) => state.user);

  useEffect(() => {
    dispatch(userClear());
  }, [dispatch]);

  const handlOnsubmin = useCallback(
    async (e: SyntheticEvent) => {
      e.preventDefault();
      dispatch(fetchLoginUser(values.name, values.password));
    },
    [dispatch, values]
  );
  return (
    <Row className="">
      <Col md="8" className={styles.bg}></Col>
      <Col md="4" className="mt-5 position-relative">
        <h2 className="ms-3 me-3 mb-2">Добро пожаловать в WindSharing!</h2>
        <form className="ms-3 me-3 mt-5" onSubmit={handlOnsubmin}>
          <Form.Group className="mb-3">
            <Form.Label>Имя</Form.Label>
            <input
              name="name"
              type="text"
              placeholder="Имя"
              value={values.name}
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
              value={values.password}
              onChange={handleChange}
              className="form-control"
            />
          </Form.Group>
          <Form.Group className="mb-1">
            <Link to="/forgot" className="link">
              Забыли пароль?
            </Link>
            <Button
              variant="warning"
              type="submit"
              className="float-end"
              disabled={loading ? true : false}
            >
              {loading ? "Ожидание..." : "Войти"}
            </Button>
          </Form.Group>
          {error && <div className={`${styles.error} red`}>{error}</div>}
        </form>
        <div
          className={`${styles.register_link_block} position-absolute bottom-0 start-50 translate-middle-x mb-5`}
        >
          <div>Еще не зарегистрировались в WindSharing? </div>
          <Link to="/register" className="link">
            Зарегистрироваться
          </Link>
        </div>
      </Col>
    </Row>
  );
};

export default memo(LoginPage);
