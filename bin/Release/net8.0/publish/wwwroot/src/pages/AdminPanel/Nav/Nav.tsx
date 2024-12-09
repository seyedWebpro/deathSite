import Layout from "../../../layouts/adminPanel";
import Title from "../../../components/modules/title/Title";
import { MdOutlineFileUpload } from "react-icons/md";

const Nav = () => {
  return (
    <Layout>
      <Title className="sm:justify-center" title={"تغییر لوگو "} />

      <div className="flex gap-10 justify-center items-center">
        <div className="text-center border-2 p-10 relative border-dashed rounded-2xl w-max">
            <MdOutlineFileUpload className="mx-auto text-3xl"/>
            <p className="my-4">عکس جدید را انتخاب کنید</p>
             <input className=" absolute top-0 left-0 w-full h-full opacity-0" type="file" name="" id="" />
        </div>
        <img src="/images/logo.svg" alt="logo" className="lg:h-[34px] lg:w-[79px]"/>
      </div>

      <Title className="sm:justify-center" title={"تغییر لینک "} />
      
    </Layout>
  );
};

export default Nav;
