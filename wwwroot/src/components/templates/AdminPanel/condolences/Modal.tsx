import { FaPlay } from "react-icons/fa";
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
        <Button variant={"default"} className="sm:p-2">مشاهده</Button>
      </DialogTrigger>
      <DialogContent className="w-full max-w-[425px] sm:max-w-full"> 
        <DialogHeader>
          <DialogTitle className="flex items-center justify-end gap-2 py-3">
            پیام تسلیت 11228222
          </DialogTitle>
        </DialogHeader>
    <p className="text-center">خدا همه رفتگانتون رو بیامرزه</p>
      </DialogContent>
    </Dialog>
  );
};

export default Modal;
