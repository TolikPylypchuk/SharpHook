namespace SharpHook.Native;

/// <summary>
/// Represents a virtual key code.
/// </summary>
/// <seealso cref="KeyboardEventData" />
public enum KeyCode : ushort
{
    /// <summary>Escape</summary>
    VcEscape = 0x001B,

    /// <summary>F1</summary>
    VcF1 = 0x0070,

    /// <summary>F2</summary>
    VcF2 = 0x0071,

    /// <summary>F3</summary>
    VcF3 = 0x0072,

    /// <summary>F4</summary>
    VcF4 = 0x0073,

    /// <summary>F5</summary>
    VcF5 = 0x0074,

    /// <summary>F6</summary>
    VcF6 = 0x0075,

    /// <summary>F7</summary>
    VcF7 = 0x0076,

    /// <summary>F8</summary>
    VcF8 = 0x0077,

    /// <summary>F9</summary>
    VcF9 = 0x0078,

    /// <summary>F10</summary>
    VcF10 = 0x0079,

    /// <summary>F11</summary>
    VcF11 = 0x007A,

    /// <summary>F12</summary>
    VcF12 = 0x007B,

    /// <summary>F13</summary>
    VcF13 = 0xF000,

    /// <summary>F14</summary>
    VcF14 = 0xF001,

    /// <summary>F15</summary>
    VcF15 = 0xF002,

    /// <summary>F16</summary>
    VcF16 = 0xF003,

    /// <summary>F17</summary>
    VcF17 = 0xF004,

    /// <summary>F18</summary>
    VcF18 = 0xF005,

    /// <summary>F19</summary>
    VcF19 = 0xF006,

    /// <summary>F20</summary>
    VcF20 = 0xF007,

    /// <summary>F21</summary>
    VcF21 = 0xF008,

    /// <summary>F22</summary>
    VcF22 = 0xF009,

    /// <summary>F23</summary>
    VcF23 = 0xF00A,

    /// <summary>F24</summary>
    VcF24 = 0xF00B,

    /// <summary>`</summary>
    VcBackquote = 0x00C0,

    /// <summary>§</summary>
    VcSection = 0x00C1,

    /// <summary>0</summary>
    Vc0 = 0x0030,

    /// <summary>1</summary>
    Vc1 = 0x0031,

    /// <summary>2</summary>
    Vc2 = 0x0032,

    /// <summary>3</summary>
    Vc3 = 0x0033,

    /// <summary>4</summary>
    Vc4 = 0x0034,

    /// <summary>5</summary>
    Vc5 = 0x0035,

    /// <summary>6</summary>
    Vc6 = 0x0036,

    /// <summary>7</summary>
    Vc7 = 0x0037,

    /// <summary>8</summary>
    Vc8 = 0x0038,

    /// <summary>9</summary>
    Vc9 = 0x0039,

    /// <summary>+</summary>
    VcPlus = 0x0209,

    /// <summary>-</summary>
    VcMinus = 0x002D,

    /// <summary>=</summary>
    VcEquals = 0x003D,

    /// <summary>*</summary>
    VcAsterisk = 0x0097,

    /// <summary>@</summary>
    VcAt = 0x0200,

    /// <summary>&amp;</summary>
    VcAmpersand = 0x0096,

    /// <summary>$</summary>
    VcDollar = 0x0203,

    /// <summary>!</summary>
    VcExclamationMark = 0x0205,

    /// <summary>¡</summary>
    VcExclamationDown = 0x0206,

    /// <summary>Backspace</summary>
    VcBackspace = 0x0008,

    /// <summary>Tab</summary>
    VcTab = 0x0009,

    /// <summary>Caps Lock</summary>
    VcCapsLock = 0x0014,

    /// <summary>A</summary>
    VcA = 0x0041,

    /// <summary>B</summary>
    VcB = 0x0042,

    /// <summary>C</summary>
    VcC = 0x0043,

    /// <summary>D</summary>
    VcD = 0x0044,

