import { Container, Menu, Icon, Button, Image } from "semantic-ui-react";
import { useContext } from "react";
import { AppUserContext } from "../context/StateContext.tsx";
import { NavLink, useNavigate } from "react-router-dom";

const NavBar = () => {
  const { appUser, setAppUser } = useContext(AppUserContext);
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("upstorageshop_user");

    setAppUser(undefined);

    navigate("/login");
  };

  return (
    <Menu fixed="top" inverted>
      <Container>
        <Menu.Item as="a" header>
          <Image
            size="mini"
            src="/upstorage_logo_730_608 (1).png"
            style={{ marginRight: "1.5em", maxWidth: "130px" }}
          />
        </Menu.Item>
        <Menu.Item as={NavLink} to="/">
          Home
        </Menu.Item>
        <Menu.Item as={NavLink} to="/orders">
          Orders
        </Menu.Item>
        <Menu.Item as={NavLink} to="/dashboard">
          Dashboard
        </Menu.Item>
        <Menu.Item as={NavLink} to="/crawlerlogs">
          Crawler Logs
        </Menu.Item>
        <Menu.Item as={NavLink} to="/settings">
          Settings
        </Menu.Item>
        <Menu.Item as={NavLink} to="/hahhahahhahahahh">
          Not Found
        </Menu.Item>
        {!appUser && (
          <Menu.Item as={NavLink} to="/login" position="right">
            <Icon name="sign-in" /> Login
          </Menu.Item>
        )}
        {appUser && (
          <Menu.Item as={Button} onClick={handleLogout} position="right">
            <Icon name="sign-out" /> Logout
          </Menu.Item>
        )}
      </Container>
    </Menu>
  );
};

export default NavBar;
