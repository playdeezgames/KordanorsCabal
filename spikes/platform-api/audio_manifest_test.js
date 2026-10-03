// node audio_manifest_test.js : the JS sound list must follow the Sfx enum in api.odin, and every file must exist.
const fs = require("fs");
const api = fs.readFileSync("api.odin", "utf8");
const enumBody = api.match(/Sfx :: enum u8 \{([^}]*)\}/)[1];
const names = enumBody.split(",").map((s) => s.trim()).filter(Boolean);
const js = fs.readFileSync("platform.js", "utf8");
const manifest = [...js.matchAll(/\["(\w+)", "([\w.]+\.wav)"\]/g)].map((m) => [m[1], m[2]]);
let bad = 0; const check = (ok, msg) => { if (!ok) { bad++; console.log("FAIL:", msg); } };
check(names[0] === "None", "Sfx starts with None");
check(manifest.length === names.length - 1, `manifest has ${manifest.length} sounds, enum has ${names.length - 1}`);
manifest.forEach(([n, f], i) => { check(names[i + 1] === n, `slot ${i + 1}: enum ${names[i + 1]} vs js ${n}`); check(fs.existsSync("assets/" + f), `missing assets/${f}`); });
console.log(bad ? bad + " problems" : `ok: ${manifest.length} sounds match the Sfx enum and exist`);
process.exit(bad ? 1 : 0);
