import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { TPlace, TAllPlacesState, TPlaceEditState } from "../types";
import { SliceNames } from "../../utils/constant";

const allPlacesInitialState: TAllPlacesState = {
  items: [],
  loading: false,
  error: "",
};

const placeEditInitialState: TPlaceEditState = {
  place: null,
  loading: false,
  error: "",
};

const allPlacesSlice = createSlice({
  name: SliceNames.ALL_PLACE,
  initialState: allPlacesInitialState,
  reducers: {
    allPlacesLoading: (state: TAllPlacesState) => {
      state.loading = true;
      state.items = [];
      state.error = "";
    },

    allPlacesReceived: (
      state: TAllPlacesState,
      action: PayloadAction<Array<TPlace>>
    ) => {
      state.loading = false;
      state.items = action.payload;
      state.error = "";
    },

    allPlacesError: (state: TAllPlacesState, action: PayloadAction<string>) => {
      state.loading = false;
      state.items = [];
      state.error = action.payload;
    },
  },
});

const placeEditSlice = createSlice({
  name: SliceNames.PLACE_EDIT,
  initialState: placeEditInitialState,
  reducers: {
    placeEditLoading: (state: TPlaceEditState) => {
      state.loading = true;
      state.place = null;
      state.error = "";
    },

    placeEditReceived: (
      state: TPlaceEditState,
      action: PayloadAction<TPlace>
    ) => {
      state.loading = false;
      state.place = action.payload;
      state.error = "";
    },

    placeEditError: (state: TPlaceEditState, action: PayloadAction<string>) => {
      state.loading = false;
      state.place = null;
      state.error = action.payload;
    },
  },
});

export const { allPlacesLoading, allPlacesReceived, allPlacesError } =
  allPlacesSlice.actions;
export const allPlacesReducer = allPlacesSlice.reducer;

export const { placeEditLoading, placeEditReceived, placeEditError } =
  placeEditSlice.actions;
export const placeEditReducer = placeEditSlice.reducer;
