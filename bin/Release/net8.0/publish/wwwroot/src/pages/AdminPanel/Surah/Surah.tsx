import Layout from "../../../layouts/adminPanel";
import Title from "../../../components/modules/title/Title";
import { FaPlay, FaTrash } from "react-icons/fa";
import { Button } from "../../../components/shadcn/ui/button";

const Surah = () => {
  return (
    <Layout>
      <Title className="sm:justify-center" title={"+ افزودن سوره جدید "} />
      <div className="flex sm:flex-col items-center gap-2">
        <div className="sm:w-full">
          <p className="mb-2">نام سوره</p>
          <input
            type="text"
            className="w-[300px] lg:w-full rounded-md border border-black px-4 py-2.5 sm:w-full"
          />
        </div>

        <div className="sm:w-full">
          <p className="mb-2">فایل</p>
          <input
            type="file"
            className="w-[300px] lg:w-full rounded-md border border-black px-4 py-2 sm:w-full"
          />
        </div>
        <Button className="mt-8 sm:w-full" variant={"main"}>
          افزودن
        </Button>
      </div>
      <div className="mt-8">
        <Title className="sm:justify-center" title={"سوره ها"} />
        <div className="grid md:!grid-cols-[1fr] lg:grid-cols-[1fr,1fr] grid-cols-[1fr,1fr,1fr] gap-10">
          <div className="flex items-center justify-between p-5 shadow-xl">
            <div className="flex items-center gap-5">
              <p>سوره آل عمران</p>
              <div className="bg-DoubleSpanishWhite flex items-center justify-center rounded-full p-4">
                <FaPlay className="cursor-pointer"/>
              </div>
            </div>
            <FaTrash className="cursor-pointer" />
          </div>
          <div className="flex items-center justify-between gap-5 p-5 shadow-xl">
            <div className="flex items-center gap-5">
              <p>سوره آل عمران</p>
              <div className="bg-DoubleSpanishWhite flex items-center justify-center rounded-full p-4">
                <FaPlay className="cursor-pointer"/>
              </div>
            </div>
            <FaTrash className="cursor-pointer" />
          </div>
          <div className="flex items-center justify-between gap-5 p-5 shadow-xl">
            <div className="flex items-center gap-5">
              <p>سوره آل عمران</p>
              <div className="bg-DoubleSpanishWhite flex items-center justify-center rounded-full p-4">
                <FaPlay className="cursor-pointer"/>
              </div>
            </div>
            <FaTrash className="cursor-pointer" />
          </div>
        </div>
      </div>
    </Layout>
  );
};

export default Surah;
