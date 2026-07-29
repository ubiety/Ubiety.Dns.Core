#!/usr/bin/env bash
#
# Builds the documentation site on Netlify.
#
# The Netlify build image has no .NET, so the SDK version pinned in global.json is installed into
# the build sandbox first. Run from the repository root.

set -euo pipefail

# /opt/build/cache persists between Netlify builds, so the SDK is downloaded once rather than
# on every deploy. Falls back to the home directory elsewhere.
if [ -d /opt/build/cache ]; then
    DOTNET_ROOT="${DOTNET_ROOT:-/opt/build/cache/dotnet}"
else
    DOTNET_ROOT="${DOTNET_ROOT:-$HOME/.dotnet}"
fi
export DOTNET_ROOT
export PATH="$DOTNET_ROOT:$PATH"

# Read the pinned version rather than hardcoding it, so global.json stays the single source.
SDK_VERSION=$(python3 -c "import json; print(json.load(open('global.json'))['sdk']['version'])")

if ! command -v dotnet >/dev/null 2>&1 || [ "$(dotnet --version 2>/dev/null || true)" != "$SDK_VERSION" ]; then
    echo "Installing .NET SDK $SDK_VERSION into $DOTNET_ROOT (about 236MB, cached for later builds)"
    curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh
    bash /tmp/dotnet-install.sh --version "$SDK_VERSION" --install-dir "$DOTNET_ROOT" --no-path
fi

dotnet --info | head -4

# docfx is pinned in .config/dotnet-tools.json.
dotnet tool restore
dotnet docfx docs/docfx.json --warningsAsErrors

test -f docs/_site/index.html || { echo "docs/_site/index.html missing; the build produced nothing"; exit 1; }
echo "Documentation built: $(find docs/_site -name '*.html' | wc -l | tr -d ' ') pages"
