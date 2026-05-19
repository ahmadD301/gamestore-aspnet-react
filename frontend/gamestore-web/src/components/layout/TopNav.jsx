import { useState } from "react";
import { NavLink } from "react-router-dom";

export default function TopNav() {
  const [isOpen, setIsOpen] = useState(false);

  function toggleMenu() {
    setIsOpen((prev) => !prev);
  }

  function closeMenu() {
    setIsOpen(false);
  }

  return (
    <header className="top-nav">
      <div className="container nav-inner">
        <NavLink to="/" className="brand" onClick={closeMenu}>
          <span className="brand-mark" aria-hidden="true" />
          <span className="brand-text">GameStore</span>
        </NavLink>

        <button
          type="button"
          className="nav-toggle"
          aria-expanded={isOpen}
          aria-controls="primary-navigation"
          aria-label="Toggle navigation"
          onClick={toggleMenu}
        >
          <span className="nav-toggle-bar" />
          <span className="nav-toggle-bar" />
          <span className="nav-toggle-bar" />
        </button>

        <nav
          id="primary-navigation"
          className={`nav-links ${isOpen ? "open" : ""}`}
          aria-label="Primary"
        >
          <NavLink
            to="/"
            end
            className={({ isActive }) =>
              `nav-link ${isActive ? "active" : ""}`
            }
            onClick={closeMenu}
          >
            Home
          </NavLink>

          <NavLink
            to="/games"
            className={({ isActive }) =>
              `nav-link ${isActive ? "active" : ""}`
            }
            onClick={closeMenu}
          >
            Games
          </NavLink>

          <NavLink
            to="/admin"
            className={({ isActive }) =>
              `nav-link ${isActive ? "active" : ""}`
            }
            onClick={closeMenu}
          >
            Admin
          </NavLink>

          <NavLink
            to="/login"
            className={({ isActive }) =>
              `nav-link ${isActive ? "active" : ""}`
            }
            onClick={closeMenu}
          >
            Login
          </NavLink>
        </nav>
      </div>
    </header>
  );
}
