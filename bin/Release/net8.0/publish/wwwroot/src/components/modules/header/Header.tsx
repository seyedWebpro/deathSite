import { Link } from "react-router-dom";

type Props = {};

const Header = (_props: Props) => {
  return (
    <header
      dir="rtl"
      className="flex w-full flex-col items-center pl-[86px] pt-12 sm:!pl-4 lg:pl-7 lg:pt-3.5"
    >
      <div className="flex w-full items-center justify-between">
        <Link
          to={"/"}
          className="logo flex items-center gap-2 rounded-[692.1px_0_0_692.1px] bg-white py-1 pl-14 pr-10 sm:!px-4 lg:rounded-[516.04px_0_0_516.04px] lg:p-2 lg:!py-1 lg:pl-10 lg:pr-7"
        >
          <img
            src="/images/logo.svg"
            alt="logo"
            className="lg:h-[34px] lg:w-[79px]"
          />
        </Link>
        <div className="block text-[37.11px] text-[#381B00] lg:hidden">
          بِسْمِ اللّهِ الرَّحْمَنِ الرَّحيمِ
        </div>
        <Link
          to="/martyr/packages"
          className=" relative rounded-[66.03px] border border-[#381B00] bg-transparent px-11 pb-2 pt-2 text-[19.52px] text-[#381B00] lg:rounded-[37.18px] lg:px-6 lg:pb-2 lg:pt-2 lg:text-[14px]"
        >
          ثبت خاطرات شما
        </Link>
      </div>
      <div className="hidden w-full items-center justify-center pr-7 lg:flex">
        <div className="mt-2 text-[21.09px] text-[#381B00] lg:mt-4">
          بِسْمِ اللّهِ الرَّحْمَنِ الرَّحيمِ
        </div>
      </div>
    </header>
  );
};

export default Header;
