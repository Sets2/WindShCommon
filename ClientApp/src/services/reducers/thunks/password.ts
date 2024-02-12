import { forgotPasswordUser, resetPasswordUser } from "../../utils/api";
import {
  passwordLoading,
  forgotPasswordReceived,
  resetPasswordReceived,
  passwordError,
} from "../slices";
import { AppDispatch, AppThunk } from "../../types";

export const fetchForgotPasswordUser =
  (email: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(passwordLoading());
    await forgotPasswordUser(email)
      .then(() => {
        dispatch(forgotPasswordReceived());
      })
      .catch((ex) => {
        console.error(ex);
        dispatch(passwordError(ex.message));
      });
  };

export const fetchResetPasswordUser =
  (password: string, code: string): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(passwordLoading());
    await resetPasswordUser(password, code)
      .then(() => {
        dispatch(resetPasswordReceived());
      })
      .catch((ex) => {
        console.error(ex);
        dispatch(passwordError(ex.message));
      });
  };
