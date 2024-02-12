import { tokenUser } from "../../utils/api";
import { saveTokens, clearTokens } from "../../utils/token";
import {
  userLoginByToken,
  tokenLoading,
  tokenReceived,
  tokenError,
} from "../slices";
import { AppDispatch, AppThunk } from "../../types";

export const fetchTokenUser =
  (refreshToken: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(tokenLoading());
    await tokenUser(refreshToken)
      .then((data) => {
        dispatch(tokenReceived());
        saveTokens(data.accessToken, data.refreshToken);
        dispatch(userLoginByToken());
      })
      .catch((ex) => {
        console.error(ex);
        dispatch(tokenError(ex.message));
        clearTokens();
      });
  };
