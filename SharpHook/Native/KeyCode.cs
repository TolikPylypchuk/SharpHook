namespace SharpHook.Native;

/// <summary>
/// Represents a virtual key code.
/// </summary>
/// <seealso cref="KeyboardEventData" />
public enum KeyCode : ushort
{
    /// <summary>Undefined key</summary>
    VcUndefined = 0x0000,

    /// <summary>Escape</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcEscape = 0x001B,

    /// <summary>F1</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF1 = 0x0070,

    /// <summary>F2</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF2 = 0x0071,

    /// <summary>F3</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF3 = 0x0072,

    /// <summary>F4</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF4 = 0x0073,

    /// <summary>F5</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF5 = 0x0074,

    /// <summary>F6</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF6 = 0x0075,

    /// <summary>F7</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF7 = 0x0076,

    /// <summary>F8</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF8 = 0x0077,

    /// <summary>F9</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF9 = 0x0078,

    /// <summary>F10</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF10 = 0x0079,

    /// <summary>F11</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF11 = 0x007A,

    /// <summary>F12</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF12 = 0x007B,

    /// <summary>F13</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF13 = 0xF000,

    /// <summary>F14</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF14 = 0xF001,

    /// <summary>F15</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF15 = 0xF002,

    /// <summary>F16</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF16 = 0xF003,

    /// <summary>F17</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF17 = 0xF004,

    /// <summary>F18</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF18 = 0xF005,

    /// <summary>F19</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF19 = 0xF006,

    /// <summary>F20</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF20 = 0xF007,

    /// <summary>F21</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcF21 = 0xF008,

    /// <summary>F22</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcF22 = 0xF009,

    /// <summary>F23</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcF23 = 0xF00A,

    /// <summary>F24</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcF24 = 0xF00B,

    /// <summary>`</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcBackQuote = 0x00C0,

    /// <summary>0</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc0 = 0x0030,

    /// <summary>1</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc1 = 0x0031,

    /// <summary>2</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc2 = 0x0032,

    /// <summary>3</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc3 = 0x0033,

    /// <summary>4</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc4 = 0x0034,

    /// <summary>5</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc5 = 0x0035,

    /// <summary>6</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc6 = 0x0036,

    /// <summary>7</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc7 = 0x0037,

    /// <summary>8</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc8 = 0x0038,

    /// <summary>9</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc9 = 0x0039,

    /// <summary>-</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMinus = 0x002D,

    /// <summary>=</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcEquals = 0x003D,

    /// <summary>
    /// Backspace (on Windows and Linux) or Delete (on macOS)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcBackspace = 0x0008,

    /// <summary>Tab</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcTab = 0x0009,

    /// <summary>Caps Lock</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcCapsLock = 0x0014,

    /// <summary>A</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcA = 0x0041,

    /// <summary>B</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcB = 0x0042,

    /// <summary>C</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcC = 0x0043,

    /// <summary>D</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcD = 0x0044,

    /// <summary>E</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcE = 0x0045,

    /// <summary>F</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF = 0x0046,

    /// <summary>G</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcG = 0x0047,

    /// <summary>H</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcH = 0x0048,

    /// <summary>I</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcI = 0x0049,

    /// <summary>J</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcJ = 0x004A,

    /// <summary>K</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcK = 0x004B,

    /// <summary>L</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcL = 0x004C,

    /// <summary>M</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcM = 0x004D,

    /// <summary>N</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcN = 0x004E,

    /// <summary>O</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcO = 0x004F,

    /// <summary>P</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcP = 0x0050,

    /// <summary>Q</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcQ = 0x0051,

    /// <summary>R</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcR = 0x0052,

    /// <summary>S</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcS = 0x0053,

    /// <summary>T</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcT = 0x0054,

    /// <summary>U</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcU = 0x0055,

    /// <summary>V</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcV = 0x0056,

    /// <summary>W</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcW = 0x0057,

    /// <summary>X</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcX = 0x0058,

