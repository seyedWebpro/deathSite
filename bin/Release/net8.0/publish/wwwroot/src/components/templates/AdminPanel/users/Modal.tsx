import { Button } from "../../../shadcn/ui/button";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "../../../shadcn/ui/dialog";

type Props = {};

const Modal = (props: Props) => {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant={"default"}>ویرایش</Button>
      </DialogTrigger>
      <DialogContent className="w-full max-w-[425px] sm:max-w-full">
        <DialogHeader>
          <DialogTitle className="flex items-center justify-end gap-2 py-3">
            ویرایش کاربر 1122811228
          </DialogTitle>
        </DialogHeader>

        <div>
         <div className="mt-5">
         <label className="text-center w-full block" htmlFor="">نام کاربری</label>
            <input className="border-b-2 outline-none border-black p-2 w-full mb-8" type="text" />
         </div>
         <div>
         <label className="text-center w-full block" htmlFor="">شماره موبایل</label>
            <input className="border-b-2 outline-none border-black p-2 w-full mb-8" type="text" />
         </div>
         <div>
         <label className="text-center w-full block" htmlFor="">پسورد</label>
            <input className="border-b-2 outline-none border-black p-2 w-full mb-8" type="text" />
         </div>
         <Button className="w-full" variant={"main"}>ثبت</Button>
        </div>

      </DialogContent>
    </Dialog>
  );
};

export default Modal;
