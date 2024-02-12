import { AppDispatch, AppThunk } from "../../types";
import { initAppAction } from "../slices/index";

import { getCurrentUser } from "../thunks/user";
import { fetchAllActivities } from "./activity";
import { fetchAllSpots } from "./spot";
import { getAuthToken } from "../../utils/token";

export const initAppThunk = (): AppThunk => {
  return async (dispatch: AppDispatch) => {
    try {
      await dispatch(fetchAllActivities());
      await dispatch(fetchAllSpots());
      if (getAuthToken()) {
        await dispatch(getCurrentUser());
      }
    } catch (error) {
    } finally {
      setTimeout(() => {
        dispatch(initAppAction());
      }, 2500);
    }
  };
};
