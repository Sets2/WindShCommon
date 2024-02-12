import { FC, useCallback, useEffect } from "react";
import { Link } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";
import {
  fetchAllActivities,
  fetchDeleteActivity,
} from "../../services/reducers/thunks/activity";
import { PencilFill } from "react-bootstrap-icons";
import styles from "./activity-list.module.css";

const ActivityList: FC = () => {
  const dispatch = useAppDispatch();
  const { loading, error, items } = useAppSelector((state) => state.activities);
  // Получение активностей:
  useEffect(() => {
    if (!loading && !error && items.length === 0) {
      dispatch(fetchAllActivities());
    }
  }, [loading, items, error, dispatch]);

  const handleOnDelete = useCallback(
    async (e: any) => {
      dispatch(fetchDeleteActivity(e.target.getAttribute("data-id")));
    },
    [dispatch]
  );

  return (
    <div className={styles.main}>
      {items && items.length > 0 && (
        <>
          <table className="table caption-top">
            <thead>
              <tr>
                <th scope="col" className={styles.col_min}>
                  #
                </th>
                <th scope="col">Активность</th>
                <th scope="col"></th>
              </tr>
            </thead>
            <tbody>
              {items.map((x, index) => (
                <tr key={x.id}>
                  <th>{index + 1}</th>
                  <td>{x.name}</td>
                  <td>
                    <Link to={x.id} className="me-2">
                      <PencilFill title="Редактировать"></PencilFill>
                    </Link>
                    <span role="button" onClick={handleOnDelete} data-id={x.id}>
                      ❌
                    </span>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <Link to="new">Создать активность</Link>
        </>
      )}
      {error && <p>Ошибка при загрузке активностей: {error}</p>}
    </div>
  );
};

export default ActivityList;
