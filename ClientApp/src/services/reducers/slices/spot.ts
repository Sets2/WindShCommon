import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  TSpotListItem,
  TAllSpotsState,
  TSpotEditState,
  TSpot,
  TBaseState,
} from "../types";
import { SliceNames } from "../../utils/constant";

const allSpotsInitialState: TAllSpotsState = {
  items: [],
  loading: false,
  error: "",
};

const spotEditInitialState: TSpotEditState = {
  spot: null,
  loading: false,
  error: "",
};

const baseInitialState: TBaseState = {
  loading: false,
  error: "",
};

const allSpotsSlice = createSlice({
  name: SliceNames.ALL_SPOT,
  initialState: allSpotsInitialState,
  reducers: {
    allSpotsLoading: (state: TAllSpotsState) => {
      state.loading = true;
      state.items = [];
      state.error = "";
    },

    allSpotsReceived: (
      state: TAllSpotsState,
      action: PayloadAction<Array<TSpotListItem>>
    ) => {
      state.loading = false;
      state.items = action.payload;
      state.error = "";
    },

    allSpotsError: (state: TAllSpotsState, action: PayloadAction<string>) => {
      state.loading = false;
      state.items = [];
      state.error = action.payload;
    },
  },
});

const spotEditSlice = createSlice({
  name: SliceNames.SPOT_EDIT,
  initialState: spotEditInitialState,
  reducers: {
    spotEditLoading: (state: TSpotEditState) => {
      state.loading = true;
      state.spot = null;
      state.error = "";
    },

    spotEditReceived: (state: TSpotEditState, action: PayloadAction<TSpot>) => {
      state.loading = false;
      state.spot = action.payload;
      state.error = "";
    },

    spotEditError: (state: TSpotEditState, action: PayloadAction<string>) => {
      state.loading = false;
      state.spot = null;
      state.error = action.payload;
    },
    spotEditClear: () => spotEditInitialState,
  },
});

const spotAddSlice = createSlice({
  name: SliceNames.SPOT_ADD,
  initialState: spotEditInitialState,
  reducers: {
    spotAddLoading: (state: TSpotEditState) => {
      state.loading = true;
      state.error = "";
    },

    spotAddReceived: (state: TSpotEditState) => {
      state.loading = false;
      state.error = "";
    },

    spotAddError: (state: TSpotEditState, action: PayloadAction<string>) => {
      state.loading = false;
      state.error = action.payload;
    },
  },
});

const spotPhotoAddSlice = createSlice({
  name: SliceNames.SPOT_PHOTO_ADD,
  initialState: baseInitialState,
  reducers: {
    spotPhotoAddLoading: (state: TBaseState) => {
      state.loading = true;
      state.error = "";
    },

    spotPhotoAddReceived: (state: TBaseState) => {
      state.loading = false;
      state.error = "";
    },

    spotPhotoAddError: (state: TBaseState, action: PayloadAction<string>) => {
      state.loading = false;
      state.error = action.payload;
    },
  },
});

const spotIamHere = createSlice({
  name: SliceNames.SPOT_I_AM_HERE,
  initialState: baseInitialState,
  reducers: {
    spotIamHereLoading: (state: TBaseState) => {
      state.loading = true;
      state.error = "";
    },

    spotIamHereReceived: (state: TBaseState) => {
      state.loading = false;
      state.error = "";
    },

    spotIamHereError: (state: TBaseState, action: PayloadAction<string>) => {
      state.loading = false;
      state.error = action.payload;
    },
  },
});

export const { allSpotsLoading, allSpotsReceived, allSpotsError } =
  allSpotsSlice.actions;
export const allSpotsReducer = allSpotsSlice.reducer;

export const {
  spotEditLoading,
  spotEditReceived,
  spotEditError,
  spotEditClear,
} = spotEditSlice.actions;
export const spotEditReducer = spotEditSlice.reducer;

export const { spotAddLoading, spotAddReceived, spotAddError } =
  spotAddSlice.actions;
export const spotAddReducer = spotAddSlice.reducer;

export const { spotPhotoAddLoading, spotPhotoAddReceived, spotPhotoAddError } =
  spotPhotoAddSlice.actions;
export const spotPhotoAddReducer = spotPhotoAddSlice.reducer;

export const { spotIamHereLoading, spotIamHereReceived, spotIamHereError } =
  spotIamHere.actions;
export const spotIamHereReducer = spotIamHere.reducer;
