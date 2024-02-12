import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import {
  TAllUsersState,
  TInfoUser,
  TInfoUserAdmin,
  TUserState,
} from "../../types";
import { ROLE_ADMIN, SliceNames } from "../../utils/constant";

const userInitialState: TUserState = {
  loading: false,
  isAdmin: false,
  loggedIn: false,
  user: null,
  error: "",
};

const allUsersInitialState: TAllUsersState = {
  items: [],
  loading: false,
  error: "",
};

const userSlice = createSlice({
  name: SliceNames.USER,
  initialState: userInitialState,
  reducers: {
    currentUserLoadingAction: () => userInitialState,

    currentUserReceivedAction: (
      state: TUserState,
      action: PayloadAction<TInfoUser>
    ) => {
      state.loading = false;
      state.loggedIn = true;
      state.isAdmin = action.payload.roles?.includes(ROLE_ADMIN) ?? false;
      state.user = action.payload;
    },

    currentUserErrorAction: (
      state: TUserState,
      action: PayloadAction<string>
    ) => {
      state.loading = false;
      state.isAdmin = false;
      state.error = action.payload;
      state.user = null;
    },

    userLoginLoading: () => userInitialState,

    userLoginReceived: (
      state: TUserState,
      action: PayloadAction<TInfoUser>
    ) => {
      state.loading = false;
      state.loggedIn = true;
      state.user = action.payload;
      state.isAdmin = action.payload.roles?.includes(ROLE_ADMIN) ?? false;
      state.error = "";
    },

    userLoginByToken: (state: TUserState) => {
      state.loggedIn = true;
      state.error = "";
    },

    userLogoutLoading: (state: TUserState) => {
      state.loading = true;
      state.error = "";
    },

    userLogoutReceived: () => userInitialState,

    userSetInfoLoading: (state: TUserState) => {
      state.loading = true;
      state.error = "";
    },

    userSetInfoReceived: (state: TUserState) => {
      state.loading = false;
      state.error = "";
    },

    userSetInfoError: (state: TUserState, action: PayloadAction<string>) => {
      state.loading = false;
      state.error = action.payload;
    },

    userError: (state: TUserState, action: PayloadAction<string>) => {
      state.loading = false;
      state.loggedIn = false;
      state.user = null;
      state.error = action.payload;
    },

    userClear: () => userInitialState,
  },
});

const allUsersSlice = createSlice({
  name: SliceNames.ALL_USER,
  initialState: allUsersInitialState,
  reducers: {
    allUsersLoading: (state: TAllUsersState) => {
      state.loading = true;
      state.items = [];
      state.error = "";
    },

    allUsersReceived: (
      state: TAllUsersState,
      action: PayloadAction<Array<TInfoUserAdmin>>
    ) => {
      state.loading = false;
      state.items = action.payload;
      state.error = "";
    },

    allUsersError: (state: TAllUsersState, action: PayloadAction<string>) => {
      state.loading = false;
      state.items = [];
      state.error = action.payload;
    },
  },
});

export const {
  currentUserLoadingAction,
  currentUserReceivedAction,
  currentUserErrorAction,
  userLoginLoading,
  userLoginReceived,
  userLogoutLoading,
  userLogoutReceived,
  userSetInfoLoading,
  userLoginByToken,
  userSetInfoReceived,
  userError,
  userSetInfoError,
  userClear,
} = userSlice.actions;
export const userReducer = userSlice.reducer;

export const { allUsersLoading, allUsersReceived, allUsersError } =
  allUsersSlice.actions;
export const allUsersReducer = allUsersSlice.reducer;