    /// <summary>Y</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcY = 0x0059,

    /// <summary>Z</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcZ = 0x005A,

    /// <summary>[</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcOpenBracket = 0x005B,

    /// <summary>]</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcCloseBracket = 0x005C,

    /// <summary>\</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcBackSlash = 0x005D,

    /// <summary>;</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcSemicolon = 0x003B,

    /// <summary>'</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcQuote = 0x00DE,

    /// <summary>Enter</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcEnter = 0x000A,

    /// <summary>,</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcComma = 0x002C,

    /// <summary>.</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcPeriod = 0x002E,

    /// <summary>/</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcSlash = 0x002F,

    /// <summary>Space</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcSpace = 0x0020,

    /// <summary>
    /// The &lt;&gt; keys on the US standard keyboard, or the \| key on the non-US 102-key keyboard,
    /// or the Section key (§) on the macOS ISO keyboard
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc102 = 0x0099,

    /// <summary>Miscellaneous OEM-specific key</summary>
    /// <remarks>Available on: Windows</remarks>
    VcMisc = 0x0E01,

    /// <summary>Print Screen</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcPrintScreen = 0x009A,

    /// <summary>Print</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcPrint = 0x009C,

    /// <summary>Select</summary>
    /// <remarks>Available on: Windows</remarks>
    VcSelect = 0x009D,

    /// <summary>Execute</summary>
    /// <remarks>Available on: Windows</remarks>
    VcExecute = 0x009E,

    /// <summary>Scroll Lock</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcScrollLock = 0x0091,

    /// <summary>Pause</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcPause = 0x0013,

    /// <summary>Cancel</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcCancel = 0x00D3,

    /// <summary>Help</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcHelp = 0x009F,

    /// <summary>Insert</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcInsert = 0x009B,

    /// <summary>
    /// Delete (on Windows and Linux) or Forward Delete (on macOS)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcDelete = 0x007F,

    /// <summary>Home</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcHome = 0x0024,

    /// <summary>End</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcEnd = 0x0023,

    /// <summary>Page Up</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcPageUp = 0x0021,

    /// <summary>Page Down</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcPageDown = 0x0022,

    /// <summary>Up Arrow</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcUp = 0x0026,

    /// <summary>Left Arrow</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcLeft = 0x0025,

    /// <summary>Right Arrow</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcRight = 0x0027,

    /// <summary>Down Arrow</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcDown = 0x0028,

    /// <summary>Num Lock</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcNumLock = 0x0090,

    /// <summary>Num-Pad Clear</summary>
    /// <remarks>Available on: Windows, macOS</remarks>
    VcNumPadClear = 0x000C,

    /// <summary>Num-Pad /</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadDivide = 0x006F,

    /// <summary>Num-Pad *</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadMultiply = 0x006A,

    /// <summary>Num-Pad -</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadSubtract = 0x006D,

    /// <summary>Num-Pad =</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadEquals = 0x007C,

    /// <summary>Num-Pad +</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadAdd = 0x006B,

    /// <summary>Num-Pad Enter</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadEnter = 0x007D,

    /// <summary>Num-Pad Decimal</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadDecimal = 0x006E,

    /// <summary>Num-Pad Separator</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadSeparator = 0x006C,

    /// <summary>Num-Pad ±</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadPlusMinus = 0x007E,

    /// <summary>Num-Pad 0</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad0 = 0x0060,

    /// <summary>Num-Pad 1</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad1 = 0x0061,

    /// <summary>Num-Pad 2</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad2 = 0x0062,

    /// <summary>Num-Pad 3</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad3 = 0x0063,

    /// <summary>Num-Pad 4</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad4 = 0x0064,

    /// <summary>Num-Pad 5</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad5 = 0x0065,

    /// <summary>Num-Pad 6</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad6 = 0x0066,

    /// <summary>Num-Pad 7</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad7 = 0x0067,

    /// <summary>Num-Pad 8</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad8 = 0x0068,

