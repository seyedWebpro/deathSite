import Layout from "../../../layouts/adminPanel";
import DataTable, { TableColumn } from "react-data-table-component";
import Title from "../../../components/modules/title/Title";
import { Button } from "../../../components/shadcn/ui/button";
import Modal from "../../../components/templates/AdminPanel/users/Modal";
import { useState } from "react";
import { GoDownload } from "react-icons/go";

interface UsersData {
  id: number;
  name: string;
  phone: any;
  date: any;
  edit:any;
  delete: any; 
}

const Users = () => {
  const [filterText, setFilterText] = useState("");

  const columns: TableColumn<UsersData>[]= [

    {
      name: "نام کاربری",
      selector: (row: { name: string }) => row.name,
    },
    {
      name: "شماره موبایل",
      selector: (row: { phone: string }) => row.phone,
    },
    {
      name: "تاریخ عضویت",
      selector: (row: { date: string }) => row.date,
    },
    {
      name: "ویرایش",
      selector: (row: { edit: string }) => row.edit,
    },
    {
      name: "حذف",
      selector: (row: { delete: string }) => row.delete,
    },
  ];

  const data = [
    {
      id: 1,
      name: "شاهین مشکل گشا",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 2,
      name: "رضا مرادی",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 3,
      name: "مریم مشکل گشا",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 4,
      name: "محمد زارع",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 5,
      name: "فریدون شهریاری",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 6,
      name: "نفیسه کیوانی",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 7,
      name: "دنیا رضایی",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 8,
      name: "شهرام مرادی",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 9,
      name: "کریم زراعتی",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 10,
      name: "رها سلطانی",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 10,
      name: "شاهرخ ماهانی",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 10,
      name: "سیاوش باقری",
      phone: "09046417084",
      date: "1403/05/01",
      edit: <Modal />,
      delete: <Button variant={"danger"}>حذف</Button>,
    },
  ];

  const filteredData = data.filter((item) => item.name.includes(filterText));

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
      <Title className="sm:justify-center" title={"مدیریت کاربران"} />
      <div className="mb-4">
        <input
          type="text" 
          placeholder="جستجوی نام کاربری"
          value={filterText}
          onChange={(e) => setFilterText(e.target.value)}
          className="mb-5 mt-4 w-[300px] rounded-md border-b border-black p-2 outline-none sm:w-full"
        />
      </div>

      <div className="mb-4 mr-auto block sm:ml-auto w-max">
        <Button className="px-10 sm:px-4" onClick={() => exportCSV(filteredData, columns)}>
         اکسپورت
         <GoDownload/>
        </Button>
      </div>

      <div>
        <DataTable
          responsive
          pagination
          columns={columns}
          data={filteredData}
          noDataComponent={
            <div className="text-2xl">کاربری با این اسم پیدا نشد.</div>
          }
        />
      </div>
    </Layout>
  );
};

export default Users;
