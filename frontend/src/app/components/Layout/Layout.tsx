import { Outlet } from "@tanstack/react-router";
import AppFooter from "../AppFooter/AppFooter";
import AppHeader from "../AppHeader/AppHeader";

const Layout = () => (
  <>
    <AppHeader />
    <Outlet />
    <AppFooter />
  </>
);

export default Layout;
