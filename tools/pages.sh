#!/bin/bash
# Prepares the GitHub Pages site. Usage: tools/pages.sh
# Runs the tests, makes the release web build and copies it to site/, which .github/workflows/pages.yml publishes
# when it is pushed to main. site/ is committed output (like pub-*): rebuild and commit it to ship a new version.
set -euo pipefail
cd "$(dirname "$0")/.."
./tools/test.sh
ODIN_FLAGS="-o:size" ./tools/build.sh web
rm -rf site
mkdir -p site
cp -r build/web/. site/
touch site/.nojekyll
du -sh site
echo "site/ is ready: commit it and push to main to publish"
