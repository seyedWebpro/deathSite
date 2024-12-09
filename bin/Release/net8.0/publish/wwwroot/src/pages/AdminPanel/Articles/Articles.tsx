import Layout from "../../../layouts/adminPanel";
import Title from "../../../components/modules/title/Title";
import { useState } from "react";
import { CKEditor } from "@ckeditor/ckeditor5-react";
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";
import { Button } from "../../../components/shadcn/ui/button";
import Modal from "../../../components/templates/AdminPanel/articles/Modal";
import DataTable, { TableColumn } from "react-data-table-component";
interface ArticleData {
  id: number;
  title: string;
  body:any;
  cover:any;
  date: string;
  edit:any;
  delete:any;
}
const Articles = () => {
  const [editorData, setEditorData] = useState("");

  const columns: TableColumn<ArticleData>[]= [
    {
      name: "تیتر",
      selector: (row: { title: string }) => row.title,
    },
    {
      name: "متن",
      selector: (row: { body:any }) => row.body,
    },
    {
      name: "کاور",
      selector: (row: { cover:any }) => row.cover,
    },
    {
      name: "تاریخ انتشار",
      selector: (row: { date: string }) => row.date,
    },
    {
      name: "ویرایش",
      selector: (row: { edit:any }) => row.edit,
    },
    {
      name: "حذف",
      selector: (row: { delete:any }) => row.delete,
    },
  ];
  
  const data = [            
    {
      id: 1,
      title:"چطور شهید خودمان را ثبت کنیم؟",
      body: <Modal articleShow={true}/>,
      cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
      date: "1403/05/01",
      edit:<Modal/> ,
      delete:<Button variant={"danger"}>حذف</Button>
    },
    {
      id: 2,
      title:"چطور شهید خودمان را ثبت کنیم؟",
      body: <Modal articleShow={true}/>,
      cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
      date: "1403/05/01",
      edit: <Modal/>,
      delete:<Button variant={"danger"}>حذف</Button>
    },
    {                                
      id: 3,
      title:"چطور شهید خودمان را ثبت کنیم؟",
      body: <Modal articleShow={true}/>,
      cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
      date: "1403/05/01",
      edit: <Modal/>,
      delete:<Button variant={"danger"}>حذف</Button>
    },
    {
      id: 4,
      title:"چطور شهید خودمان را ثبت کنیم؟",
      body: <Modal articleShow={true}/>,
      cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
      date: "1403/05/01",
      edit: <Modal/>,
      delete:<Button variant={"danger"}>حذف</Button>
    },
    {
      id: 5,
      title:"چطور شهید خودمان را ثبت کنیم؟",
      body: <Modal articleShow={true}/>,
      cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
      date: "1403/05/01",
      edit: <Modal/>,
      delete:<Button variant={"danger"}>حذف</Button>
    },
    {
      id: 6,
      title:"چطور شهید خودمان را ثبت کنیم؟",
      body: <Modal articleShow={true}/>,
      cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
      date: "1403/05/01",
      edit: <Modal/>,
      delete:<Button variant={"danger"}>حذف</Button>
    },
    {
      id: 7,
      title:"چطور شهید خودمان را ثبت کنیم؟",
      body: <Modal articleShow={true}/>,
      cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
      date: "1403/05/01",
      edit: <Modal/>,
      delete:<Button variant={"danger"}>حذف</Button>
    },
    {
      id: 8,
      title:"چطور شهید خودمان را ثبت کنیم؟",
      body: <Modal articleShow={true}/>,
      cover: <img src="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ2qLE1GXRzDLzoFHGMGQkXJUh1b5osxM7v6w&s" className='w-20 rounded-lg' alt="" />,
      date: "1403/05/01",
      edit: <Modal/>,
      delete:<Button variant={"danger"}>حذف</Button>
    },
  ];

  return (
    <Layout>
      <Title className="sm:justify-center" title={"مقالات"} />

      <div>
        <DataTable
          responsive
          progressComponent={".... "}
          pagination
          columns={columns}
          data={data}
        />
      </div> 
      <Title className="mt-10 sm:justify-center" title={"+ مقاله جدید"} />
      
      <div>
        <p className="mb-2">تیتر</p>
        <input
          type="text"
          className="w-[300px] rounded-md border border-black px-4 py-2.5 sm:w-full lg:w-full"
        />
      </div>

      <div className="mt-10">
        <p className="mb-2">بدنه</p>
        <CKEditor
          editor={ClassicEditor}
          data={editorData}
          onChange={(_event, editor) => {
            const newData = editor.getData();
            setEditorData(newData);
          }}
        />
      </div>

      <Button variant={'main'} className="mx-auto block mb-10 mt-10 px-7">ثبت</Button>
    </Layout>
  );
};

export default Articles;
