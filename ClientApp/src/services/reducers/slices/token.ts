import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { TTokenState } from "../../types";
import { SliceNames } from "../../utils/constant";

const tokenInitialState: TTokenState = {
  loading: false,
  error: "",
};

const tokenSlice = createSlice({
  name: SliceNames.TOKEN,
  initialState: tokenInitialState,
  reducers: {
    tokenLoading: (state: TTokenState) => {
      state.loading = true;
      state.error = "";
    },

    tokenReceived: () => tokenInitialState,

    tokenError: (state: TTokenState, action: PayloadAction<string>) => {
      state.loading = false;
      state.error = action.payload;
    },
  },
});
export const { tokenLoading, tokenReceived, tokenError } = tokenSlice.actions;
export const tokenReducer = tokenSlice.reducer;