    /// <summary>Num-Pad 9</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad9 = 0x0069,

    /// <summary>Num-Pad (</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadOpenParenthesis = 0xEE01,

    /// <summary>Num-Pad )</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadCloseParenthesis = 0xEE02,

    /// <summary>Left Shift</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcLeftShift = 0xA010,

    /// <summary>Right Shift</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcRightShift = 0xB010,

    /// <summary>Left Control</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcLeftControl = 0xA011,

    /// <summary>Right Control</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcRightControl = 0xB011,

    /// <summary>
    /// Left Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcLeftAlt = 0xA012,

    /// <summary>
    /// Right Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcRightAlt = 0xB012,

    /// <summary>
    /// Left Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcLeftMeta = 0xA09D,

    /// <summary>
    /// Right Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcRightMeta = 0xB09D,

    /// <summary>Context Menu</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcContextMenu = 0x020D,

    /// <summary>Function</summary>
    /// <remarks>Available on: macOS</remarks>
    VcFunction = 0x020E,

    /// <summary>
    /// Function key when used to change an input source on macOS
    /// </summary>
    /// <remarks>Available on: macOS</remarks>
    VcChangeInputSource = 0x020F,

    /// <summary>Power</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcPower = 0xE05E,

    /// <summary>Sleep</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcSleep = 0xE05F,

    /// <summary>Wake</summary>
    /// <remarks>Available on: Linux</remarks>
    VcWake = 0xE063,

    /// <summary>Media</summary>
    /// <remarks>Available on: Linux</remarks>
    VcMedia = 0xE023,

    /// <summary>Play/Pause Media</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMediaPlay = 0xE022,

    /// <summary>Stop Media</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcMediaStop = 0xE024,

    /// <summary>Previous Media</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMediaPrevious = 0xE010,

    /// <summary>Next Media</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMediaNext = 0xE019,

    /// <summary>Select Media</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcMediaSelect = 0xE06D,

    /// <summary>Eject Media</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcMediaEject = 0xE02C,

    /// <summary>Close Media</summary>
    /// <remarks>Available on: Linux</remarks>
    VcMediaClose = 0xE02D,

    /// <summary>Eject/Close Media</summary>
    /// <remarks>Available on: Linux</remarks>
    VcMediaEjectClose = 0xE02F,

    /// <summary>Record Media</summary>
    /// <remarks>Available on: Linux</remarks>
    VcMediaRecord = 0xE031,

    /// <summary>Rewind Media</summary>
    /// <remarks>Available on: Linux</remarks>
    VcMediaRewind = 0xE033,

    /// <summary>Volume Mute</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcVolumeMute = 0xE020,

    /// <summary>Volume Down</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcVolumeDown = 0xE030,

    /// <summary>Volume Up</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcVolumeUp = 0xE02E,

    /// <summary>Attn</summary>
    /// <remarks>Available on: Windows</remarks>
    VcAttn = 0xE090,

    /// <summary>CrSel</summary>
    /// <remarks>Available on: Windows</remarks>
    VcCrSel = 0xE091,

    /// <summary>ExSel</summary>
    /// <remarks>Available on: Windows</remarks>
    VcExSel = 0xE092,

    /// <summary>Erase EOF</summary>
    /// <remarks>Available on: Windows</remarks>
    VcEraseEof = 0xE093,

    /// <summary>Play</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcPlay = 0xE094,

    /// <summary>Zoom</summary>
    /// <remarks>Available on: Windows</remarks>
    VcZoom = 0xE095,

    /// <summary>Reserved for future use</summary>
    /// <remarks>Available on: Windows</remarks>
    VcNoName = 0xE096,

    /// <summary>PA1</summary>
    /// <remarks>Available on: Windows</remarks>
    VcPa1 = 0xE097,

    /// <summary>Launch app 1</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcApp1 = 0xE026,

    /// <summary>Launch app 2</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcApp2 = 0xE027,

