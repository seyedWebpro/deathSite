 import Layout from "../../../layouts/adminPanel";
import Title from "../../../components/modules/title/Title";
import { GoDotFill } from "react-icons/go";
import Modal from "../../../components/templates/AdminPanel/packages/Modal";

const Packages = () => {
  return (
    <Layout>
      <div className="flex justify-between text-xl sm:justify-between sm:flex-col">
      <Title className="sm:justify-center" title={"پکیج ها"} />
      <Modal edit={false}/>

      </div>
      <div className="mt-8 text-xl grid md:grid-cols-[1fr,1fr] md:gap-10 sm:!gap-20 sm:!grid-cols-[1fr] grid-cols-[1fr,1fr,1fr] gap-5">
        <div className="relative p-4 text-center [&>*]:!text-xl">
          <p className="border-b border-black pb-5">پکیج طلایی</p>
          <p className="border-b border-black pb-5 pt-5">120 روزه</p>
          <p className="border-b border-black pb-5 pt-5">330000 هزار تومن</p>
          <div>
            <ul className="space-y-3 pt-6">
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
            </ul>
          </div>
          <Modal edit={true}/>
        </div>
        <div className="relative p-4 text-center [&>*]:!text-xl">
          <p className="border-b border-black pb-5">پکیج طلایی</p>
          <p className="border-b border-black pb-5 pt-5">120 روزه</p>
          <p className="border-b border-black pb-5 pt-5">330000 هزار تومن</p>
          <div>
            <ul className="space-y-3 pt-6">
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
            </ul>
          </div>
          <Modal edit={true}/>
        </div>
        <div className="relative p-4 text-center [&>*]:!text-xl">
          <p className="border-b border-black pb-5">پکیج طلایی</p>
          <p className="border-b border-black pb-5 pt-5">120 روزه</p>
          <p className="border-b border-black pb-5 pt-5">330000 هزار تومن</p>
          <div>
            <ul className="space-y-3 pt-6">
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
              <li className="flex items-center justify-center gap-2">
                <GoDotFill className="text-orange-600" /> امکان ثبت 200 عکس
              </li>
            </ul>
          </div>
          <Modal edit={true}/>
        </div>
      </div>

    </Layout>
  );
};

export default Packages;
