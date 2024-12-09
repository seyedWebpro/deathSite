import { useState } from "react";
import Header from "../../../components/modules/header/Header";
import { SlCalender } from "react-icons/sl";
import { IoCloudUploadOutline } from "react-icons/io5";

import DatePicker from "react-multi-date-picker";
import persian from "react-date-object/calendars/persian";
import persian_fa from "react-date-object/locales/persian_fa";
import { Button } from "../../../components/shadcn/ui/button";
import { FaPlus } from "react-icons/fa";

const MartyrRegister = () => {
  const [birthDate, setBirthDate] = useState("۱۳۰۳/۰۹/۰۶");
  const [deathDate, setDeathDate] = useState("۱۴۰۳/۰۹/۰۶");

  return (
    <div>
      <Header />
      <div className="register_martyr flex min-h-screen flex-col">
        <div
          className={`relative mt-20 flex-grow overflow-y-hidden rounded-t-[120px] bg-white px-24 pb-0 pt-20 text-right xs:!px-4 sm:pt-10 md:px-10`}
        >
          <div className="flex justify-center">
            <p className="relative -top-14 text-center text-2xl sm:-top-4">
              ثبت شهید
            </p>
            <img
              src="/images/02ecb2a6bbf8b10559ad6cdfbb4320d3_prev_ui.png"
              className="absolute -top-14 w-44"
              alt=""
            />
          </div>
          <p className="mb-7 text-xl sm:mt-20">اطلاعات اولیه</p>

          <div
            dir="rtl"
            className="grid grid-cols-[1fr,1fr,1fr,1fr] items-end gap-8 sm:!grid-cols-[1fr,1fr] md:grid-cols-[1fr,1fr,1fr]"
          >
            <div className="relative w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm">
                نام و نام خانوادگی
              </p>
              <input
                type="text"
                placeholder="حسین رحیمی"
                className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
              />
            </div>
            <div className="relative w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm">نام پدر</p>
              <input
                type="text"
                placeholder="علی "
                className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
              />
            </div>

            <div className="w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm">محل شهادت</p>
              <input
                type="text"
                placeholder="شلمچه  "
                className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
              />
            </div>
            <div className="relative w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm">علت شهادت</p>
              <input
                type="text"
                placeholder="مین"
                className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
              />
            </div>
            <div className="relative w-full">
              <p dir="rtl" className="mb-2 text-right text-blue-500 md:text-sm">
                تاریخ تولد
              </p>
              <DatePicker
                value={birthDate}
                onChange={setBirthDate as any}
                calendar={persian}
                locale={persian_fa}
                hideOnScroll
                editable={false}
                calendarPosition="bottom-left"
              />
              <SlCalender className="absolute bottom-2 left-2" />
            </div>
            <div className="relative w-full">
              <p dir="rtl" className="mb-2 text-right text-blue-500 md:text-sm">
                تاریخ وفات
              </p>
              <DatePicker
                value={deathDate}
                onChange={setDeathDate as any}
                calendar={persian}
                locale={persian_fa}
                hideOnScroll
                editable={false}
                calendarPosition="bottom-left"
              />
              <SlCalender className="absolute bottom-2 left-2" />
            </div>
          </div>

          <div className="mt-8 flex gap-10 md:flex-col">
            <div className="w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm">مسئولیت ها</p>
              <div className="flex items-end justify-between gap-4">
                <input
                  type="text"
                  placeholder="سردار  "
                  className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
                />
                <div className="rounded-lg bg-brown p-2 cursor-pointer text-white">
                  <FaPlus />
                </div>
              </div>
              <div className="mt-3 flex gap-2">
                <p className="rounded-md bg-[#e8d4ba] p-1 text-sm">سردار</p>
                <p className="rounded-md bg-[#e8d4ba] p-1 text-sm">بیسیمچی</p>
              </div>
            </div>

            <div className="w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm">عملیات ها</p>
              <div className="flex items-end justify-between gap-4">
                <input
                  type="text"
                  placeholder="ولفجر 3  "
                  className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
                />
                <div className="rounded-lg bg-brown p-2 cursor-pointer text-white">
                  <FaPlus />
                </div>
              </div>
              <div className="mt-3 flex gap-2">
                <p className="rounded-md bg-[#e8d4ba] p-1 text-sm">ولفجر 4</p>
                <p className="rounded-md bg-[#e8d4ba] p-1 text-sm">ولفجر 3</p>
              </div>
            </div>
          </div>

          <div>
            <p className="mb-4 mt-10 text-xl">زندگی نامه </p>
            <textarea
              name=""
              id=""
              placeholder="ایشون در تهران چشم به ..."
              dir="rtl"
              className="h-[200px] w-full resize-none rounded-lg border border-gray-400 p-3 outline-none"
            ></textarea>
            <p className="mb-4 mt-7 text-xl">وصیت نامه </p>
            <textarea
              name=""
              id=""
              placeholder=" به نام خداوند بخشنده و ..."
              dir="rtl"
              className="h-[200px] w-full resize-none rounded-lg border border-gray-400 p-3 outline-none"
            ></textarea>
            <p className="mb-4 mt-7 text-xl">خاطرات </p>
            <textarea
              name=""
              id=""
              placeholder=" به نام خداوند بخشنده و ..."
              dir="rtl"
              className="h-[200px] w-full resize-none rounded-lg border border-gray-400 p-3 outline-none"
            ></textarea>
          </div>

          <div>
            <p className="mb-4 mt-10 text-xl">تصاویر</p>
            <div
              dir="rtl"
              className="mb-10 grid md:grid-cols-[1fr,1fr,1fr] sm:!grid-cols-[1fr,1fr] grid-cols-[1fr,1fr,1fr,1fr] items-end gap-8"
            >
              <div className="relative rounded-lg border border-dashed border-gray-400 p-8 text-center">
                <p>بارگذاری تصویر</p>
                <IoCloudUploadOutline className="mx-auto block text-3xl" />
                <input
                  type="file"
                  className="absolute left-0 top-0 h-full w-full opacity-0"
                />
              </div>
              <img
                src="/images/abbas.png"
                className="h-[130px] w-full rounded-lg object-cover"
                alt=""
              />
              <img
                src="/images/abbas.png"
                className="h-[130px] w-full rounded-lg object-cover"
                alt=""
              />
              <img
                src="/images/abbas.png"
                className="h-[130px] w-full rounded-lg object-cover"
                alt=""
              />
              <img
                src="/images/abbas.png"
                className="h-[130px] w-full rounded-lg object-cover"
                alt=""
              />
              <img
                src="/images/abbas.png"
                className="h-[130px] w-full rounded-lg object-cover"
                alt=""
              />
            </div>
          </div>

          <div>
            <p className="mb-4 mt-10 text-xl">ویس ها</p>
            <div
              dir="rtl"
              className="mb-10 md:grid-cols-[1fr,1fr] sm:!grid-cols-[1fr] grid grid-cols-[1fr,1fr,1fr,1fr] items-center gap-8"
            >
              <div className="relative rounded-lg border border-dashed border-gray-400 p-8 text-center">
                <p>بارگذاری ویس ها</p>
                <IoCloudUploadOutline className="mx-auto block text-3xl" />
                <input
                  type="file"
                  className="absolute left-0 top-0 h-full w-full opacity-0"
                />
              </div>
              <audio
                className="w-full"
                controls
                src="/images/QURAN - 5 - 128 - mahanmusic.net.mp3"
              ></audio>
              <audio
                className="w-full"
                controls
                src="/images/QURAN - 5 - 128 - mahanmusic.net.mp3"
              ></audio>
              <audio
                className="w-full"
                controls
                src="/images/QURAN - 5 - 128 - mahanmusic.net.mp3"
              ></audio>
            </div>
          </div>

          <div>
            <p className="mb-4 mt-10 text-xl">ویدیو ها</p>
            <div
              dir="rtl"
              className="mb-10 grid md:grid-cols-[1fr,1fr,1fr] sm:!grid-cols-[1fr,1fr] grid-cols-[1fr,1fr,1fr,1fr] items-end gap-8"
            >
              <div className="relative flex h-full flex-col items-center justify-center rounded-lg border border-dashed border-gray-400 p-8 text-center">
                <p>بارگذاری ویدیو</p>
                <IoCloudUploadOutline className="mx-auto block text-3xl" />
                <input
                  type="file"
                  className="absolute left-0 top-0 h-full w-full opacity-0"
                />
              </div>
              <video
                controls
                className="h-[180px] w-full rounded-lg object-fill"
                src="/images/b768044b04e3cde64a93773dfce2930d55121669-480p.mp4"
              ></video>
              <video
                controls
                className="h-[180px] w-full rounded-lg object-fill"
                src="/images/b768044b04e3cde64a93773dfce2930d55121669-480p.mp4"
              ></video>
              <video
                controls
                className="h-[180px] w-full rounded-lg object-fill"
                src="/images/b768044b04e3cde64a93773dfce2930d55121669-480p.mp4"
              ></video>
            </div>
          </div>

          <p className="mb-7 text-xl">سایر اطلاعات</p>

          <div
            dir="rtl"
            className="grid grid-cols-[1fr,1fr,1fr,1fr] items-end gap-8 sm:!grid-cols-[1fr,1fr] md:grid-cols-[1fr,1fr,1fr]"
          >
            <div className="relative w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm">
                آخرین مسئولیت{" "}
              </p>
              <input
                type="text"
                placeholder="بیسیمچی "
                className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
              />
            </div>

            <div className="w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm"> گروه شهدا</p>
              <input
                type="text"
                placeholder="...  "
                className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
              />
            </div>

            <div className="w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm">یگان</p>
              <input
                type="text"
                placeholder="سوم"
                className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
              />
            </div>

            <div className="w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm">نیرو</p>
              <input
                type="text"
                placeholder="ارتش"
                className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
              />
            </div>

            <div className="w-full" dir="rtl">
              <p className="text-right text-blue-500 md:text-sm">قشر</p>
              <input
                type="text"
                placeholder="..."
                className="w-full border-b border-black bg-transparent px-2 py-2 outline-none"
              />
            </div>
          </div>

          <div>
            <p className="mb-4 mt-10 text-center text-xl">شعر</p>
            <div dir="rtl" className="flex justify-center gap-4 sm:flex-col">
              <input
                className="w-[250px] rounded-md border border-gray-400 px-3 pb-2 pt-1 outline-none sm:w-full"
                type="text"
                placeholder="مصرع اول"
              />
              <input
                className="w-[250px] rounded-md border border-gray-400 px-3 pb-2 pt-1 outline-none sm:w-full"
                type="text"
                placeholder="مصرع دوم"
              />
            </div>
          </div>
          <Button
            className="mx-auto mb-10 mt-16 block px-6 !pb-10 text-xl"
            variant={"main"}
          >
            ثبت شهید
          </Button>
        </div>
      </div>
    </div>
  );
};

export default MartyrRegister;
