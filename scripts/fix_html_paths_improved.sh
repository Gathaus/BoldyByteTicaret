#!/bin/bash

# Improved script to fix the paths in HTML files for proper serving from our backend

# Set directories
TEMPLATES_DIR="web/templates"

echo "Fixing paths in HTML files..."

# Function to fix paths in a file
fix_paths() {
    local file=$1
    echo "Processing $file"
    
    # Fix CSS and JS paths
    sed -i '' -e 's|="../common/assets/|="/common/assets/|g' $file
    sed -i '' -e 's|="../home_electronic/assets/|="/home_electronic/assets/|g' $file
    sed -i '' -e 's|="../home_tech/assets/|="/home_electronic/assets/|g' $file
    sed -i '' -e 's|="assets/|="/inner_pages/assets/|g' $file
    
    # Fix internal links to other pages
    sed -i '' -e 's|href="about.html"|href="/about"|g' $file
    sed -i '' -e 's|href="contact.html"|href="/contact"|g' $file
    sed -i '' -e 's|href="login.html"|href="/login"|g' $file
    sed -i '' -e 's|href="profile.html"|href="/profile"|g' $file
    sed -i '' -e 's|href="register.html"|href="/register"|g' $file
    sed -i '' -e 's|href="products.html"|href="/products"|g' $file
    sed -i '' -e 's|href="products_layout_2.html"|href="/products-layout-2"|g' $file
    sed -i '' -e 's|href="cart.html"|href="/cart"|g' $file
    sed -i '' -e 's|href="checkout.html"|href="/checkout"|g' $file
    sed -i '' -e 's|href="single_product.html"|href="/single-product"|g' $file
    sed -i '' -e 's|href="single_product_pay.html"|href="/single-product-pay"|g' $file
    
    # Fix external links to other pages
    sed -i '' -e 's|href="https://ui-themez.smartinnovates.net/items/swoo_html/inner_pages/|href="/|g' $file
    
    # Fix home link including those with # or empty
    sed -i '' -e 's|href="#"|href="/"|g' $file
    sed -i '' -e 's|href="./"|href="/"|g' $file
    sed -i '' -e 's|href="index.html"|href="/"|g' $file
    
    # Fix remaining image paths
    sed -i '' -e 's|src="img/|src="/inner_pages/assets/img/|g' $file
    
    # Fix more specific image paths for specific parts of the HTML 
    sed -i '' -e 's|src="assets/img/|src="/inner_pages/assets/img/|g' $file
    
    # Fix absolute URLs to the original site
    sed -i '' -e 's|href="https://ui-themez.smartinnovates.net/items/swoo_html/home_electronic/index.html"|href="/"|g' $file
    sed -i '' -e 's|href="https://ui-themez.smartinnovates.net/items/swoo_html/home_baby/index.html"|href="/"|g' $file
    sed -i '' -e 's|href="https://ui-themez.smartinnovates.net/items/swoo_html/home_lighting/index.html"|href="/"|g' $file
    sed -i '' -e 's|href="https://ui-themez.smartinnovates.net/items/swoo_html/home_pets/index.html"|href="/"|g' $file
    sed -i '' -e 's|href="https://ui-themez.smartinnovates.net/items/swoo_html/home_tech/index.html"|href="/"|g' $file
    
    echo "Fixed paths in $file"
}

# Fix inner_pages first
for file in ${TEMPLATES_DIR}/inner_pages/*.html; do
    fix_paths "$file"
done

# Fix home_electronic pages
for file in ${TEMPLATES_DIR}/home_electronic/*.html; do
    fix_paths "$file"
done

echo "Done fixing paths in HTML files." 