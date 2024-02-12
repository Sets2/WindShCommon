import {
  ThunkAction,
  Action,
  ThunkDispatch,
  AnyAction,
} from "@reduxjs/toolkit";
import { ReactNode } from "react";

import { store } from "./store";

export type TCheckSuccess<T> = T & {
  success: boolean;
  message: string;
};

export type TLoginUser = TInfoUser & TTokenUser;

export type TTokenUser = {
  token: string;
  refreshToken: string;
};

export type TInfoUser = {
  about?: string;
  age?: number;
  email: string;
  fio?: string;
  userName: string;
  fotoFileName?: string;
  id: string;
  roles?: Array<string>;
};

export type TInfoUserAdmin = TInfoUser & {
  isActive: boolean;
};

export type TTokenState = {
  loading: boolean;
  error: string;
};

export type TPasswordState = {
  allowResetPassword: boolean;
  loading: boolean;
  error: string;
};

export type TUserState = {
  user: TInfoUser | null;
  loggedIn: boolean;
  isAdmin: boolean;
  loading: boolean;
  error: string;
};

export type TAllUsersState = {
  items: Array<TInfoUserAdmin>;
  loading: boolean;
  error: string;
};

export type TShowModal = {
  content: ReactNode;
  title: ReactNode | null;
  onClose?: () => void;
};

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch &
  ThunkDispatch<RootState, null, AnyAction>;
export type AppThunk = ThunkAction<void, RootState, null, Action<string>>;

export type TWeatherRequest = {
  key: string;
  q: ReactNode | null;
  onClose?: () => void;
};
