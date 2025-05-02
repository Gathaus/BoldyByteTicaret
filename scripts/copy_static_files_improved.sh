#!/bin/bash

# Improved script to copy static files from frontend to backend for server-side rendering

# Set base directories (using absolute paths for reliability)
SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
PROJECT_ROOT="$( cd "$SCRIPT_DIR/.." && pwd )"
FRONTEND_DIR="$( cd "$PROJECT_ROOT/../../frontend/swoo_html" && pwd )"
FRONTEND_DIR2="$( cd "$PROJECT_ROOT/../../frontend/swoo_html2" && pwd )"
BACKEND_DIR="$PROJECT_ROOT/web/templates"

echo "Copying files from $FRONTEND_DIR and $FRONTEND_DIR2 to $BACKEND_DIR"

# Create required directories if they don't exist
mkdir -p ${BACKEND_DIR}/home_electronic/assets/js
mkdir -p ${BACKEND_DIR}/home_electronic/assets/css
mkdir -p ${BACKEND_DIR}/home_electronic/assets/img
mkdir -p ${BACKEND_DIR}/common/assets/css/lib
mkdir -p ${BACKEND_DIR}/common/assets/js/lib
mkdir -p ${BACKEND_DIR}/common/assets/js/gsap_lib
mkdir -p ${BACKEND_DIR}/common/assets/fonts
mkdir -p ${BACKEND_DIR}/inner_pages/assets/js
mkdir -p ${BACKEND_DIR}/inner_pages/assets/css
mkdir -p ${BACKEND_DIR}/inner_pages/assets/img/products
mkdir -p ${BACKEND_DIR}/inner_pages/assets/img/brands
mkdir -p ${BACKEND_DIR}/inner_pages/assets/img/payment
mkdir -p ${BACKEND_DIR}/inner_pages/assets/img/header

# Copy HTML templates
echo "Copying HTML templates..."
cp ${FRONTEND_DIR}/home_electronic/index.html ${BACKEND_DIR}/home_electronic/

# Copy inner pages templates
echo "Copying inner pages templates..."
cp ${FRONTEND_DIR2}/inner_pages/*.html ${BACKEND_DIR}/inner_pages/ 2>/dev/null || :

# Copy home_electronic assets recursively
echo "Copying home_electronic assets... (this may take a moment)"
cp -R ${FRONTEND_DIR}/home_electronic/assets/* ${BACKEND_DIR}/home_electronic/assets/ 2>/dev/null || :

# Copy inner pages assets recursively
echo "Copying inner pages assets... (this may take a moment)"
cp -R ${FRONTEND_DIR2}/inner_pages/assets/* ${BACKEND_DIR}/inner_pages/assets/ 2>/dev/null || :

# Copy both common assets directories from both frontend directories
echo "Copying common assets... (this may take a moment)"
cp -R ${FRONTEND_DIR}/common/assets/* ${BACKEND_DIR}/common/assets/ 2>/dev/null || :
cp -R ${FRONTEND_DIR2}/common/assets/* ${BACKEND_DIR}/common/assets/ 2>/dev/null || :

# Fix permissions
echo "Setting permissions..."
find ${BACKEND_DIR} -type d -exec chmod 755 {} \;
find ${BACKEND_DIR} -type f -exec chmod 644 {} \;

echo "Done copying static files."

# Run the HTML path fixer
echo "Now running HTML path fixer..."
${SCRIPT_DIR}/fix_html_paths_improved.sh 