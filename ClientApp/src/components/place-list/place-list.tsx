import { FC, useEffect } from "react";
import { Row, Col } from "react-bootstrap";
import { Link } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";
import { fetchAllPlaces } from "../../services/reducers/thunks/place";

const PlaceList: FC = () => {
  const dispatch = useAppDispatch();
  const { loading, error, items } = useAppSelector((state) => state.places);
  // Получение спотов:
  useEffect(() => {
    if (!loading && !error && items.length === 0) {
      dispatch(fetchAllPlaces());
    }
  }, [loading, items, error, dispatch]);

  return (
    <div>
      {items && items.length > 0 && (
        <>
          {items.map((x) => (
            <Row key={x.id}>
              <Col>
                <p key={x.id}>{x.note}</p>
              </Col>
              <Col>
                <Link to={x.id}>Редактировать</Link>
              </Col>
            </Row>
          ))}
        </>
      )}
      {error && <p>Ошибка при загрузке спотов: {error}</p>}
    </div>
  );
};

export default PlaceList;
