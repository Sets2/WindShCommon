import {
  Action,
  AnyAction,
  ThunkAction,
  ThunkDispatch,
} from "@reduxjs/toolkit";
import { ReactNode } from "react";
import { store } from "../store";

export type TRootState = ReturnType<typeof store.getState>;

export type TAppDispatch = typeof store.dispatch &
  ThunkDispatch<TRootState, null, AnyAction>;

export type TAppThunk = ThunkAction<void, TRootState, null, Action<string>>;

export type TAppState = {
  isInitialized: boolean;
  activityFilter: Array<string>;
};

export type TAmHereSpot = { userId: string; spotId: string; comment: string };
export type TUserSpot = {
  userName: string;
  createDataTime: Date;
  comment: string;
};

export type TSpotCreate = {
  name: string;
  placeId?: string;
  activityId: string;
  note: string;
  latitude: number;
  longitude: number;
};

export type TSpotUpdate = TSpotCreate & {
  id: string;
  isActive: boolean;
};

export type TSpotListItem = TSpotUpdate & {
  createDataTime?: Date;
  activityName?: string;
};

export type TSpot = TSpotListItem & {
  placeName?: string;
  photos?: Array<TPhoto>;
  userSpots?: Array<TUserSpot>;
};

export type TPhoto = {
  id: string;
  fileName: string;
  comment?: string;
  createDataTime: string;
};

export type TBaseState = {
  loading: boolean;
  error: string;
};

export type TAllSpotsState = TBaseState & {
  items: Array<TSpotListItem>;
};

export type TSpotEditState = TBaseState & {
  spot: TSpot | null;
};

export type TActivityCreate = {
  name: string;
  iconName: string;
};
export type TActivity = TActivityCreate & {
  id: string;
};

export type TAllActivitiesState = TBaseState & {
  items: Array<TActivity>;
};

export type TActivityEditState = TBaseState & {
  activity: TActivity | null;
};

export type TPlace = {
  id: string;
  name: string;
  note: string;
  latitude: number;
  longitude: number;
};

export type TAllPlacesState = TBaseState & {
  items: Array<TPlace>;
};

export type TPlaceEditState = TBaseState & {
  place: TPlace | null;
};

export type TModalState = {
  isShowModal: boolean;
  title: ReactNode | null;
  contentModal: ReactNode | null;
  onClose?: () => void;
};
export type TWeatherCondition = {
  code: number;
  icon: string;
  text: string;
};

export type TWeather = {
  mintemp_c: number;
  avgtemp_c: number;
  maxtemp_c: number;
  maxwind_kph: number;
  avgvis_km: number;
  gust_kph: number;
  wind_dir: number;
  uv: number;
  avghumidity: number;
  condition: TWeatherCondition;
};

export type TWeatherState = TBaseState & {
  data?: TWeather;
};
