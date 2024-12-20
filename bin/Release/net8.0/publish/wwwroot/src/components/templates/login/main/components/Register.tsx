import { Button } from "../../../..//shadcn/ui/button";
// import usePostData from "@/src/hooks/usePostData";
import { getFromLocalStorage, saveIntoLocalStorage } from "../../../../../utils/utils";
import { registerSchema } from "../../../../../validations/rules";
import { useFormik } from "formik";
import React, { useState } from "react";
import { LuEye } from "react-icons/lu";
import usePostData from "../../../../../hooks/usePostData";
import { toast } from "../../../../../hooks/use-toast";
// import { toast } from "../../../../../hooks/use-toast";

interface formValues {
  firstName: string;
  lastName: string;
  password: string;
  confirmPassword: string;
  phone: string;
}
const Register = ({
  setStep,
}: {
  setStep: React.Dispatch<React.SetStateAction<string>>;
}) => {
  const successFunc = (data: { statusCode: number }) => {
    if (data.statusCode === 200) {
      toast({
        variant: "success",
        title: "اطلاعات با موفقیت ثبت شد",
      });
      setStep("otp");
      saveIntoLocalStorage("registerUserData", formHandler.values);
      formHandler.resetForm();
    } else {
      toast({
        variant: "danger",
        title: "با عرض پوزش لطفا مجدد مراحل رو طی کنید",
      });
      localStorage.clear();
      location.reload();
    }
  };

  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const phoneNumber = getFromLocalStorage("otpRegisterPhoneNumber");
  const formHandler = useFormik({
    initialValues: {
      firstName: "",
      lastName: "",
      password: "",
      confirmPassword: "",
      phone: phoneNumber,
    },
    onSubmit: (values: formValues) => {
      const data = {
        firstName: values.firstName,
        lastName: values.lastName,
        password: values.password,
        confirmPassword: values.confirmPassword,
        phoneNumber: values.phone,
      };
      mutation(data as any); 
    },
    validationSchema: registerSchema,
  });

  const { mutate: mutation, isPending } = usePostData<{ values: formValues }>(
    "/api/user/RegisterUser",
    null,
    false,
    successFunc,
  );

  const submitHandler = (event: React.FormEvent) => {
    event.preventDefault();
    formHandler.handleSubmit();
  };

  return (
    <form className="w-full" dir="rtl">
      <div className="flex items-center justify-between">
        <p style={{ fontFamily: "system-ui" }} dir="ltr">
          +98{phoneNumber?.slice(1, 11)}
        </p>
        <Button
          className="!rounded-sm !px-4"
          onClick={() => setStep("login")}
          variant={"default"}
        >
          ویرایش
        </Button>
      </div>
      <p className="mb-2 mt-2 text-2xl">ثبت‌نام</p>
      <p className="text-sm font-thin">لطفا مشخصات صحیح خود را وارد کنید</p>
      <input
        className="mt-3 w-full rounded-md border border-solid border-gray-400 py-2 pl-6 pr-3 text-base"
        type="text"
        name="firstName"
        placeholder="نام"
        value={formHandler.values.firstName}
        onChange={formHandler.handleChange}
        onBlur={formHandler.handleBlur}
      />
      {formHandler.touched.firstName && formHandler.errors.firstName && (
        <span className="mt-2 block w-full text-center text-sm text-red-600">
          {formHandler.errors.firstName}
        </span>
      )}
      <input
        className="mt-3 w-full rounded-md border border-solid border-gray-400 py-2 pl-6 pr-3 text-base"
        type="text"
        name="lastName"
        placeholder="نام خانوادگی"
        value={formHandler.values.lastName}
        onBlur={formHandler.handleBlur}
        onChange={formHandler.handleChange}
      />
      {formHandler.touched.lastName && formHandler.errors.lastName && (
        <span className="mt-2 block w-full text-center text-sm text-red-600">
          {formHandler.errors.lastName}
        </span>
      )}
      <div className="relative">
        <input
          type={showPassword ? "text" : "password"}
          dir="ltr"
          value={formHandler.values.password}
          onBlur={formHandler.handleBlur}
          onChange={formHandler.handleChange}
          className="mt-3 w-full rounded-md border border-solid border-gray-400 py-2 pl-9 pr-3 text-base placeholder:text-right"
          name="password"
          placeholder="رمز عبور"
        />
        {formHandler.touched.password && formHandler.errors.password && (
          <span className="mt-2 block w-full text-center text-sm text-red-600">
            {formHandler.errors.password}
          </span>
        )}
        <LuEye
          onClick={() => setShowPassword((prev) => !prev)}
          className="absolute left-3 top-[23px] cursor-pointer"
        />
      </div>
      <div className="relative">
        <input
          type={showConfirmPassword ? "text" : "password"}
          dir="ltr"
          value={formHandler.values.confirmPassword}
          onBlur={formHandler.handleBlur}
          onChange={formHandler.handleChange}
          className="mt-3 w-full rounded-md border border-solid border-gray-400 py-2 pl-9 pr-3 text-base placeholder:text-right"
          name="confirmPassword"
          placeholder="تکرار رمز عبور"
        />
        {formHandler.touched.confirmPassword &&
          formHandler.errors.confirmPassword && (
            <span className="mt-2 block w-full text-center text-sm text-red-600">
              {formHandler.errors.confirmPassword}
            </span>
          )}
        <LuEye
          onClick={() => setShowConfirmPassword((prev) => !prev)}
          className="absolute left-3 top-[23px] cursor-pointer"
        />
      </div>
      <Button
        type="submit"
        onClick={(event) => submitHandler(event)}
        className="mx-auto mt-5 !block h-[36px] w-max !rounded-full !px-12 text-center"
        variant={"main"}
        disabled={!formHandler.isValid || !formHandler.dirty}
      >
        {/* {isPending ? <ButtonLoader /> : "ثبت نام"} */}
        ثبت نام
      </Button>
    </form>
  );
};

export default Register;
