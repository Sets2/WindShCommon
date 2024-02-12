import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { TAppState } from "../types";
import { SliceNames } from "../../utils/constant";

const appInitialState: TAppState = {
  isInitialized: false,
  activityFilter: [],
};

const appSlice = createSlice({
  name: SliceNames.APP,
  initialState: appInitialState,
  reducers: {
    initAppAction: (state: TAppState) => {
      state.isInitialized = true;
    },
    initActivityFilter: (
      state: TAppState,
      action: PayloadAction<Array<string>>
    ) => {
      state.activityFilter = action.payload;
    },
    setActivityFilter: (
      state: TAppState,
      action: PayloadAction<{ id: string; flag: boolean }>
    ) => {
      state.activityFilter = action.payload.flag
        ? [...state.activityFilter, action.payload.id]
        : [...state.activityFilter.filter((x) => x !== action.payload.id)];
    },
  },
});

export const { initAppAction, initActivityFilter, setActivityFilter } =
  appSlice.actions;
export const appReducer = appSlice.reducer;
