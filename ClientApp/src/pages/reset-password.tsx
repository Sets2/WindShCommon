import { Col, Row } from "react-bootstrap";
import Button from "react-bootstrap/Button";
import Form from "react-bootstrap/Form";
import styles from "./page.module.css";

const ResetPasswordPage = () => {
  return (
    <Row className="">
      <Col md="8" className={styles.bg}></Col>
      <Col md="4" className="mt-5 position-relative">
        <h2 className="ms-3 me-3 mb-2">Придумайте новый пароль</h2>
        <Form className="ms-3 me-3 mt-5">
          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label>Новый пароль</Form.Label>
            <Form.Control type="password" placeholder="пароль" />
          </Form.Group>
          <Form.Group className="mb-3" controlId="formBasicEmail">
            <Form.Label>Код из письма</Form.Label>
            <Form.Control type="password" placeholder="код из письма" />
          </Form.Group>          
          <Button variant="warning" type="submit" className="float-end">
             Сохранить
          </Button>
        </Form>
      </Col>
    </Row>
  );
};

export default ResetPasswordPage;
