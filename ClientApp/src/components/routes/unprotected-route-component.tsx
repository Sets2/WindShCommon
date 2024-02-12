import React, { memo, ReactNode } from "react";
import { Navigate, Outlet, useLocation, Location } from "react-router-dom";

type TUnprotectedRouteProps = {
  redirectCondition: boolean;
  noWrapFlag: boolean;
  redirectPath: string;
  children?: ReactNode;
};

export type TCustomizedState = {
  location: Location;
};

export const UnprotectedRoute = ({
  redirectCondition,
  redirectPath,
  children,
  noWrapFlag,
}: TUnprotectedRouteProps) => {
  const location = useLocation();
  const state = location.state as TCustomizedState;
  if (redirectCondition) {
    let to = redirectPath;
    if (state?.location) {
      to = state?.location?.pathname;
      const state2 = state?.location.state as TCustomizedState;
      const state3 = state2?.location?.state as TCustomizedState;
      if (to === "/login" && state3?.location?.pathname === "/login") {
        to = redirectPath;
      }
    }
    return <Navigate to={to} state={location} />;
  }
  return (
    <>
      {noWrapFlag ? <Outlet /> : <>{children ? <>{children}</> : <Outlet />}</>}
    </>
  );
};

export default memo(UnprotectedRoute);
