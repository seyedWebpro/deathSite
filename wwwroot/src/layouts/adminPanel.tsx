 import Topbar from "../components/modules/topbar/Topbar";

type Props = {
  children: React.ReactNode;
  className?:string
};

const Layout = (props: Props) => {
  return (
    <div className="flex flex-col min-h-screen adminPanel">
      <Topbar />
      <div
        dir="rtl"
        className={`${props.className ? props.className : ""} mt-20 flex-grow rounded-t-[120px] bg-white px-24 pt-20 pb-0 xs:!px-4 sm:pt-10 md:px-10`}
      >
        {props.children}
      </div>
    </div>
  );
};

export default Layout;
