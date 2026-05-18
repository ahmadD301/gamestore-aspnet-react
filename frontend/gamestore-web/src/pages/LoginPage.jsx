import { useState } from "react";
import {
  useNavigate,
  useLocation,
} from "react-router-dom";
import { useAuth } from "../auth/AuthContext";

export default function LoginPage() {
  const navigate = useNavigate();
  const location = useLocation();

  const { login } = useAuth();

  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const [error, setError] = useState("");

  const [isLoading, setIsLoading] = useState(false);

  async function handleSubmit(event) {
    event.preventDefault();

    setError("");

    try {
      setIsLoading(true);

      const response = await login(
        formData.email,
        formData.password
      );

      const isAdmin =
        response?.roles?.includes(
          "Admin"
        );

      const defaultRedirect =
        isAdmin ? "/admin" : "/";

      const redirectTo =
        location.state?.from?.pathname
        || defaultRedirect;

      navigate(redirectTo, {
        replace: true,
      });
    } catch (error) {
      setError(
        error?.response?.data?.message ??
          "Login failed."
      );
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <div>
      <h1>Login</h1>

      <form onSubmit={handleSubmit}>
        <div>
          <label htmlFor="login-email">
            Email
          </label>

          <input
            id="login-email"
            name="email"
            type="email"
            value={formData.email}
            onChange={(event) =>
              setFormData({
                ...formData,
                email: event.target.value,
              })
            }
          />
        </div>

        <div>
          <label htmlFor="login-password">
            Password
          </label>

          <input
            id="login-password"
            name="password"
            type="password"
            value={formData.password}
            onChange={(event) =>
              setFormData({
                ...formData,
                password: event.target.value,
              })
            }
          />
        </div>

        {error && (
          <p>
            {error}
          </p>
        )}

        <button
          type="submit"
          disabled={isLoading}
        >
          {isLoading
            ? "Signing in..."
            : "Login"}
        </button>
      </form>
    </div>
  );
}