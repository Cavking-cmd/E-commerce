# E-commerce Frontend

This is a plain HTML + Tailwind CSS + JavaScript frontend for the E-commerce application.

## Features

- User authentication (login, profile)
- Product browsing (home, product list, product details)
- Shopping cart management
- Order creation, listing, and details
- Wishlist management
- Vendor management
- Admin area (optional)
- Payment integration

## Setup

1. Open the HTML files in a modern browser.
2. The frontend communicates with the backend API via JavaScript fetch calls.
3. Update the `API_BASE_URL` variable in each HTML file's script section to point to your ASP.NET Core backend API URL.
4. Authentication uses JWT tokens stored in `localStorage`.

## Usage

- Start with `index.html` for product browsing.
- Use `login.html` to authenticate.
- Navigate to other pages as needed.

## Notes

- This frontend uses Tailwind CSS via CDN for styling.
- No frameworks are used; all code is plain HTML, CSS, and JavaScript.
- Customize and extend as needed.