    /// <summary>E</summary>
    VcE = 0x0045,

    /// <summary>F</summary>
    VcF = 0x0046,

    /// <summary>G</summary>
    VcG = 0x0047,

    /// <summary>H</summary>
    VcH = 0x0048,

    /// <summary>I</summary>
    VcI = 0x0049,

    /// <summary>J</summary>
    VcJ = 0x004A,

    /// <summary>K</summary>
    VcK = 0x004B,

    /// <summary>L</summary>
    VcL = 0x004C,

    /// <summary>M</summary>
    VcM = 0x004D,

    /// <summary>N</summary>
    VcN = 0x004E,

    /// <summary>O</summary>
    VcO = 0x004F,

    /// <summary>P</summary>
    VcP = 0x0050,

    /// <summary>Q</summary>
    VcQ = 0x0051,

    /// <summary>R</summary>
    VcR = 0x0052,

    /// <summary>S</summary>
    VcS = 0x0053,

    /// <summary>T</summary>
    VcT = 0x0054,

    /// <summary>U</summary>
    VcU = 0x0055,

    /// <summary>V</summary>
    VcV = 0x0056,

    /// <summary>W</summary>
    VcW = 0x0057,

    /// <summary>X</summary>
    VcX = 0x0058,

    /// <summary>Y</summary>
    VcY = 0x0059,

    /// <summary>Z</summary>
    VcZ = 0x005A,

    /// <summary>[</summary>
    VcOpenBracket = 0x005B,

    /// <summary>]</summary>
    VcCloseBracket = 0x005C,

    /// <summary>\</summary>
    VcBackSlash = 0x005D,

    /// <summary>:</summary>
    VcColon = 0x0201,

    /// <summary>;</summary>
    VcSemicolon = 0x003B,

    /// <summary>'</summary>
    VcQuote = 0x00DE,

    /// <summary>"</summary>
    VcDoubleQuote = 0x0098,

    /// <summary>Enter</summary>
    VcEnter = 0x000A,

    /// <summary>&lt;</summary>
    VcLess = 0x0099,

    /// <summary>&gt;</summary>
    VcGreater = 0x00A0,

    /// <summary>,</summary>
    VcComma = 0x002C,

    /// <summary>.</summary>
    VcPeriod = 0x002E,

    /// <summary>/</summary>
    VcSlash = 0x002F,

    /// <summary>#</summary>
    VcNumberSign = 0x0208,

    /// <summary>{</summary>
    VcOpenBrace = 0x00A1,

    /// <summary>{</summary>
    VcCloseBrace = 0x00A2,

    /// <summary>(</summary>
    VcOpenParenthesis = 0x0207,

    /// <summary>)</summary>
    VcCloseParenthesis = 0x020A,

    /// <summary>Space</summary>
    VcSpace = 0x0020,

    /// <summary>Print Screen</summary>
    VcPrintScreen = 0x009A,

    /// <summary>Scroll Lock</summary>
    VcScrollLock = 0x0091,

    /// <summary>Pause</summary>
    VcPause = 0x0013,

    /// <summary>Cancel</summary>
    VcCancel = 0x00D3,

    /// <summary>Insert</summary>
    VcInsert = 0x009B,

    /// <summary>Delete</summary>
    VcDelete = 0x007F,

    /// <summary>Home</summary>
    VcHome = 0x0024,

    /// <summary>End</summary>
    VcEnd = 0x0023,

    /// <summary>Page Up</summary>
    VcPageUp = 0x0021,

    /// <summary>Page Down</summary>
    VcPageDown = 0x0022,

    /// <summary>Up Arrow</summary>
    VcUp = 0x0026,

    /// <summary>Left Arrow</summary>
    VcLeft = 0x0025,

    /// <summary>Begin</summary>
    VcBegin = 0xFF58,

    /// <summary>Right Arrow</summary>
    VcRight = 0x0027,

    /// <summary>Down Arrow</summary>
    VcDown = 0x0028,

    /// <summary>Num Lock</summary>
    VcNumLock = 0x0090,

