import { Col, Row } from "react-bootstrap";
import Form from "react-bootstrap/Form";
import { Link } from "react-router-dom";
import styles from "./page.module.css";

const ForgotPasswordPage = () => {
  return (
    <Row className="">
      <Col md="8" className={styles.bg}></Col>
      <Col md="4" className="mt-5 position-relative">
        <h2 className="ms-3 me-3 mb-2">Восстановление пароля</h2>
        <Form className="ms-3 me-3 mt-5">
          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label>Укажите свой адрес электронной почты</Form.Label>
            <Form.Control type="email" placeholder="Эл. почта" />
          </Form.Group>

          <Link className="btn btn-warning float-end" to={"/reset"}>
            Продолжить
          </Link>
        </Form>
      </Col>
    </Row>
  );
};

export default ForgotPasswordPage;
