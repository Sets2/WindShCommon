import {
  getAllActivities,
  getActivity,
  updateActivity,
  createActivity,
  deleteActivity,
} from "../../utils/api";

import { AppDispatch, AppThunk } from "../../types";
import {
  allActivitiesLoading,
  allActivitiesReceived,
  allActivitiesError,
  activityEditError,
  activityEditLoading,
  activityEditReceived,
  activityCreateLoading,
  activityCreateReceived,
  activityCreateError,
} from "../slices/activity";
import { TActivity } from "../types";
import { initActivityFilter } from "../slices/app";

export const fetchAllActivities =
  (): AppThunk => async (dispatch: AppDispatch) => {
    dispatch(allActivitiesLoading());
    await getAllActivities()
      .then((data: Array<TActivity>) => {
        if (data !== undefined) {
          dispatch(allActivitiesReceived(data));
          dispatch(initActivityFilter(data.map((x) => x.id)));
        }
      })
      .catch((ex) => {
        console.log("catch getAllActivities");
        dispatch(allActivitiesError(ex.message));
        console.error(ex);
      });
  };

export const fetchActivity =
  (id: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(activityEditLoading());
    await getActivity(id)
      .then((data) => {
        console.log("activityEditReceived", data);
        if (data !== undefined) {
          dispatch(activityEditReceived(data));
        }
      })
      .catch((ex) => {
        console.log("catch activityEditError");
        dispatch(activityEditError(ex.message));
        console.error(ex);
      });
  };

export const fetchUpdateActivity =
  (id: string, name: string, iconName: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(activityEditLoading());
    await updateActivity(id, name, iconName)
      .then((data) => {
        console.log("activityEditReceived", data);
        if (data !== undefined) {
          dispatch(activityEditReceived(data));
          dispatch(fetchAllActivities());
        }
      })
      .catch((ex) => {
        console.log("catch activityEditError");
        dispatch(activityEditError(ex.message));
        console.error(ex);
      });
  };

export const fetchCreateActivity =
  (name: string, iconName: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(activityCreateLoading());
    await createActivity({ name, iconName })
      .then((data) => {
        if (data !== undefined) {
          dispatch(activityCreateReceived());
          dispatch(fetchAllActivities());
        }
      })
      .catch((ex) => {
        dispatch(activityCreateError(ex.message));
        dispatch(fetchAllActivities());
        console.error(ex);
      });
  };
export const fetchDeleteActivity =
  (id: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(activityEditLoading());
    await deleteActivity(id)
      .then((data) => {
        console.log("activityEditReceived", data);
        if (data !== undefined) {
          dispatch(activityEditReceived(data));
          dispatch(fetchAllActivities());
        }
      })
      .catch((ex) => {
        console.log("catch activityEditError");
        dispatch(activityEditError(ex.message));
        console.error(ex);
      });
  };
