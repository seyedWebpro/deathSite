import Layout from "../../../layouts/adminPanel";
import Title from "../../../components/modules/title/Title";
import DataTable from "react-data-table-component";
import { Button } from "../../../components/shadcn/ui/button";
import { GoDownload } from "react-icons/go";

const Price = () => {
  const columns = [
    {
      name: "کاربر ",
      selector: (row: { user: string }) => row.user,
    },
    {
      name: "مبلغ",
      sortable: true,
      selector: (row: { price: string }) => row.price,
    },
    {
      name: "تاریخ ",
      selector: (row: { date: string }) => row.date,
    },
    {
      name: "وضعیت",
      selector: (row: { status: string }) => row.status,
    },
    {
      name: "نوع",
      sortable: true,
      selector: (row: { type: string }) => row.type,
    },
  ];

  const data = [
    {
      id: 1,
      user: "شاهین مشکل گشا",
      price: "12233434 تومان",
      date: "1403/05/01",
      status: "موفق",
      type: "ارتقا",
    },
    {
      id: 2, 
      user: "شاهین مشکل گشا",
      price: "12233434 تومان",
      date: "1403/05/01",
      status: "ناموفق",
      type: "تمدید",
    },
    {
      id: 3,
      user: "شاهین مشکل گشا",
      price: "12233434 تومان",
      date: "1403/05/01",
      status: "موفق",
      type: "تمدید",
    },
    {
      id: 4,
      user: "شاهین مشکل گشا",
      price: "12233434 تومان",
      date: "1403/05/01",
      status: "ناموفق",
      type: "تمدید",
    },
    {
      id: 5,
      user: "شاهین مشکل گشا",
      price: "12233434 تومان",
      date: "1403/05/01",
      status: "موفق",
      type: "ثبت نام",
    },
    {
      id: 6,
      user: "شاهین مشکل گشا",
      price: "12233434 تومان",
      date: "1403/05/01",
      status: "موفق",
      type: "تمدید",
    },
    {
      id: 7,
      user: "شاهین مشکل گشا",
      price: "12233434 تومان",
      date: "1403/05/01",
      status: "موفق",
      type: "ارتقا",
    },
    {
      id: 8,
      user: "شاهین مشکل گشا",
      price: "12233434 تومان",
      date: "1403/05/01",
      status: "موفق",
      type: "تمدید",
    },
  ];

  const exportCSV = (data: any[], columns: any[]) => {
    const headers = columns.map((col) => col.name).join(",");
    const rows = data.map((row) =>
      columns.map((col) => col.selector(row)).join(","),
    );
    const csvContent = [headers, ...rows].join("\n");

    const blob = new Blob(["\uFEFF" + csvContent], {
      type: "text/csv;charset=utf-8;",
    });
    const url = URL.createObjectURL(blob);

    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", "export.csv");
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };
  return (
    <Layout>
      <Title className="sm:justify-center" title={"پرداخت ها"} />

      <div className="mb-4 mr-auto block w-max sm:ml-auto">
        <Button
          className="px-10 sm:px-4"
          onClick={() => exportCSV(data, columns)}
        >
          اکسپورت
          <GoDownload />
        </Button>
      </div>

      <div>
        <DataTable
          responsive
          progressComponent={".... "}
          pagination
          columns={columns}
          data={data}
        />
      </div>
    </Layout>
  );
};

export default Price;
