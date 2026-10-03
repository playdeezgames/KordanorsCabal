package game

import "core:encoding/json"

// The config text is `{"sfx_volume":0.5,"music_volume":0.5}`. Anything unreadable or out of range gives the defaults.

config_text :: proc(c: Config) -> string {
	data, err := json.marshal(c, {spec = .JSON}, context.temp_allocator)
	if err != nil { return "{}" }
	return string(data)
}

config_parse :: proc(text: string, c: ^Config) -> bool {
	parsed: Config
	if json.unmarshal(transmute([]byte)text, &parsed, .JSON, context.temp_allocator) != nil { return false }
	if !(parsed.sfx_volume >= 0 && parsed.sfx_volume <= 1 && parsed.music_volume >= 0 && parsed.music_volume <= 1) { return false } // also rejects NaN
	c^ = parsed
	return true
}
