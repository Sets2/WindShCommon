import { FC, useCallback, useEffect } from "react";
import { Form } from "react-bootstrap";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";
import { fetchAllUsers } from "../../services/reducers/thunks/user";
import { toggleUser } from "../../services/utils/api";
import styles from "./user-list.module.css";

const UserList: FC = () => {
  const dispatch = useAppDispatch();
  const { loading, error, items } = useAppSelector((state) => state.users);
  // Получение спотов:
  useEffect(() => {
    if (!loading && !error && items.length === 0) {
      dispatch(fetchAllUsers());
    }
  }, [loading, items, error, dispatch]);

  const handleToggle = useCallback(
    async (e: any) => {
      e.preventDefault();
      await toggleUser(e.target.getAttribute("data-id"))
        .then((data) => {
          console.log("toggleUser", data);
          dispatch(fetchAllUsers());
        })
        .catch((ex) => {
          console.error(ex);
        });
    },
    [dispatch]
  );
  return (
    <div className={styles.main}>
      <table className="table  table-hover">
        <thead>
          <tr>
            <th scope="col">#</th>
            <th scope="col">Логин</th>
            <th scope="col">Ф. И. О.</th>
            <th scope="col">Почта</th>
            <th scope="col">Возвраст</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody>
          {items.map((x, index) => (
            <tr key={x.id}>
              <th>{index}</th>
              <td>{x.userName}</td>
              <td>{x.fio}</td>
              <td>{x.email}</td>
              <td>{x.age}</td>
              <td>
                <Form.Check
                  type="switch"
                  id="custom-switch"
                  label="Активен"
                  checked={x.isActive}
                  onChange={handleToggle}
                  data-id={x.id}
                />
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
export default UserList;
