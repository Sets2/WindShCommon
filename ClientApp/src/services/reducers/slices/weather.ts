import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { TWeather, TWeatherState, } from "../types";
import { SliceNames } from "../../utils/constant";

const weatherInitialState: TWeatherState = {
    data: undefined,
    loading: false,
    error: "",
};

const weatherSlice = createSlice({
    name: SliceNames.ALL_ACTIVITY,
    initialState: weatherInitialState,
    reducers: {
        weatherLoading: (state: TWeatherState) => {
            state.loading = true;
            state.data = undefined;
            state.error = "";
        },

        weatherReceived: (
            state: TWeatherState,
            action: PayloadAction<TWeather>
        ) => {
            state.loading = false;
            state.data = action.payload;
            state.error = "";
        },

        weatherError: (
            state: TWeatherState,
            action: PayloadAction<string>
        ) => {
            state.loading = false;
            state.data = undefined;
            state.error = action.payload;
        },
    },
});

export const {
    weatherLoading,
    weatherReceived,
    weatherError,
} = weatherSlice.actions;
export const weatherReducer = weatherSlice.reducer;