    /// <summary>Num-Pad Clear</summary>
    VcNumPadClear = 0x000C,

    /// <summary>Num-Pad Divide</summary>
    VcNumPadDivide = 0x006F,

    /// <summary>Num-Pad Multiply</summary>
    VcNumPadMultiply = 0x006A,

    /// <summary>Num-Pad Subtract</summary>
    VcNumPadSubtract = 0x006D,

    /// <summary>Num-Pad Equals</summary>
    VcNumPadEquals = 0x007C,

    /// <summary>Num-Pad Add</summary>
    VcNumPadAdd = 0x006B,

    /// <summary>Num-Pad Enter</summary>
    VcNumPadEnter = 0x007D,

    /// <summary>Num-Pad Decimal</summary>
    VcNumPadDecimal = 0x006E,

    /// <summary>Num-Pad Separator</summary>
    VcNumPadSeparator = 0x006C,

    /// <summary>Num-Pad Comma</summary>
    VcNumPadComma = 0x007E,

    /// <summary>Num-Pad 0</summary>
    VcNumPad0 = 0x0060,

    /// <summary>Num-Pad 1</summary>
    VcNumPad1 = 0x0061,

    /// <summary>Num-Pad 2</summary>
    VcNumPad2 = 0x0062,

    /// <summary>Num-Pad 3</summary>
    VcNumPad3 = 0x0063,

    /// <summary>Num-Pad 4</summary>
    VcNumPad4 = 0x0064,

    /// <summary>Num-Pad 5</summary>
    VcNumPad5 = 0x0065,

    /// <summary>Num-Pad 6</summary>
    VcNumPad6 = 0x0066,

    /// <summary>Num-Pad 7</summary>
    VcNumPad7 = 0x0067,

    /// <summary>Num-Pad 8</summary>
    VcNumPad8 = 0x0068,

    /// <summary>Num-Pad 9</summary>
    VcNumPad9 = 0x0069,

    /// <summary>Num-Pad End</summary>
    VcNumPadEnd = 0xEE00 | VcEnd,

    /// <summary>Num-Pad Down</summary>
    VcNumPadDown = 0xEE00 | VcDown,

    /// <summary>Num-Pad Page Down</summary>
    VcNumPadPageDown = 0xEE00 | VcPageDown,

    /// <summary>Num-Pad Left</summary>
    VcNumPadLeft = 0xEE00 | VcLeft,

    /// <summary>Num-Pad Begin</summary>
    VcNumPadBegin = 0xEE00 | VcBegin,

    /// <summary>Num-Pad Right</summary>
    VcNumPadRight = 0xEE00 | VcRight,

    /// <summary>Num-Pad Home</summary>
    VcNumPadHome = 0xEE00 | VcHome,

    /// <summary>Num-Pad Up</summary>
    VcNumPadUp = 0xEE00 | VcUp,

    /// <summary>Num-Pad Page Up</summary>
    VcNumPadPageUp = 0xEE00 | VcPageUp,

    /// <summary>Num-Pad Insert</summary>
    VcNumPadInsert = 0xEE00 | VcInsert,

    /// <summary>Num-Pad Delete</summary>
    VcNumPadDelete = 0xEE00 | VcDelete,

    /// <summary>Left Shift</summary>
    VcLeftShift = 0xA010,

    /// <summary>Right Shift</summary>
    VcRightShift = 0xB010,

    /// <summary>Left Control</summary>
    VcLeftControl = 0xA011,

    /// <summary>Right Control</summary>
    VcRightControl = 0xB011,

    /// <summary>
    /// Left Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    VcLeftAlt = 0xA012,

    /// <summary>
    /// Right Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    VcRightAlt = 0xB012,

    /// <summary>
    /// Alt Graph
    /// </summary>
    VcAltGraph = 0xFF7E,

    /// <summary>
    /// Left Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    VcLeftMeta = 0xA09D,

    /// <summary>
    /// Right Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    VcRightMeta = 0xB09D,

