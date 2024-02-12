import React, { memo, ReactNode, useEffect } from "react";
import { Navigate, Outlet, useLocation } from "react-router-dom";

import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";

import { getCurrentUser } from "../../services/reducers/thunks";
import { getAuthToken } from "../../services/utils/token";

type TProtectedRouteProps = {
  children?: ReactNode;
};

export const ProtectedRoute = ({ children }: TProtectedRouteProps) => {
  const dispatch = useAppDispatch();
  const location = useLocation();

  const { loggedIn } = useAppSelector((state) => state.user);
  const { isInitialized } = useAppSelector((state) => state.app);

  useEffect(() => {
    if (!loggedIn) {
      const token = getAuthToken();
      if (token) {
        dispatch(getCurrentUser());
      }
    }
  }, [dispatch, loggedIn]);

  if (!isInitialized) {
    return <Navigate to="/" replace state={{ location }} />;
  }

  if (!loggedIn) {
    return <Navigate to="login" replace state={{ location }} />;
  }

  return <>{children ? <>{children}</> : <Outlet />}</>;
};

export default memo(ProtectedRoute);
