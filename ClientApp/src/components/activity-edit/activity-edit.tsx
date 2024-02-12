import { FC, useState, useEffect, useCallback, SyntheticEvent } from "react";
import { Form, Button, FloatingLabel } from "react-bootstrap";
import { useParams } from "react-router";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";
import { useFormAndValidation } from "../../hooks/use-form-and-validation";
import {
  fetchActivity,
  fetchCreateActivity,
  fetchUpdateActivity,
} from "../../services/reducers/thunks/activity";
import { useNavigate } from "react-router-dom";

const ActivityEdit: FC = () => {
  const navigate = useNavigate();
  let { id } = useParams();
  const dispatch = useAppDispatch();
  const { loading, error, activity } = useAppSelector(
    (state) => state.activity
  );
  const { values, setValues, isChange, setIsChange, handleChange } =
    useFormAndValidation();
  const [isNewRecord, setIsNewRecord] = useState(true);

  useEffect(() => {
    setIsNewRecord(id === "new" ? true : false);
  }, [id]);

  useEffect(() => {
    if (!isNewRecord) {
      setValues({
        id: activity ? activity.id : "",
        name: activity ? activity.name : "",
        iconName: activity ? activity.iconName : "",
      });
    }
  }, [activity, isNewRecord, setValues]);

  useEffect(() => {
    if (
      (!loading && !error && !activity && id) ||
      (activity?.id !== id && id)
    ) {
      if (!isNewRecord) {
        dispatch(fetchActivity(id));
      }
    }
  }, [id, loading, activity, error, dispatch, isNewRecord]);

  const handleOnSubmit = useCallback(
    async (e: SyntheticEvent) => {
      e.preventDefault();
      isNewRecord
        ? dispatch(fetchCreateActivity(values.name, values.iconName))
        : dispatch(
            fetchUpdateActivity(values.id, values.name, values.iconName)
          );
      setIsChange(false);
      navigate("/admin/activity");
    },
    [dispatch, values, setIsChange, navigate, isNewRecord]
  );

  return (
    <div>
      <h2>{id === "new" ? "Создание" : "Редактирование"} активности</h2>
      <Form onSubmit={handleOnSubmit}>
        <input type="hidden" id="id" value={activity?.id} />
        <FloatingLabel
          controlId="floatingInput"
          label="Название активности"
          className="mb-3"
        >
          <Form.Control
            type="text"
            value={values.name || ""}
            placeholder="Название активности"
            name="name"
            onChange={handleChange}
          />
        </FloatingLabel>
        <FloatingLabel controlId="iconName" label="Имя иконки" className="mb-3">
          <Form.Control
            type="text"
            value={values.iconName || ""}
            placeholder="Имя иконки"
            name="iconName"
            onChange={handleChange}
          />
        </FloatingLabel>
        <Button
          variant="primary"
          type="submit"
          // onClick={handleOnSubmit}
          disabled={!isChange ? true : false}
        >
          {loading ? "Сохранение..." : "Сохранить"}
        </Button>
      </Form>

      {error && <p>Ошибка при загрузке спота: {error}</p>}
    </div>
  );
};

export default ActivityEdit;
