// import { ButtonLoader } from "../../../../modules/loader/Loader";
import { Button } from "../../../../shadcn/ui/button";
// import { getFromLocalStorage } from "../../../../../utils/utils"; 
// import { useNavigate } from "react-router-dom";  
import { useState } from "react";
import { LuEye } from "react-icons/lu";
// import Cookies from "js-cookie";
// import { toast } from "../../../../../hooks/use-toast";
// import usePostData from "@/src/hooks/usePostData"; 

const Password = ({
  setStep,
}: {
  setStep: React.Dispatch<React.SetStateAction<string>>;
}) => {
  // const successFunc = (data: {
  //   statusCode: number;
  //   RefreshToken: string;
  //   accessToken: string;
  // }) => {
  //   if (data.statusCode === 200) {
  //     toast({
  //       variant: "success",
  //       title: "با موفقیت وارد شدید",
  //     });
  //     Cookies.set("RefreshToken", data.RefreshToken, {
  //       expires: 9999999,
  //       path: "",
  //     });
  //     Cookies.set("AccessToken", data.accessToken, {
  //       expires: 9999999,
  //       path: "",
  //     });
  //     navigate("/dashboard");
  //   } else if (data.statusCode === 401) {
  //     toast({
  //       variant: "danger",
  //       title: "پسورد وارد شده اشتباه است",
  //     });
  //   } else if (data.statusCode === 409) {
  //     toast({
  //       variant: "danger",
  //       title: "این شماره قادر به ورود به سایت نیست",
  //     });
  //   } else {
  //     toast({
  //       variant: "danger",
  //       title: "با عرض پوزش لطفا مجدد مراحل رو طی کنید",
  //     });
  //     localStorage.clear();
  //     location.reload();
  //   }
  // };

  // const phoneNumber = getFromLocalStorage("otpLoginPhoneNumber");
  const [password, setPassword] = useState<string>("");
  const [error, setError] = useState(true);
  const [showPassword, setShowPassword] = useState(false);
  // const navigate = useNavigate();

  const inputChangeHandler = (value: string) => {
    setPassword(value);
    if (/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z0-9.@$-_#]{8,}$/.test(value)) {
      setError(false);
    } else {
      setError(true);
    }
  };
  // const { mutate: mutation, isPending } = usePostData<{ password: string }>(
  //   `/loginByPassword/${phoneNumber}`,
  //   null,
  //   false,
  //   successFunc,
  // );

  // const { mutate: resendCode } = usePostData<any>(
  //   `/resendCode/${phoneNumber}`,
  //   null,
  //   false,
  // );
  const resendCodeHandler = () => {
    setStep("otp");
    // resendCode({});
  };
 
  return (
    <div className="w-full  " dir="rtl">
      <div className="flex items-center justify-between">
        {/* <p style={{fontFamily:"system-ui"}} dir="ltr">+98{phoneNumber.slice(1, 11)}</p> */}
        <p style={{fontFamily:"system-ui"}} dir="ltr">+98 </p>
        <Button
          onClick={() => setStep("login")}
          className="!rounded-sm !px-4"
          variant={"default"}
        >
          ویرایش
        </Button>
      </div>
      <p className="mt-2">لطفا رمز عبور خود را وارد کنید</p>
      <div className="relative my-6">
        <input
          dir="ltr"
          type={showPassword ? "text" : "password"}
          className="w-full rounded-md border border-solid border-gray-400 py-3 pl-10 pr-3 text-base placeholder:text-right"
          placeholder="رمز عبور"
          value={password}
          onChange={(event) => inputChangeHandler(event.target.value)}
        />
        <LuEye
          onClick={() => setShowPassword((prev) => !prev)}
          className="absolute left-3 top-[15px] cursor-pointer"
        />
      </div>
      <Button
        disabled={!error ? false : true}
        className="mt-5 h-[36px] w-full justify-center !rounded-full text-center"
        variant={"main"}
        // onClick={() => mutation({ password: password })}
      >
        {/* {isPending ? <ButtonLoader /> : "ورود"} */}
        ورود
      </Button>
      <p className="text-center mt-5">فراموشی رمز عبور</p>
      <Button
        onClick={resendCodeHandler}
        className="mx-auto mt-5 !block !rounded-full !px-4 font-thin"
        variant={"default"}
      >
        ورود با کد یکبار مصرف
      </Button>
    </div>
  );
};

export default Password;
