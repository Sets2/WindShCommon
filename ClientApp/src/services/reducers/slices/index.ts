// Объект пользователя:
import {
    userReducer,
    currentUserLoadingAction,
    currentUserReceivedAction,
    currentUserErrorAction,
    userLoginLoading,
    userLoginReceived,
    userLoginByToken,
    userLogoutLoading,
    userLogoutReceived,
    userSetInfoLoading,
    userSetInfoReceived,
    userSetInfoError,
    userError,
    userClear,
} from "./user";

// Объект восстановаления/сброса пароля:
import {
    passwordReducer,
    passwordLoading,
    forgotPasswordReceived,
    resetPasswordReceived,
    passwordError,
    passwordClearError,
} from "./password";

// Объект токена:
import { tokenReducer, tokenLoading, tokenReceived, tokenError } from "./token";

// Объект спота:
import {
    allSpotsLoading,
    allSpotsReceived,
    allSpotsError,
    allSpotsReducer,
} from "./spot";

// Объект апп:
import { initAppAction, appReducer } from "./app";

// Объект активности:
import {
    allActivitiesError,
    activityEditLoading,
    activityEditReceived,
    allActivitiesReducer,
} from "./activity";

// Объект место:
import {
    allPlacesLoading,
    allPlacesReceived,
    allPlacesError,
    allPlacesReducer,
} from "./place";

// Объект погоды:
import {
    weatherLoading,
    weatherReceived,
    weatherError,
    weatherReducer,
} from "./weather";

import { showModal, closeModal, modalReducer } from "./modal-slice";

export {
    userReducer,
    passwordReducer,
    tokenReducer,
    allSpotsReducer,
    appReducer,
    allActivitiesReducer,
    allPlacesReducer,
    modalReducer,
    weatherReducer
};

export {
    currentUserLoadingAction,
    currentUserReceivedAction,
    currentUserErrorAction,
    userLoginLoading,
    userLoginReceived,
    userLoginByToken,
    userLogoutLoading,
    userLogoutReceived,
    userSetInfoLoading,
    userSetInfoReceived,
    userSetInfoError,
    userError,
    userClear,
    passwordLoading,
    forgotPasswordReceived,
    resetPasswordReceived,
    passwordError,
    passwordClearError,
    tokenLoading,
    tokenReceived,
    tokenError,
    allSpotsLoading,
    allSpotsReceived,
    allSpotsError,
    initAppAction,
    allActivitiesError,
    activityEditLoading,
    activityEditReceived,
    allPlacesLoading,
    allPlacesReceived,
    allPlacesError,
    showModal,
    closeModal,
    weatherLoading,
    weatherReceived,
    weatherError,
};
