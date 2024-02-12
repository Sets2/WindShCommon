import { FC, memo, useEffect, useState } from "react";
import {
  MainPage,
  LoginPage,
  RegisterPage,
  ForgotPasswordPage,
  ResetPasswordPage,
  NotFound404,
  AdminPage,
  ProfilePage,
} from "../../pages";
import { Routes, Route } from "react-router-dom";
import Header from "../header/header";
import { UnprotectedRoute } from "../routes/unprotected-route-component";
import { useAppDispatch, useAppSelector } from "../../hooks/use-app-dispatch";
import { ProtectedRoute } from "../routes/protected-route-component";
import AppLoader from "../app-loader/app-loader";
import Modal from "../modal/modal";
import { initAppThunk, signalRWeather } from "../../services/reducers/thunks";
import { URL_API_WEATHER } from "../../services/utils/constant";
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from "@microsoft/signalr";

import styles from "./app.module.css";

const App: FC = () => {
  const dispatch = useAppDispatch();
  const { loggedIn, user } = useAppSelector((state) => state.user);
  const { isInitialized } = useAppSelector((state) => state.app);
  const { isShowModal } = useAppSelector((state) => state.modal);

  const [connection, setConnection] = useState<HubConnection>();

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(URL_API_WEATHER + "hubs/weather")
      .withAutomaticReconnect()
      .build();
    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection && user) {
      if (connection.state === HubConnectionState.Disconnected) {
        connection
          .start()
          .then((result) => {
            console.log("Connected SignalR!");

            try {
              connection.send("SubscribeOnGettingWeather", user?.id);
            } catch (e) {
              console.log("ошибка SubscribeOnGettingWeather", e);
            }

            connection.on("RecieveWeather", (data) => {
              console.log("SignalR Answer");
              //connection.stop();
              dispatch(signalRWeather(data));
            });
          })
          .catch((e) => console.error("Connection SignalR failed: ", e));
      }
    }
  }, [connection, user]);

  useEffect(() => {
    if (!isInitialized) {
      dispatch(initAppThunk());
    }
  }, [dispatch, isInitialized]);

  return (
    <>
      {isInitialized && <Header />}
      <div className={styles.app}>
        <Routes>
          <Route
            element={
              <UnprotectedRoute
                redirectCondition={isInitialized}
                redirectPath="main"
                noWrapFlag={true}
              />
            }
          >
            <Route path="/" element={<AppLoader />} />
          </Route>
          <Route
            element={
              <UnprotectedRoute
                redirectCondition={!isInitialized || loggedIn}
                redirectPath="/"
                noWrapFlag={false}
              />
            }
          >
            <Route path="login" element={<LoginPage />} />
            <Route path="register" element={<RegisterPage />} />
            <Route path="forgot" element={<ForgotPasswordPage />} />
            <Route path="reset" element={<ResetPasswordPage />} />
          </Route>
          <Route element={<ProtectedRoute />}>
            <Route path="profile" element={<ProfilePage />} />
            <Route path="admin/*" element={<AdminPage />} />
          </Route>
          <Route path="main" element={<MainPage />} />
          <Route path="*" element={<NotFound404 />} />
        </Routes>
      </div>
      {isShowModal && <Modal />}
    </>
  );
};

export default memo(App);
