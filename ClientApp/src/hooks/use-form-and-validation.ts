import { useState, useCallback, ChangeEvent } from "react";

interface IKeyValue {
  [key: string]: string;
}

export function useFormAndValidation() {
  const [values, setValues] = useState<IKeyValue>({});
  const [isChange, setIsChange] = useState(false);
  const [errors, setErrors] = useState({});
  const [isValid, setIsValid] = useState<boolean | undefined>(true);

  const handleChange = (
    e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>
  ) => {
    const { name, value } = e.target;
    setValues({ ...values, [name]: value });
    setErrors({ ...errors, [name]: e.target.validationMessage });
    setIsValid(e.target.closest("form")?.checkValidity());
    setIsChange(true);
  };

  const resetForm = useCallback(
    (newValues: IKeyValue = {}, newErrors = {}, newIsValid = false) => {
      setValues(newValues);
      setErrors(newErrors);
      setIsValid(newIsValid);
      setIsChange(false);
    },
    [setValues, setErrors, setIsValid]
  );

  return {
    values,
    isChange,
    setIsChange,
    handleChange,
    errors,
    isValid,
    resetForm,
    setValues,
    setIsValid,
  };
}
