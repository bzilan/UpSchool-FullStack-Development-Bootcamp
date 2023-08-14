import {
  AuthLoginCommand,
  AuthRegisterCommand,
  LocalJwt,
} from "../types/AuthTypes.ts";
import React, { useContext, useState } from "react";
import { Button, Form, Header, Icon } from "semantic-ui-react";
import api from "../utils/axiosInstance.ts";
import { getClaimsFromJwt } from "../utils/jwtHelper.ts";
import { toast } from "react-toastify";
import { useNavigate, Link } from "react-router-dom";
import { AppUserContext } from "../context/StateContext.tsx"; // import your image

const BASE_URL = import.meta.env.VITE_API_URL;

/*export type LoginPageProps = {

 }*/

function LoginPage() {
  const { setAppUser } = useContext(AppUserContext);

  const navigate = useNavigate();

  const [authLoginCommand, setAuthLoginCommand] = useState<AuthLoginCommand>({
    email: "",
    password: "",
  });

  const [authRegisterCommand, setAuthRegisterCommand] =
    useState<AuthRegisterCommand>({
      firstName: "",
      lastName: "",
      email: "",
      password: "",
    });

  const [isRegistered, setIsRegistered] = useState<boolean>(false);
  const [registerError, setRegisterError] = useState<string | null>(null);
  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    try {
      const response = await api.post(
        "/Authentication/Login",
        authLoginCommand
      );

      if (response.status === 200) {
        const accessToken = response.data.accessToken;
        const { uid, email, given_name, family_name } =
          getClaimsFromJwt(accessToken);
        const expires: string = response.data.expires;

        setAppUser({
          id: uid,
          email,
          firstName: given_name,
          lastName: family_name,
          expires,
          accessToken,
        });

        const localJwt: LocalJwt = {
          accessToken,
          expires,
        };

        localStorage.setItem("upstorageshop_user", JSON.stringify(localJwt));
        navigate("/");
      } else {
        toast.error(response.statusText);
      }
    } catch (error) {
      toast.error("Something went wrong!");
    }
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setAuthLoginCommand({
      ...authLoginCommand,
      [event.target.name]: event.target.value,
    });
  };

  const onGoogleLoginClick = (e: React.FormEvent) => {
    e.preventDefault();

    window.location.href = `${BASE_URL}/Authentication/GoogleSignInStart`;
  };

  const handleRegisterSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    try {
      const response = await api.post(
        "/Authentication/Register",
        authRegisterCommand
      );

      if (response.status === 200) {
        setIsRegistered(true);
      } else {
        setRegisterError("Error registering.");
      }
    } catch (error) {
      setRegisterError("Something went wrong!");
    }
  };

  const handleRegisterInputChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setAuthRegisterCommand({
      ...authRegisterCommand,
      [event.target.name]: event.target.value,
    });
  };

  return (
    <div
      style={{
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "70vh",
      }}
    >
      <div style={{ maxWidth: 450 }}>
        {/* Login Form */}
        <div style={{ marginBottom: "1rem" }}>
          <Header as="h2" color="teal" textAlign="center">
            Log-in to your account
          </Header>
          <Form size="large" onSubmit={handleSubmit}>
            <Form.Input
              fluid
              icon="mail"
              iconPosition="left"
              placeholder="Email"
              value={authLoginCommand.email}
              onChange={handleInputChange}
              name="email"
            />
            <Form.Input
              fluid
              icon="lock"
              iconPosition="left"
              placeholder="Password"
              type="password"
              value={authLoginCommand.password}
              onChange={handleInputChange}
              name="password"
            />
            <Button color="teal" fluid size="large" type="submit">
              Login
            </Button>
            <Button
              color="red"
              fluid
              onClick={onGoogleLoginClick}
              size="large"
              style={{ marginTop: "1em" }}
              type="button"
            >
              <Icon name="google" /> Sign in with Google
            </Button>
          </Form>
          {/* Additional content for login, e.g., "Forgot password?" */}
          <p style={{ textAlign: "center", marginTop: "1rem" }}>
            <Link to="/forgot-password">Forgot password?</Link>
          </p>
        </div>

        {/* Register Form */}
        <div>
          <Header as="h2" color="teal" textAlign="center">
            Register
          </Header>
          <Form size="large" onSubmit={handleRegisterSubmit}>
            <Form.Input
              fluid
              icon="user"
              iconPosition="left"
              placeholder="First Name"
              value={authRegisterCommand.firstName}
              onChange={handleRegisterInputChange}
              name="firstName"
            />
            <Form.Input
              fluid
              icon="user"
              iconPosition="left"
              placeholder="Last Name"
              value={authRegisterCommand.lastName}
              onChange={handleRegisterInputChange}
              name="lastName"
            />
            <Form.Input
              fluid
              icon="mail"
              iconPosition="left"
              placeholder="Email"
              value={authRegisterCommand.email}
              onChange={handleRegisterInputChange}
              name="email"
            />
            <Form.Input
              fluid
              icon="lock"
              iconPosition="left"
              placeholder="Password"
              type="password"
              value={authRegisterCommand.password}
              onChange={handleRegisterInputChange}
              name="password"
            />
            <Button color="yellow" fluid size="large" type="submit">
              Sign Up
            </Button>
          </Form>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;
