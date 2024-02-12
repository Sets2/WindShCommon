import { combineReducers } from "redux";
import {
    userReducer,
    passwordReducer,
    tokenReducer,
    allSpotsReducer,
    appReducer,
    allPlacesReducer,
    modalReducer,
    weatherReducer
} from "./slices";
import { activityEditReducer, allActivitiesReducer } from "./slices/activity";
import { placeEditReducer } from "./slices/place";
import {
    spotEditReducer,
    spotAddReducer,
    spotIamHereReducer,
} from "./slices/spot";
import { allUsersReducer } from "./slices/user";

export const rootReducer = combineReducers({
  user: userReducer,
  users: allUsersReducer,
  password: passwordReducer,
  token: tokenReducer,
  spots: allSpotsReducer,
  spot: spotEditReducer,
  spotAdd: spotAddReducer,
  spotIamHere: spotIamHereReducer,
  activities: allActivitiesReducer,
  activity: activityEditReducer,
  app: appReducer,
  places: allPlacesReducer,
  place: placeEditReducer,
    modal: modalReducer,
    weather: weatherReducer
});
