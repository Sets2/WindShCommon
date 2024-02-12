import { FC, memo, useCallback } from "react";
import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { Link, useLocation } from "react-router-dom";
import styles from "./header.module.css";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";
import { userClear } from "../../services/reducers/slices";
import { clearTokens } from "../../services/utils/token";
import { Dropdown } from "react-bootstrap";
import { ROLE_ADMIN, URL_KIBANA_LOG } from "../../services/utils/constant";

const Header: FC = () => {
  const dispatch = useAppDispatch();
  const location = useLocation();
  const { loggedIn, user } = useAppSelector((state) => state.user);
  const handleClickExit = useCallback(() => {
    dispatch(userClear());
    clearTokens();
  }, [dispatch]);
  return (
    <Navbar variant="light" className={`${styles.header} mb-0`}>
      <Container>
        <Navbar.Brand as={Link} to="/">
          <h3>WindSharing</h3>
        </Navbar.Brand>

        <Nav>
          {location.pathname !== "/login" && !loggedIn && (
            <Link to="/login" className={`${styles.link} nav-link`}>
              Войти
            </Link>
          )}
          {loggedIn && (
            <Dropdown>
              <Dropdown.Toggle
                className="p-0 m-0"
                variant="Warning"
                title={user?.fio}
              >
                {user?.userName}
              </Dropdown.Toggle>
              <Dropdown.Menu>
                <Dropdown.Item to="/profile" as={Link}>
                  Профиль
                </Dropdown.Item>
                <Dropdown.Divider />
                {user?.roles?.includes(ROLE_ADMIN) && (
                  <>
                    <Dropdown.Header>Админка</Dropdown.Header>
                    <Dropdown.Item to="/admin" as={Link}>
                      Админка
                    </Dropdown.Item>
                    <Dropdown.Item href={URL_KIBANA_LOG}>
                      Kibane log
                    </Dropdown.Item>
                    <Dropdown.Divider />
                  </>
                )}
                <Dropdown.Item onClick={handleClickExit}>Выйти</Dropdown.Item>
              </Dropdown.Menu>
            </Dropdown>
          )}
        </Nav>
      </Container>
    </Navbar>
  );
};

export default memo(Header);
