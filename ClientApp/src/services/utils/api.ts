import {
    TSpotListItem,
    TSpotCreate,
    TSpotUpdate,
    TAmHereSpot,
    TActivityCreate,
} from "../reducers/types";
import { TInfoUser } from "../types";
import { URL_API } from "./constant";
import { getAuthToken, getRefreshToken, saveTokens } from "./token";

const fetchWithRefresh = async (
    url: string,
    params: RequestInit,
    isNoJsonAnswer: boolean = false
) => {
    try {
        const responce = await fetch(url, params);
        return await checkResponce(responce, isNoJsonAnswer);
    } catch (ex: unknown) {
        if (ex instanceof Error) {
            if (
                ex.message.includes("401") ||
                ex.message === "jwt malformed" ||
                ex.message === "jwt expired"
            ) {
                var refreshToken = getRefreshToken();
                if (refreshToken) {
                    const refreshData = await tokenUser(refreshToken);
                    if (!refreshData.success) {
                        return Promise.reject(refreshData);
                    } else {
                        saveTokens(refreshData.accessToken, refreshData.refreshToken);
                        (params.headers as { [key: string]: string }).authorization =
                            refreshData.accessToken;
                        const responce = await fetch(url, params);
                        return await checkResponce(responce, isNoJsonAnswer);
                    }
                } else {
                    return Promise.reject("RefreshToken no find on local storage");
                }
            } else {
                return Promise.reject(ex);
            }
        }
    }
};

const checkResponce = async (
    response: Response,
    isNoJsonAnswer: boolean = false
) => {
    if (!response.ok) {
        const data = response.json();
        const result = await data.then(
            (result) => result,
            (error) => error
        );
        if (typeof result === "string" || result instanceof String) {
            throw new Error(String(result));
        } else {
            if (result.title) {
                let str = result.title;
                if (str === "Unauthorized") {
                    str = "Не верный логин или пароль";
                }
                console.error(result);
                throw new Error(str);
            } else {
                if (result)
                    throw new Error("Запрос вернул status = " + response.status);
            }
        }
    } else {
        return response.status === 204 || isNoJsonAnswer ? "" : response.json();
    }
};

const checkError = (data: any) => {
    if (data.error) {
        throw new Error(data.error);
    } else {
        return data;
    }
};

export const getWeather = async (latitude: number, longitude: number) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(
        URL_API + "/weather/get_weather_forecast",
        {
            method: "POST",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json",
                authorization: "Bearer " + authToken,
            },
            body: JSON.stringify({
                spot: `${latitude},${longitude}`,
                days: 1
            }),
        },
        true
    ).then((data) => {
        return data;
    });
};

export const registerUser = async (
    name: string,
    email: string,
    password: string
) => {
    return await fetch(URL_API + "/Auth/user", {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            userName: name,
            email: email,
            password: password,
        }),
    })
        .then(checkResponce)
        .then((data) => checkError(data))
        .then((data) => {
            return data;
        });
};

export const loginUser = async (username: string, password: string) => {
    return await fetch(URL_API + "/Auth/login", {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            username: username,
            password: password,
        }),
    })
        .then(checkResponce)
        .then((data) => checkError(data))
        .then((data) => {
            return data;
        });
};

export const me = async () => {
    const authToken = getAuthToken();
    return await fetch(URL_API + "/Auth/me", {
        method: "GET",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
    })
        .then(checkResponce)
        .then((data) => checkError(data))
        .then((data) => {
            return data;
        });
};

export const tokenUser = async (refreshToken: string) => {
    return await fetch(URL_API + "/auth/token", {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            token: refreshToken,
        }),
    })
        .then(checkResponce)
        .then((data) => {
            return data;
        });
};

export const setInfoUser = async (user: TInfoUser) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/userWind/" + user.id, {
        method: "PUT",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify(user),
    })
        .then((data) => checkError(data))
        .then((data) => {
            return data;
        });
};

function arrayBufferToBase64(buffer: ArrayBuffer) {
    return btoa(String.fromCharCode(...new Uint8Array(buffer)));
}

export const getPhoto = async (url: string) => {
    const authToken = getAuthToken();
    const response = await fetch(URL_API + "/" + url, {
        method: "GET",
        headers: {
            Pragma: "no-cache",
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
    });
    const binaryData = await response.arrayBuffer();
    const base64 = arrayBufferToBase64(binaryData);
    const dataUrl = `data:${response.headers.get(
        "content-type"
    )};base64,${base64}`;
    return { key: url, data: dataUrl };
};

export const uploadFile = async (file: File, url: string) => {
    const authToken = getAuthToken();
    var data = new FormData();
    data.append("data", file, file.name);
    return await fetchWithRefresh(URL_API + url, {
        method: "PUT",
        headers: {
            authorization: "Bearer " + authToken,
        },
        body: data,
    })
        .then((data) => checkError(data))
        .then((data) => {
            return data;
        });
};

export const forgotPasswordUser = async (email: string) => {
    return await fetch(URL_API + "/password-reset", {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            email: email,
        }),
    })
        .then(checkResponce)
        .then((data) => {
            return data;
        });
};

