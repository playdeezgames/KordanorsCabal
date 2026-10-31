import wave, array, ctypes, sys
RATE = 22050
def load(name):
    w = wave.open("wav/%s.wav" % name); assert w.getframerate() == RATE and w.getnchannels() == 1 and w.getsampwidth() == 2
    a = array.array("h"); a.frombytes(w.readframes(w.getnframes())); return a
def section(letter):
    mel = load("theme_%s_mel_1" % letter) + load("theme_%s_mel_2" % letter)
    bass = load("theme_%s_bass_1" % letter) + load("theme_%s_bass_2" % letter)
    n = min(len(mel), len(bass)); assert abs(len(mel) - len(bass)) < 50, (len(mel), len(bass))
    return [0.42 * mel[i] + 0.30 * bass[i] for i in range(n)]
secs = {k: section(k) for k in "ABC"}
form = "ABACBA"
x = []
for k in form: x += secs[k]
n = len(x)
# soften the square waves: one-pole low pass around 2 kHz
a = 0.45; y = [0.0] * n; p = 0.0
for i in range(n):
    p += a * (x[i] - p); y[i] = p
# echo, wrapped around the end so the loop is seamless
out = y[:]
for delay, gain in ((int(0.42 * RATE), 0.28), (int(0.91 * RATE), 0.16)):
    for i in range(n): out[(i + delay) % n] += gain * y[i]
peak = max(abs(v) for v in out); scale = 0.85 * 32767 / peak
pcm = array.array("h", (int(v * scale) for v in out))
print("seconds %.1f peak %.0f" % (n / RATE, peak)); open("theme.raw","wb").write(pcm.tobytes()); sys.exit()
# encode with libsndfile (ogg/vorbis)
sf = ctypes.CDLL("libsndfile.so.1")
class Info(ctypes.Structure): _fields_ = [("frames", ctypes.c_int64), ("samplerate", ctypes.c_int), ("channels", ctypes.c_int), ("format", ctypes.c_int), ("sections", ctypes.c_int), ("seekable", ctypes.c_int)]
sf.sf_open.restype = ctypes.c_void_p
def write(path, fmt, quality):
    info = Info(0, RATE, 1, fmt, 0, 0)
    h = sf.sf_open(path.encode(), 0x20, ctypes.byref(info)); assert h, sf.sf_strerror(None)
    q = ctypes.c_double(quality); sf.sf_command(ctypes.c_void_p(h), 0x1303, ctypes.byref(q), 8)
    buf = (ctypes.c_short * len(pcm)).from_buffer(pcm)
    sf.sf_writef_short.restype = ctypes.c_int64
    sf.sf_writef_short.argtypes = [ctypes.c_void_p, ctypes.c_void_p, ctypes.c_int64]
    k = sf.sf_writef_short(ctypes.c_void_p(h), buf, len(pcm)); assert k == len(pcm), k
    sf.sf_close(ctypes.c_void_p(h))
write("MinorTheme.new.ogg", 0x200000 | 0x0060, 0.4)
print("written")
