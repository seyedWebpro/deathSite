import Header from "../../../../components/modules/header/Header";
 import { Button } from "../../../../components/shadcn/ui/button";
import { Link } from "react-router-dom";
const Step3 = () => { 
  return (
    <div>
      <Header />
      <div className="flex min-h-screen flex-col">
        <div
          className={`mt-20 flex-grow rounded-t-[120px] bg-white px-24 pb-0 pt-20 xs:!px-4 sm:pt-10 md:px-10`}
        >
          <div className="mx-auto w-[730px] md:w-full md:px-4">
            <p className="text-center text-xl">
              لطفا مشخصات فردی را وارد نمایید
            </p>
            <div className="mt-10 flex justify-between gap-7 sm:flex-col-reverse">
          
              <div className="md:w-full" dir="rtl">
                <p className="text-right text-blue-500 md:text-sm">
                  شماره همراه        *
                </p>
                <input
                  type="number"
                  placeholder="09045417084"
                  className="border-b border-black bg-transparent px-2 py-2 outline-none md:w-full"
                />
              </div>
              <div className="md:w-full" dir="rtl">
                <p className="text-right text-blue-500 md:text-sm">
                  نام و نام خانوادگی شما*
                </p>
                <input
                  type="text"
                  placeholder="  رحیمی..."
                  className="border-b border-black bg-transparent px-2 py-2 outline-none md:w-full"
                />
              </div>
              <div className="md:w-full" dir="rtl">
                <p className="text-right text-blue-500 md:text-sm">
                  نام شما      *
                </p>
                <input
                  type="text"
                  placeholder="حسین  ..."
                  className="border-b border-black bg-transparent px-2 py-2 outline-none md:w-full"
                />
              </div>
            </div>

            <div className="w-full text-center">
          
              <div className="mt-10 flex w-full justify-between">
                <Link to={"/martyr/packages/Invoice"}>
                  <Button className="px-7" variant="main">
                 تایید شماره تلفن همراه
                  </Button>
                </Link>
                <Link to={"/martyr/packages/step2"}>
                  <Button variant={"link"} className="px-7 hover:!no-underline">
                    مرحله قبل
                  </Button>
                </Link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Step3;
