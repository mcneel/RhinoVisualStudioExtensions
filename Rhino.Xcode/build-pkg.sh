#!/bin/sh
# Builds the Xcode templates installer into artifacts/.
# Usage: ./build-pkg.sh ["Developer ID Installer: Name (TEAMID)"]
set -e
here=$(cd "$(dirname "$0")" && pwd)
out="$here/../artifacts/xcode"
version=$(sed -n 's:.*<Version>\(.*\)</Version>.*:\1:p' "$here/../Directory.Build.props")
pkg="$out/RhinoXcodeTemplates-$version.pkg"

rm -rf "$out"
mkdir -p "$out/root"
# Keep Finder metadata and extended attributes out of the payload.
ditto --norsrc --noextattr --noqtn "$here/Templates" "$out/root"
find "$out/root" -name .DS_Store -delete
# Stored without the colon so the repo checks out on Windows.
cmd="$out/root/File Templates/Rhino/Rhino Command.xctemplate"
mv "$cmd/cmd___VARIABLE_productName___.cpp" "$cmd/cmd___VARIABLE_productName:identifier___.cpp"

set -- ${1:+--sign "$1"}
COPYFILE_DISABLE=1 pkgbuild \
  --root "$out/root" \
  --install-location "/Library/Application Support/McNeel/Rhino Xcode Templates" \
  --scripts "$here/Scripts" \
  --identifier com.mcneel.rhino.xcodetemplates \
  --version "$version" \
  "$@" \
  "$pkg"
rm -rf "$out/root"
echo "Created $pkg"
