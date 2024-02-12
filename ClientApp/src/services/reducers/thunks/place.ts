import { getAllPlaces, getPlace, updatePlace } from "../../utils/api";
import { allPlacesLoading, allPlacesReceived, allPlacesError } from "../slices";
import { AppDispatch, AppThunk } from "../../types";
import {
  placeEditError,
  placeEditLoading,
  placeEditReceived,
} from "../slices/place";

export const fetchAllPlaces = (): AppThunk => async (dispatch: AppDispatch) => {
  dispatch(allPlacesLoading());
  await getAllPlaces()
    .then((data) => {
      console.log("allPlacesReceived", data);
      if (data !== undefined) {
        dispatch(allPlacesReceived(data));
      }
    })
    .catch((ex) => {
      console.log("catch getAllPlaces");
      dispatch(allPlacesError(ex.message));
      console.error(ex);
    });
};

export const fetchPlace =
  (id: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(placeEditLoading());
    await getPlace(id)
      .then((data) => {
        console.log("placeEditReceived", data);
        if (data !== undefined) {
          dispatch(placeEditReceived(data));
        }
      })
      .catch((ex) => {
        console.log("catch placeEditError");
        dispatch(placeEditError(ex.message));
        console.error(ex);
      });
  };

export const fetchUpdatePlace =
  (
    id: string,
    name: string,
    note: string,
    latitude: number,
    longitude: number
  ): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(placeEditLoading());
    await updatePlace(id, name, note, latitude, longitude)
      .then((data) => {
        console.log("placeEditReceived", data);
        if (data !== undefined) {
          dispatch(placeEditReceived(data));
        }
      })
      .catch((ex) => {
        console.log("catch placeEditError");
        dispatch(placeEditError(ex.message));
        console.error(ex);
      });
  };
