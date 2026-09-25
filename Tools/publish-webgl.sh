#!/usr/bin/env bash
set -euo pipefail

if [[ $# -lt 1 ]]; then
  echo "Usage: bash Tools/publish-webgl.sh /path/to/webgl-build.zip [release-tag]" >&2
  exit 2
fi

ZIP_PATH="$1"

if [[ ! -f "$ZIP_PATH" ]]; then
  echo "WebGL zip not found: $ZIP_PATH" >&2
  exit 2
fi

if ! command -v gh >/dev/null 2>&1; then
  echo "GitHub CLI (gh) is required. Install it, then run: gh auth login" >&2
  exit 2
fi

if ! command -v unzip >/dev/null 2>&1; then
  echo "unzip is required." >&2
  exit 2
fi

WEBGL_ROOT="$(
  unzip -Z1 "$ZIP_PATH" |
    awk -F/ '/^BridgeTrollSimulator\..*-WebGL\// { print $1; exit }'
)"

if [[ -z "$WEBGL_ROOT" ]]; then
  echo "Could not find a BridgeTrollSimulator.<version>-WebGL root folder in the zip." >&2
  exit 1
fi

VERSION="${WEBGL_ROOT#BridgeTrollSimulator.}"
VERSION="${VERSION%-WebGL}"
TAG="${2:-webgl-$VERSION}"
ASSET_NAME="${WEBGL_ROOT}-ready.zip"

echo "WebGL root : $WEBGL_ROOT"
echo "Version    : $VERSION"
echo "Release tag: $TAG"
echo "Asset name : $ASSET_NAME"

if gh release view "$TAG" >/dev/null 2>&1; then
  echo "Updating existing release asset..."
  gh release upload "$TAG" "$ZIP_PATH#$ASSET_NAME" --clobber

  echo "Starting GitHub Pages deployment..."
  gh workflow run deploy-webgl-pages.yml --ref main -f tag="$TAG"
else
  echo "Creating release..."
  gh release create "$TAG" "$ZIP_PATH#$ASSET_NAME" \
    --title "Bridge Troll Simulator WebGL $VERSION" \
    --notes "Playable WebGL build of Bridge Troll Simulator $VERSION."

  echo "The published-release event will start the GitHub Pages deployment automatically."
fi

echo
echo "Check deployment status with:"
echo "  gh run list --workflow deploy-webgl-pages.yml --limit 5"
