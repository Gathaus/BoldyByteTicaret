# Inner Pages Integration Documentation

This document explains how the inner pages from `swoo_html2` have been integrated with the Go backend for server-side rendering.

## Overview

In addition to the main home page from `swoo_html`, inner pages from `swoo_html2/inner_pages` have been integrated. These pages include:

- products.html - Product listing page
- products_layout_2.html - Alternative product listing layout
- single_product.html - Single product details page
- single_product_pay.html - Single product payment page
- cart.html - Shopping cart page
- checkout.html - Checkout page
- profile.html - User profile page
- login.html - Login page
- register.html - Registration page
- about.html - About page
- contact.html - Contact page

## Directory Structure

```
ecommerce-api/
├── web/
│   ├── templates/
│   │   ├── home_electronic/       # Home Electronic theme templates
│   │   │   └── index.html         # Main homepage template
│   │   ├── inner_pages/           # Inner pages templates
│   │   │   ├── products.html
│   │   │   ├── products_layout_2.html
│   │   │   ├── single_product.html
│   │   │   └── ... other inner pages
│   ├── static/                    # Static assets (CSS, JS, images)
│   │   ├── home_electronic/assets/
│   │   ├── inner_pages/assets/
│   │   ├── common/assets/
```

## Routes Configuration

All inner pages are configured in `pkg/api/routes/routes.go` with the following URL patterns:

- `/` - Home page (home_electronic)
- `/products` - Products listing
- `/products-layout-2` - Alternative products layout
- `/single-product` - Single product details
- `/single-product-pay` - Single product payment
- `/cart` - Shopping cart
- `/checkout` - Checkout
- `/profile` - User profile
- `/login` - Login
- `/register` - Registration
- `/about` - About page
- `/contact` - Contact page

## Asset Serving

Static assets are served with the following path mappings:

- `/assets/*` - Served from `./web/templates/home_electronic/assets/*`
- `/inner_pages/assets/*` - Served from `./web/templates/inner_pages/assets/*`
- `/common/assets/*` - Served from `./web/templates/common/assets/*`

## How to Add New Inner Pages

If you need to add more pages from swoo_html2, follow these steps:

1. Copy the HTML file to `web/templates/inner_pages/`
2. Update the paths in the HTML file (can use the `scripts/fix_html_paths.sh` script)
3. Add a new handler method in `pkg/api/handlers/static_handler.go`
4. Add a new route in the `setupStaticRoutes` function in `pkg/api/routes/routes.go`

### Example:

To add a new page "new_page.html":

1. Copy the file to `web/templates/inner_pages/new_page.html`
2. Run `./scripts/fix_html_paths.sh` to fix paths
3. Add a handler in static_handler.go:
   ```go
   // NewPage handles the new page request
   func (h *StaticHandler) NewPage(c *gin.Context) {
       c.File("./web/templates/inner_pages/new_page.html")
   }
   ```
4. Add a route in routes.go:
   ```go
   router.GET("/new-page", staticHandler.NewPage)
   ```

## Path Fixing Script

The `scripts/fix_html_paths.sh` script is provided to automatically fix paths in HTML files. It converts relative paths like `../common/assets/` to absolute paths like `/common/assets/` to work with our server structure. 