    /// <summary>Launch app 3</summary>
    /// <remarks>Available on: Linux</remarks>
    VcApp3 = 0xE028,

    /// <summary>Launch app 4</summary>
    /// <remarks>Available on: Linux</remarks>
    VcApp4 = 0xE029,

    /// <summary>Launch browser</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAppBrowser = 0xE025,

    /// <summary>Launch calculator</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAppCalculator = 0xE021,

    /// <summary>Launch mail</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcAppMail = 0xE06C,

    /// <summary>Browser Search</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserSearch = 0xE065,

    /// <summary>Browser Home</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserHome = 0xE032,

    /// <summary>Browser Back</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserBack = 0xE06A,

    /// <summary>Browser Forward</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserForward = 0xE069,

    /// <summary>Browser Stop</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserStop = 0xE068,

    /// <summary>Browser Refresh</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserRefresh = 0xE067,

    /// <summary>Browser Favorites</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserFavorites = 0xE066,

    /// <summary>IME Katakana/Hiragana toggle</summary>
    /// <remarks>Available on: Linux</remarks>
    VcKatakanaHiragana = 0x0106,

    /// <summary>IME Katakana mode</summary>
    /// <remarks>Available on: Linux</remarks>
    VcKatakana = 0x00F1,

    /// <summary>IME Hiragana mode</summary>
    /// <remarks>Available on: Linux</remarks>
    VcHiragana = 0x00F2,

    /// <summary>IME Kana mode</summary>
    /// <remarks>Available on: Windows, macOS</remarks>
    VcKana = 0x0015,

    /// <summary>IME Kanji mode</summary>
    /// <remarks>Available on: Windows</remarks>
    VcKanji = 0x0019,

    /// <summary>IME Hangul mode</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcHangul = 0x00E9,

    /// <summary>IME Junja mode</summary>
    /// <remarks>Available on: Windows</remarks>
    VcJunja = 0x00E8,

    /// <summary>IME Final mode</summary>
    /// <remarks>Available on: Windows</remarks>
    VcFinal = 0x00E7,

    /// <summary>IME Hanja mode</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcHanja = 0x00E6,

    /// <summary>IME Accept</summary>
    /// <remarks>Available on: Windows</remarks>
    VcAccept = 0x001E,

    /// <summary>IME Convert (henkan)</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcConvert = 0x001C,

    /// <summary>IME Non-Convert (muhenkan)</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcNonConvert = 0x001D,

    /// <summary>IME On</summary>
    /// <remarks>Available on: Windows</remarks>
    VcImeOn = 0x0109,

    /// <summary>IME Off</summary>
    /// <remarks>Available on: Windows</remarks>
    VcImeOff = 0x0108,

    /// <summary>IME Mode Change</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcModeChange = 0x0107,

    /// <summary>IME Process</summary>
    /// <remarks>Available on: Windows</remarks>
    VcProcess = 0x0105,

    /// <summary>IME Alphanumeric mode (eisū)</summary>
    /// <remarks>Available on: macOS</remarks>
    VcAlphanumeric = 0x00F0,

    /// <summary>_</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcUnderscore = 0x020B,

    /// <summary>Yen</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcYen = 0x020C,

    /// <summary>JP Comma</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcJpComma = 0x0210,

    /// <summary>Stop</summary>
    /// <remarks>Available on: Linux</remarks>
    VcStop = 0xFF78,

    /// <summary>Props</summary>
    /// <remarks>Available on: Linux</remarks>
    VcProps = 0xFF76,

    /// <summary>Front</summary>
    /// <remarks>Available on: Linux</remarks>
    VcFront = 0xFF77,

    /// <summary>Open</summary>
    /// <remarks>Available on: Linux</remarks>
    VcOpen = 0xFF74,

    /// <summary>Find</summary>
    /// <remarks>Available on: Linux</remarks>
    VcFind = 0xFF70,

    /// <summary>Again</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAgain = 0xFF79,

