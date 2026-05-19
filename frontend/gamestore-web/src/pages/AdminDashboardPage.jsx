import { Link }
from "react-router-dom";

import { useGames }
from "../hooks/games/useGames";

import { FolderKanban, BookOpen, Settings } from "lucide-react";

export default function AdminDashboardPage() {
  const { data } = useGames({
    page: 1,
    pageSize: 5,
  });

  const totalGames =
    data?.totalItems
    ?? data?.totalCount
    ?? data?.items?.length
    ?? 0;

  return (
    <div className="page admin-dashboard">
      <div className="container">
        <div className="admin-header">
          <div>
            <p className="breadcrumb">Admin</p>
            <h1>Admin Dashboard</h1>
          </div>
        </div>

        <div className="dashboard-grid">
          <div className="dashboard-card stat-card">
            <p>Total Games</p>
            <h2>{totalGames}</h2>
          </div>

          <div className="dashboard-card stat-card">
            <p>Total Users</p>
            <h2>0</h2>
          </div>

          <div className="dashboard-card stat-card">
            <p>Recent Activity</p>
            <h2>{data?.items?.length ?? 0}</h2>
          </div>
        </div>

        <div className="dashboard-actions">
          <Link className="action-card" to="/admin/games">
            <FolderKanban className="action-icon" aria-hidden="true" />
            <div>
              <h3>Manage Games</h3>
              <p>Update catalog listings and pricing.</p>
            </div>
          </Link>

          <Link className="action-card" to="/games">
            <BookOpen className="action-icon" aria-hidden="true" />
            <div>
              <h3>View Catalog</h3>
              <p>Explore the storefront experience.</p>
            </div>
          </Link>

          <Link className="action-card" to="/admin">
            <Settings className="action-icon" aria-hidden="true" />
            <div>
              <h3>Settings</h3>
              <p>Configure admin preferences.</p>
            </div>
          </Link>
        </div>

        <div className="activity-feed">
          <div className="section-header">
            <h2>Recent activity</h2>
            <p>Latest catalog changes and updates.</p>
          </div>

          {!data?.items?.length ? (
            <p className="muted">No recent activity.</p>
          ) : (
            <ul>
              {data.items.map((game) => (
                <li key={game.id}>
                  <span className="activity-dot" aria-hidden="true" />
                  <div>
                    <p>
                      {game.updatedBy || game.createdBy || "System"} updated
                      {" "}
                      {game.title}
                    </p>
                    <span className="muted">Catalog update</span>
                  </div>
                </li>
              ))}
            </ul>
          )}
        </div>
      </div>
    </div>
  );
}