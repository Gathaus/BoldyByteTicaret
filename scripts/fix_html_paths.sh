#!/bin/bash

# Script to fix the paths in HTML files for proper serving from our backend

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
    
    # Fix home link
    sed -i '' -e 's|href="#"|href="/"|g' $file
    
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

# Define the file to fix
file="web/templates/home_electronic/index.html"

# Fix paths in the HTML file
sed -i '' -e 's|src="assets/|src="/assets/|g' $file
sed -i '' -e 's|href="assets/|href="/assets/|g' $file
sed -i '' -e 's|src="../common/|src="/common/|g' $file
sed -i '' -e 's|href="../common/|href="/common/|g' $file

echo "Paths fixed in $file"

echo "Done fixing paths in HTML files." 