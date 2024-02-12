import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { TActivity, TAllActivitiesState, TActivityEditState } from "../types";
import { SliceNames } from "../../utils/constant";

const allActivitiesInitialState: TAllActivitiesState = {
  items: [],
  loading: false,
  error: "",
};

const activityEditInitialState: TActivityEditState = {
  activity: null,
  loading: false,
  error: "",
};

const allActivitiesSlice = createSlice({
  name: SliceNames.ALL_ACTIVITY,
  initialState: allActivitiesInitialState,
  reducers: {
    allActivitiesLoading: (state: TAllActivitiesState) => {
      state.loading = true;
      state.items = [];
      state.error = "";
    },

    allActivitiesReceived: (
      state: TAllActivitiesState,
      action: PayloadAction<Array<TActivity>>
    ) => {
      state.loading = false;
      state.items = action.payload;
      state.error = "";
    },

    allActivitiesError: (
      state: TAllActivitiesState,
      action: PayloadAction<string>
    ) => {
      state.loading = false;
      state.items = [];
      state.error = action.payload;
    },
  },
});

const activityEditSlice = createSlice({
  name: SliceNames.ACTIVITY_EDIT,
  initialState: activityEditInitialState,
  reducers: {
    activityEditLoading: (state: TActivityEditState) => {
      state.loading = true;
      state.activity = null;
      state.error = "";
    },

    activityEditReceived: (
      state: TActivityEditState,
      action: PayloadAction<TActivity>
    ) => {
      state.loading = false;
      state.activity = action.payload;
      state.error = "";
    },

    activityEditError: (
      state: TActivityEditState,
      action: PayloadAction<string>
    ) => {
      state.loading = false;
      state.activity = null;
      state.error = action.payload;
    },
  },
});

const activityCreateSlice = createSlice({
  name: SliceNames.ACTIVITY_CREATE,
  initialState: activityEditInitialState,
  reducers: {
    activityCreateLoading: (state: TActivityEditState) => {
      state.loading = true;
      state.error = "";
    },

    activityCreateReceived: (state: TActivityEditState) => {
      state.loading = false;
      state.error = "";
    },

    activityCreateError: (
      state: TActivityEditState,
      action: PayloadAction<string>
    ) => {
      state.loading = false;
      state.error = action.payload;
    },
  },
});

export const {
  allActivitiesLoading,
  allActivitiesReceived,
  allActivitiesError,
} = allActivitiesSlice.actions;
export const allActivitiesReducer = allActivitiesSlice.reducer;

export const { activityEditLoading, activityEditReceived, activityEditError } =
  activityEditSlice.actions;
export const activityEditReducer = activityEditSlice.reducer;

export const {
  activityCreateLoading,
  activityCreateReceived,
  activityCreateError,
} = activityCreateSlice.actions;
export const activityCreateReducer = activityCreateSlice.reducer;
