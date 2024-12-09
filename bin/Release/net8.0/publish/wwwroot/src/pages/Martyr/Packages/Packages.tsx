import React from "react";
import Header from "../../../components/modules/header/Header";
import { Button } from "../../../components/shadcn/ui/button";
import { FaCheck } from "react-icons/fa";
import { GrClose } from "react-icons/gr";
import { Link } from "react-router-dom";

const MartyrPackages = () => {
  return (
    <div>
      <Header />
      <div className="flex flex-col min-h-screen">

      <div
        dir="rtl"
        className={` mt-20 flex-grow rounded-t-[120px] bg-white px-24 pt-20 pb-0 xs:!px-4 sm:pt-10 md:px-10`}
      >
        <p className=" text-center text-xl">لطفا یک بسته را انتخاب کنید</p>
        <div className="overflow-x-auto  sm:p-2">
        <table
          className="mx-auto sm:[&>*]:text-sm mb-10 mt-10 w-2/3 sm:w-full rounded-lg bg-[#fffbfb30] [&>*]:text-center"
          dir="rtl"
        >
          <tr>
            <td>بسته</td>
            <td>بسته رایگان</td>
            <td>بسته اقتصادی</td>
            <td>بسته عادی</td>
            <td>بسته ویژه</td>
          </tr>
          <tr>
            <td>قیمت</td>
            <td>0 تومان</td>
            <td>199,000 تومان</td>
            <td>499,000 تومان</td>
            <td>899,000 تومان</td>
          </tr>
          <tr>
            <td>انتخاب</td>
            <td>
              <Link to={"/martyr/packages/step2"}>
                <Button variant={"main"}>سفارش</Button>
              </Link>
            </td>
            <td>
              <Link to={"/martyr/packages/step2"}>
                <Button variant={"main"}>سفارش</Button>{" "}
              </Link>
            </td>
            <td>
              <Link to={"/martyr/packages/step2"}>
                <Button variant={"main"}>سفارش</Button>
              </Link>
            </td>
            <td>
              <Link to={"/martyr/packages/step2"}>
                <Button variant={"main"}>سفارش</Button>{" "}
              </Link>
            </td>
          </tr>
          <tr>
            <td>حداکثر تعداد تصویر</td>
            <td> 1</td>
            <td> 7</td>
            <td> 40</td>
            <td>1000 </td>
          </tr>
          <tr>
            <td>حداکثر تعداد فیلم</td>
            <td> 0</td>
            <td> 1</td>
            <td> 4</td>
            <td>9</td>
          </tr>
          <tr>
            <td>مراسم ختم انلاین رایگان (مداح،سخنران،قاری)</td>
            <td> 0</td>
            <td> 1</td>
            <td> 1</td>
            <td>1</td>
          </tr>
          <tr>
            <td>زیارت مجازی</td>
            <td>
              <GrClose className="mx-auto text-red-600" />
            </td>
            <td>
              <GrClose className="mx-auto text-red-600" />
            </td>
            <td>
              <GrClose className="mx-auto text-red-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
          </tr>
          <tr>
            <td>شجره نامه</td>
            <td>
              <GrClose className="mx-auto text-red-600" />
            </td>
            <td>
              <GrClose className="mx-auto text-red-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
          </tr>
          <tr>
            <td>حلالیت و نماز هدیه</td>
            <td>
              <GrClose className="mx-auto text-red-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
          </tr>
          <tr>
            <td>امکان پاسخ به پیام های تسلیت</td>
            <td>
              <GrClose className="mx-auto text-red-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
          </tr>
          <tr>
            <td>خلاصه زندگینامه</td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
          </tr>
          <tr>
            <td>زندگینامه</td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
          </tr>
          <tr>
            <td>نقشه مزار و مسیریابی</td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
            <td>
              <FaCheck className="mx-auto text-green-600" />
            </td>
          </tr>
        </table>
        </div>
      </div>
 
    </div>
    </div>
  );
};

export default MartyrPackages;
