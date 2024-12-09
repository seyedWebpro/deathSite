import React from "react";
import { useRoutes } from "react-router-dom";
import routes from "./routes"; 
import ScrollToUp from "./utils/ScrollToUp";
import { Toaster } from "./components/shadcn/ui/toaster";

const App: React.FC = () => { 

  const router = useRoutes(routes);
  return (
    <>
      {/* <ScrollToUp /> */}
      {router}
      <Toaster />
    </>
  );
};

export default App;
