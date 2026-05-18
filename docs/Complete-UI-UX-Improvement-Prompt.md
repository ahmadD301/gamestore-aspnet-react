I need you to improve the UI/UX layout and styling for my entire GameStore web application. Below is the complete inventory of all pages and components. Please enhance the design with modern, professional styling while maintaining consistency across the entire application.

=== DESIGN REQUIREMENTS ===

**Color Scheme & Brand Identity:**
- Primary color: #6366f1 (indigo-500) - for primary actions, links, and key elements
- Primary hover: #4f46e5 (indigo-600)
- Secondary color: #8b5cf6 (purple-500) - for accents and secondary actions
- Background: #0f172a (slate-900) - dark theme base
- Card background: #1e293b (slate-800)
- Text primary: #f1f5f9 (slate-100)
- Text secondary: #94a3b8 (slate-400)
- Border color: #334155 (slate-700)
- Success: #10b981 (emerald-500)
- Error: #ef4444 (red-500)
- Warning: #f59e0b (amber-500)

**Typography:**
- Font family: 'Inter', system-ui, -apple-system, sans-serif
- Headings: Bold, proper hierarchy (h1: 2.5rem, h2: 2rem, h3: 1.5rem)
- Body text: 1rem with 1.5 line-height for readability
- Use font weights: 400 (normal), 500 (medium), 600 (semibold), 700 (bold)

**Layout Principles:**
- Maximum content width: 1400px (centered with auto margins)
- Consistent spacing scale: 0.5rem, 1rem, 1.5rem, 2rem, 3rem, 4rem
- Responsive breakpoints: mobile (<768px), tablet (768px-1024px), desktop (>1024px)
- Card-based design with subtle shadows and rounded corners (0.75rem border-radius)
- Proper use of white space and breathing room between sections

**Component Standards:**
- Buttons: Rounded corners, proper padding (0.75rem 1.5rem), hover effects with transitions
- Inputs: Clear borders, focus states with ring effects, proper labels
- Cards: Elevation with box-shadow, hover effects with scale transform
- Tables: Alternating row colors, proper spacing, responsive design
- Forms: Inline validation, clear error states, good label-input association

=== PAGES TO IMPROVE ===

**1. HomePage (/) - Landing Page**
Current: Minimal with just an h1 "GameStore"
Improve to:
- Hero section with gradient background and compelling headline
- Feature highlights (3-4 cards showcasing key features)
- Call-to-action buttons: "Browse Games" and "Admin Login"
- Modern hero layout with centered content
- Add visual interest with subtle animations or decorative elements

**2. GamesPage (/games) - Game Catalog**
Current structure:
- Header with "Game Catalog" title
- Filters: SearchBar and GenreFilter
- Grid of GameCard components
- Pagination controls

Improvements needed:
- Sticky header with filters that stays visible on scroll
- Enhanced search bar with icon and clear button
- Modern select dropdown for genre filter (consider custom styled select)
- Responsive grid: 1 column mobile, 2 columns tablet, 3-4 columns desktop
- Smooth pagination with disabled state styling
- Add results count display
- Loading skeleton for game cards
- Empty state with illustration or icon

**3. GameDetailsPage (/games/:id) - Game Detail View**
Current structure:
- Back link
- Game title (h1)
- Genre badge
- Description
- Price (h2)
- Release date

Improvements needed:
- Two-column layout on desktop (image placeholder + details)
- Prominent back button with icon
- Large, styled price display with currency formatting
- Genre badge with color coding
- Well-formatted description with max-width for readability
- Metadata section (release date, genre) with icons
- Add "Add to Cart" or "Purchase" CTA button
- Related games section at bottom

**4. LoginPage (/login) - Authentication**
Current structure:
- Simple form with email, password, submit button

Improvements needed:
- Centered card layout with max-width 400px
- Brand logo/title at top
- Styled form inputs with icons (email icon, lock icon)
- "Remember me" checkbox option
- "Forgot password?" link
- Social login options (optional visual placeholders)
- Loading state for submit button
- Success/error feedback with proper styling

