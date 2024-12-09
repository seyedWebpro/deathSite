import jalaali from "jalaali-js";

export const saveIntoLocalStorage = (key: string, value: string | {}) => {
  if (typeof window !== "undefined" && localStorage) {
    return localStorage.setItem(key, JSON.stringify(value));
  }
};

export const getFromLocalStorage = (key: string) => {
  if (typeof window !== "undefined" && localStorage) {
    return JSON.parse(localStorage.getItem(key) as string);
  }
};

export function convertToJalali(date: string) {
  const newDate = new Date(date);
  const jalaaliDate = jalaali.toJalaali(
    newDate.getFullYear(),
    newDate.getMonth() + 1,
    newDate.getDate(),
  );

  const months = [
    "فروردین",
    "اردیبهشت",
    "خرداد",
    "تیر",
    "مرداد",
    "شهریور",
    "مهر",
    "آبان",
    "آذر",
    "دی",
    "بهمن",
    "اسفند",
  ];

  const day = jalaaliDate.jd;
  const month = months[jalaaliDate.jm - 1];
  const year = jalaaliDate.jy;

  return `${day} ${month} ${year}`;
}

export function getJalaliDateInfo(jalaliDate: string) {
  const [year, month, day] = jalaliDate.split("/").map(Number);
  const gregorianDate = jalaali.toGregorian(year, month, day);
  const date = new Date(
    gregorianDate.gy,
    gregorianDate.gm - 1,
    gregorianDate.gd,
  );

  const daysOfWeek = [
    "یکشنبه",
    "دوشنبه",
    "سه‌شنبه",
    "چهارشنبه",
    "پنج‌شنبه",
    "جمعه",
    "شنبه",
  ];

  const months = [
    "فروردین",
    "اردیبهشت",
    "خرداد",
    "تیر",
    "مرداد",
    "شهریور",
    "مهر",
    "آبان",
    "آذر",
    "دی",
    "بهمن",
    "اسفند",
  ];

  const dayOfWeek = daysOfWeek[date.getDay()];
  const monthName = months[month - 1];

  return {
    dayOfWeek,
    monthName,
    day,
    year,
  };
}

export const formatNumber = (num: string) => {
  return num.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
};
 