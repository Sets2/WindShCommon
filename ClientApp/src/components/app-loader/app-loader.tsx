import React, { FC, memo } from "react";

import styles from "./app-loader.module.css";

const AppLoader: FC = () => {
    return (
        <div className={styles.app_loader}>
            <header className={styles.app_loader_header}>
                <div className={styles.app_loader_text}>
                    <span style={{ '--num': 0 } as React.CSSProperties} >W</span>
                    <span style={{ '--num': 1 } as React.CSSProperties} >i</span>
                    <span style={{ '--num': 2 } as React.CSSProperties} >n</span>
                    <span style={{ '--num': 3 } as React.CSSProperties} >d</span>
                    <span style={{ '--num': 4 } as React.CSSProperties} >S</span>
                    <span style={{ '--num': 5 } as React.CSSProperties} >h</span>
                    <span style={{ '--num': 6 } as React.CSSProperties} >a</span>
                    <span style={{ '--num': 7 } as React.CSSProperties} >r</span>
                    <span style={{ '--num': 8 } as React.CSSProperties} >i</span>
                    <span style={{ '--num': 9 } as React.CSSProperties} >n</span>
                    <span style={{ '--num': 10 } as React.CSSProperties} >g</span>
                </div>
            </header>
        </div>
    );
};

export default memo(AppLoader);
