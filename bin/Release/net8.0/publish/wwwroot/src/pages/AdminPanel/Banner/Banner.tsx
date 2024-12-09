import DataTable, { TableColumn } from 'react-data-table-component';
import Title from '../../../components/modules/title/Title'
import Layout from '../../../layouts/adminPanel'
import { Button } from '../../../components/shadcn/ui/button';
import Modal from '../../../components/templates/AdminPanel/banner/Modal';

interface BannerData {
  id: number;
  title: string;
  body:any;
  cover:any;
  date: string;
  edit:any;
  delete:any;
}
const Banner =() => {

    const columns: TableColumn<BannerData>[] = [
        {
          name: "تیتر ",
          selector: (row: { title: string }) => row.title,
        },
        {
          name: "متن",
          selector: (row: { body: string }) => row.body,
        },
        {
          name: "کاور",
          selector: (row: { cover: string }) => row.cover,
        },
        {
          name: "تاریخ انتشار",
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
        title:"چطور شهید خودمان را ثبت کنیم؟",
          body: <Modal bannershow={true}/>,
          cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
          date: "1403/05/01",
          edit:<Modal/> ,
          delete:<Button variant={"danger"}>حذف</Button>
        },
        {
          id: 2,
        title:"چطور شهید خودمان را ثبت کنیم؟",
          body: <Modal bannershow={true}/>,
          cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
          date: "1403/05/01",
          edit: <Modal/>,
          delete:<Button variant={"danger"}>حذف</Button>
        },
        {
          id: 3,
        title:"چطور شهید خودمان را ثبت کنیم؟",
          body: <Modal bannershow={true}/>,
          cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
          date: "1403/05/01",
          edit: <Modal/>,
          delete:<Button variant={"danger"}>حذف</Button>
        },
        {
          id: 4,
        title:"چطور شهید خودمان را ثبت کنیم؟",
          body: <Modal bannershow={true}/>,
          cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
          date: "1403/05/01",
          edit: <Modal/>,
          delete:<Button variant={"danger"}>حذف</Button>
        },
        {
          id: 5,
        title:"چطور شهید خودمان را ثبت کنیم؟",
          body: <Modal bannershow={true}/>,
          cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
          date: "1403/05/01",
          edit: <Modal/>,
          delete:<Button variant={"danger"}>حذف</Button>
        },
        {
          id: 6,
        title:"چطور شهید خودمان را ثبت کنیم؟",
          body: <Modal bannershow={true}/>,
          cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
          date: "1403/05/01",
          edit: <Modal/>,
          delete:<Button variant={"danger"}>حذف</Button>
        },
        {
          id: 7,
        title:"چطور شهید خودمان را ثبت کنیم؟",
          body: <Modal bannershow={true}/>,
          cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
          date: "1403/05/01",
          edit: <Modal/>,
          delete:<Button variant={"danger"}>حذف</Button>
        },
        {
          id: 8,
        title:"چطور شهید خودمان را ثبت کنیم؟",
          body: <Modal bannershow={true}/>,
          cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
          date: "1403/05/01",
          edit: <Modal/>,
          delete:<Button variant={"danger"}>حذف</Button>
        },
      ];
  return (
    <Layout>
        <div className='flex justify-between items-baseline'> 
      <Title className="sm:justify-center" title={"بنر"} />
      <Modal bannerNews={true} />
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
  )
}

export default Banner
