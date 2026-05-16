# Auth Refresh/Login Fix Notes

## After Step 17 — Admin dashboard get this error
## Context
Issues observed:
- Infinite refresh loop and logout spam
- Redirect to /login after refresh
- Incorrect auth endpoint path usage
- Missing refresh cookie behavior on localhost
- 500 from /api/auth/refresh due to null user
- Protected route not rendering children

## Root Causes
1. Frontend used default export incorrectly for ProtectedRoute.
2. Frontend called auth endpoints without /api prefix.
3. refreshAccessToken was not defined.
4. Interceptor retried refresh and called logout in a loop when refresh failed.
5. ProtectedRoute always rendered <Outlet /> even when used with children.
6. Duplicate /admin route caused inconsistent routing.
7. Logout was used before initialization (temporal dead zone).
8. Strict Mode double-invoked restoreSession causing duplicate refresh requests in dev.
9. Backend refresh cookie Secure flag blocked cookie on http://localhost.
10. TokenResult.User was never set, causing Refresh to throw when accessing roles.

## Fixes Applied

### Frontend

1. Fix ProtectedRoute import
- File: [frontend/gamestore-web/src/App.jsx](../frontend/gamestore-web/src/App.jsx)
- Change: use default import instead of named import.

2. Add refreshAccessToken and wire interceptors
- File: [frontend/gamestore-web/src/auth/AuthContext.jsx](../frontend/gamestore-web/src/auth/AuthContext.jsx)
- Change: added refreshAccessToken and used it in setupInterceptors.

3. Use /api/auth/* endpoints
- File: [frontend/gamestore-web/src/auth/AuthContext.jsx](../frontend/gamestore-web/src/auth/AuthContext.jsx)
- Change: update login/refresh/logout to /api/auth/*.

4. Harden interceptor retry flow
- File: [frontend/gamestore-web/src/api/setupInterceptors.js](../frontend/gamestore-web/src/api/setupInterceptors.js)
- Change: skip refresh/logout endpoints, use best-effort logout with skipRequest, prevent retry loops.

5. Make ProtectedRoute render children
- File: [frontend/gamestore-web/src/routes/ProtectedRoute.jsx](../frontend/gamestore-web/src/routes/ProtectedRoute.jsx)
- Change: render children if provided, else <Outlet /> for nested routes.

6. Remove duplicate /admin route and align to dashboard
- File: [frontend/gamestore-web/src/App.jsx](../frontend/gamestore-web/src/App.jsx)
- Change: single /admin route renders AdminDashboardPage.

7. Fix logout initialization order
- File: [frontend/gamestore-web/src/auth/AuthContext.jsx](../frontend/gamestore-web/src/auth/AuthContext.jsx)
- Change: define logout before useEffect that references it.

8. Prevent double refresh on initial load in dev
- File: [frontend/gamestore-web/src/auth/AuthContext.jsx](../frontend/gamestore-web/src/auth/AuthContext.jsx)
- Change: add a ref guard so restoreSession runs once even in React Strict Mode.

9. Redirect based on role after login
- File: [frontend/gamestore-web/src/pages/LoginPage.jsx](../frontend/gamestore-web/src/pages/LoginPage.jsx)
- Change: use AuthContext login, check roles, redirect to /admin for Admin else /.

### Backend

10. Fix refresh cookie options for http localhost
- File: [backend/GameStore.Api/Controllers/AuthController.cs](../backend/GameStore.Api/Controllers/AuthController.cs)
- Change: set cookie Secure flag based on Request.IsHttps and centralize cookie options.

11. Set TokenResult.User to avoid null on refresh
- File: [backend/GameStore.Api/Services/Auth/JwtTokenService.cs](../backend/GameStore.Api/Services/Auth/JwtTokenService.cs)
- Change: include User when creating TokenResult.

## Why the Redirect to /login Happened
- The refresh cookie was not being sent because Secure cookies do not work on http://localhost.
- With no refresh cookie, /api/auth/refresh returned 401, AuthContext cleared accessToken.
- ProtectedRoute saw no token and redirected to /login.

## How to Verify
1. Start backend and frontend.
2. Log in as admin; confirm /api/auth/login sets refresh cookie.
3. Refresh /admin; confirm /api/auth/refresh returns 200 and user stays on /admin.
4. Check that a user without Admin role is redirected to / on login.
5. Confirm no repeating /api/auth/refresh or /api/auth/logout requests in the Network tab.
