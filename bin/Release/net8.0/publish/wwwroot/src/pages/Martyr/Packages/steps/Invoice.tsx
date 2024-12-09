import Header from "../../../../components/modules/header/Header";
import { Button } from "../../../../components/shadcn/ui/button";

const Invoice = () => {
  return (
    <div>
      <Header />
      <div className="flex min-h-screen flex-col">
        <div
          className={`mt-20 flex-grow rounded-t-[120px] bg-white px-24 pb-0 pt-20 xs:!px-4 sm:pt-10 md:px-10`}
        >
          <div className="text-center">
            <p className="mb-10 text-xl">پیش فاکتور</p>

            <div
              dir="rtl"
              className={`flex-grow rounded-t-[120px] bg-white px-24 pb-0 xs:!px-4 md:px-10`}
            >
              <div className="mx-auto w-1/3 xl:w-full overflow-x-auto rounded-xl bg-[#eae9e991] px-4 py-4 sm:p-2">
                <table
                  className="invoice_table mx-auto w-full rounded-lg bg-[#fffbfb30] sm:w-full [&>*]:text-center sm:[&>*]:text-sm"
                  dir="rtl"
                >
                  <tr>
                    <td> شماره پیش فاکتور</td>
                    <td>6591 </td>
                  </tr>
                  <tr>
                    <td> نام پرداخت کننده </td>
                    <td> حسین رحیمی </td>
                  </tr>
                  <tr>
                    <td> تاریخ </td>
                    <td> 1403/05/05</td>
                  </tr>
                  <tr>
                    <td>زمان</td>
                    <td> 17:01:30</td>
                  </tr>
                  <tr>
                    <td>مبلغ </td>
                    <td> 199000 تومان </td>
                  </tr>
                </table>
              </div>
              <Button variant={"main"} className="mx-auto mt-5 px-6">
                پرداخت
              </Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Invoice;
