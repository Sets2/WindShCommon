import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { TPasswordState } from "../../types";
import { SliceNames } from "../../utils/constant";

const passwordInitialState: TPasswordState = {
  loading: false,
  allowResetPassword: false,
  error: "",
};

const passwordSlice = createSlice({
  name: SliceNames.PASSWORD,
  initialState: passwordInitialState,
  reducers: {
    passwordLoading: (state: TPasswordState) => {
      state.loading = true;
      state.error = "";
    },

    forgotPasswordReceived: (state: TPasswordState) => {
      state.loading = false;
      state.allowResetPassword = true;
      state.error = "";
    },

    resetPasswordReceived: (state: TPasswordState) => {
      state.loading = false;
      state.allowResetPassword = false;
      state.error = "";
    },

    passwordError: (state: TPasswordState, action: PayloadAction<string>) => {
      state.loading = false;
      state.allowResetPassword = false;
      state.error = action.payload;
    },

    passwordClearError: (state: TPasswordState) => {
      state.error = "";
    },
  },
});
export const {
  passwordLoading,
  forgotPasswordReceived,
  resetPasswordReceived,
  passwordError,
  passwordClearError,
} = passwordSlice.actions;
export const passwordReducer = passwordSlice.reducer;
