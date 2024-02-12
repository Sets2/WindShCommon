import { FC, memo, useEffect, useRef, useState, useCallback } from "react";
import { Map, Placemark, YMaps } from "@pbe/react-yandex-maps";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";

import { TSpotListItem } from "../../services/reducers/types";
import { showModal } from "../../services/reducers/slices";
import AddSpotContentModal from "../../components/modal/modal-content/add-spot/add-spot";

import styles from "./main.module.css";
import ViewSpotContentModal from "../../components/modal/modal-content/view-spot/view-spot";
import ActivityFilter from "../../components/modal/modal-content/activity-filter/activity-filter";
const MainPage: FC = () => {
  const dispatch = useAppDispatch();
  const myMap = useRef();
  const [spots, setSpots] = useState<Array<TSpotListItem>>([]);
  const [filterButtonName, setFilterButtonName] = useState("Фильтр");
  const loggedIn = useAppSelector((state) => state.user.loggedIn);
  const { activityFilter } = useAppSelector((state) => state.app);
  const allActivites = useAppSelector((state) => state.activities.items);

  const allSpots = useAppSelector((state) => state.spots.items);
  const loadSpots = useAppSelector((state) => state.spots.loading);

  useEffect(() => {
    if (allSpots.length > 0) {
      if (activityFilter) {
        setSpots(allSpots.filter(x => activityFilter.includes(x.activityId)));
      } else {
        setSpots(allSpots);
      }
    }
  }, [allSpots, activityFilter]);

  useEffect(() => {
    const activityFilterLength = activityFilter.length;
    const allActivitesLength = allActivites.length;
    if (activityFilterLength === allActivitesLength) {
      setFilterButtonName("Фильтр");
    } else {
      setFilterButtonName(
        `Фильтр (${activityFilterLength} из ${allActivitesLength})`
      );
    }
  }, [activityFilter, allActivites]);

  const mapData = {
    center: [44.751574, 37.573856],
    zoom: 7,
  };

  const onclickPlacemark = (spot: TSpotListItem) => {
    dispatch(
      showModal({
        title: spot.name,
        content: <ViewSpotContentModal spotId={spot.id} />,
      })
    );
  };

  const handleOpenModalAddSpot = useCallback(() => {
    dispatch(
      showModal({
        title: "Новый спот",
        content: <AddSpotContentModal />,
      })
    );
  }, [dispatch, showModal]);

  const handleOpenModalActivityFilter = useCallback(() => {
    dispatch(
      showModal({
        title: "Фильтр активностей",
        content: <ActivityFilter />,
      })
    );
  }, [dispatch, showModal]);

  return (
    <div>
      <div className={styles.div_filter}>
        {loggedIn && <span onClick={handleOpenModalAddSpot}>+ Спот</span>}
        <span onClick={handleOpenModalActivityFilter}>{filterButtonName}</span>
      </div>
      <div className={styles.map}>
        {spots.length > 0 ? (
          <YMaps>
            <Map
              instanceRef={myMap}
              width={"100%"}
              height={"100%"}
              defaultState={mapData}
            >
              {spots.map((item) => (
                <Placemark
                  key={item.id}
                  geometry={[item.latitude, item.longitude]}
                  properties={{
                    /*balloonContentHeader: "Балун метки",
                    balloonContent: { balloonContent },
                    balloonContentFooter: "Подвал",*/
                    hintContent: item.name,
                  }}
                  onClick={() => onclickPlacemark(item)}
                  modules={[
                    /*"geoObject.addon.balloon",*/ "geoObject.addon.hint",
                  ]}
                />
              ))}
              {/*<ZoomControl />*/}
              {/*<GeolocationControl />*/}
            </Map>
          </YMaps>
        ) : (
          <>Загрузка спотов</>
        )}
      </div>
    </div>
  );
};

export default memo(MainPage);
