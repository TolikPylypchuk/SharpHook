namespace SharpHook.Native;

/// <summary>
/// Represents a virtual key code.
/// </summary>
/// <seealso cref="KeyboardEventData" />
public enum KeyCode : ushort
{
    /// <summary>Escape</summary>
    VcEscape = 0x0001,

    /// <summary>F1</summary>
    VcF1 = 0x003B,

    /// <summary>F2</summary>
    VcF2 = 0x003C,

    /// <summary>F3</summary>
    VcF3 = 0x003D,

    /// <summary>F4</summary>
    VcF4 = 0x003E,

    /// <summary>F5</summary>
    VcF5 = 0x003F,

    /// <summary>F6</summary>
    VcF6 = 0x0040,

    /// <summary>F7</summary>
    VcF7 = 0x0041,

    /// <summary>F8</summary>
    VcF8 = 0x0042,

    /// <summary>F9</summary>
    VcF9 = 0x0043,

    /// <summary>F10</summary>
    VcF10 = 0x0044,

    /// <summary>F11</summary>
    VcF11 = 0x0057,

    /// <summary>F12</summary>
    VcF12 = 0x0058,

    /// <summary>F13</summary>
    VcF13 = 0x005B,

    /// <summary>F14</summary>
    VcF14 = 0x005C,

    /// <summary>F15</summary>
    VcF15 = 0x005D,

    /// <summary>F16</summary>
    VcF16 = 0x0063,

    /// <summary>F17</summary>
    VcF17 = 0x0064,

    /// <summary>F18</summary>
    VcF18 = 0x0065,

    /// <summary>F19</summary>
    VcF19 = 0x0066,

    /// <summary>F20</summary>
    VcF20 = 0x0067,

    /// <summary>F21</summary>
    VcF21 = 0x0068,

    /// <summary>F22</summary>
    VcF22 = 0x0069,

    /// <summary>F23</summary>
    VcF23 = 0x006A,

    /// <summary>F24</summary>
    VcF24 = 0x006B,

    /// <summary>`</summary>
    VcBackquote = 0x0029,

    /// <summary>1</summary>
    Vc1 = 0x0002,

    /// <summary>2</summary>
    Vc2 = 0x0003,

    /// <summary>3</summary>
    Vc3 = 0x0004,

    /// <summary>4</summary>
    Vc4 = 0x0005,

    /// <summary>5</summary>
    Vc5 = 0x0006,

    /// <summary>6</summary>
    Vc6 = 0x0007,

    /// <summary>7</summary>
    Vc7 = 0x0008,

    /// <summary>8</summary>
    Vc8 = 0x0009,

    /// <summary>9</summary>
    Vc9 = 0x000A,

    /// <summary>0</summary>
    Vc0 = 0x000B,

    /// <summary>-</summary>
    VcMinus = 0x000C,

    /// <summary>=</summary>
    VcEquals = 0x000D,

    /// <summary>Backspace</summary>
    VcBackspace = 0x000E,

    /// <summary>Tab</summary>
    VcTab = 0x000F,

    /// <summary>Caps Lock</summary>
    VcCapsLock = 0x003A,

    /// <summary>A</summary>
    VcA = 0x001E,

    /// <summary>B</summary>
    VcB = 0x0030,

    /// <summary>C</summary>
    VcC = 0x002E,

    /// <summary>D</summary>
    VcD = 0x0020,

    /// <summary>E</summary>
    VcE = 0x0012,

    /// <summary>F</summary>
    VcF = 0x0021,

    /// <summary>G</summary>
    VcG = 0x0022,

    /// <summary>H</summary>
    VcH = 0x0023,

    /// <summary>I</summary>
    VcI = 0x0017,

    /// <summary>J</summary>
    VcJ = 0x0024,

    /// <summary>K</summary>
    VcK = 0x0025,

    /// <summary>L</summary>
    VcL = 0x0026,

    /// <summary>M</summary>
    VcM = 0x0032,

    /// <summary>N</summary>
    VcN = 0x0031,

    /// <summary>O</summary>
    VcO = 0x0018,

    /// <summary>P</summary>
    VcP = 0x0019,

    /// <summary>Q</summary>
    VcQ = 0x0010,

    /// <summary>R</summary>
    VcR = 0x0013,

    /// <summary>S</summary>
    VcS = 0x001F,

    /// <summary>T</summary>
    VcT = 0x0014,

    /// <summary>U</summary>
    VcU = 0x0016,

    /// <summary>V</summary>
    VcV = 0x002F,

    /// <summary>W</summary>
    VcW = 0x0011,

    /// <summary>X</summary>
    VcX = 0x002D,

    /// <summary>Y</summary>
    VcY = 0x0015,