export const resetPasswordUser = async (password: string, code: string) => {
    return await fetch(URL_API + "/password-reset/reset", {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
        body: JSON.stringify({
            password: password,
            token: code,
        }),
    })
        .then(checkResponce)
        //  .then(checkSuccess)
        .then((data) => {
            return data;
        });
};

export const getAllUsers = async () => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/userWind ", {
        method: "GET",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
    }).then(
        (data) => {
            return data;
        },
        (error) => {
            throw new Error(error);
        }
    );
};
/*
export const logoutUser = async (refreshToken: string) => {
  return await fetch(URL_API + "/auth/logout", {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      token: refreshToken,
    }),
  })
    .then(checkResponce)
    //  .then(checkSuccess)
    .then((data) => {
      return data;
    });
};

export const getInfoUser = async (authToken: string) => {
  return await fetchWithRefresh(URL_API + "/auth/user ", {
    method: "GET",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      authorization: "Bearer " + authToken,
    },
  })
    //  .then((data) => checkSuccess<TInfoUser>(data))
    .then((data) => {
      return data;
    });
};
*/

export const getAllSpots = async () => {
    return await fetchWithRefresh(URL_API + "/Spot/50/0 ", {
        method: "GET",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
    }).then(
        (data) => {
            return data;
        },
        (error) => {
            throw new Error(error);
        }
    );
};

export const getSpot = async (spotId: any) => {
    return await fetchWithRefresh(URL_API + "/Spot/" + spotId, {
        method: "GET",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
    }).then(
        (data) => {
            return data;
        },
        (error) => {
            throw new Error(error);
        }
    );
};

export const updateSpot = async (item: TSpotUpdate) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/spot/" + item.id, {
        method: "PUT",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify(item),
    }).then((data) => {
        return data;
    });
};

export const addSpot = async (item: TSpotCreate) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/spot", {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify(item),
    }).then((data) => {
        return data;
    });
};

export const deleteSpot = async (id: string) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/spot/" + id, {
        method: "DELETE",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify({
            id: id,
        }),
    }).then((data) => {
        return data;
    });
};

export const spotIamHere = async (item: TAmHereSpot) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/spot/iamhere", {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify(item),
    }).then((data) => {
        return data;
    });
};

export const getAllActivities = async () => {
    return await fetchWithRefresh(URL_API + "/activity ", {
        method: "GET",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
    });
};

export const getActivity = async (id: any) => {
    return await fetchWithRefresh(URL_API + "/Activity/" + id, {
        method: "GET",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
        },
    });
};

export const updateActivity = async (
    id: string,
    name: string,
    iconName: string
) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/activity/" + id, {
        method: "PUT",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify({
            id: id,
            name: name,
            iconName: iconName,
        }),
    }).then((data) => {
        return data;
    });
};

export const createActivity = async (item: TActivityCreate) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/activity", {
        method: "POST",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify(item),
    }).then((data) => {
        return data;
    });
};

export const deleteActivity = async (id: string) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/activity/" + id, {
        method: "DELETE",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify({
            id: id,
        }),
    }).then((data) => {
        return data;
    });
};

export const getAllPlaces = async () => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/Place ", {
        method: "GET",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
    });
};

export const getPlace = async (placeId: any) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/Place/" + placeId, {
        method: "GET",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
    });
};

export const updatePlace = async (
    id: string,
    name: string,
    note: string,
    latitude: number,
    longitude: number
) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/place/" + id, {
        method: "PUT",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify({
            id: id,
            name: name,
            note: note,
            latitude: latitude,
            longitude: longitude,
        }),
    }).then((data) => {
        return data;
    });
};

export const toggleUser = async (id: string) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/userwind/toggle/" + id, {
        method: "PUT",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify({
            id: id,
        }),
    }).then((data) => {
        return data;
    });
};

export const deletePhoto = async (id: string) => {
    const authToken = getAuthToken();
    return await fetchWithRefresh(URL_API + "/SpotPhoto/" + id, {
        method: "DELETE",
        headers: {
            Accept: "application/json",
            "Content-Type": "application/json",
            authorization: "Bearer " + authToken,
        },
        body: JSON.stringify({
            id: id,
        }),
    }).then((data) => {
        return data;
    });
};
