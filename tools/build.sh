#!/bin/bash
# Builds the Odin port. Usage: tools/build.sh [web|native|all]   (default: all). Output goes to build/ (git-ignored).
set -euo pipefail
cd "$(dirname "$0")/.."
what="${1:-all}"
OPTS="-collection:kc=odin -vet-shadowing -vet-unused"
ODIN_JS="$(odin root)/core/sys/wasm/js/odin.js"
CONTENT=src/KordanorsCabal/Content

build_web() {
	mkdir -p build/web/assets
	odin build odin/platform/web -target:js_wasm32 -out:build/web/platform.wasm -collection:kc=odin ${ODIN_FLAGS:-}
	cp odin/platform/web/page/index.html odin/platform/web/page/platform.js odin/platform/web/page/layout.js odin/platform/web/page/storage.js build/web/
	cp "$ODIN_JS" build/web/odin.js
	cp "$CONTENT"/*.wav "$CONTENT"/*.ogg build/web/assets/
	echo "web build: build/web (serve with tools/serve.sh)"
}
build_native() {
	mkdir -p build/native
	odin build odin/platform/native -out:build/native/kordanors-cabal -collection:kc=odin ${ODIN_FLAGS:-}
	mkdir -p build/native/assets && cp "$CONTENT"/*.wav "$CONTENT"/*.ogg build/native/assets/
	echo "native build: build/native/kordanors-cabal (run from build/native)"
}
case "$what" in
	web) build_web ;;
	native) build_native ;;
	all) build_web; build_native ;;
	*) echo "usage: $0 [web|native|all]"; exit 2 ;;
esac
