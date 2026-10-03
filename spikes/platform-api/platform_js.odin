#+build js
package main

// Browser platform. odin.js runs `main` once (init); then the page's own loop calls `platform_frame` every
// animation frame. Everything crossing the boundary is a number, a (ptr, len) string, or a pointer into wasm memory.
import "base:runtime"

foreign import platform_env "platform"

@(default_calling_convention = "contextless")
foreign platform_env {
	js_download_text :: proc(filename, text: string) ---
	js_request_file_pick :: proc() ---
	js_entropy_u32 :: proc() -> u32 ---
	js_log :: proc(message: string) ---
}

js_core: Core
js_out: Step_Output
js_events: [64]Input_Event
js_event_count: int
js_ready: bool

push_event :: proc "contextless" (e: Input_Event) {
	if js_event_count < len(js_events) { js_events[js_event_count] = e; js_event_count += 1 }
}

main :: proc() {
	core_init(&js_core, Services{
		storage_get = storage_get, storage_set = storage_set, storage_remove = storage_remove,
		download_text = proc(filename, text: string) { js_download_text(filename, text) },
		request_file_pick = proc() { js_request_file_pick() },
		entropy = proc() -> u64 { return u64(js_entropy_u32()) << 32 | u64(js_entropy_u32()) },
		log = proc(message: string) { js_log(message) },
	})
	js_ready = true
}

// ---- exports called by platform.js ----------------------------------------------------------------
@(export) platform_command :: proc "c" (command: i32) { push_event({kind = .Command, command = Command(command)}) }
@(export) platform_tap :: proc "c" (col, row: i32, precise: bool) { push_event({kind = .Tap, col = i16(col), row = i16(row), precise = precise}) }
@(export) platform_file_cancelled :: proc "c" () { push_event({kind = .File_Cancelled}) }
// JS asks for a buffer, copies the file's UTF-8 bytes into it, then reports it; the Odin side frees it after the step.
@(export) platform_alloc :: proc "c" (size: i32) -> rawptr {
	context = runtime.default_context()
	buf, _ := runtime.mem_alloc(int(size), 16)
	return raw_data(buf)
}
@(export) platform_file_text :: proc "c" (ptr: rawptr, length: i32) {
	push_event({kind = .File_Text, text = string((cast([^]u8)ptr)[:length])})
}
@(export) platform_frame :: proc "c" (dt: f64) {
	context = runtime.default_context()
	if !js_ready { return }
	core_step(&js_core, {dt = dt, events = js_events[:js_event_count]}, &js_out)
	for i in 0 ..< js_event_count { if js_events[i].kind == .File_Text { runtime.mem_free(raw_data(js_events[i].text)) } }
	js_event_count = 0
}
// ---- output getters ---------------------------------------------------------------------------------
@(export) platform_frame_ptr :: proc "c" () -> rawptr { return js_out.frame }
@(export) platform_sfx_count :: proc "c" () -> i32 { return i32(js_out.sfx_count) }
@(export) platform_sfx_at :: proc "c" (i: i32) -> i32 { return i32(js_out.sfx[i]) }
@(export) platform_sfx_volume :: proc "c" () -> f32 { return js_out.sfx_volume }
@(export) platform_music_volume :: proc "c" () -> f32 { return js_out.music_volume }
@(export) platform_quit :: proc "c" () -> bool { return js_out.quit_requested }
