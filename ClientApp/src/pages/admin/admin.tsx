import { FC, memo } from "react";
import { Navigate, useLocation } from "react-router-dom";
import { useAppSelector } from "../../hooks/use-app-dispatch";
import { NavLink, Route, Routes } from "react-router-dom";
import SpotEdit from "../../components/spot-edit/spot-edit";
import SpotList from "../../components/spot-list/spot-list";
import PlaceList from "../../components/place-list/place-list";
import styles from "./admin.module.css";
import ActivityList from "../../components/activity-list/activity-list";
import UserList from "../../components/user-list/user-list";
import ActivityEdit from "../../components/activity-edit/activity-edit";
import { Col, Row } from "react-bootstrap";

const AdminPage: FC = () => {
  const location = useLocation();
  const { isAdmin } = useAppSelector((state) => state.user);

  if (!isAdmin) {
    return <Navigate to="/main" replace state={{ location }} />;
  }

  return (
    <main className={styles.main}>
      <Row>
        <Col md="1">
          <ul className="mt-30">
            <li>
              <NavLink
                to="spot"
                className={({ isActive }) =>
                  isActive ? styles.activeNavItem : undefined
                }
              >
                Споты
              </NavLink>
            </li>
            {/* <li>
              <NavLink
                to="place"
                className={({ isActive }) =>
                  isActive ? styles.activeNavItem : undefined
                }
              >
                Места
              </NavLink>
            </li> */}
            <li>
              <NavLink
                to="activity"
                className={({ isActive }) =>
                  isActive ? styles.activeNavItem : undefined
                }
              >
                Активности
              </NavLink>
            </li>
            <li>
              <NavLink
                to="user"
                className={({ isActive }) =>
                  isActive ? styles.activeNavItem : undefined
                }
              >
                Пользователи
              </NavLink>
            </li>
          </ul>
        </Col>
        <Col md="11" className={styles.right_content}>
          <Routes>
            <Route path="user" element={<UserList />} />
            <Route path="spot" element={<SpotList />} />
            <Route path="spot/:id" element={<SpotEdit />} />
            <Route path="place" element={<PlaceList />} />
            <Route path="activity" element={<ActivityList />} />
            <Route path="activity/:id" element={<ActivityEdit />} />
            <Route path="activity/delete/:id" element={<ActivityEdit />} />
          </Routes>
        </Col>
      </Row>
    </main>
  );
};

export default memo(AdminPage);
