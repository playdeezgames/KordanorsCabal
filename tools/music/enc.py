import ctypes, array, sys
RATE = 22050
pcm = array.array("h"); pcm.frombytes(open("theme.raw", "rb").read()); pcm = array.array("h", (int(v * 0.45) for v in pcm))   # the old theme sits at about 0.17 rms
sf = ctypes.CDLL("libsndfile.so.1")
class Info(ctypes.Structure): _fields_ = [("frames", ctypes.c_int64), ("samplerate", ctypes.c_int), ("channels", ctypes.c_int), ("format", ctypes.c_int), ("sections", ctypes.c_int), ("seekable", ctypes.c_int)]
sf.sf_open.restype = ctypes.c_void_p; sf.sf_open.argtypes = [ctypes.c_char_p, ctypes.c_int, ctypes.POINTER(Info)]
sf.sf_command.argtypes = [ctypes.c_void_p, ctypes.c_int, ctypes.c_void_p, ctypes.c_int]
sf.sf_writef_short.restype = ctypes.c_int64; sf.sf_writef_short.argtypes = [ctypes.c_void_p, ctypes.c_void_p, ctypes.c_int64]
sf.sf_close.argtypes = [ctypes.c_void_p]; sf.sf_strerror.restype = ctypes.c_char_p; sf.sf_strerror.argtypes = [ctypes.c_void_p]
def write(path, fmt, quality):
    info = Info(0, RATE, 1, fmt, 0, 0)
    h = sf.sf_open(path.encode(), 0x20, ctypes.byref(info)); assert h, sf.sf_strerror(None)
    if quality is not None:
        q = ctypes.c_double(quality); print("quality set", sf.sf_command(h, 0x1303, ctypes.byref(q), 8))
    CH = 4096
    for i in range(0, len(pcm), CH):
        chunk = pcm[i:i + CH]; buf = (ctypes.c_short * len(chunk)).from_buffer(chunk)
        k = sf.sf_writef_short(h, buf, len(chunk)); assert k == len(chunk), (i, k, sf.sf_strerror(h))
    sf.sf_close(h)
kind = sys.argv[1]
if kind == "ogg": write("MinorTheme.new.ogg", 0x200000 | 0x0060, 0.25)
else: write("MinorTheme.new.mp3", 0x230000 | 0x0000, None)
print("written", kind)
