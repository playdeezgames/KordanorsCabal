// Pure layout computation for the platform (task 16). No DOM access: takes the viewport and returns rectangles in CSS pixels,
// so it runs in the page and in node tests. Units: the paper (the 22 x 23 cell area) is 176 x 184 frame pixels shown 2:1 wide,
// i.e. 352 x 184 layout units; the original VIC-20 border around it is 32 units at the sides and 28 above and below.
(function (root, factory) {
	if (typeof module === "object" && module.exports) module.exports = factory(); else root.computeLayout = factory().computeLayout;
})(typeof self !== "undefined" ? self : this, function () {
	const PAPER_W = 352, PAPER_H = 184, BORDER_W = 32, BORDER_H = 28;
	const MIN_SIDES_SCALE = 1.3;   // below this the side control columns cost too much screen: controls are hidden
	const VERTICAL_MARGIN = 6;     // minimum space above and below the paper on touch devices
	const clamp = (v, lo, hi) => Math.max(lo, Math.min(hi, v));

	function computeLayout({ width, height, touch, insets = {} }) {
		const inset = { left: insets.left || 0, right: insets.right || 0, top: insets.top || 0, bottom: insets.bottom || 0 };
		const area = { x: inset.left, y: inset.top, w: width - inset.left - inset.right, h: height - inset.top - inset.bottom };

		if (touch && height > width) return { mode: "rotate", scale: 0, paper: null, controls: null }; // portrait on a touch device: ask to rotate

		let mode, scale, paper, controls = null;
		if (!touch) {
			// Desktop: the whole original frame (paper plus border), integer scale when it fits, no on-screen controls.
			const fit = Math.min(area.w / (PAPER_W + 2 * BORDER_W), area.h / (PAPER_H + 2 * BORDER_H));
			scale = fit >= 1 ? Math.floor(fit) : fit;
			mode = "desktop";
		} else {
			const c = clamp(Math.round(area.w * 0.13), 96, 128);
			const sides = Math.min((area.w - 2 * c) / PAPER_W, (area.h - 2 * VERTICAL_MARGIN) / PAPER_H);
			if (sides >= MIN_SIDES_SCALE) {
				mode = "touch-sides"; scale = sides;
				controls = buildControls(area, c, scale);
			} else {
				mode = "touch-full"; scale = Math.min(area.w / PAPER_W, (area.h - 2 * VERTICAL_MARGIN) / PAPER_H);
			}
		}
		const w = PAPER_W * scale, h = PAPER_H * scale;
		paper = { x: area.x + (area.w - w) / 2, y: area.y + (area.h - h) / 2, w, h };
		const upscale = clamp(Math.ceil(scale), 1, 4); // draw at an integer multiple, then let the browser scale down smoothly
		return { mode, scale, upscale, paper, controls, cell: { w: paper.w / 22, h: paper.h / 23 } };
	}

	function buildControls(area, c, scale) {
		const b = clamp(Math.round(area.h * 0.15), 44, 60);          // button height (touch target)
		const gap = 6;
		const left = { x: area.x + 4, w: c - 8 }, right = { x: area.x + area.w - c + 4, w: c - 8 };
		const dpadH = 3 * b + 2 * gap, top = area.y + (area.h - dpadH) / 2;
		const dpad = {
			up:    { x: left.x, y: top, w: left.w, h: b },
			left:  { x: left.x, y: top + b + gap, w: (left.w - gap) / 2, h: b },
			right: { x: left.x + (left.w + gap) / 2, y: top + b + gap, w: (left.w - gap) / 2, h: b },
			down:  { x: left.x, y: top + 2 * (b + gap), w: left.w, h: b },
		};
		const actionsH = 2 * b + gap + b, atop = area.y + (area.h - actionsH) / 2;
		const actions = {
			confirm: { x: right.x, y: atop, w: right.w, h: 2 * b },
			cancel:  { x: right.x, y: atop + 2 * b + gap, w: right.w, h: b },
		};
		return { ...dpad, ...actions };
	}

	return { computeLayout, constants: { PAPER_W, PAPER_H, BORDER_W, BORDER_H, MIN_SIDES_SCALE } };
});
