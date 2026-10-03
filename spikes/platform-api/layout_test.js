// node layout_test.js : checks the layout rules against real device viewports (CSS pixels).
const { computeLayout } = require("./layout.js");
const devices = [
	// name, width, height (landscape CSS px with the browser chrome hidden), touch, insets
	["iPhone SE landscape", 667, 375, true], ["iPhone 12/13/14 landscape", 844, 390, true, { left: 47, right: 47 }], ["iPhone 14 Pro Max landscape", 932, 430, true, { left: 59, right: 59 }],
	["Pixel 7 landscape", 915, 412, true], ["Galaxy S8 landscape", 740, 360, true], ["small Android landscape", 640, 360, true],
	["iPad landscape", 1024, 768, true], ["iPad Pro 12.9 landscape", 1366, 1024, true],
	["desktop 1280x720", 1280, 720, false], ["desktop 1920x1080", 1920, 1080, false], ["desktop small 600x400", 600, 400, false],
	["iPhone 14 portrait", 390, 844, true], ["Pixel 7 portrait", 412, 915, true],
];
let failures = 0;
const check = (ok, msg) => { if (!ok) { failures++; console.log("  FAIL:", msg); } };
for (const [name, width, height, touch, insets] of devices) {
	const L = computeLayout({ width, height, touch, insets });
	const line = L.mode === "rotate" ? "rotate prompt" :
		`${L.mode.padEnd(11)} scale ${L.scale.toFixed(2)} paper ${Math.round(L.paper.w)}x${Math.round(L.paper.h)} cell ${L.cell.w.toFixed(1)}x${L.cell.h.toFixed(1)} upscale ${L.upscale}` +
		(L.controls ? ` buttons ${Math.round(Math.min(...Object.values(L.controls).map(r => Math.min(r.w, r.h))))}px+` : " no controls");
	console.log(name.padEnd(30), `${width}x${height}`.padEnd(10), line);
	if (L.mode === "rotate") { check(touch && height > width, "rotate only for touch portrait"); continue; }
	const ins = { left: 0, right: 0, top: 0, bottom: 0, ...insets };
	const p = L.paper;
	check(p.x >= ins.left - 0.01 && p.x + p.w <= width - ins.right + 0.01 && p.y >= ins.top - 0.01 && p.y + p.h <= height - ins.bottom + 0.01, "paper inside the safe area");
	if (!touch) { check(L.scale < 1 || Number.isInteger(L.scale), "integer scale on desktop"); check(!L.controls, "no controls on desktop"); }
	if (L.controls) {
		const rects = Object.entries(L.controls);
		for (const [n, r] of rects) {
			check(Math.min(r.w, r.h) >= 40, `${n} button at least 40px (${Math.round(Math.min(r.w, r.h))})`);
			check(r.x >= ins.left && r.x + r.w <= width - ins.right && r.y >= 0 && r.y + r.h <= height, `${n} button on screen`);
			check(r.x + r.w <= p.x + 0.01 || r.x >= p.x + p.w - 0.01, `${n} button does not cover the paper`);
		}
		for (let i = 0; i < rects.length; i++) for (let j = i + 1; j < rects.length; j++) {
			const a = rects[i][1], b = rects[j][1];
			check(a.x + a.w <= b.x + 0.01 || b.x + b.w <= a.x + 0.01 || a.y + a.h <= b.y + 0.01 || b.y + b.h <= a.y + 0.01, `${rects[i][0]} and ${rects[j][0]} overlap`);
		}
	}
	if (touch) check(L.cell.h >= 10, `cell height at least 10px (${L.cell.h.toFixed(1)})`);
}
console.log(failures ? `${failures} FAILURES` : "all layout checks passed");
process.exit(failures ? 1 : 0);