**5. CreateGamePage (/admin/games/new) & EditGamePage (/admin/games/:id/edit)**
Current structure:
- Page title
- GameForm component

Improvements needed:
- Consistent admin header with breadcrumbs
- Form in centered card container (max-width 800px)
- Two-column layout for form fields where appropriate
- Image upload preview section (placeholder)
- Action buttons: "Save" (primary) and "Cancel" (secondary)
- Save & Continue option
- Proper spacing between form sections

**6. AdminDashboardPage (/admin) - Admin Landing**
Current structure:
- Title
- Single link to "Manage Games"

Improvements needed:
- Grid of dashboard cards (stats + quick actions)
- Statistics cards: Total Games, Total Users, Recent Activity
- Quick action cards with icons: Manage Games, View Analytics, Settings
- Recent activity feed (last 5 actions)
- Modern dashboard layout with responsive grid

**7. AdminGamesPage (/admin/games) - Game Management Table**
Current structure:
- Header with "Manage Games" title and "Add Game" link
- Table with columns: Title, Genre, Price, Created By, Updated By, Actions

Improvements needed:
- Enhanced header with search and filter options
- Sticky table header
- Modern table styling with hover effects on rows
- Action buttons with icons (Edit pencil icon, Delete trash icon)
- Confirmation modal for delete action
- Pagination for large datasets
- Responsive: Convert to cards on mobile
- Bulk action support (select multiple, bulk delete)
- Sort indicators on column headers

=== COMPONENTS TO IMPROVE ===

**LoadingSpinner**
- Animated spinner using indigo color scheme
- Center positioning with overlay option
- Smooth spin animation (CSS animation)

**ErrorBanner**
- Red background with white text
- Icon on left (exclamation circle)
- Dismiss button on right
- Slide-in animation

**EmptyState**
- Centered content with icon or illustration
- Supportive message
- Call-to-action button to resolve empty state
- Gray color scheme (not alarming)

**SearchBar**
- Search icon on left inside input
- Clear button (X) on right when text is present
- Placeholder text: "Search games..."
- Rounded borders, proper padding
- Focus state with ring effect

**GenreFilter**
- Custom styled select or native select with enhanced styling
- Dropdown icon
- Proper label (visible or sr-only)
- Matches SearchBar height and style

**GameCard**
- Card elevation with hover effect (lift + shadow increase)
- Image placeholder at top (aspect ratio 16:9)
- Genre badge positioned top-right or below image
- Title, description (truncated to 3 lines)
- Footer with price (large, bold) and release date (small, muted)
- Smooth transition on hover (transform: translateY(-4px))
- Cursor pointer
- Focus state for accessibility

**GameForm**
- Proper field spacing (1.5rem between fields)
- Labels with font-weight 500
- Inputs with consistent height (2.75rem)
- Textarea with min-height
- Error messages in red below fields
- Required field indicators (asterisk)
- Submit button: Full-width or right-aligned, primary color
- Disabled state styling for submit button

=== ADDITIONAL REQUIREMENTS ===

**Responsive Design:**
- Mobile-first approach
- Hamburger menu for navigation on mobile (if you add navigation)
- Touch-friendly button sizes (min 44px height)
- Readable font sizes on all devices

**Accessibility:**
- Maintain all ARIA labels and roles
- Proper focus indicators
- Color contrast ratios meet WCAG AA standards
- Screen reader friendly

**Animations & Transitions:**
- Subtle transitions (200-300ms) for hover states
- Page transitions if using routing animations
- Loading states with skeleton screens
- Smooth scrolling behavior

**Navigation (Global Component - Add if Missing):**
- Top navigation bar with logo, links (Home, Games, Admin), and user profile/login
- Sticky positioning
- Active route highlighting
- Mobile responsive hamburger menu

=== OUTPUT FORMAT ===

Please provide:
1. Complete, updated CSS file with all improvements (organized by sections: variables, global styles, components, pages)
2. Updated JSX for each component/page with new className structure
3. Brief explanation of key layout changes
4. Responsive breakpoints clearly documented
5. Any new utility classes created

Ensure all styles use the exact color scheme specified above and maintain consistency across the entire application. The design should feel modern, professional, and cohesive.