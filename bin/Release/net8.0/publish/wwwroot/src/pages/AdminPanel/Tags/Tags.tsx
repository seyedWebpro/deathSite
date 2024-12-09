import Layout from "../../../layouts/adminPanel";
import Title from "../../../components/modules/title/Title";
import { FaTrash } from "react-icons/fa";
import { Button } from "../../../components/shadcn/ui/button";

const Tags = () => {
  return (
    <Layout>
      <Title className="sm:justify-center" title={"+ افزودن برچسب جدید "} />
      <div className="flex gap-2">
        <input
          type="text"
          placeholder="شهید..."
          className="w-[300px] rounded-md border border-black px-4 py-2 sm:w-full"
        />
        <Button variant={"main"}>افزودن</Button>
      </div>
      <div className="mt-8">
        <Title className="sm:justify-center" title={"برچسب ها"} />
        <div className="[&>*]:bg-DoubleSpanishWhite flex flex-wrap gap-3 sm:justify-center [&>*]:flex [&>*]:items-center [&>*]:gap-2 [&>*]:rounded-md [&>*]:px-3 [&>*]:py-2">
          <p>
            شهید
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            جانباز
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            شهید
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            جانباز
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            شهید
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            جانباز
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            شهید
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            جانباز
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            شهید
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            جانباز
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            شهید
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            جانباز
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            شهید
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            جانباز
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            شهید
            <FaTrash className="cursor-pointer text-xs" />
          </p>
          <p>
            جانباز
            <FaTrash className="cursor-pointer text-xs" />
          </p>
        </div>
      </div>
    </Layout>
  );
};

export default Tags;
