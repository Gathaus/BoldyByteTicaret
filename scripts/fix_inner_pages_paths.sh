#!/bin/bash

# Define the directory with inner pages
inner_pages_dir="web/templates/inner_pages"

# Process each HTML file in the inner_pages directory
for file in ${inner_pages_dir}/*.html; do
    echo "Fixing paths in $file"
    
    # Fix assets paths
    sed -i '' -e 's|src="assets/|src="/inner_pages/assets/|g' $file
    sed -i '' -e 's|href="assets/|href="/inner_pages/assets/|g' $file
    
    # Fix common assets paths
    sed -i '' -e 's|src="../common/|src="/common/|g' $file
    sed -i '' -e 's|href="../common/|href="/common/|g' $file
    
    # Fix links to other pages
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
    
    # Fix links to home page
    sed -i '' -e 's|href="../home_electronic/index.html"|href="/"|g' $file
    sed -i '' -e 's|href="#"|href="/"|g' $file
    
    echo "Fixed paths in $file"
done

echo "All inner pages paths fixed!" 