    /// <summary>Z</summary>
    VcZ = 0x002C,

    /// <summary>[</summary>
    VcOpenBracket = 0x001A,

    /// <summary>]</summary>
    VcCloseBracket = 0x001B,

    /// <summary>\</summary>
    VcBackSlash = 0x002B,

    /// <summary>;</summary>
    VcSemicolon = 0x0027,

    /// <summary>'</summary>
    VcQuote = 0x0028,

    /// <summary>Enter</summary>
    VcEnter = 0x001C,

    /// <summary>,</summary>
    VcComma = 0x0033,

    /// <summary>.</summary>
    VcPeriod = 0x0034,

    /// <summary>/</summary>
    VcSlash = 0x0035,

    /// <summary>Space</summary>
    VcSpace = 0x0039,

    /// <summary>Print Screen</summary>
    VcPrintScreen = 0x0E37,

    /// <summary>Scroll Lock</summary>
    VcScrollLock = 0x0046,

    /// <summary>Pause</summary>
    VcPause = 0x0E45,

    /// <summary>
    /// Either the angle bracket key or the backslash key on the RT 102-key keyboard
    /// </summary>
    VcLesserGreater = 0x0E46,

    /// <summary>Insert</summary>
    VcInsert = 0x0E52,

    /// <summary>Delete</summary>
    VcDelete = 0x0E53,

    /// <summary>Home</summary>
    VcHome = 0x0E47,

    /// <summary>End</summary>
    VcEnd = 0x0E4F,

    /// <summary>Page Up</summary>
    VcPageUp = 0x0E49,

    /// <summary>Page Down</summary>
    VcPageDown = 0x0E51,

    /// <summary>Up Arrow</summary>
    VcUp = 0xE048,

    /// <summary>Left Arrow</summary>
    VcLeft = 0xE04B,

    /// <summary>Clear</summary>
    VcClear = 0xE04C,

    /// <summary>Right Arrow</summary>
    VcRight = 0xE04D,

    /// <summary>Down Arrow</summary>
    VcDown = 0xE050,

    /// <summary>Num Lock</summary>
    VcNumLock = 0x0045,

    /// <summary>Num-Pad Divide</summary>
    VcNumPadDivide = 0x0E35,

    /// <summary>Num-Pad Multiply</summary>
    VcNumPadMultiply = 0x0037,

    /// <summary>Num-Pad Subtract</summary>
    VcNumPadSubtract = 0x004A,

    /// <summary>Num-Pad Equals</summary>
    VcNumPadEquals = 0x0E0D,

    /// <summary>Num-Pad Add</summary>
    VcNumPadAdd = 0x004E,

    /// <summary>Num-Pad Enter</summary>
    VcNumPadEnter = 0x0E1C,

    /// <summary>Num-Pad Separator</summary>
    VcNumPadSeparator = 0x0053,

    /// <summary>Num-Pad 1</summary>
    VcNumPad1 = 0x004F,

    /// <summary>Num-Pad 2</summary>
    VcNumPad2 = 0x0050,

    /// <summary>Num-Pad 3</summary>
    VcNumPad3 = 0x0051,

    /// <summary>Num-Pad 4</summary>
    VcNumPad4 = 0x004B,

    /// <summary>Num-Pad 5</summary>
    VcNumPad5 = 0x004C,

    /// <summary>Num-Pad 6</summary>
    VcNumPad6 = 0x004D,

    /// <summary>Num-Pad 7</summary>
    VcNumPad7 = 0x0047,

    /// <summary>Num-Pad 8</summary>
    VcNumPad8 = 0x0048,

    /// <summary>Num-Pad 9</summary>
    VcNumPad9 = 0x0049,

    /// <summary>Num-Pad 0</summary>
    VcNumPad0 = 0x0052,

    /// <summary>Num-Pad End</summary>
    VcNumPadEnd = 0xEE00 | VcNumPad1,

    /// <summary>Num-Pad Down</summary>
    VcNumPadDown = 0xEE00 | VcNumPad2,

    /// <summary>Num-Pad Page Down</summary>
    VcNumPadPageDown = 0xEE00 | VcNumPad3,

    /// <summary>Num-Pad Left</summary>
    VcNumPadLeft = 0xEE00 | VcNumPad4,

    /// <summary>Num-Pad Clear</summary>
    VcNumPadClear = 0xEE00 | VcNumPad5,

    /// <summary>Num-Pad Right</summary>
    VcNumPadRight = 0xEE00 | VcNumPad6,

    /// <summary>Num-Pad Home</summary>
    VcNumPadHome = 0xEE00 | VcNumPad7,

    /// <summary>Num-Pad Up</summary>
    VcNumPadUp = 0xEE00 | VcNumPad8,