    /// <summary>Context Menu</summary>
    VcContextMenu = 0x020D,

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

    /// <summary>Volume Down</summary>
    VcVolumeDown = 0xE030,

    /// <summary>Volume Up</summary>
    VcVolumeUp = 0xE02E,

    /// <summary>Browser</summary>
    VcAppBrowser = 0xE020,

    /// <summary>Calculator</summary>
    VcAppCalculator = 0xE021,

    /// <summary>Mail</summary>
    VcAppMail = 0xE06C,

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

    /// <summary>^</summary>
    VcCircumflex = 0x0202,

    /// <summary>` (dead key)</summary>
    VcDeadGrave = 0x0080,

    /// <summary>´ (dead key)</summary>
    VcDeadAcute = 0x0081,

    /// <summary>^ (dead key)</summary>
    VcDeadCircumflex = 0x0082,

    /// <summary>~ (dead key)</summary>
    VcDeadTilde = 0x0083,

    /// <summary>¯ (dead key)</summary>
    VcDeadMacron = 0x0084,

    /// <summary>˘ (dead key)</summary>
    VcDeadBreve = 0x0085,

    /// <summary>˙ (dead key)</summary>
    VcDeadAboveDot = 0x0086,

    /// <summary>¨ (dead key)</summary>
    VcDeadDiaeresis = 0x0087,

    /// <summary>˚ (dead key)</summary>
    VcDeadAboveRing = 0x0088,

    /// <summary>˝ (dead key)</summary>
    VcDeadDoubleAcute = 0x0089,

    /// <summary>ˇ (dead key)</summary>
    VcDeadCaron = 0x008A,

    /// <summary>¸ (dead key)</summary>
    VcDeadCedilla = 0x008B,

    /// <summary>˛ (dead key)</summary>
    VcDeadOgonek = 0x008C,

    /// <summary>Iota (dead key)</summary>
    VcDeadIota = 0x008D,

    /// <summary>Voiced Sound (dead key)</summary>
    VcDeadVoicedSound = 0x008E,

    /// <summary>Semivoiced Sound (dead key)</summary>
    VcDeadSemivoicedSound = 0x008F,

    /// <summary>Katakana</summary>
    VcKatakana = 0x00F1,

    /// <summary>Kana</summary>
    VcKana = 0x0015,

    /// <summary>Kana Lock</summary>
    VcKanaLock = 0x0106,

    /// <summary>Kanji</summary>
    VcKanji = 0x0019,

    /// <summary>Hiragana</summary>
    VcHiragana = 0x00F2,

    /// <summary>Accept</summary>
    VcAccept = 0x001E,

    /// <summary>Convert</summary>
    VcConvert = 0x001C,

    /// <summary>Compose</summary>
    VcCompose = 0xFF20,

    /// <summary>Input Method On/Off</summary>
    VcInputMethodOnOff = 0x0107,

    /// <summary>All Candidates</summary>
    VcAllCandidates = 0x0100,

    /// <summary>Alphanumeric</summary>
    VcAlphanumeric = 0x00F0,

    /// <summary>Code Input</summary>
    VcCodeInput = 0x0102,

    /// <summary>Wull Width</summary>
    VcFullWidth = 0x00F3,

    /// <summary>Half Width</summary>
    VcHalfWidth = 0x00F4,

    /// <summary>Non-Convert</summary>
    VcNonConvert = 0x001D,

    /// <summary>Previous Candidate</summary>
    VcPreviousCandidate = 0x0101,

    /// <summary>Roman Characters</summary>
    VcRomanCharacters = 0x00F5,

    /// <summary>_</summary>
    VcUnderscore = 0x020B,

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

    /// <summary>Sun Paste</summary>
    VcSunPaste = 0xFF7D,

    /// <summary>Sun Cut</summary>
    VcSunCut = 0xFF7B,

    /// <summary>Undefined key</summary>
    VcUndefined = 0x0000,

    /// <summary>Undefined character</summary>
    CharUndefined = 0xFFFF
}
