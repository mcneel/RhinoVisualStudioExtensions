#!/usr/bin/env bash
# Builds and installs the templates, then creates a C++ utility plug-in with the SDK submodule.
# Usage: build/new-cpp-plugin.sh [name] [output-dir]
#   defaults: name=MyCppPlugin, output-dir=artifacts/cpp/<name>
set -euo pipefail

cwd="$(pwd)"
repo="$(cd "$(dirname "$0")/.." && pwd)"
name="${1:-MyCppPlugin}"
out="${2:-$repo/artifacts/cpp/$name}"
config=Debug

if [ -e "$out" ]; then
  echo "error: $out already exists" >&2
  exit 1
fi

echo "== Building templates"
dotnet build "$repo/Rhino.Templates/Rhino.Templates.csproj" -c $config -clp:NoSummary

# Pick the newest package in case older versions are still in the folder.
package="$(ls -t "$repo"/artifacts/bin/Rhino.Templates/$config/Rhino.Templates.*.nupkg | head -1)"

echo "== Uninstalling $(basename "$package")"
dotnet new uninstall "Rhino.Templates"

echo "== Installing $(basename "$package")"
dotnet new install "$package" --force

echo "== Creating $name in $out"
dotnet new rhino -lang cpp -n "$name" -o "$out"

echo "== Adding SDK submodule"
cd "$out"
git init -q
git submodule add https://github.com/mcneel/rhino_sdk_cpp.git SDK

echo "== Done: open $out/$name.xcodeproj"
cd "$cwd"
open "$out/$name.xcodeproj"
