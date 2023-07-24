import "./App.css";
import { Container } from "semantic-ui-react";
import { Routes, Route } from "react-router-dom";
import { useEffect, useState } from "react";
import { LocalJwt, LocalUser } from "./types/AuthTypes.ts";
import LoginPage from "./pages/LoginPage.tsx";
import NotFoundPage from "./pages/NotFoundPage.tsx";
import { getClaimsFromJwt } from "./utils/jwtHelper.ts";
import { useNavigate } from "react-router-dom";
import { AppUserContext } from "./context/StateContext.tsx";
import { ToastContainer } from "react-toastify";
import NavBar from "./components/NavBar.tsx";
import SocialLogin from "./pages/SocialLogin.tsx";
import OrdersPage from "./pages/OrdersPage.tsx";
import DashboardPage from "./pages/DashboardPage.tsx";
import { ProductCrawlType } from "./types/OrderTypes.ts";
import CrawlerLogsPage from "./pages/CrawlerLogsPage.tsx";
import OrderAddPage from "./pages/OrderAddPage.tsx";
import SettingsPage from "./pages/SettingsPage.tsx";

function App() {
  const navigate = useNavigate();
  const [appUser, setAppUser] = useState<LocalUser | undefined>(undefined);

  const handleCrawlStart = (
    productCount: number,
    crawlType: ProductCrawlType
  ) => {
    // Your logic for handling the crawl start
    console.log("Product Count:", productCount);
    console.log("Crawl Type:", crawlType);
  };

  useEffect(() => {
    const jwtJson = localStorage.getItem("upstorage_user");

    if (!jwtJson) {
      navigate("/login");
      return;
    }

    const localJwt: LocalJwt = JSON.parse(jwtJson);

    const { uid, email, given_name, family_name } = getClaimsFromJwt(
      localJwt.accessToken
    );

    const expires: string = localJwt.expires;

    setAppUser({
      id: uid,
      email,
      firstName: given_name,
      lastName: family_name,
      expires,
      accessToken: localJwt.accessToken,
    });
  }, []);

  const handleLoginOnClick = () => {
    navigate("/login");
  };

  const handleLogout = () => {
    localStorage.removeItem("crawler_user");

    setAppUser(undefined);

    navigate("/login");
  };

  return (
    <>
      <AppUserContext.Provider value={{ appUser, setAppUser }}>
        <ToastContainer />
        <NavBar />
        <Container className="App">
          <Routes>
            <Route path="/" element={<LoginPage />} />
            <Route path="/social-login" element={<SocialLogin />} />
            <Route path="/orders" element={<OrdersPage />} />
            <Route path="/crawlerlogs" element={<CrawlerLogsPage />} />
            <Route path="/orderadd" element={<OrderAddPage />} />
            <Route path="/settings" element={<SettingsPage />} />
            <Route
              path="/dashboards"
              element={<DashboardPage onCrawlStart={handleCrawlStart} />}
            />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </Container>
      </AppUserContext.Provider>
    </>
  );
}

export default App;
