import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { SliceNames } from "../../utils/constant";
import { TModalState } from "../types";
import { TShowModal } from "../../types";

const modalInitialState: TModalState = {
  isShowModal: false,
  title: null,
  contentModal: null,
  onClose: undefined,
};

const modalSlice = createSlice({
  name: SliceNames.MODAL,
  initialState: modalInitialState,
  reducers: {
    showModal: (state: TModalState, action: PayloadAction<TShowModal>) => {
      state.isShowModal = true;
      state.onClose = action.payload.onClose;
      state.contentModal = action.payload.content;
      state.title = action.payload.title;
    },
    closeModal: () => modalInitialState,
  },
});
export const { showModal, closeModal } = modalSlice.actions;
export const modalReducer = modalSlice.reducer;
