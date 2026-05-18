# Frontend Page and Component Inventory

This document describes each page and component in the frontend, including:
- Page or component name
- Main sections/elements
- Current order and nesting
- Interactive elements (buttons, forms, filters, links)
- Layout patterns used (grid, flex, table, header/filters)

The inventory is based on the React files in src/pages and src/components.

---

## Pages

### HomePage
- File: frontend/gamestore-web/src/pages/HomePage.jsx
- Purpose: Minimal landing page.
- Structure and order:
  1. div (root)
     - h1: "GameStore"
- Interactive elements: None.
- Layout patterns: No explicit layout classes; simple vertical flow.

### GamesPage
- File: frontend/gamestore-web/src/pages/GamesPage.jsx
- Purpose: Game catalog with filters and pagination.
- Conditional rendering:
  - Loading: LoadingSpinner component only.
  - Error: ErrorBanner component only.
  - Empty: EmptyState component only.
  - Success: full catalog layout.
- Structure and order when data is present:
  1. div.games-page (root)
     1.1 div.catalog-header
         - h1: "Game Catalog"
         - div.catalog-filters
           - SearchBar component
           - GenreFilter component
     1.2 div.games-grid
         - list of Link elements, each wrapping a GameCard
     1.3 div.pagination
         - button: "Previous"
         - span: "Page X of Y"
         - button: "Next"
- Interactive elements:
  - SearchBar input (text) with debounced search.
  - GenreFilter select dropdown.
  - Pagination buttons (Previous/Next).
  - Each game card is wrapped in a Link to /games/:id.
- Layout patterns:
  - Header + filters grouping.
  - Grid for cards (games-grid class name).
  - Pagination row with inline controls.

### GameDetailsPage
- File: frontend/gamestore-web/src/pages/GameDetailsPage.jsx
- Purpose: Single game details view.
- Conditional rendering:
  - Loading: LoadingSpinner component only.
  - Error: ErrorBanner component only.
- Structure and order:
  1. div.game-details (root)
     - Link: back to /games
     - h1: game title
     - span.genre-badge: game genre
     - p: game description
     - h2: price
     - p: release date
- Interactive elements:
  - Back navigation Link.
- Layout patterns:
  - Single-column detail layout; uses badge styling for genre.

### LoginPage
- File: frontend/gamestore-web/src/pages/LoginPage.jsx
- Purpose: Authentication form with redirect behavior.
- Structure and order:
  1. div (root)
     - h1: "Login"
     - form
       - div: Email label + input (type=email)
       - div: Password label + input (type=password)
       - p: error message (conditional)
       - button: submit
- Interactive elements:
  - Email input.
  - Password input.
  - Submit button (disabled while loading).
- Layout patterns:
  - Vertical form flow; no explicit layout classes.

### CreateGamePage
- File: frontend/gamestore-web/src/pages/CreateGamePage.jsx
- Purpose: Admin-only create game form.
- Structure and order:
  1. div (root)
     - h1: "Create Game"
     - GameForm component
- Interactive elements:
  - GameForm fields and submit button (see component details).
- Layout patterns:
  - Simple header + form layout.

### EditGamePage
- File: frontend/gamestore-web/src/pages/EditGamePage.jsx
- Purpose: Admin-only edit game form.
- Conditional rendering:
  - Loading: LoadingSpinner component only.
  - Error: ErrorBanner component only.
- Structure and order when data is present:
  1. div (root)
     - h1: "Edit Game"
     - GameForm component (pre-filled initialValues)
- Interactive elements:
  - GameForm fields and submit button (see component details).
- Layout patterns:
  - Simple header + form layout.

### AdminPage
- File: frontend/gamestore-web/src/pages/AdminPage.jsx
- Purpose: Placeholder admin dashboard (not routed in App.jsx).
- Structure and order:
  1. div (root)
     - h1: "Admin Dashboard"
- Interactive elements: None.
- Layout patterns: Simple vertical flow.

### AdminDashboardPage
- File: frontend/gamestore-web/src/pages/AdminDashboardPage.jsx
- Purpose: Admin landing with navigation.
- Structure and order:
  1. div (root)
     - h1: "Admin Dashboard"
     - div.admin-links
       - Link: "Manage Games" (to /admin/games)
- Interactive elements:
  - Link to Manage Games.
- Layout patterns:
  - Header + link section; link grouping container.

