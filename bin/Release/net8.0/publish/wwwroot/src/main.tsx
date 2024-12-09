import { createRoot } from "react-dom/client";
import App from "./App";
import "./index.css";
import { BrowserRouter } from "react-router-dom";
import ReactQueryProvider from "./providers/QueryWrapper.js";
 

createRoot(document.getElementById("root")!).render(
  <BrowserRouter>
    <ReactQueryProvider>
      <App />
    </ReactQueryProvider>
  </BrowserRouter>,
);
