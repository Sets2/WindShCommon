import {
  addSpot,
  deleteSpot,
  getAllSpots,
  getSpot,
  spotIamHere,
  updateSpot,
  uploadFile,
} from "../../utils/api";
import {
  allSpotsLoading,
  allSpotsReceived,
  allSpotsError,
  closeModal,
} from "../slices";
import { AppDispatch, AppThunk } from "../../types";
import {
  spotAddError,
  spotAddLoading,
  spotAddReceived,
  spotEditError,
  spotEditLoading,
  spotEditReceived,
  spotPhotoAddError,
  spotPhotoAddLoading,
  spotPhotoAddReceived,
  spotIamHereLoading,
  spotIamHereReceived,
  spotIamHereError,
} from "../slices/spot";
import { TAmHereSpot, TSpotCreate, TSpotUpdate } from "../types";

export const fetchAllSpots = (): AppThunk => async (dispatch: AppDispatch) => {
  dispatch(allSpotsLoading());
  await getAllSpots()
    .then((data) => {
      if (data !== undefined) {
        dispatch(allSpotsReceived(data));
      }
    })
    .catch((ex) => {
      dispatch(allSpotsError(ex.message));
      console.error(ex);
    });
};

export const fetchSpot =
  (id: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(spotEditLoading());
    await getSpot(id)
      .then((data) => {
        if (data !== undefined) {
          dispatch(spotEditReceived(data));
        }
      })
      .catch((ex) => {
        console.log("catch spotEditError");
        dispatch(spotEditError(ex.message));
        console.error(ex);
      });
  };

export const fetchUpdateSpot =
  (item: TSpotUpdate): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(spotEditLoading());
    await updateSpot(item)
      .then((data) => {
        if (data !== undefined) {
          dispatch(spotEditReceived(data));
          dispatch(fetchAllSpots());
        }
      })
      .catch((ex) => {
        console.log("catch spotEditError");
        dispatch(spotEditError(ex.message));
        console.error(ex);
      });
  };

export const fetchAddSpot =
  (item: TSpotCreate): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(spotAddLoading());
    await addSpot(item)
      .then((data) => {
        if (data !== undefined) {
          dispatch(spotAddReceived());
          dispatch(closeModal());
          dispatch(fetchAllSpots());
        }
      })
      .catch((ex) => {
        dispatch(spotAddError(ex.message));
        console.error(ex);
      });
  };

export const fetchUploadSpotPhoto =
  (file: File, spotId: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(spotPhotoAddLoading());
    await uploadFile(file, "/spotPhoto/photo/" + spotId)
      .then(() => {
        dispatch(spotPhotoAddReceived());
        dispatch(fetchSpot(spotId));
      })
      .catch((ex) => {
        console.error(ex);
        dispatch(spotPhotoAddError(ex.message));
      });
  };

export const fetchSpotIamHere =
  (item: TAmHereSpot): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(spotIamHereLoading());
    await spotIamHere(item)
      .then(() => {
        dispatch(spotIamHereReceived());
        dispatch(fetchSpot(item.spotId));
      })
      .catch((ex) => {
        dispatch(spotIamHereError(ex.message));
        console.error(ex);
      });
  };

export const fetchDeleteSpot =
  (id: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(spotEditLoading());
    await deleteSpot(id)
      .then((data) => {
        console.log("activityEditReceived", data);
        if (data !== undefined) {
          dispatch(spotEditReceived(data));
          dispatch(fetchAllSpots());
        }
      })
      .catch((ex) => {
        console.log("catch activityEditError");
        dispatch(spotEditError(ex.message));
        console.error(ex);
      });
  };
