import Layout from "../../../layouts/adminPanel";
import Title from "../../../components/modules/title/Title";
import { Button } from "../../../components/shadcn/ui/button";

const Sms = () => {
  return (
    <Layout>
      <div>
        <div className="mb-20">
          <Title className="sm:justify-center" title={" پیامک جدید   "} />

          <div className="flex items-end gap-3 sm:flex-col">
            <input
              placeholder="سلام..."
              type="text"
              className="w-[300px] border-b border-black p-3 outline-none sm:w-full"
            />
            <Button variant={"main"} className="sm:w-full !text-xl">
              ثبت
            </Button>
          </div>
        </div>

        <Title className="sm:justify-center" title={" پیامک فعلی   "} />

        <div className="flex justify-center  bg-DoubleSpanishWhite py-3 text-black">
          <p className="!text-2xl">سلام به سایت ما خوش امدید</p>
        </div>
      </div>
    </Layout>
  );
};

export default Sms;
