import React, { FC, memo, useCallback } from "react";
import {
  useAppSelector,
  useAppDispatch,
} from "../../../../hooks/use-app-dispatch";
import { setActivityFilter } from "../../../../services/reducers/slices/app";
import styles from "./activity-filter.module.css";

const ActivityFilter: FC = () => {
  const dispatch = useAppDispatch();

  const { items, error, loading } = useAppSelector((state) => state.activities);
  const { activityFilter } = useAppSelector((state) => state.app);

  const handleCheckboxChange = useCallback(
    (e: React.ChangeEvent<HTMLInputElement>) => {
      dispatch(
        setActivityFilter({ id: e.target.value, flag: e.target.checked })
      );
    },
    [dispatch]
  );

  if (error) {
    return (
      <div className={`${styles.main} red`}>
        Ошибка загрузки активностей: {error}
      </div>
    );
  }
  return (
    <div className={styles.main}>
      {loading ? (
        <div>Загрузка...</div>
      ) : (
        <div>
          {items.map((item) => (
            <p key={item.id}>
              <input
                id={`checkbox_${item.id}`}
                type="checkbox"
                value={item.id}
                checked={activityFilter.includes(item.id)}
                onChange={handleCheckboxChange}
              />
              <label htmlFor={`checkbox_${item.id}`} className="ms-2">
                {item.name}
              </label>
            </p>
          ))}
        </div>
      )}
    </div>
  );
};

export default memo(ActivityFilter);
