import { Link }
from "react-router-dom";

export default function AdminDashboardPage() {
  return (
    <div>
      <h1>Admin Dashboard</h1>

      <div className="admin-links">
        <Link to="/admin/games">
          Manage Games
        </Link>
      </div>
    </div>
  );
}