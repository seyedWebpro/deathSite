import Layout from "../../../layouts/adminPanel";
import Title from "../../../components/modules/title/Title";
import { LuPackagePlus } from "react-icons/lu";
import { MdOutlineNewLabel, MdOutlineArticle } from "react-icons/md";
import { AiOutlineMenu } from "react-icons/ai";
import { RiVoiceprintFill } from "react-icons/ri";
import { IoNewspaperOutline } from "react-icons/io5";
import { PiFlagBannerFold } from "react-icons/pi";
import { Link } from "react-router-dom";

const ContentManagement = () => {
  return (
    <Layout className="lg:px-6">
      <Title className="sm:justify-center" title={"مدیریت محتوا"} />

      <div className="grid sm:!grid-cols-[1fr,1fr] md:grid-cols-[1fr,1fr,1fr] grid-cols-[1fr,1fr,1fr,1fr] gap-4 mt-12">
        <Link
          to={"/adminPanel/contentManagement/packages"}
          className="cursor-pointer rounded-xl p-4 text-center shadow-lg transition-transform hover:-translate-y-2"
        >
          <LuPackagePlus className="mx-auto mb-7 text-4xl" />
          <p className="mb-4 !text-xl">پکیج ها</p>
        </Link>
        <Link
          to={"/adminPanel/contentManagement/tags"}
          className="cursor-pointer rounded-xl p-4 text-center shadow-lg transition-transform hover:-translate-y-2"
        >
          <MdOutlineNewLabel className="mx-auto mb-7 text-4xl" />
          <p className="mb-4 !text-xl">برچسب ها</p>
        </Link>
        <Link
          to={"/adminPanel/contentManagement/nav"}
          className="cursor-pointer rounded-xl p-4 text-center shadow-lg transition-transform hover:-translate-y-2"
        >
          <AiOutlineMenu className="mx-auto mb-7 text-4xl" />
          <p className="mb-4 !text-xl">منو و لوگو ها</p>
        </Link>
        <Link
          to={"/adminPanel/contentManagement/surah"}
          className="cursor-pointer rounded-xl p-4 text-center shadow-lg transition-transform hover:-translate-y-2"
        >
          <RiVoiceprintFill className="mx-auto mb-7 text-4xl" />
          <p className="mb-4 !text-xl">سوره ها</p>
        </Link>
        <Link
          to={"/adminPanel/contentManagement/news"}
          className="cursor-pointer rounded-xl p-4 text-center shadow-lg transition-transform hover:-translate-y-2"
        >
          <IoNewspaperOutline className="mx-auto mb-7 text-4xl" />
          <p className="mb-4 !text-xl">اخبار</p>
        </Link>
        <Link
          to={"/adminPanel/contentManagement/articles"}
          className="cursor-pointer rounded-xl p-4 text-center shadow-lg transition-transform hover:-translate-y-2"
        >
          <MdOutlineArticle className="mx-auto mb-7 text-4xl" />
          <p className="mb-4 !text-xl">مقلات</p>
        </Link>
        <Link
          to={"/adminPanel/contentManagement/banner"}
          className="cursor-pointer rounded-xl p-4 text-center shadow-lg transition-transform hover:-translate-y-2"
        >
          <PiFlagBannerFold className="mx-auto mb-7 text-4xl" />
          <p className="mb-4 !text-xl">بنر </p>
        </Link>
      </div>
    </Layout>
  );
};

export default ContentManagement;
