 import Title from '../../../components/modules/title/Title'
import Layout from '../../../layouts/adminPanel'
import DataTable, { TableColumn } from 'react-data-table-component'
import { Button } from '../../../components/shadcn/ui/button'
import Modal from '../../../components/templates/AdminPanel/deceaseds/Modal'

interface DeceasedsData {
  id: number;
  name: string;
  phone:any;
  date:any; 
  details:any;
  status:any;
}
const Deceaseds = () => {

    const columns   : TableColumn<DeceasedsData>[] = [
        {
          name: "نویسنده",
          selector: (row: { name: string }) => row.name,
        },
        {
          name: "شماره موبایل",
          selector: (row: { phone: string }) => row.phone,
        },
        {
          name: "تاریخ انتشار",
          selector: (row: { date: string }) => row.date,
        },
        {
          name: "جزئیات متوفی",
          selector: (row: { details: string }) => row.details,
        },
        {
          name: "وضعیت",
          selector: (row: { status: string }) => row.status,
        },
      ];
     
      const data = [
        {
          id: 1,
          name: "شاهین مشکل گشا",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
          status: (
            <div className='flex gap-2'>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"danger"}>حذف</Button>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"main"}>تایید</Button>
            </div>
          ),
        },
        {
          id: 2,
          name: "رضا مرادی",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
          status: 'تایید شده',
        },
        {
          id: 3,
          name: "مریم مشکل گشا",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
            status: (
            <div className='flex gap-2'>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"danger"}>حذف</Button>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"main"}>تایید</Button>
            </div>
          ),
        },
        {
          id: 4,
          name: "محمد زارع",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
          status: 'رد شده', 
        },
        {
          id: 5,
          name: "فریدون شهریاری",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
            status: (
            <div className='flex gap-2'>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"danger"}>حذف</Button>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"main"}>تایید</Button>
            </div>
          ),
        },
        {
          id: 6,
          name: "نفیسه کیوانی",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
          status: 'تایید شده', 
        },
        {
          id: 7,
          name: "دنیا رضایی",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
            status: (
            <div className='flex gap-2'>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"danger"}>حذف</Button>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"main"}>تایید</Button>
            </div>
          ),
        },
        {
          id: 8,
          name: "شهرام مرادی",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
            status: (
            <div className='flex gap-2'>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"danger"}>حذف</Button>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"main"}>تایید</Button>
            </div>
          ),
        },
        {
          id: 9,
          name: "کریم زراعتی",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
            status: (
            <div className='flex gap-2'>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"danger"}>حذف</Button>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"main"}>تایید</Button>
            </div>
          ),
        },
        {
          id: 10,
          name: "رها سلطانی",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
            status: (
            <div className='flex gap-2'>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"danger"}>حذف</Button>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"main"}>تایید</Button>
            </div>
          ),
        },
        {
          id: 10,
          name: "شاهرخ ماهانی",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
            status: (
            <div className='flex gap-2'>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"danger"}>حذف</Button>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"main"}>تایید</Button>
            </div>
          ),
        },
        {
          id: 10,
          name: "سیاوش باقری",
          phone: "09046417084",
          date: "1403/05/01",
          details: <Modal />,
            status: (
            <div className='flex gap-2'>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"danger"}>حذف</Button>
            <Button className='sm:p-2 sm:text-xs sm:h-8' variant={"main"}>تایید</Button>
            </div>
          ),
        },
      ];

      
  return (
    <Layout>
    <Title className="sm:justify-center" title={"مدیریت متوفی ها" } />

    <div>
        <DataTable
          responsive
          pagination
          columns={columns}
          data={data}
          noDataComponent={
            <div className="text-2xl">کاربری با این اسم پیدا نشد.</div>
          }
        />
      </div>
    </Layout>
  )
}

export default Deceaseds