### AdminGamesPage
- File: frontend/gamestore-web/src/pages/AdminGamesPage.jsx
- Purpose: Admin management table for games.
- Conditional rendering:
  - Loading: LoadingSpinner component only.
  - Error: ErrorBanner component only.
- Structure and order when data is present:
  1. div (root)
     1.1 div.admin-header
         - h1: "Manage Games"
         - Link: "Add Game" (to /admin/games/new)
     1.2 table.admin-table
         - thead: Title, Genre, Price, Created By, Updated By, Actions
         - tbody: list of rows
           - cells: title, genre, price, createdBy, updatedBy
           - actions cell:
             - Link: "Edit" (to /admin/games/:id/edit)
             - button: "Delete"
- Interactive elements:
  - Add Game link.
  - Edit link per row.
  - Delete button per row (with confirm dialog).
- Layout patterns:
  - Header with action link.
  - Table layout for management rows.

---

## Components

### LoadingSpinner
- File: frontend/gamestore-web/src/components/common/LoadingSpinner.jsx
- Purpose: Generic loading indicator.
- Structure and order:
  1. div.loading-spinner (role=status, aria-live=polite)
     - span.sr-only: "Loading..."
     - div.spinner (aria-hidden)
- Interactive elements: None.
- Layout patterns: Centered loader container (implied by class name).

### ErrorBanner
- File: frontend/gamestore-web/src/components/common/ErrorBanner.jsx
- Purpose: Inline error alert.
- Structure and order:
  1. div.error-banner (role=alert, aria-live=assertive)
     - error message text
- Interactive elements: None.
- Layout patterns: Single-line banner block.

### EmptyState
- File: frontend/gamestore-web/src/components/common/EmptyState.jsx
- Purpose: Empty results message.
- Structure and order:
  1. section.empty-state (aria-labelledby=empty-title)
     - h2#empty-title: title
     - p: description
- Interactive elements: None.
- Layout patterns: Vertical stack within a section.

### SearchBar
- File: frontend/gamestore-web/src/components/games/SearchBar.jsx
- Purpose: Catalog search input.
- Structure and order:
  1. div.search-bar
     - label.sr-only (for input)
     - input#game-search (type=text)
- Interactive elements:
  - Text input with onChange handler.
- Layout patterns: Compact input wrapper (implied by class name).

### GenreFilter
- File: frontend/gamestore-web/src/components/games/GenreFilter.jsx
- Purpose: Genre dropdown filter.
- Structure and order:
  1. div.genre-filter
     - label.sr-only (for select)
     - select#genre-select
       - option: "All Genres" (value="")
       - option list from genres
- Interactive elements:
  - Select dropdown with onChange handler.
- Layout patterns: Compact select wrapper (implied by class name).

### GameCard
- File: frontend/gamestore-web/src/components/games/GameCard.jsx
- Purpose: Summary card for a game.
- Structure and order:
  1. article.game-card
     - div.game-card-header
       - h2: game title
       - span.genre-badge: genre
     - p.game-description: description
     - div.game-card-footer
       - strong: price
       - span: release date
- Interactive elements: None inside the card itself (links are applied by parent page).
- Layout patterns:
  - Header + body + footer structure; likely card layout with header and footer rows.

### GameForm
- File: frontend/gamestore-web/src/components/games/GameForm.jsx
- Purpose: Reusable create/edit form.
- Structure and order:
  1. form.game-form
     - div: label + input (Title)
       - error span (conditional)
     - div: label + textarea (Description)
       - error span (conditional)
     - div: label + input type=number (Price)
       - error span (conditional)
     - div: label + input type=date (Release Date)
     - div: label + select (Genre)
       - option: "Select Genre"
       - options for genres
       - error span (conditional)
     - button: submit
- Interactive elements:
  - Text input (Title).
  - Textarea (Description).
  - Number input (Price).
  - Date input (Release Date).
  - Select dropdown (Genre).
  - Submit button (disabled while loading).
- Layout patterns:
  - Vertical stacked form fields; error messages inline per field.

---

## Route Coverage Note
The following pages are routed in App.jsx: HomePage (/), LoginPage (/login), GamesPage (/games), GameDetailsPage (/games/:id), AdminDashboardPage (/admin), AdminGamesPage (/admin/games), CreateGamePage (/admin/games/new), EditGamePage (/admin/games/:id/edit). AdminPage exists but is not wired to a route.
