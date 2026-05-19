import { Link } from "react-router-dom";

export default function HomePage() {
  return (
    <div className="page home-page">
      <section className="hero">
        <div className="container hero-inner">
          <div className="hero-content">
            <p className="eyebrow">Next-gen game marketplace</p>
            <h1>GameStore</h1>
            <p className="hero-subtitle">
              Discover curated titles, explore new genres, and manage your
              collection with a premium storefront experience.
            </p>

            <div className="hero-actions">
              <Link className="button primary" to="/games">
                Browse Games
              </Link>
              <Link className="button secondary" to="/login">
                Admin Login
              </Link>
            </div>
          </div>

          <div className="hero-visual" aria-hidden="true">
            <div className="hero-orb" />
            <div className="hero-panel">
              <div className="hero-panel-row" />
              <div className="hero-panel-row" />
              <div className="hero-panel-row" />
            </div>
          </div>
        </div>
      </section>

      <section className="features">
        <div className="container">
          <div className="section-header">
            <h2>Designed for players and publishers</h2>
            <p>
              Streamlined discovery, instant insights, and a management suite
              built for growth.
            </p>
          </div>

          <div className="features-grid">
            <article className="feature-card">
              <h3>Curated Catalog</h3>
              <p>
                Browse a premium collection of titles with smart filters and
                genre exploration.
              </p>
            </article>

            <article className="feature-card">
              <h3>Admin Control</h3>
              <p>
                Manage releases, pricing, and updates with a focused admin
                dashboard.
              </p>
            </article>

            <article className="feature-card">
              <h3>Fast Checkout</h3>
              <p>
                Streamlined purchase flows keep customers moving from discovery
                to play.
              </p>
            </article>

            <article className="feature-card">
              <h3>Live Insights</h3>
              <p>
                Keep tabs on catalog performance and recent activity with
                real-time signals.
              </p>
            </article>
          </div>
        </div>
      </section>
    </div>
  );
}