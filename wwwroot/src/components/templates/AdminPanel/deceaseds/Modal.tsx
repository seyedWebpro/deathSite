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
        <Button variant={"default"}>مشاهده</Button>
      </DialogTrigger>
      <DialogContent className="w-3/4 max-w-full sm:max-w-full">
        <DialogHeader>
          <DialogTitle className="flex items-center justify-end gap-2 py-3">
            متوفی 11228222
          </DialogTitle>
        </DialogHeader>

        <main className="h-[550px] overflow-y-scroll pr-4">
          <div className="flex justify-evenly gap-7" dir="rtl">
            <div>
              <p className="mb-5 font-bold">اطلاعات</p>
              <div className="grid grid-cols-[1fr,1fr] gap-3">
                <div>
                  <p className="text-center">نام و نام خانوادگی:</p>
                  <p className="mt-3 text-center text-sm text-gray-500">
                    مرتضی آوینی
                  </p>
                </div>
                <div>
                  <p className="text-center">محل شهادت:</p>
                  <p className="mt-3 text-center text-sm text-gray-500">فکه </p>
                </div>
                <div>
                  <p className="text-center">تاریخ شهادت:</p>
                  <p className="mt-3 text-center text-sm text-gray-500">
                    1372/1/20
                  </p>
                </div>
                <div>
                  <p className="text-center">تاریخ تولد:</p>
                  <p className="mt-3 text-center text-sm text-gray-500">
                    1326/5/21
                  </p>
                </div>
                <div>
                  <p className="text-center">سن:</p>
                  <p className="mt-3 text-center text-sm text-gray-500">
                    45 سال
                  </p>
                </div>
              </div>
            </div>
            <div>
              <p className="mb-5 font-bold">عکس ها</p>
              <div className="grid grid-cols-[1fr,1fr] gap-3">
                <img
                  src="https://upload.wikimedia.org/wikipedia/fa/thumb/f/f9/Avinisajed.jpg/330px-Avinisajed.jpg"
                  className="w-28 rounded-lg"
                  alt=""
                />
                <img
                  src="https://upload.wikimedia.org/wikipedia/fa/thumb/f/f9/Avinisajed.jpg/330px-Avinisajed.jpg"
                  className="w-28 rounded-lg"
                  alt=""
                />
                <img
                  src="https://upload.wikimedia.org/wikipedia/fa/thumb/f/f9/Avinisajed.jpg/330px-Avinisajed.jpg"
                  className="w-28 rounded-lg"
                  alt=""
                />
                <img
                  src="https://upload.wikimedia.org/wikipedia/fa/thumb/f/f9/Avinisajed.jpg/330px-Avinisajed.jpg"
                  className="w-28 rounded-lg"
                  alt=""
                />
                <img
                  src="https://upload.wikimedia.org/wikipedia/fa/thumb/f/f9/Avinisajed.jpg/330px-Avinisajed.jpg"
                  className="w-28 rounded-lg"
                  alt=""
                />
              </div>
            </div>
            <div>
              <p className="mb-5 font-bold">ویس ها</p>
              <div className="grid grid-cols-[1fr,1fr] gap-5">
                <div className="flex items-center gap-5">
                  <p> ویس 1 </p>
                  <div className="flex items-center justify-center rounded-full bg-DoubleSpanishWhite p-4">
                    <FaPlay className="cursor-pointer" />
                  </div>
                </div>
                <div className="flex items-center gap-5">
                  <p>ویس 2</p>
                  <div className="flex items-center justify-center rounded-full bg-DoubleSpanishWhite p-4">
                    <FaPlay className="cursor-pointer" />
                  </div>
                </div>
                <div className="flex items-center gap-5">
                  <p> ویس 3 </p>
                  <div className="flex items-center justify-center rounded-full bg-DoubleSpanishWhite p-4">
                    <FaPlay className="cursor-pointer" />
                  </div>
                </div>
                <div className="flex items-center gap-5">
                  <p> ویس 4 </p>
                  <div className="flex items-center justify-center rounded-full bg-DoubleSpanishWhite p-4">
                    <FaPlay className="cursor-pointer" />
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div className="" dir="rtl">
            <p className="mb-5 font-bold">معرفی </p>
            <p className="leading-7">
              سید مرتضی آوینی (متولد 21 شهریور 1326 در شهر ری – شهادت 20 فروردین
              1372 در فکه) مستندساز، عکاس، روزنامه نگار، نویسنده و نظریه پرداز
              «سینمای اسلامی» ایرانی بود. وی در سال 1344 در دانشگاه تهران در
              رشته معماری تحصیل کرد. در طول انقلاب ایران، آوینی فعالیت هنری خود
              را به عنوان کارگردان فیلم های مستند آغاز کرد و از فیلمسازان برجسته
              جنگی به شمار می رفت. او بیش از 80 فیلم درباره جنگ ایران و عراق
              ساخت. به گفته اگنس دیویکتور، آوینی روش های اصلی فیلمبرداری را
              ابداع کرد و جنبه باطنی جنگ ایران و عراق را بر اساس اندیشه عرفانی
              شیعه به تصویر کشید.
            </p>
          </div>

          <div className="mt-4" dir="rtl">
            <p className="mb-5 font-bold">وصیت نامه </p>
            <p className="leading-7">
              زندگی زیباست، اما شهادت از آن زیباتر است؛ سلامت تن زیباست، اما
              پرنده‌ی عشق، تن را قفسی می‌بیند که در باغ نهاده باشند. و مگر نه
              آنکه گردن‌ها را باریک آفریده‌اند، تا در مقتل کربلای عشق، آسانتر
              بریده شوند. و مگر نه آن‌که از پسر آدم، عهدی ازلی ستانده‌اند که
              حسین را از سر خویش، بیشتر دوست داشته باشد. و مگر نه آن‌که خانه تن،
              راه فرسودگی می‌پیماید تا خانه روح، آباد شود. و مگر این عاشق
              بی‌قرار را بر این سفینه سرگردان آسمانی، که کره‌ی زمین باشد، برای
              ماندن در اصطبل خواب و خور آفریده‌اند. و مگر از درون این خاک، اگر
              نردبانی به آسمان نباشد، جز کرم‌هایی فربه و تن‌پرور برمی‌آید. ای
              شهید، ای آن‌که بر کرانه‌ی ازلی و ابدی وجود بر نشسته‌ای، دستی برار
              و ما قبرستان نشینان عادات سخیف را نیز، از این منجلاب بیرون کش".
            </p>
          </div>
        </main>
      </DialogContent>
    </Dialog>
  );
};

export default Modal;
