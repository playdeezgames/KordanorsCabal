// Browser side of the platform interface (task 15). Everything the page does for the game is in this file
// plus storage.js: draw one RGBA frame, turn keys/taps/buttons into commands, play sounds, store text, offer files.
(async function () {
	const FRAME_W = 208, FRAME_H = 240, STRETCH_X = 2, BORDER_X = 16, BORDER_Y = 28, CELL_COLS = 22, CELL_ROWS = 23;
	const COMMAND = { up: 1, down: 2, left: 3, right: 4, confirm: 5, cancel: 6 };
	const SFX_FILES = [null, "RollDice.wav", "EnemyDeath.wav", "EnemyHit.wav", "LevelUp.wav", "Miss.wav", "PlayerDeath.wav", "PlayerHit.wav", "UnlockDoor.wav"];
	const MAX_IMPORT_BYTES = 2 * 1024 * 1024;

	const seedParam = new URLSearchParams(location.search).get("seed");
	const fixedSeed = seedParam !== null && /^\d{1,9}$/.test(seedParam) ? Number(seedParam) : null;
	let fixedSeedHalf = 0;

	const mem = new odin.WasmMemoryInterface();
	let exports = null;

	// ---- services the Odin side imports ------------------------------------------------------------
	const platformImports = {
		js_download_text(np, nl, tp, tn) { offerDownload(mem.loadString(np, nl), mem.loadString(tp, tn)); },
		js_request_file_pick() { offerFilePick(); },
		// ?seed=N fixes the seed of new games (for bug reports and tests): the high word is 0 and the low word is N
		js_entropy_u32() {
			if (fixedSeed !== null) { fixedSeedHalf ^= 1; return fixedSeedHalf ? 0 : fixedSeed; }
			return crypto.getRandomValues(new Uint32Array(1))[0] | 0;
		},
		js_log(p, n) { console.log(mem.loadString(p, n)); },
	};


	// ---- file transfer (tasks 14 and 29): export a save as a file, import one --------------------------------------------
	// Both go through a small modal with a real <a download> link or a real <input type=file> that the person taps themselves.
	// Browsers (iOS Safari above all) only allow downloads and file pickers from a direct tap, and the game asks from inside the
	// animation-frame loop, so a programmatic click could be blocked. The core's interface does not change (api.odin).
	const trace = (...a) => { if (window.__trace) window.__trace(...a); }; // test hook
	const transfer = { el: document.getElementById("transfer"), title: document.getElementById("transfer-title"), body: document.getElementById("transfer-body"), pending: null, url: null };
	function closeTransfer(cancelled) {
		transfer.el.classList.add("hidden");
		if (transfer.url) { URL.revokeObjectURL(transfer.url); transfer.url = null; }
		const wasPick = transfer.pending === "pick";
		transfer.pending = null; transfer.body.replaceChildren();
		if (cancelled && wasPick) { trace("file_cancelled"); exports.platform_file_cancelled(); }
	}
	function openTransfer(title, kind) { transfer.title.textContent = title; transfer.pending = kind; transfer.body.replaceChildren(); transfer.el.classList.remove("hidden"); }
	function offerDownload(filename, text) {
		openTransfer("SAVE FILE READY", "download");
		transfer.url = URL.createObjectURL(new Blob([text], { type: "application/json" }));
		const a = document.createElement("a");
		a.href = transfer.url; a.download = filename; a.textContent = "DOWNLOAD " + filename; a.className = "big";
		a.addEventListener("click", () => setTimeout(() => closeTransfer(false), 300));
		transfer.body.append(a);
	}
	function offerFilePick() {
		openTransfer("CHOOSE A SAVE FILE", "pick");
		const label = document.createElement("label"); label.className = "big"; label.textContent = "CHOOSE FILE";
		const input = document.createElement("input"); input.type = "file"; input.accept = ".json,application/json";
		input.addEventListener("change", async () => {
			const file = input.files && input.files[0];
			if (!file || file.size > MAX_IMPORT_BYTES) { closeTransfer(true); return; }
			const bytes = new Uint8Array(await file.arrayBuffer());
			const ptr = exports.platform_alloc(bytes.length);
			new Uint8Array(mem.memory.buffer, ptr, bytes.length).set(bytes);
			transfer.pending = null; closeTransfer(false);
			trace("file_text", bytes.length);
			exports.platform_file_text(ptr, bytes.length);
		});
		label.append(input);
		transfer.body.append(label);
	}
	document.getElementById("transfer-close").addEventListener("click", () => closeTransfer(true));

	await odin.runWasm("platform.wasm", null, { ...storageImports(mem), platform: platformImports }, mem);
	exports = mem.exports;

	// ---- layout and presentation (see layout.js) ------------------------------------------------------------
	const params = new URLSearchParams(location.search);
	const touch = params.has("touch") ? params.get("touch") === "1" : (matchMedia("(pointer: coarse)").matches || navigator.maxTouchPoints > 0 && !matchMedia("(hover: hover)").matches);
	const canvas = document.getElementById("screen"), ctx = canvas.getContext("2d");
	const staging = document.createElement("canvas"); staging.width = FRAME_W; staging.height = FRAME_H;
	const stagingCtx = staging.getContext("2d");
	const controlsEl = document.getElementById("controls"), probe = document.getElementById("probe");
	let layout = null;
	function insets() { const s = getComputedStyle(probe); return { top: parseFloat(s.paddingTop) || 0, right: parseFloat(s.paddingRight) || 0, bottom: parseFloat(s.paddingBottom) || 0, left: parseFloat(s.paddingLeft) || 0 }; }
	function applyLayout() {
		const vv = window.visualViewport;
		layout = computeLayout({ width: vv ? vv.width : innerWidth, height: vv ? vv.height : innerHeight, touch, insets: insets() });
		document.getElementById("rotate").classList.toggle("hidden", layout.mode !== "rotate");
		const hide = layout.mode === "rotate";
		canvas.style.display = hide ? "none" : "block";
		controlsEl.classList.toggle("hidden", !layout.controls);
		if (hide) return;
		const p = layout.paper;
		canvas.width = 176 * STRETCH_X * layout.upscale; canvas.height = 184 * layout.upscale;
		canvas.style.left = p.x + "px"; canvas.style.top = p.y + "px"; canvas.style.width = p.w + "px"; canvas.style.height = p.h + "px";
		canvas.classList.toggle("crisp", Number.isInteger(layout.scale) && layout.upscale === layout.scale);
		if (layout.controls) for (const b of controlsEl.children) {
			const r = layout.controls[b.dataset.cmd];
			b.style.left = r.x + "px"; b.style.top = r.y + "px"; b.style.width = r.w + "px"; b.style.height = r.h + "px";
		}
		window.__platform && (window.__platform.layout = layout);
	}
	addEventListener("resize", applyLayout); addEventListener("orientationchange", applyLayout);
	if (window.visualViewport) visualViewport.addEventListener("resize", applyLayout);
	applyLayout();
	document.getElementById("startnote").textContent = touch ? "landscape - fullscreen where the browser allows it" : "arrow keys, space, escape";

	// ---- start gesture: unlock audio, go fullscreen, lock landscape (all need a user gesture) -----------------------------
	let started = false, startInfo = { fullscreen: null, orientationLock: null };
	async function start() {
		if (started) return; started = true;
		unlockAudio();
		document.getElementById("start").classList.add("hidden");
		if (touch) {
			try { await document.documentElement.requestFullscreen({ navigationUI: "hide" }); startInfo.fullscreen = "ok"; } catch (e) { startInfo.fullscreen = (e.name || "") + ": " + (e.message || e); }
			try { await screen.orientation.lock("landscape"); startInfo.orientationLock = "ok"; } catch (e) { startInfo.orientationLock = (e.name || "") + ": " + (e.message || e); }
			applyLayout();
		}
		window.__platform.startInfo = startInfo;
	}

	// ---- input ----------------------------------------------------------------------------------------
	const KEYS = {
		ArrowUp: "up", ArrowDown: "down", ArrowLeft: "left", ArrowRight: "right", Numpad8: "up", Numpad2: "down", Numpad4: "left", Numpad6: "right",
		Space: "confirm", Enter: "confirm", NumpadEnter: "confirm", Numpad5: "confirm", Escape: "cancel", Backspace: "cancel",
	};
	addEventListener("keydown", (e) => {
		if (!started) { start(); e.preventDefault(); return; }
		const name = KEYS[e.code];
		if (!name || e.repeat) return;
		e.preventDefault();
		exports.platform_command(COMMAND[name]);
	});
	// A tap on either overlay is the start gesture. Fullscreen needs "transient user activation": pointerdown counts for a
	// mouse, but for touch only pointerup does (HTML spec, activation-triggering input events).
	for (const id of ["start", "rotate"]) {
		const el = document.getElementById(id);
		el.addEventListener("pointerdown", (e) => { e.preventDefault(); if (e.pointerType === "mouse") start(); });
		el.addEventListener("pointerup", (e) => { e.preventDefault(); if (e.pointerType !== "mouse") start(); });
	}
	canvas.addEventListener("pointerdown", (e) => {
		e.preventDefault();
		if (!started) return start();
		const r = canvas.getBoundingClientRect();
		const col = Math.floor((e.clientX - r.left) / r.width * CELL_COLS), row = Math.floor((e.clientY - r.top) / r.height * CELL_ROWS);
		exports.platform_tap(col, row, e.pointerType !== "touch"); // a finger is not precise (task 16)
	});
	for (const b of controlsEl.children) {
		b.addEventListener("pointerdown", (e) => { e.preventDefault(); if (!started) return start(); exports.platform_command(COMMAND[b.dataset.cmd]); });
	}
	addEventListener("contextmenu", (e) => e.preventDefault());

	// ---- audio (task 20) -----------------------------------------------------------------------------------
	// Sounds are Web Audio buffers (low latency, fine on iOS); the theme streams through an <audio> element routed through a
	// gain node, because iOS Safari ignores element.volume. Everything starts on the first user gesture, as browsers require.
	// The order here must equal the Sfx enum in api.odin (checked by audio_manifest_test.js).
	const SFX = [
		["Character_Creation", "RollDice.wav"], ["Enemy_Death", "EnemyDeath.wav"], ["Enemy_Hit", "EnemyHit.wav"], ["Level_Up", "LevelUp.wav"],
		["Miss", "Miss.wav"], ["Player_Death", "PlayerDeath.wav"], ["Player_Hit", "PlayerHit.wav"], ["Unlock_Door", "UnlockDoor.wav"],
	]; // Sfx value i + 1 plays SFX[i]
	// Shipped music files, in preference order. Only the ogg exists today; when an mp3 or m4a of the theme is added (iPhones may not play
	// Ogg), add ["audio/mpeg", "MinorTheme.mp3"] or ["audio/mp4", "MinorTheme.m4a"] here and to tools/build.sh.
	const MUSIC_SOURCES = [['audio/ogg; codecs="vorbis"', "MinorTheme.ogg"]];
	const audioState = { ctx: null, buffers: [], decoded: 0, sfxGain: null, musicGain: null, music: null, musicFile: null, errors: [] };
	function unlockAudio() {
		const s = audioState;
		if (s.ctx) { if (s.ctx.state !== "running") s.ctx.resume(); if (s.music && s.music.paused && !document.hidden) s.music.play().catch(() => {}); return; }
		try {
			s.ctx = new (window.AudioContext || window.webkitAudioContext)();
			s.sfxGain = s.ctx.createGain(); s.sfxGain.connect(s.ctx.destination);
			s.musicGain = s.ctx.createGain(); s.musicGain.connect(s.ctx.destination);
		} catch (err) { s.errors.push("context " + err); return; }
		SFX.forEach(async ([, file], i) => {
			try { s.buffers[i + 1] = await s.ctx.decodeAudioData(await (await fetch("assets/" + file)).arrayBuffer()); s.decoded++; } catch (err) { s.errors.push(file + ": " + err); }
		});
		// Try each shipped format the browser claims it can play ("probably" before "maybe"); fall back to the next on any error.
		const probe = document.createElement("audio");
		const rank = (type) => ({ probably: 0, maybe: 1 })[probe.canPlayType(type)] ?? 9;
		const candidates = MUSIC_SOURCES.filter(([type]) => rank(type) < 9).sort((x, y) => rank(x[0]) - rank(y[0]));
		const tryMusic = (i) => {
			if (i >= candidates.length) { s.errors.push("no playable music format"); return; }
			const el = new Audio("assets/" + candidates[i][1]); el.loop = true; el.preload = "auto";
			const next = (why) => { s.errors.push(candidates[i][1] + ": " + why); el.pause(); tryMusic(i + 1); };
			el.addEventListener("error", () => next("load error"), { once: true });
			try { s.ctx.createMediaElementSource(el).connect(s.musicGain); } catch (err) { s.errors.push("music graph " + err); }
			el.play().then(() => { s.music = el; s.musicFile = candidates[i][1]; }).catch((err) => { if (err.name === "NotSupportedError" || err.name === "AbortError") next(err.name); else { s.music = el; s.errors.push("music play: " + err.name); } });
		};
		tryMusic(0);
		document.addEventListener("visibilitychange", () => { if (!s.music) return; if (document.hidden) s.music.pause(); else s.music.play().catch(() => {}); });
	}
	function playSfx(i) {
		const s = audioState;
		if (!s.ctx || !s.buffers[i] || s.ctx.state !== "running") return;
		const src = s.ctx.createBufferSource();
		src.buffer = s.buffers[i]; src.connect(s.sfxGain); src.start();
	}

	// ---- the frame loop -----------------------------------------------------------------------------------------
	let prev = performance.now(), lastBorder = "";
	function frame(now) {
		exports.platform_frame(Math.min((now - prev) / 1000, 0.25)); prev = now;
		const ptr = exports.platform_frame_ptr();
		if (ptr && layout.mode !== "rotate") {
			const bytes = new Uint8ClampedArray(mem.memory.buffer, ptr, FRAME_W * FRAME_H * 4);
			stagingCtx.putImageData(new ImageData(bytes, FRAME_W, FRAME_H), 0, 0);
			ctx.imageSmoothingEnabled = false;
			ctx.drawImage(staging, BORDER_X, BORDER_Y, 176, 184, 0, 0, canvas.width, canvas.height); // the paper only; the border becomes the page background
			const border = `rgb(${bytes[0]},${bytes[1]},${bytes[2]})`; // pixel (0,0) is always border
			if (border !== lastBorder) { document.body.style.background = document.documentElement.style.background = border; lastBorder = border; }
		}
		if (audioState.ctx) { audioState.sfxGain.gain.value = exports.platform_sfx_volume(); audioState.musicGain.gain.value = exports.platform_music_volume(); }
		for (let i = 0, n = exports.platform_sfx_count(); i < n; i++) playSfx(exports.platform_sfx_at(i));
		requestAnimationFrame(frame);
	}
	if (document.hidden) window.requestAnimationFrame = (f) => setTimeout(() => f(performance.now()), 16); // testing aid for hidden panes
	window.__platform = { exports, mem, layout, touch, audio: audioState, offerDownload, offerFilePick };
	requestAnimationFrame(frame);
})();
