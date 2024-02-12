import styles from "./page.module.css";

const PageNotFound = () => {
  return (
    <div className={styles.main}>
      <section className={styles.container}>
        <h1 className="text_type_main-medium mb-6">Страница не найдена</h1>
      </section>
    </div>
  );
};

export default PageNotFound;
