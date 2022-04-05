#!/usr/bin/env bash

CURRENT_DIR=$( cd -- "$( dirname -- "${BASH_SOURCE[0]}" )" &> /dev/null && pwd )
LIBRARY_DIR="$CURRENT_DIR/Assets/Libraries"

# echo "$LIBRARY_DIR"

# delete the flowing files and folders: 
#   /Assets/Libraries/PhoenixTests and its children
#   /Assets/Libraries/Vendor/BestHTTP and its children
#   /Assets/Libraries/Vendor/nunit.framework.dll
#   /Assets/Libraries/Vendor/NSubstitute.dll
rm -rf "$LIBRARY_DIR/PhoenixTests"
rm -rf "$LIBRARY_DIR/Vendor/BestHTTP"
rm -rf "$LIBRARY_DIR/Vendor/nunit.framework.dll"      
rm -rf "$LIBRARY_DIR/Vendor/NSubstitute.dll"   

echo "OK"

