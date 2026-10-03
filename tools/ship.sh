#!/bin/bash
# Packages the web build for itch.io (HTML5). Usage:
#   tools/ship.sh            build the release web build and write build/kordanors-cabal-html5.zip
#   tools/ship.sh --push     also upload it with butler (channel "html5" of thegrumpygamedev/kordanors-cabal); needs `butler login`
# Pushing is never done unless --push is given. The old shippit.sh (VB builds) is untouched.
set -euo pipefail
cd "$(dirname "$0")/.."
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
if [ "${1:-}" = "--push" ]; then
	butler push build/kordanors-cabal-html5.zip thegrumpygamedev/kordanors-cabal:html5
else
	echo "not pushed (add --push to upload)"
fi
