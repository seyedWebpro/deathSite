import { Button } from "../../../shadcn/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "../../../shadcn/ui/dialog";
import { MdModeEdit } from "react-icons/md";

type Props = {
  edit: boolean;
};

const Modal = (props: Props) => {
  return (
    <Dialog>
      <DialogTrigger asChild>
        {props.edit ? (
          <MdModeEdit className="absolute left-8 top-0 cursor-pointer text-xl text-orange-600" />
        ) : (
          <p className="my-5 cursor-pointer !text-2xl sm:text-center">
            افزودن پکیج جدید +
          </p>
        )}
      </DialogTrigger>
      <DialogContent className="w-full lg:!max-w-[95%] max-w-[825px] !px-0 overflow-hidden sm:max-w-full">
        <DialogHeader>
          <DialogTitle className="flex items-center px-6 justify-end gap-2 py-3">
            {props.edit ? "ویرایش پکیج طلایی" : "پکیج جدید"}
          </DialogTitle>
        </DialogHeader>

    <div className="overflow-y-scroll px-4 h-[500px] sm:h-[400px]">
    <div dir="rtl" className="mt-5 grid md:grid-cols-[1fr,1fr] sm:[&>*]:text-sm sm:!grid-cols-[1fr] grid-cols-[1fr,1fr,1fr] gap-2 ">
          <div className="">
            <label className="block w-full text-center" htmlFor="">
              نام پکیج
            </label>
            <input
              className="mb-8 w-full border-b-2 border-black p-2 outline-none"
              type="text"
            />
          </div>
          <div>
            <label className="block w-full text-center" htmlFor="">
              قیمت پکیج
            </label>
            <input
              className="mb-8 w-full border-b-2 border-black p-2 outline-none"
              type="number"
            />
          </div>
          <div>
            <label className="block w-full text-center" htmlFor="">
              مبلغ تمدید دوره ای
            </label>
            <input
              className="mb-8 w-full border-b-2 border-black p-2 outline-none"
              type="text"
            />
          </div>
          <div>
            <label className="block w-full text-center" htmlFor="">
              مدت زمان اعتبار به روز
            </label>
            <input
              className="mb-8 w-full border-b-2 border-black p-2 outline-none"
              type="text"
            />
          </div>
          <div>
            <label className="block w-full text-center" htmlFor="">
              تعداد تصاویر
            </label>
            <input
              className="mb-8 w-full border-b-2 border-black p-2 outline-none"
              type="text"
            />
          </div>
          <div>
            <label className="block w-full text-center" htmlFor="">
              تعداد ویدیو
            </label>
            <input
              className="mb-8 w-full border-b-2 border-black p-2 outline-none"
              type="text"
            />
          </div>
          <div>
            <label className="block w-full text-center" htmlFor="">
              تعداد اعالمیه
            </label>
            <input
              className="mb-8 w-full border-b-2 border-black p-2 outline-none"
              type="text"
            />
          </div>
          <div>
            <label className="block w-full text-center" htmlFor="">
              تعداد مجاز فایل صوتی افراد
            </label>
            <input
              className="mb-8 w-full border-b-2 border-black p-2 outline-none"
              type="text"
            />
          </div>
        </div>

        <div dir="rtl" className="mt-5 grid md:grid-cols-[1fr,1fr] grid-cols-[1fr,1fr,1fr] md:gap-5 sm:[&>*]:text-sm gap-2">
          <div className="flex items-baseline justify-center gap-2">
            <label className="block w-full text-center" htmlFor="">
              بارکد ایجاد شود
            </label>
            <input
              className="mb-8 sm:!w-auto w-full border-b-2 border-black p-2 outline-none"
              type="checkbox"
            />
          </div>

          <div className="flex items-baseline justify-center gap-2">
            <label className="block w-full text-center" htmlFor="">
              نمایش
            </label>
            <input
              className="mb-8 sm:!w-auto w-full border-b-2 border-black p-2 outline-none"
              type="checkbox"
            />
          </div>

          <div className="flex items-baseline justify-center gap-2">
            <label className="block w-full text-center" htmlFor="">
              قابلیت انتخاب قالب
            </label>
            <input
              className="mb-8 sm:!w-auto w-full border-b-2 border-black p-2 outline-none"
              type="checkbox"
            />
          </div>
          <div className="flex items-baseline justify-center gap-2">
            <label className="block w-full text-center" htmlFor="">
              قابلیت قرادادن پیام تسلیت
            </label>
            <input
              className="mb-8 sm:!w-auto w-full border-b-2 border-black p-2 outline-none"
              type="checkbox"
            />
          </div>
          <div className="flex items-baseline justify-center gap-2">
            <label className="block w-full text-center" htmlFor="">
              قابلیت ارسال محتوا از سوی بازدید کننده جهت تغییر
            </label>
            <input
              className="mb-8 sm:!w-auto w-full border-b-2 border-black p-2 outline-none"
              type="checkbox"
            />
          </div>
          <div className="flex items-baseline justify-center gap-2">
            <label className="block w-full text-center" htmlFor="">
              قابلیت لوکیشین و مسیریابی
            </label>
            <input
              className="mb-8 sm:!w-auto w-full border-b-2 border-black p-2 outline-none"
              type="checkbox"
            />
          </div>
          <div className="flex items-baseline justify-center gap-2">
            <label className="block w-full text-center" htmlFor="">
              قابلیت به اشتراک گذاری
            </label>
            <input
              className="mb-8 sm:!w-auto w-full border-b-2 border-black p-2 outline-none"
              type="checkbox"
            />
          </div>
          <div className="flex items-baseline justify-center gap-2">
            <label className="block w-full text-center" htmlFor="">
              مجوز قراردادن فایل 360درجه
            </label>
            <input
              className="mb-8 sm:!w-auto w-full border-b-2 border-black p-2 outline-none"
              type="checkbox"
            />
          </div>
          <div className="flex items-baseline justify-center gap-2">
            <label className="block w-full text-center" htmlFor="">
              قالبیت به روز رسانی
            </label>
            <input
              className="mb-8 sm:!w-auto w-full border-b-2 border-black p-2 outline-none"
              type="checkbox"
            />
          </div>
        </div>

        <div className="mt-5"> 
          <Button className="mt-5 w-full mb-3" variant={"main"}>
            ثبت
          </Button>
        </div>
    </div>
      </DialogContent>
    </Dialog>
  );
};

export default Modal;
