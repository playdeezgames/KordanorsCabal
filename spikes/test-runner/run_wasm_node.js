// Run an Odin js_wasm32 module under node (no browser): node run_wasm_node.js file.wasm
const fs = require("fs"), vm = require("vm");
const odinSrc = fs.readFileSync(process.env.ODIN_JS, "utf8");
global.window = global; global.self = global;
global.requestAnimationFrame = global.window.requestAnimationFrame = (f) => setTimeout(() => f(performance.now()), 0);
vm.runInThisContext(odinSrc);
(async () => {
  const wasmPath = process.argv[2];
  const origFetch = global.fetch;
  global.fetch = async (p) => ({ arrayBuffer: async () => fs.readFileSync(p) });
  const mem = new odin.WasmMemoryInterface();
  let captured = "";
  const origLog = console.log; console.log = (...a) => { captured += a.join(" ") + "\n"; origLog(...a); };
  await odin.runWasm(wasmPath, null, {}, mem);
  if (/TESTS FAILED/.test(captured) || !/TESTS PASSED/.test(captured)) { console.error("wasm test run did not pass"); process.exit(1); }
})().catch((e) => { console.error("FAILED:", e); process.exit(1); });
