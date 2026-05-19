import { Link, useNavigate, useParams }
  from "react-router-dom";

import { useGameById }
  from "../hooks/games/useGameById";

import LoadingSpinner
  from "../components/common/LoadingSpinner";

import ErrorBanner
  from "../components/common/ErrorBanner";
import EmptyState
  from "../components/common/EmptyState";

export default function GameDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const {
    data,
    isLoading,
    isError,
    error,
  } = useGameById(id);

  if (isLoading) {
    return <LoadingSpinner />;
  }

  if (isError) {
    return (
      <ErrorBanner
        message={
          error?.response?.data?.detail
          ??
          "Failed to load game."
        }
      />
    );
  }

  return (
    <div className="page game-details-page">
      <div className="container">
        <Link to="/games" className="back-button">
          <span className="back-icon" aria-hidden="true" />
          Back to catalog
        </Link>

        <div className="game-details">
          <div className="details-media">
            {data.coverImageUrl ? (
              <img src={data.coverImageUrl} alt={data.title} />
            ) : (
              <div className="details-media-placeholder" aria-hidden="true" />
            )}
          </div>

          <div className="details-info">
            <div className="details-header">
              <h1>{data.title}</h1>
              <span className="genre-badge">
                {data.genre}
              </span>
            </div>

            <p className="details-description">
              {data.description}
            </p>

            <div className="details-price">
              <span className="price-label">Price</span>
              <span className="price-value">${data.price}</span>
            </div>

            <div className="details-meta">
              <div className="meta-item">
                <span className="meta-icon" aria-hidden="true" />
                <div>
                  <span className="meta-label">Release date</span>
                  <span className="meta-value">
                    {data.releaseDateUtc
                      ? new Date(
                          data.releaseDateUtc
                        ).toLocaleDateString()
                      : "-"}
                  </span>
                </div>
              </div>

              <div className="meta-item">
                <span className="meta-icon" aria-hidden="true" />
                <div>
                  <span className="meta-label">Genre</span>
                  <span className="meta-value">
                    {data.genre}
                  </span>
                </div>
              </div>
            </div>

            <div className="details-actions">
              <button type="button" className="button primary">
                Purchase Game
              </button>
              <button type="button" className="button secondary">
                Add to Wishlist
              </button>
            </div>
          </div>
        </div>

        <section className="related-section">
          <div className="section-header">
            <h2>Related games</h2>
            <p>Explore similar titles from the catalog.</p>
          </div>

          <EmptyState
            title="No related games yet"
            description="Browse the full catalog to discover more."
            actionLabel="View all games"
            onAction={() => navigate("/games")}
          />
        </section>
      </div>
    </div>
  );
}