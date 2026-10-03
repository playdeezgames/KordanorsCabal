#+build js
package main

// Browser platform (js_wasm32). odin.js runs `main` once (init); then web/platform.js calls `platform_frame` every animation
// frame. Everything crossing the boundary is a number, a (ptr, len) string, or a pointer into wasm memory.
import "base:runtime"
import "kc:game"

foreign import platform_env "platform"

@(default_calling_convention = "contextless")
foreign platform_env {
	js_download_text :: proc(filename, text: string) ---
	js_request_file_pick :: proc() ---
	js_entropy_u32 :: proc() -> u32 ---
	js_log :: proc(message: string) ---
}

core: ^game.Core // allocated once at start; never on the stack (task 19)
out: game.Step_Output
events: [64]game.Input_Event
event_count: int

push_event :: proc "contextless" (e: game.Input_Event) {
	if event_count < len(events) { events[event_count] = e; event_count += 1 }
}

main :: proc() {
	core = new(game.Core)
	game.core_init(core, game.Services{
		storage_get = storage_get, storage_set = storage_set, storage_remove = storage_remove,
		download_text = proc(filename, text: string) { js_download_text(filename, text) },
		request_file_pick = proc() { js_request_file_pick() },
		entropy = proc() -> u64 { return u64(js_entropy_u32()) << 32 | u64(js_entropy_u32()) },
		log = proc(message: string) { js_log(message) },
	})
}

// ---- exports called by web/platform.js ----------------------------------------------------------------
@(export) platform_command :: proc "c" (command: i32) { push_event({kind = .Command, command = game.Command(command)}) }
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
	if core == nil { return }
	game.core_step(core, {dt = dt, events = events[:event_count]}, &out)
	for i in 0 ..< event_count { if events[i].kind == .File_Text { runtime.mem_free(raw_data(events[i].text)) } }
	event_count = 0
}
// ---- output getters ---------------------------------------------------------------------------------
@(export) platform_frame_ptr :: proc "c" () -> rawptr { return out.frame }
@(export) platform_sfx_count :: proc "c" () -> i32 { return i32(out.sfx_count) }
@(export) platform_sfx_at :: proc "c" (i: i32) -> i32 { return i32(out.sfx[i]) }
@(export) platform_sfx_volume :: proc "c" () -> f32 { return out.sfx_volume }
@(export) platform_music_volume :: proc "c" () -> f32 { return out.music_volume }
@(export) platform_quit :: proc "c" () -> bool { return out.quit_requested }
