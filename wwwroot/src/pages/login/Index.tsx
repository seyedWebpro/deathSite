import { useEffect, useState } from "react";
import Header from "../../components/modules/header/Header";
import { getFromLocalStorage } from "../../utils/utils";
import Otp from "../../components/templates/login/main/components/Otp";
import Register from "../../components/templates/login/main/components/Register";
import Password from "../../components/templates/login/main/components/Password";
import Login from "../../components/templates/login/main/components/Login";

const Index = () => {
  const [step, setStep] = useState<string>("login");

  useEffect(() => {
    const registerPhoneNumber = getFromLocalStorage("otpRegisterPhoneNumber");
    const otpLoginPhoneNumber = getFromLocalStorage("otpLoginPhoneNumber");
    const registerUserData = getFromLocalStorage("registerUserData");

    if (!otpLoginPhoneNumber) {
      if (!registerUserData) {
        if (registerPhoneNumber) {
          setStep("register");
        }
      } else {
        setStep("login");
      }
    } else {
      setStep("password");
    }
  }, []);

  return (
    <div>
      <Header />
      <div className="flex items-center pt-32 lg:pt-20">
        <div className="z-50 ml-auto mr-36 w-1/2 sm:!px-5 lg:mr-auto lg:!w-full lg:px-28 xl:w-1/3">
          {step === "login" && <Login setStep={setStep} />}
          {step === "otp" && <Otp setStep={setStep} />}
          {step === "register" && <Register setStep={setStep} />}
          {step === "password" && <Password setStep={setStep} />}
        </div>

        <img
          src="/images/flower.png"
          className="absolute bottom-0 left-0 md:!h-auto md:!object-contain lg:h-[200px] lg:w-1/3 lg:object-contain"
          style={{ transform: "rotateY(180deg)" }}
          alt=""
        />
      </div>
    </div>
  );
};

export default Index;
