#+build !js
package main

import "core:os"
import "core:strings"

// Native stand-in: one file per key under ./saves/ (D6: core:os only in native files).
SAVE_DIR :: "saves"

key_path :: proc(key: string) -> string {
	safe, _ := strings.replace_all(key, ":", "_", context.temp_allocator)
	return strings.concatenate({SAVE_DIR, "/", safe, ".json"}, context.temp_allocator)
}

storage_set :: proc(key, value: string) -> bool {
	os.make_directory(SAVE_DIR)
	return os.write_entire_file(key_path(key), transmute([]byte)value) == nil
}
storage_remove :: proc(key: string) { os.remove(key_path(key)) }
storage_get :: proc(key: string, allocator := context.allocator) -> (value: string, ok: bool) {
	data, read_err := os.read_entire_file(key_path(key), allocator)
	if read_err != nil { return "", false }
	return string(data), true
}
