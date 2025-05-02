#!/bin/bash

# Script to copy static files from frontend to backend for server-side rendering

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
mkdir -p ${BACKEND_DIR}/common/assets/fonts
mkdir -p ${BACKEND_DIR}/inner_pages/assets/js
mkdir -p ${BACKEND_DIR}/inner_pages/assets/css
mkdir -p ${BACKEND_DIR}/inner_pages/assets/img

# Copy HTML template
echo "Copying HTML templates..."
cp ${FRONTEND_DIR}/home_electronic/index.html ${BACKEND_DIR}/home_electronic/

# Copy inner pages templates
echo "Copying inner pages templates..."
cp ${FRONTEND_DIR2}/inner_pages/*.html ${BACKEND_DIR}/inner_pages/ 2>/dev/null || :

# Copy home_electronic assets
echo "Copying home_electronic assets..."
cp -R ${FRONTEND_DIR}/home_electronic/assets/js/* ${BACKEND_DIR}/home_electronic/assets/js/ 2>/dev/null || :
cp -R ${FRONTEND_DIR}/home_electronic/assets/css/* ${BACKEND_DIR}/home_electronic/assets/css/ 2>/dev/null || :
cp -R ${FRONTEND_DIR}/home_electronic/assets/img/* ${BACKEND_DIR}/home_electronic/assets/img/ 2>/dev/null || :

# Copy inner pages assets
echo "Copying inner pages assets..."
cp -R ${FRONTEND_DIR2}/inner_pages/assets/js/* ${BACKEND_DIR}/inner_pages/assets/js/ 2>/dev/null || :
cp -R ${FRONTEND_DIR2}/inner_pages/assets/css/* ${BACKEND_DIR}/inner_pages/assets/css/ 2>/dev/null || :
cp -R ${FRONTEND_DIR2}/inner_pages/assets/img/* ${BACKEND_DIR}/inner_pages/assets/img/ 2>/dev/null || :

# Copy common assets
echo "Copying common assets..."
cp -R ${FRONTEND_DIR}/common/assets/css/* ${BACKEND_DIR}/common/assets/css/ 2>/dev/null || :
cp -R ${FRONTEND_DIR}/common/assets/js/* ${BACKEND_DIR}/common/assets/js/ 2>/dev/null || :
cp -R ${FRONTEND_DIR}/common/assets/fonts/* ${BACKEND_DIR}/common/assets/fonts/ 2>/dev/null || :
cp -R ${FRONTEND_DIR2}/common/assets/css/* ${BACKEND_DIR}/common/assets/css/ 2>/dev/null || :
cp -R ${FRONTEND_DIR2}/common/assets/js/* ${BACKEND_DIR}/common/assets/js/ 2>/dev/null || :
cp -R ${FRONTEND_DIR2}/common/assets/fonts/* ${BACKEND_DIR}/common/assets/fonts/ 2>/dev/null || :

echo "Done copying static files." 