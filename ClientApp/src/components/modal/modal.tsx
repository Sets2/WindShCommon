import ReactDOM from "react-dom";
import React, {
  memo,
  useEffect,
  FC,
  useCallback,
  SyntheticEvent,
  useState,
} from "react";
import { useAppSelector, useAppDispatch } from "../../hooks/use-app-dispatch";
import { closeModal } from "../../services/reducers/slices/modal-slice";

import styles from "./modal.module.css";

const modalRoot = document.getElementById("react-modals") as HTMLElement;

const Modal: FC = () => {
  const dispatch = useAppDispatch();
  const { contentModal, title, onClose } = useAppSelector(
    (state) => state.modal
  );
  const [opacity, setOpacity] = useState(0);

  const handleCloseModal = useCallback(() => {
    if (onClose !== undefined) {
      onClose();
    }
    setOpacity(0);
    setTimeout(() => {
      dispatch(closeModal());
    }, 1000);
  }, [dispatch, onClose]);

  const close = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Escape" || e.key === "Esc") {
        handleCloseModal();
      }
    },
    [handleCloseModal]
  );

  useEffect(() => {
    window.addEventListener("keydown", close);
    return () => window.removeEventListener("keydown", close);
  }, [close]);

  useEffect(() => {
    setOpacity(1);
  }, []);

  const handleDivClick = useCallback((e: SyntheticEvent) => {
    e.stopPropagation();
  }, []);

  var variantClassName = styles.middle;

  return ReactDOM.createPortal(
    <div
      className={`${styles.modal_overlay} modal_a`}
      style={{ opacity: opacity }}
    >
      <div
        className={`${styles.modal} ${variantClassName}`}
        onClick={handleDivClick}
      >
        <div className={styles.header}>
          <span className="fw-bold">{title}</span>
          <span className={styles.close_icon} onClick={handleCloseModal}>
            ✕
          </span>
        </div>
        <div className={styles.body}>{contentModal}</div>
      </div>
    </div>,
    modalRoot
  );
};
export default memo(Modal);
