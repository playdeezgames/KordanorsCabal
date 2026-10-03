#!/bin/bash
# Runs the suite on both targets and returns non-zero if either fails. Usage: ./run_tests.sh [extra odin flags]
set -eo pipefail
cd "$(dirname "$0")"
ODIN_JS="$(odin root)/core/sys/wasm/js/odin.js"
echo "== native (odin test)"; odin test . -out:tests_native "$@" -define:ODIN_TEST_THREADS=1 2>&1 | tee /dev/stderr | grep -q "TESTS PASSED"
echo "== wasm under node"; odin build . -target:js_wasm32 -out:tests.wasm "$@" && ODIN_JS="$ODIN_JS" node run_wasm_node.js tests.wasm
