import { getCookie, setCookie } from "./cookie";

export const getAuthToken = (): string => {
  return getCookie("token") as string;
};

export const getRefreshToken = (): string => {
  return localStorage.getItem("token") as string;
};

export const saveTokens = (authToken: string, refreshToken: string) => {
  if (authToken.indexOf("Bearer") === 0) {
    authToken = authToken.split("Bearer ")[1];
  }
  setCookie("token", authToken, { expires: 20 * 60 * 1000 });
  localStorage.setItem("token", refreshToken);
};

export const clearTokens = () => {
  setCookie("token", null, { expires: -1 });
  localStorage.removeItem("token");
};