    /// <summary>Undo</summary>
    /// <remarks>Available on: Linux</remarks>
    VcUndo = 0xFF7A,

    /// <summary>Redo</summary>
    /// <remarks>Available on: Linux</remarks>
    VcRedo = 0xFF7F,

    /// <summary>Copy</summary>
    /// <remarks>Available on: Linux</remarks>
    VcCopy = 0xFF7C,

    /// <summary>Paste</summary>
    /// <remarks>Available on: Linux</remarks>
    VcPaste = 0xFF7D,

    /// <summary>Cut</summary>
    /// <remarks>Available on: Linux</remarks>
    VcCut = 0xFF7B,

    /// <summary>Line Feed</summary>
    /// <remarks>Available on: Linux</remarks>
    VcLineFeed = 0xC001,

    /// <summary>Macro</summary>
    /// <remarks>Available on: Linux</remarks>
    VcMacro = 0xC002,

    /// <summary>Scale</summary>
    /// <remarks>Available on: Linux</remarks>
    VcScale = 0xC003,

    /// <summary>Setup</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSetup = 0xC004,

    /// <summary>File</summary>
    /// <remarks>Available on: Linux</remarks>
    VcFile = 0xC005,

    /// <summary>Send File</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSendFile = 0xC006,

    /// <summary>Delete File</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeleteFile = 0xC007,

    /// <summary>MS DOS</summary>
    /// <remarks>Available on: Linux</remarks>
    VcMsDos = 0xC008,

    /// <summary>Lock</summary>
    /// <remarks>Available on: Linux</remarks>
    VcLock = 0xC009,

    /// <summary>Rotate Display</summary>
    /// <remarks>Available on: Linux</remarks>
    VcRotateDisplay = 0xC00A,

    /// <summary>Cycle Windows</summary>
    /// <remarks>Available on: Linux</remarks>
    VcCycleWindows = 0xC00B,

    /// <summary>Computer</summary>
    /// <remarks>Available on: Linux</remarks>
    VcComputer = 0xC00C,

    /// <summary>Phone</summary>
    /// <remarks>Available on: Linux</remarks>
    VcPhone = 0xC00D,

    /// <summary>ISO</summary>
    /// <remarks>Available on: Linux</remarks>
    VcIso = 0xC00E,

    /// <summary>Config</summary>
    /// <remarks>Available on: Linux</remarks>
    VcConfig = 0xC00F,

    /// <summary>Exit</summary>
    /// <remarks>Available on: Linux</remarks>
    VcExit = 0xC010,

    /// <summary>Move</summary>
    /// <remarks>Available on: Linux</remarks>
    VcMove = 0xC011,

    /// <summary>Edit</summary>
    /// <remarks>Available on: Linux</remarks>
    VcEdit = 0xC012,

    /// <summary>Scroll Up</summary>
    /// <remarks>Available on: Linux</remarks>
    VcScrollUp = 0xC013,

    /// <summary>Scroll Down</summary>
    /// <remarks>Available on: Linux</remarks>
    VcScrollDown = 0xC014,

    /// <summary>New</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNew = 0xC015,

    /// <summary>Play CD</summary>
    /// <remarks>Available on: Linux</remarks>
    VcPlayCd = 0xC016,

    /// <summary>Pause CD</summary>
    /// <remarks>Available on: Linux</remarks>
    VcPauseCd = 0xC017,

    /// <summary>Dashboard</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDashboard = 0xC018,

    /// <summary>Suspend</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSuspend = 0xC019,

    /// <summary>Close</summary>
    /// <remarks>Available on: Linux</remarks>
    VcClose = 0xC01A,

    /// <summary>Fast-Forward</summary>
    /// <remarks>Available on: Linux</remarks>
    VcFastForward = 0xC01C,

    /// <summary>Bass Boost</summary>
    /// <remarks>Available on: Linux</remarks>
    VcBassBoost = 0xC01D,

