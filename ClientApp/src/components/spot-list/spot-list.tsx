import { FC, useCallback, useEffect } from "react";
import { PencilFill } from "react-bootstrap-icons";
import { Link } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";
import {
  fetchAllSpots,
  fetchDeleteSpot,
} from "../../services/reducers/thunks/spot";
import styles from "./spot-list.module.css";

const SpotList: FC = () => {
  const dispatch = useAppDispatch();
  const { loading, error, items } = useAppSelector((state) => state.spots);
  // Получение спотов:
  useEffect(() => {
    if (!loading && !error && items.length === 0) {
      dispatch(fetchAllSpots());
    }
  }, [loading, items, error, dispatch]);

  const handleOnDelete = useCallback(
    async (e: any) => {
      e.preventDefault();
      dispatch(fetchDeleteSpot(e.target.getAttribute("data-id")));
    },
    [dispatch]
  );

  return (
    <div className={styles.main}>
      <table className="table  table-hover">
        <thead>
          <tr>
            <th scope="col" className={styles.col_min}>
              #
            </th>
            <th scope="col">Дата создания</th>
            <th scope="col">Название</th>
            <th scope="col">Активность</th>
            <th scope="col">Долгота</th>
            <th scope="col">Широта</th>
            <th scope="col">Активно</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody>
          {items.map((x, index) => (
            <tr key={x.id}>
              <th>{index}</th>
              <td>{x.createDataTime?.toString()}</td>
              <td>{x.name}</td>
              <td>{x.activityName}</td>
              <td>{x.longitude}</td>
              <td>{x.latitude}</td>
              <td>{x.isActive ? "Да" : "Нет"}</td>
              <td>
                <Link to={x.id} className="me-2">
                  <PencilFill title="Редактировать"></PencilFill>
                </Link>
                <span
                  role="button"
                  title="Удалить спот"
                  onClick={handleOnDelete}
                  data-id={x.id}
                >
                  ❌
                </span>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
      {error && <p>Ошибка при загрузке спотов: {error}</p>}
    </div>
  );
};

export default SpotList;
