import { MdOutlineFileUpload } from "react-icons/md";
import { Button } from "../../../shadcn/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "../../../shadcn/ui/dialog";
type Props = {
  bannershow?: boolean;
 bannerNews?: boolean;
};

const Modal = (props: Props) => {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant={"default"}>
          {props.bannerNews ? "+ بنر جدید " : props.bannershow ? "مشاهده" : "ویرایش"}
        </Button>
      </DialogTrigger>
      <DialogContent className="w-full max-w-[425px] sm:max-w-full">
        <DialogHeader>
          <DialogTitle className="flex items-center justify-end gap-2 py-3">
            { props.bannerNews ? 'بنر جدید': props.bannershow
              ? "  مشاهده بنر 1122811228"
              : "  ویرایش بنر 1122811228"}
          </DialogTitle>
        </DialogHeader>

        <div>
          {props.bannerNews ? (
            <>
              <div className="mt-5">
                <label className="block w-full text-center" htmlFor="">
                  تیتر
                </label>
                <input
                  className="mb-8 w-full border-b-2 border-black p-2 outline-none"
                  type="text"
                />
              </div>
              <div>
                <label className="block w-full text-center" htmlFor="">
                  متن
                </label>
                <input
                  className="mb-8 w-full border-b-2 border-black p-2 outline-none"
                  type="text"
                />
              </div>
              <div>
                <label className="block w-full text-center" htmlFor="">
                  کاور
                </label>
                <div className="relative flex items-center justify-center gap-2 p-8 text-center">
                  <MdOutlineFileUpload className="text-3xl" />
                  <p className="my-4">عکس بنر را انتخاب کنید</p>
                  <input
                    className="absolute left-0 top-0 h-full w-full opacity-0"
                    type="file"
                    name=""
                    id=""
                  />
                </div>
              </div>
              <Button className="w-full" variant={"main"}>
                ثبت
              </Button>
            </>
          ) : props.bannershow ? (
            <p className="text-center">به راحتی </p>
          ) : (
            <>
              <div className="mt-5">
                <label className="block w-full text-center" htmlFor="">
                  تیتر
                </label>
                <input
                  className="mb-8 w-full border-b-2 border-black p-2 outline-none"
                  type="text"
                />
              </div>
              <div>
                <label className="block w-full text-center" htmlFor="">
                  متن
                </label>
                <input
                  className="mb-8 w-full border-b-2 border-black p-2 outline-none"
                  type="text"
                />
              </div>
              <div>
                <label className="block w-full text-center" htmlFor="">
                  کاور
                </label>
                <div className="relative flex items-center justify-center gap-2 p-8 text-center">
                  <MdOutlineFileUpload className="text-3xl" />
                  <p className="my-4">عکس جدید را انتخاب کنید</p>
                  <input
                    className="absolute left-0 top-0 h-full w-full opacity-0"
                    type="file"
                    name=""
                    id=""
                  />
                </div>
              </div>
              <Button className="w-full" variant={"main"}>
                ثبت
              </Button>
            </>
          )}
        </div>
      </DialogContent>
    </Dialog>
  );
};

export default Modal;
