#!/bin/bash
# Packages the web build for itch.io (HTML5). Usage:
#   tools/ship.sh            build the release web build and write build/kordanors-cabal-html5.zip
#   tools/ship.sh --push     also upload it with butler (TARGET below); needs `butler login`
# Pushing is never done unless --push is given.
set -euo pipefail
cd "$(dirname "$0")/.."
# Page: https://thegrumpygamedev.itch.io/kordanors-cabal (keep the channel name html5: the live page uses it)
TARGET="thegrumpygamedev/kordanors-cabal:html5"
./tools/test.sh
ODIN_FLAGS="-o:size" ./tools/build.sh web
python3 - <<'PY'
import os, zipfile
src, out = "build/web", "build/kordanors-cabal-html5.zip"
with zipfile.ZipFile(out, "w", zipfile.ZIP_DEFLATED, compresslevel=9) as z:
    for root, _, files in os.walk(src):
        for f in sorted(files):
            p = os.path.join(root, f)
            z.write(p, os.path.relpath(p, src))   # index.html must be at the root of the zip
size = os.path.getsize(out)
print(f"{out}: {size/1024:.0f} KiB, {len(zipfile.ZipFile(out).namelist())} files")
PY
unzip -l build/kordanors-cabal-html5.zip | tail -5
if [ "${1:-}" = "--push" ]; then
	[ -z "$(git status --porcelain)" ] || echo "WARNING: the working tree is dirty; butler ships the tree, not a commit"
	butler push build/kordanors-cabal-html5.zip "$TARGET"
	echo "pushed to $TARGET; check processing with: butler status $TARGET"
else
	echo "not pushed (add --push to upload to $TARGET)"
fi
