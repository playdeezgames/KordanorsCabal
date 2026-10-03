#!/bin/bash
# Runs every test of the Odin port; exits non-zero on any failure. Usage: tools/test.sh
set -euo pipefail
cd "$(dirname "$0")/.."
ODIN_JS="$(odin root)/core/sys/wasm/js/odin.js"
mkdir -p build
fail=0
step() { echo; echo "== $1"; }

step "generated files are up to date"
GENERATED="odin/game/font_data.odin odin/tests/reference_data.odin odin/game/content_enums.odin odin/game/content_data.odin odin/game/ui_data.odin"
for f in $GENERATED; do cp "$f" "build/$(basename "$f").before"; done
python3 tools/gen/gen_font.py >/dev/null && python3 tools/gen/gen_reference.py >/dev/null && python3 tools/gen/gen_content.py >/dev/null && python3 tools/gen/gen_ui.py >/dev/null
for f in $GENERATED; do cmp -s "$f" "build/$(basename "$f").before" || { echo "FAIL: $f changed when regenerated; run the generators and commit"; fail=1; }; done
[ "$fail" = 0 ] && echo "ok"

step "native suite (odin test)"
odin test odin/tests -collection:kc=odin -out:build/tests_native -define:ODIN_TEST_THREADS=1 2>&1 | tee build/native_tests.log | grep -E "^(FAIL|TESTS|    FAIL)|Finished" || true
grep -q "TESTS PASSED" build/native_tests.log || fail=1

step "wasm suite under node"
odin build odin/tests -target:js_wasm32 -out:build/tests.wasm -collection:kc=odin
ODIN_JS="$ODIN_JS" node tools/run_wasm_node.js build/tests.wasm 2>&1 | grep -E "^(FAIL|TESTS|    FAIL)" || fail=1

step "web page logic (node)"
(cd odin/platform/web/page && node layout_test.js | tail -1 && node audio_manifest_test.js | tail -1) || fail=1

step "type check of the shipping targets"
odin check odin/platform/web -target:js_wasm32 -collection:kc=odin -no-entry-point >/dev/null 2>&1 && odin check odin/platform/native -collection:kc=odin -no-entry-point >/dev/null 2>&1 && echo "ok" || { echo "FAIL: platform packages do not type check"; fail=1; }

echo
if [ "$fail" = 0 ]; then echo "ALL TESTS PASSED"; else echo "SOME TESTS FAILED"; exit 1; fi
