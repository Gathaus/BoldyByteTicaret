# Frontend Templates for Server-Side Rendering

This directory contains the templates and static assets needed for server-side rendering of the frontend HTML pages.

## Directory Structure

- `templates/`: Contains HTML templates for server-side rendering
  - `home_electronic/`: HTML templates for the home electronic theme
    - `index.html`: Main homepage template
- `static/`: Contains static assets (CSS, JS, images)
  - `home_electronic/assets/`: Theme-specific assets
    - `css/`: CSS files
    - `js/`: JavaScript files
    - `img/`: Images
  - `common/assets/`: Shared assets across themes
    - `css/`: CSS files
    - `js/`: JavaScript files
    - `img/`: Images

## Deployment

To deploy frontend templates, run the copy_static_files.sh script from the root of the project:

```bash
cd ecommerce-api
./scripts/copy_static_files.sh
```

This will copy the necessary files from the frontend directory to the backend web directory.

## Path Structure for Templates

The templates use the following URL structure for assets:

- `/static/home_electronic/assets/...`: For theme-specific assets
- `/static/common/assets/...`: For shared assets across themes

## Modifying Templates

When modifying the templates:

1. Update the original files in the frontend directory
2. Run the copy_static_files.sh script to update the files in the backend 