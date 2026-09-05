import { Outlet } from "@tanstack/react-router";
import AppFooter from "../AppFooter/AppFooter";
import AppHeader from "../AppHeader/AppHeader";

const Layout = () => (
  <div className={"flex min-h-screen flex-col overflow-x-clip"}>
    <AppHeader />
    <Outlet />
    <AppFooter />
  </div>
);

export default Layout;
