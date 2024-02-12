import {
  registerUser,
  loginUser,
  me,
  setInfoUser,
  uploadFile,
  getAllUsers,
} from "../../utils/api";

import { saveTokens } from "../../utils/token";

import {
  userLoginLoading,
  userLoginReceived,
  userError,
  currentUserLoadingAction,
  currentUserReceivedAction,
  currentUserErrorAction,
  userSetInfoLoading,
  userSetInfoReceived,
  userSetInfoError,
} from "../slices";

import { AppDispatch, AppThunk, TInfoUser } from "../../types";
import {
  allUsersError,
  allUsersLoading,
  allUsersReceived,
} from "../slices/user";

export const fetchRegisterUser =
  (name: string, email: string, password: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(userLoginLoading());
    await registerUser(name, email, password)
      .then((data) => {
        dispatch(userLoginReceived(data.userWind));
        saveTokens(data.token, "");
      })
      .catch((ex) => {
        console.log(ex);
        dispatch(userError(ex.message));
        console.error(ex);
      });
  };

export const fetchLoginUser =
  (email: string, password: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(userLoginLoading());
    await loginUser(email, password)
      .then((data) => {
        dispatch(userLoginReceived(data.userWind));
        saveTokens(data.token, "");
      })
      .catch((ex) => {
        console.error(ex);
        dispatch(userError(ex.message));
      });
  };

export const getCurrentUser = (): AppThunk => async (dispatch: AppDispatch) => {
  dispatch(currentUserLoadingAction());
  await me()
    .then((data) => {
      dispatch(currentUserReceivedAction(data.userWind));
      saveTokens(data.token, "");
    })
    .catch((ex) => {
      console.error(ex);
      dispatch(currentUserErrorAction(ex.message));
    });
};

export const fetchSetInfoUser =
  (userInfo: TInfoUser): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(userSetInfoLoading());
    await setInfoUser(userInfo)
      .then((data) => {
        dispatch(userSetInfoReceived(data.userWind));
      })
      .catch((ex) => {
        console.error(ex);
        dispatch(userSetInfoError(ex.message));
      });
  };

export const fetchUploadUserPhoto =
  (file: File, userId: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(userSetInfoLoading());
    await uploadFile(file, "/userWind/photo/" + userId)
      .then((data) => {
        dispatch(userSetInfoReceived(data.userWind));
        dispatch(getCurrentUser());
      })
      .catch((ex) => {
        console.error(ex);
        dispatch(userSetInfoError(ex.message));
      });
  };

export const fetchAllUsers = (): AppThunk => async (dispatch: AppDispatch) => {
  dispatch(allUsersLoading());
  await getAllUsers()
    .then((data) => {
      if (data !== undefined) {
        dispatch(allUsersReceived(data));
      }
    })
    .catch((ex) => {
      dispatch(allUsersError(ex.message));
      console.error(ex);
    });
};
/*

export const fetchLogoutUser =
    (): AppThunk => async (dispatch: AppDispatch) => {
        dispatch(userLogoutLoading());
        await logoutUser(getRefreshToken())
            .then(() => {
                dispatch(userLogoutReceived());
                clearTokens();
            })
            .catch((ex) => {
                console.error(ex);
                dispatch(userError(ex.message));
            });
    };


export const fetchGetInfoUser =
  (): AppThunk => async (dispatch: AppDispatch) => {
    dispatch(userInfoLoading());
    await getInfoUser(getAuthToken())
      .then((data) => {
        dispatch(userInfoReceived(data.user));
      })
      .catch((ex) => {
        console.error(ex);
        dispatch(userError(ex.message));
      });
  };

export const fetchSetInfoUser =
  (name: string, email: string, password: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(userInfoLoading());
    await setInfoUser(getAuthToken(), name, email, password)
      .then((data) => {
        dispatch(userInfoReceived(data.user));
      })
      .catch((ex) => {
        console.error(ex);
        dispatch(userError(ex.message));
      });
  };*/
