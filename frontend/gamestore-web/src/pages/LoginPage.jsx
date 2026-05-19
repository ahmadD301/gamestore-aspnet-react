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
    <div className="page auth-page">
      <div className="container auth-container">
        <div className="auth-card">
          <div className="auth-header">
            <h1>Welcome back</h1>
            <p>Sign in to manage your catalog.</p>
          </div>

          <form onSubmit={handleSubmit} className="auth-form">
            <div className="form-field">
              <label htmlFor="login-email">Email</label>
              <div className="input-with-icon">
                <span className="input-icon email-icon" aria-hidden="true" />
                <input
                  id="login-email"
                  name="email"
                  type="email"
                  placeholder="you@example.com"
                  value={formData.email}
                  onChange={(event) =>
                    setFormData({
                      ...formData,
                      email: event.target.value,
                    })
                  }
                />
              </div>
            </div>

            <div className="form-field">
              <label htmlFor="login-password">Password</label>
              <div className="input-with-icon">
                <span className="input-icon lock-icon" aria-hidden="true" />
                <input
                  id="login-password"
                  name="password"
                  type="password"
                  placeholder="Enter your password"
                  value={formData.password}
                  onChange={(event) =>
                    setFormData({
                      ...formData,
                      password: event.target.value,
                    })
                  }
                />
              </div>
            </div>

            <div className="auth-options">
              <label className="checkbox">
                <input type="checkbox" />
                <span>Remember me</span>
              </label>
              <a
                className="text-button"
                href="mailto:support@gamestore.com"
              >
                Forgot password?
              </a>
            </div>

            {error && (
              <p className="form-error" role="alert">
                {error}
              </p>
            )}

            <button
              type="submit"
              className="button primary full"
              disabled={isLoading}
            >
              {isLoading
                ? "Signing in..."
                : "Login"}
            </button>
          </form>

          <div className="auth-divider">
            <span>or continue with</span>
          </div>

          <div className="social-buttons">
            <button type="button" className="button secondary full">
              Google
            </button>
            <button type="button" className="button secondary full">
              Xbox Live
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}