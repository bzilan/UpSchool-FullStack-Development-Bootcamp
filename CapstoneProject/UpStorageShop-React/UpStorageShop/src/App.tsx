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
import SettingsPage from "./pages/SettingsPage.tsx";
import ProtectedRoute from "./components/ProtectedRoute.tsx";

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
    const jwtJson = localStorage.getItem("upstorageshop_user");

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

  return (
    <>
      <AppUserContext.Provider value={{ appUser, setAppUser }}>
        <ToastContainer />
        <NavBar />
        <Container className="App">
          <Routes>
            <Route path="/" element={<LoginPage />} />
            <Route path="/social-login" element={<SocialLogin />} />
            <Route
              path="/orders"
              element={
                <ProtectedRoute>
                  <OrdersPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/crawlerlogs"
              element={
                <ProtectedRoute>
                  <CrawlerLogsPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/settings"
              element={
                <ProtectedRoute>
                  <SettingsPage />
                </ProtectedRoute>
              }
            />
            <Route
              path="/dashboards"
              element={
                <ProtectedRoute>
                  <DashboardPage onCrawlStart={handleCrawlStart} />
                </ProtectedRoute>
              }
            />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </Container>
      </AppUserContext.Provider>
    </>
  );
}

export default App;
