#!/bin/bash

# Define the file to fix
file="web/templates/home_electronic/index.html"

# Fix paths in the HTML file
sed -i '' -e 's|src="/inner_pages/assets/|src="/assets/|g' $file
sed -i '' -e 's|href="/inner_pages/assets/|href="/assets/|g' $file

echo "Paths fixed in $file" 