    /// <summary>Num-Pad Page Up</summary>
    VcNumPadPageUp = 0xEE00 | VcNumPad9,

    /// <summary>Num-Pad Insert</summary>
    VcNumPadInsert = 0xEE00 | VcNumPad0,

    /// <summary>Num-Pad Delete</summary>
    VcNumPadDelete = 0xEE00 | VcNumPadSeparator,

    /// <summary>Left Shift</summary>
    VcLeftShift = 0x002A,

    /// <summary>Right Shift</summary>
    VcRightShift = 0x0036,

    /// <summary>Left Control</summary>
    VcLeftControl = 0x001D,

    /// <summary>Right Control</summary>
    VcRightControl = 0x0E1D,

    /// <summary>
    /// Left Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    VcLeftAlt = 0x0038,

    /// <summary>
    /// Right Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    VcRightAlt = 0x0E38,

    /// <summary>
    /// Left Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    VcLeftMeta = 0x0E5B,

    /// <summary>
    /// Right Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    VcRightMeta = 0x0E5C,

    /// <summary>Context Menu</summary>
    VcContextMenu = 0x0E5D,

    /// <summary>Power</summary>
    VcPower = 0xE05E,

    /// <summary>Sleep</summary>
    VcSleep = 0xE05F,

    /// <summary>Wake</summary>
    VcWake = 0xE063,

    /// <summary>Media Play</summary>
    VcMediaPlay = 0xE022,

    /// <summary>Media Stop</summary>
    VcMediaStop = 0xE024,

    /// <summary>Media Previous</summary>
    VcMediaPrevious = 0xE010,

    /// <summary>Media Next</summary>
    VcMediaNext = 0xE019,

    /// <summary>Media Select</summary>
    VcMediaSelect = 0xE06D,

    /// <summary>Media Eject</summary>
    VcMediaEject = 0xE02C,

    /// <summary>Volume Mute</summary>
    VcVolumeMute = 0xE020,

    /// <summary>Volume Up</summary>
    VcVolumeUp = 0xE030,

    /// <summary>Volume Down</summary>
    VcVolumeDown = 0xE02E,

    /// <summary>Mail</summary>
    VcAppMail = 0xE06C,

    /// <summary>Calculator</summary>
    VcAppCalculator = 0xE021,

    /// <summary>Music</summary>
    VcAppMusic = 0xE03C,

    /// <summary>Pictures</summary>
    VcAppPictures = 0xE064,

    /// <summary>Browser Search</summary>
    VcBrowserSearch = 0xE065,

    /// <summary>Browser Home</summary>
    VcBrowserHome = 0xE032,

    /// <summary>Browser Back</summary>
    VcBrowserBack = 0xE06A,

    /// <summary>Browser Forward</summary>
    VcBrowserForward = 0xE069,

    /// <summary>Browser Stop</summary>
    VcBrowserStop = 0xE068,

    /// <summary>Browser Refresh</summary>
    VcBrowserRefresh = 0xE067,

    /// <summary>Browser Favorites</summary>
    VcBrowserFavorites = 0xE066,

    /// <summary>Katakana</summary>
    VcKatakana = 0x0070,

    /// <summary>_</summary>
    VcUnderscore = 0x0073,

    /// <summary>Furigana</summary>
    VcFurigana = 0x0077,

    /// <summary>Kanji</summary>
    VcKanji = 0x0079,

    /// <summary>Hiragana</summary>
    VcHiragana = 0x007B,

    /// <summary>Yen</summary>
    VcYen = 0x007D,

    /// <summary>Num-Pad Comma</summary>
    VcNumPadComma = 0x007E,

    /// <summary>Sun Help</summary>
    VcSunHelp = 0xFF75,

    /// <summary>Sun Stop</summary>
    VcSunStop = 0xFF78,

    /// <summary>Sun Props</summary>
    VcSunProps = 0xFF76,

    /// <summary>Sun Front</summary>
    VcSunFront = 0xFF77,

    /// <summary>Sun Open</summary>
    VcSunOpen = 0xFF74,

    /// <summary>Sun Find</summary>
    VcSunFind = 0xFF7E,

    /// <summary>Sun Again</summary>
    VcSunAgain = 0xFF79,

    /// <summary>Sun Undo</summary>
    VcSunUndo = 0xFF7A,

    /// <summary>Sun Copy</summary>
    VcSunCopy = 0xFF7C,

    /// <summary>Sun Insert</summary>
    VcSunInsert = 0xFF7D,

    /// <summary>Sun Cut</summary>
    VcSunCut = 0xFF7B,

    /// <summary>Undefined key</summary>
    VcUndefined = 0x0000,

    /// <summary>Undefined character</summary>
    CharUndefined = 0xFFFF
}