    /// <summary>HP</summary>
    /// <remarks>Available on: Linux</remarks>
    VcHp = 0xC01E,

    /// <summary>Camera</summary>
    /// <remarks>Available on: Linux</remarks>
    VcCamera = 0xC01F,

    /// <summary>Sound</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSound = 0xC020,

    /// <summary>Question</summary>
    /// <remarks>Available on: Linux</remarks>
    VcQuestion = 0xC021,

    /// <summary>Email</summary>
    /// <remarks>Available on: Linux</remarks>
    VcEmail = 0xC022,

    /// <summary>Chat</summary>
    /// <remarks>Available on: Linux</remarks>
    VcChat = 0xC023,

    /// <summary>Connect</summary>
    /// <remarks>Available on: Linux</remarks>
    VcConnect = 0xC024,

    /// <summary>Finance</summary>
    /// <remarks>Available on: Linux</remarks>
    VcFinance = 0xC025,

    /// <summary>Sport</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSport = 0xC026,

    /// <summary>Shop</summary>
    /// <remarks>Available on: Linux</remarks>
    VcShop = 0xC027,

    /// <summary>Alt Erase</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAltErase = 0xC028,

    /// <summary>Brightness Down</summary>
    /// <remarks>Available on: Linux</remarks>
    VcBrightnessDown = 0xC029,

    /// <summary>Brightness Up</summary>
    /// <remarks>Available on: Linux</remarks>
    VcBrightnessUp = 0xC02A,

    /// <summary>Brightness Cycle</summary>
    /// <remarks>Available on: Linux</remarks>
    VcBrightnesCycle = 0xC02B,

    /// <summary>Brightness Auto</summary>
    /// <remarks>Available on: Linux</remarks>
    VcBrightnessAuto = 0xC02C,

    /// <summary>Switch Video Mode</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSwitchVideoMode = 0xC02D,

    /// <summary>Keyboard Light Toggle</summary>
    /// <remarks>Available on: Linux</remarks>
    VcKeyboardLightToggle = 0xC02E,

    /// <summary>Keyboard Light Down</summary>
    /// <remarks>Available on: Linux</remarks>
    VcKeyboardLightDown = 0xC02F,

    /// <summary>Keyboard Light Up</summary>
    /// <remarks>Available on: Linux</remarks>
    VcKeyboardLightUp = 0xC030,

    /// <summary>Send</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSend = 0xC031,

    /// <summary>Reply</summary>
    /// <remarks>Available on: Linux</remarks>
    VcReply = 0xC032,

    /// <summary>Forward Mail</summary>
    /// <remarks>Available on: Linux</remarks>
    VcForwardMail = 0xC033,

    /// <summary>Save</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSave = 0xC034,

    /// <summary>Documents</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDocuments = 0xC035,

    /// <summary>Battery</summary>
    /// <remarks>Available on: Linux</remarks>
    VcBattery = 0xC036,

    /// <summary>Bluetooth</summary>
    /// <remarks>Available on: Linux</remarks>
    VcBluetooth = 0xC037,

    /// <summary>WLAN</summary>
    /// <remarks>Available on: Linux</remarks>
    VcWlan = 0xC038,

    /// <summary>UWB</summary>
    /// <remarks>Available on: Linux</remarks>
    VcUwb = 0xC039,

    /// <summary>Unknown key (X11)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcX11Unknown = 0xC03A,

    /// <summary>Next Video</summary>
    /// <remarks>Available on: Linux</remarks>
    VcVideoNext = 0xC03B,

    /// <summary>Previous Video</summary>
    /// <remarks>Available on: Linux</remarks>
    VcVideoPrevious = 0xC03C,

    /// <summary>Display Off</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDisplayOff = 0xC03D,

    /// <summary>WWAN</summary>
    /// <remarks>Available on: Linux</remarks>
    VcWwan = 0xC03E,

    /// <summary>RfKill</summary>
    /// <remarks>Available on: Linux</remarks>
    VcRfKill = 0xC03F
}
