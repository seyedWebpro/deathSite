"use client";
import { Button } from "../../../../shadcn/ui/button";
import { getFromLocalStorage } from "../../../../../utils/utils";
// import { useQueryClient } from "@tanstack/react-query";
import {   useState } from "react";
// import { useNavigate } from "react-router-dom";
// import { ButtonLoader } from "../../../../modules/loader/Loader";
import {
  InputOTP,
  InputOTPGroup,
  InputOTPSlot,
} from "../../../../shadcn/ui/input-otp";
import { REGEXP_ONLY_DIGITS_AND_CHARS } from "input-otp";
// import Cookies from "js-cookie";
// import usePostData from "@/src/hooks/usePostData";
import Timer from "./Timer";
// import { toast } from "../../../../../hooks/use-toast";
const Otp = ({
  setStep,
}: {
  setStep: React.Dispatch<React.SetStateAction<string>>;
}) => {
  const otpLoginPhoneNumber = getFromLocalStorage("otpLoginPhoneNumber");
  const otpRegisterPhoneNumber = getFromLocalStorage("otpRegisterPhoneNumber");
  // const navigate = useNavigate();
  const phoneNumber = otpLoginPhoneNumber || otpRegisterPhoneNumber;
  const registerUserData = getFromLocalStorage("registerUserData");
  // const queryClient = useQueryClient();
  const [otpCode, setOtpCode] = useState("");

  // const successFunc = (data: {
  //   statusCode: number;
  //   RefreshToken: string;
  //   accessToken: string;
  // }) => {
  //   if (data.statusCode === 200) {
  //     Cookies.set("RefreshToken", data.RefreshToken, {
  //       expires: 9999999,
  //       path: "",
  //     });
  //     Cookies.set("AccessToken", data.accessToken, {
  //       expires: 9999999,
  //       path: "",
  //     });
  //     queryClient.invalidateQueries({ queryKey: ["auth"] });
  //     toast({
  //       variant: "success",
  //       title: otpLoginPhoneNumber
  //         ? "با موفقیت وارد شدید"
  //         : "با موفقیت ثبت نام شدید",
  //     });
  //     navigate("/dashboard");
  //   } else if (data.statusCode === 400) {
  //     toast({
  //       variant: "danger",
  //       title: "کد اشتباه است",
  //     });
  //   } else if (data.statusCode === 405) {
  //     toast({
  //       variant: "danger",
  //       title: "این کد قبلا مورد استفاده قرار گرفته است",
  //     });
  //   } else if (data.statusCode === 422) {
  //     toast({
  //       variant: "danger",
  //       title: "کد وارد شده منسوخ شده است",
  //     });
  //   } else if (data.statusCode === 406) {
  //     toast({
  //       variant: "danger",
  //       title: "کاربر قبلا در سایت ثبت نام شده است",
  //     });
  //   } else {
  //     toast({
  //       variant: "danger",
  //       title: "با عرض پوزش لطفا مجدد مراحل رو طی کنید",
  //     });
  //     location.reload();
  //     localStorage.clear();
  //   }
  // };

  // const { mutate: mutation, isPending } = usePostData<{ code: string }>(
  //   otpLoginPhoneNumber
  //     ? `/loginByCode/${otpLoginPhoneNumber}`
  //     : "/auth/confirmCode",
  //   null,
  //   false,
  //   successFunc,
  // );

  const submitHandler = () => {
    if (otpLoginPhoneNumber) {
      // const data = { code: otpCode };
      // mutation(data);
      
    } else {
      registerUserData.code = otpCode;
      // mutation(registerUserData);
    }
  };

  return (
    <div className="w-full" dir="rtl">
      <div className="flex items-center justify-between">
        <p style={{fontFamily:"system-ui"}} dir="ltr">+98{phoneNumber?.slice(1, 11)}</p>
        <Button
          onClick={() => setStep("login")}
          className="!rounded-sm !px-4"
          variant={"default"}
        >
          ویرایش
        </Button>
      </div>
      <p className="mt-4 text-center text-sm sm:!mt-4">
        کد فعالسازی 4 رقمی به شماره موبایل شما پیامک شد
      </p>
      <div className="relative my-6 flex flex-col  items-center justify-center gap-2 sm:!flex-row sm:!gap-0">
        <p className="text-sm">کد فعالسازی را وارد کنید</p>
        <InputOTP
          value={otpCode}
          type="text"
          dir="ltr"
          inputMode="numeric"
          onChange={(value) => setOtpCode(value)}
          maxLength={4}
          pattern={REGEXP_ONLY_DIGITS_AND_CHARS}
        >
          <InputOTPGroup dir="ltr">
            <InputOTPSlot index={0} />
            <InputOTPSlot index={1} />
            <InputOTPSlot index={2} />
            <InputOTPSlot index={3} />
          </InputOTPGroup>
        </InputOTP>
      </div>
      <span className="mb-3 block text-center text-xs text-red-600" dir="rtl">
        در صورت نیامدن کد، پیامک های spam گوشیتون رو هم چک کنید
      </span>
      <Timer />
      <Button
        disabled={otpCode.length !== 4 ? true : false}
        className="mt-5 h-[36px] w-full justify-center !rounded-full text-center"
        variant={"main"}
        onClick={submitHandler}
      >
        {/* {isPending ? <ButtonLoader /> : "ورود"} */}
        ورود
      </Button>
      {!otpRegisterPhoneNumber ? (
        <Button
          onClick={() => setStep("password")}
          className={`mx-auto mt-5 !block !rounded-full !px-4 font-thin`}
          variant={"default"}
        >
          ورود با رمز عبور
        </Button>
      ) : null}
    </div>
  );
};

export default Otp;
