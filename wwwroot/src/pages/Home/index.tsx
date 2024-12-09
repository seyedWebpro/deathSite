import { Link } from "react-router-dom";

const index = () => {
  return (
    <div>
      <Link
        className="mx-auto mt-24 block w-max text-4xl"
        to={"/adminPanel/users"}
      >
        پنل ادمین{" "}
      </Link>
      <Link className="mx-auto mt-10 block w-max text-4xl" to={"/login"}>
        ورود{" "}
      </Link>
      <Link
        className="mx-auto mt-10 block w-max text-4xl"
        to={"/martyr/packages"}
      >
        ثبت پکیج{" "}
      </Link>
      <Link
        className="mx-auto mt-10 block w-max text-4xl"
        to={"/martyr/register"}
      >
        ثبت شهید{" "}
      </Link>
      <Link className="mx-auto mt-10 block w-max text-4xl" to={"/deceased/22"}>
        صفحه متوفی{" "}
      </Link>

      <p dir="rtl" className="mt-12 text-center">
        22 صفحه توسعه داده شده
      </p>
      <p className="text-center">
        ظواهر پنل ادمین، لاگین ریجستر، ثبت پکیج و ثبت شهید درست شده است
      </p>
    </div>
  );
};

export default index;
