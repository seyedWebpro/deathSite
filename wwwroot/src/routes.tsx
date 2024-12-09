import Home from "./pages/Home";
import Login from "./pages/login/Index";
import ContentManagement from "./pages/AdminPanel/ContentManagement/ContentManagement";
import Users from "./pages/AdminPanel/Users/Users";
import Packages from "./pages/AdminPanel/Packages/Packages";
import Tags from "./pages/AdminPanel/Tags/Tags";
import Surah from "./pages/AdminPanel/Surah/Surah";
import Nav from "./pages/AdminPanel/Nav/Nav";
import Articles from "./pages/AdminPanel/Articles/Articles";
import News from "./pages/AdminPanel/News/News";
import Banner from "./pages/AdminPanel/Banner/Banner";
import Price from "./pages/AdminPanel/Price/Price";
import Deceaseds from "./pages/AdminPanel/Deceaseds/Deceaseds";
import Condolences from "./pages/AdminPanel/Condolences/Condolences";
import Sms from "./pages/AdminPanel/Sms/Sms";
import MartyrRegister from "./pages/Martyr/Register/Register";
import MartyrPackages from "./pages/Martyr/Packages/Packages";
import Step2 from "./pages/Martyr/Packages/steps/Step2";
import Step3 from "./pages/Martyr/Packages/steps/Step3";
import Invoice from "./pages/Martyr/Packages/steps/Invoice";
import Barcode from "./pages/AdminPanel/Barcode/Barcode";
import Deceased from "./pages/Deceased/Deceased";

const adminPanelRoutes = [
  {
    path: "/adminPanel/contentManagement",
    element: <ContentManagement />,
  },
  {
    path: "/adminPanel/price",
    element: <Price />,
  },
  {
    path: "/adminPanel/deceaseds",
    element: <Deceaseds />,
  },
  {
    path: "/adminPanel/condolences",
    element: <Condolences />,
  },
  {
    path: "/adminPanel/sms",
    element: <Sms />,
  },
  {
    path: "/adminPanel/contentManagement/packages",
    element: <Packages />,
  },
  {
    path: "/adminPanel/contentManagement/tags",
    element: <Tags />,
  },
  {
    path: "/adminPanel/contentManagement/surah",
    element: <Surah />,
  },
  {
    path: "/adminPanel/contentManagement/nav",
    element: <Nav />,
  },
  {
    path: "/adminPanel/contentManagement/news",
    element: <News />,
  },
  {
    path: "/adminPanel/contentManagement/articles",
    element: <Articles />,
  },
  {
    path: "/adminPanel/contentManagement/banner",
    element: <Banner />,
  },
  {
    path: "/adminPanel/barcode",
    element: <Barcode />,
  },
  {
    path: "/adminPanel/Users",
    element: <Users />,
  },
];
const routes = [
  {
    path: "/",
    element: <Home />,
  },
  {
    path: "/login",
    element: <Login />,
  },
  {
    path: "/deceased/:id",
    element: <Deceased/>,
  },
  {
    path: "/martyr/register",
    element: <MartyrRegister />,
  },
  {
    path: "/martyr/packages", 
    element: <MartyrPackages />,
  },
  {
    path: "/martyr/packages/step2",
    element: <Step2 />,
  },
  {
    path: "/martyr/packages/step3", 
    element: <Step3 />,
  },
  {
    path: "/martyr/packages/invoice",
    element: <Invoice />,
  },
  ...adminPanelRoutes,
];

export default routes;
