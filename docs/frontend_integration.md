# Frontend Integration Documentation

This document explains how the static HTML frontend (Swoo HTML template) has been integrated with the Go backend for server-side rendering.

## Overview

The frontend HTML template is served directly from the backend using Gin's static file serving capabilities. This approach allows us to:

1. Serve the HTML templates server-side
2. Use the same backend for both API endpoints and static pages
3. Eventually integrate dynamic data from the backend into the templates

## Directory Structure

```
ecommerce-api/
├── web/
│   ├── templates/                 # HTML templates
│   │   ├── home_electronic/       # Home Electronic theme templates
│   │   │   └── index.html
│   ├── static/                    # Static assets (CSS, JS, images)
│   │   ├── home_electronic/       # Theme-specific assets
│   │   │   ├── assets/
│   │   │   │   ├── css/
│   │   │   │   ├── js/
│   │   │   │   └── img/
│   │   ├── common/               # Shared assets across themes
│   │   │   ├── assets/
│   │   │   │   ├── css/
│   │   │   │   ├── js/
│   │   │   │   └── img/
```

## Implementation Details

### Server-side Routes

The static pages are served using the following route patterns:

- `/` - Home page (index.html)
- `/static/home_electronic/...` - Theme-specific assets
- `/static/common/...` - Shared assets
- 404 handler for any other routes

### Code Structure

1. `StaticHandler` - Handles requests for static HTML pages
   - Located at `pkg/api/handlers/static_handler.go`
   - Uses services to fetch dynamic data when needed

2. Static routes setup
   - Located at `pkg/api/routes/routes.go` in `setupStaticPageRoutes` function
   - Configures routes for static assets and HTML pages

### Deployment Process

To deploy the frontend templates:

1. Place the HTML files in `web/templates/{theme}/`
2. Place the static assets in `web/static/{theme}/assets/` and `web/static/common/assets/`
3. Update the HTML files to use the correct paths for assets

You can use the provided script to copy files from an existing frontend directory:

```bash
cd ecommerce-api
./scripts/copy_static_files.sh
```

## Customization

### Modifying Templates

1. Edit the HTML files in `web/templates/{theme}/`
2. Update CSS and JavaScript files in `web/static/{theme}/assets/` and `web/static/common/assets/`

### Dynamic Content

To add dynamic content to the templates, you can:

1. Extend the `StaticHandler` to fetch data from services
2. Create HTML templates with Go's template engine
3. Pass data to the templates before rendering

Example:
```go
func (h *StaticHandler) Home(c *gin.Context) {
    // Fetch products data
    products, _, err := h.productService.GetProducts(c, 10, 0)
    if err != nil {
        c.HTML(http.StatusInternalServerError, "error.html", gin.H{
            "error": "Failed to load products",
        })
        return
    }
    
    // Render template with data
    c.HTML(http.StatusOK, "index.html", gin.H{
        "products": products,
    })
}
```

## Future Improvements

1. Implement Go templates for dynamic content insertion
2. Add caching for better performance
3. Create more theme variants
4. Integrate user data when authenticated
5. Add server-side form processing for contact forms, search, etc. 