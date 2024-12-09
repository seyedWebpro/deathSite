import DataTable, { TableColumn } from "react-data-table-component";
import Title from "../../../components/modules/title/Title";
import Layout from "../../../layouts/adminPanel";
import { Button } from "../../../components/shadcn/ui/button";

interface BarcodeData {
  id: number;
  barcode: string;
  name: any; 
  delete: any;
}
const Barcode = () => {
  const columns: TableColumn<BarcodeData>[] = [
    {
      name: "بارکد ",
      selector: (row: { barcode: string }) => row.barcode,
    },
    {
      name: "مرحوم",
      selector: (row: { name: string }) => row.name,
    },
    {
      name: "حذف",
      selector: (row: { delete: string }) => row.delete,
    },
  ];

  const data = [
    {
      id: 1,
      barcode: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRends6tibfUPvxB-aovVdaEEwrIA0S1ckuKw&s" width={60} alt="" />,
      name: "عباس بابایی",
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 2,
      barcode: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRends6tibfUPvxB-aovVdaEEwrIA0S1ckuKw&s" width={60} alt="" />,
      name: "عباس بابایی",
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 3,
      barcode: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRends6tibfUPvxB-aovVdaEEwrIA0S1ckuKw&s" width={60} alt="" />,
      name: "عباس بابایی",
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 4,
      barcode: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRends6tibfUPvxB-aovVdaEEwrIA0S1ckuKw&s" width={60} alt="" />,
      name: "عباس بابایی",
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 5,
      barcode: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRends6tibfUPvxB-aovVdaEEwrIA0S1ckuKw&s" width={60} alt="" />,
      name: "عباس بابایی",
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 6,
      barcode: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRends6tibfUPvxB-aovVdaEEwrIA0S1ckuKw&s" width={60} alt="" />,
      name: "عباس بابایی",
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 7,
      barcode: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRends6tibfUPvxB-aovVdaEEwrIA0S1ckuKw&s" width={60} alt="" />,
      name: "عباس بابایی",
      delete: <Button variant={"danger"}>حذف</Button>,
    },
    {
      id: 8,
      barcode: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRends6tibfUPvxB-aovVdaEEwrIA0S1ckuKw&s" width={60} alt="" />,
      name: "عباس بابایی",
      delete: <Button variant={"danger"}>حذف</Button>,
    },
  ];
  return (
    <Layout>
      <div className="flex items-baseline justify-between">
        <Title className="sm:justify-center" title={"بارکد ها"} />
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

export default Barcode;
