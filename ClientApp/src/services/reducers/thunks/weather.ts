import { getWeather } from "../../utils/api";

import { AppDispatch, AppThunk } from "../../types";
import {
  weatherLoading,
  weatherReceived,
  weatherError,
} from "../slices/weather";
import { TWeather, TWeatherCondition } from "../types";

export const fetchWeather =
  (latitude: number, longitude: number): AppThunk =>
  async (dispatch: AppDispatch) => {
    dispatch(weatherLoading());
    await getWeather(latitude, longitude).catch((ex) => {
      console.log("catch getWeather");
      dispatch(weatherError(ex.message));
      console.error(ex);
    });
  };

export const signalRWeather =
  (data: any): AppThunk =>
  async (dispatch: AppDispatch) => {
    if (data !== undefined) {
      let dataWeather: TWeather = {
        maxtemp_c: data.forecast.forecastday[0].day.maxtemp_c,
        mintemp_c: data.forecast.forecastday[0].day.mintemp_c,
        avgtemp_c: data.current.temp_c,
        maxwind_kph: data.forecast.forecastday[0].day.maxwind_kph,
        avgvis_km: data.forecast.forecastday[0].day.avgvis_km,
        gust_kph: data.current.gust_kph,
        uv: data.current.uv,
        avghumidity: data.forecast.forecastday[0].day.avghumidity,
        wind_dir: data.current.wind_dir,
        condition: {
          icon: data.current.condition.icon,
          text: data.current.condition.text,
        } as TWeatherCondition,
      };
      dispatch(weatherReceived(dataWeather));
    }
  };
