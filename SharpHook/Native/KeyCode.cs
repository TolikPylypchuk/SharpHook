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
    VcBackquote = 0x00C0,

    /// <summary>§</summary>
    /// <remarks>Available on: macOS</remarks>
    VcSection = 0x00C1,

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

    /// <summary>+</summary>
    /// <remarks>Available on: Linux</remarks>
    VcPlus = 0x0209,

    /// <summary>-</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMinus = 0x002D,

    /// <summary>=</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcEquals = 0x003D,

    /// <summary>*</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAsterisk = 0x0097,

    /// <summary>@</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAt = 0x0200,

    /// <summary>&amp;</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAmpersand = 0x0096,

    /// <summary>$</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDollar = 0x0203,

    /// <summary>!</summary>
    /// <remarks>Available on: Linux</remarks>
    VcExclamationMark = 0x0205,

    /// <summary>¡</summary>
    /// <remarks>Available on: Linux</remarks>
    VcExclamationDown = 0x0206,

    /// <summary>Backspace (on Windows and Linux) or Delete (on macOS)</summary>
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

    /// <summary>:</summary>
    /// <remarks>Available on: Linux</remarks>
    VcColon = 0x0201,

    /// <summary>;</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcSemicolon = 0x003B,

    /// <summary>'</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcQuote = 0x00DE,

    /// <summary>"</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDoubleQuote = 0x0098,

    /// <summary>Enter</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcEnter = 0x000A,

    /// <summary>&lt;</summary>
    /// <remarks>Available on: Linux</remarks>
    VcLess = 0x0099,

    /// <summary>&gt;</summary>
    /// <remarks>Available on: Linux</remarks>
    VcGreater = 0x00A0,

    /// <summary>,</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcComma = 0x002C,

    /// <summary>.</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcPeriod = 0x002E,

    /// <summary>/</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcSlash = 0x002F,

    /// <summary>#</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumberSign = 0x0208,

    /// <summary>{</summary>
    /// <remarks>Available on: Linux</remarks>
    VcOpenBrace = 0x00A1,

    /// <summary>}</summary>
    /// <remarks>Available on: Linux</remarks>
    VcCloseBrace = 0x00A2,

    /// <summary>(</summary>
    /// <remarks>Available on: Linux</remarks>
    VcOpenParenthesis = 0x0207,

    /// <summary>)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcCloseParenthesis = 0x020A,

    /// <summary>Space</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcSpace = 0x0020,

    /// <summary>Miscellaneous OEM-specific key</summary>
    /// <remarks>Available on: Windows</remarks>
    VcMisc = 0x0E01,

    /// <summary>Print Screen</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcPrintScreen = 0x009A,

    /// <summary>Print</summary>
    /// <remarks>Available on: Windows</remarks>
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

    /// <summary>Delete (on Windows and Linux) or Forward Delete (on macOS)</summary>
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

    /// <summary>Begin</summary>
    /// <remarks>Available on: Linux</remarks>
    VcBegin = 0xFF58,

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
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadClear = 0x000C,

    /// <summary>Num-Pad Divide</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadDivide = 0x006F,

    /// <summary>Num-Pad Multiply</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadMultiply = 0x006A,

    /// <summary>Num-Pad Subtract</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadSubtract = 0x006D,

    /// <summary>Num-Pad Equals</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadEquals = 0x007C,

    /// <summary>Num-Pad Add</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadAdd = 0x006B,

    /// <summary>Num-Pad Enter</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcNumPadEnter = 0x007D,

    /// <summary>Num-Pad Decimal</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadDecimal = 0x006E,

    /// <summary>Num-Pad Separator</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadSeparator = 0x006C,

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

    /// <summary>Num-Pad End</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadEnd = 0xEE00 | VcNumPad1,

    /// <summary>Num-Pad Down</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadDown = 0xEE00 | VcNumPad2,

    /// <summary>Num-Pad Page Down</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadPageDown = 0xEE00 | VcNumPad3,

    /// <summary>Num-Pad Left</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadLeft = 0xEE00 | VcNumPad4,

    /// <summary>Num-Pad Begin</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadBegin = 0xEE | VcNumPad5,

    /// <summary>Num-Pad Right</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadRight = 0xEE00 | VcNumPad6,

    /// <summary>Num-Pad Home</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadHome = 0xEE00 | VcNumPad7,

    /// <summary>Num-Pad Up</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadUp = 0xEE00 | VcNumPad8,

    /// <summary>Num-Pad Page Up</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadPageUp = 0xEE00 | VcNumPad9,

    /// <summary>Num-Pad Insert</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadInsert = 0xEE00 | VcNumPad0,

    /// <summary>Num-Pad Delete</summary>
    /// <remarks>Available on: Linux</remarks>
    VcNumPadDelete = 0xEE00 | VcNumPadSeparator,

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

    /// <summary>Alt Graph</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAltGraph = 0xFF7E,

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

    /// <summary>Function key when used to change an input source on macOS</summary>
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

    /// <summary>Media Play</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMediaPlay = 0xE022,

    /// <summary>Media Stop</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcMediaStop = 0xE024,

    /// <summary>Media Previous</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMediaPrevious = 0xE010,

    /// <summary>Media Next</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMediaNext = 0xE019,

    /// <summary>Media Select</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcMediaSelect = 0xE06D,

    /// <summary>Media Eject</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcMediaEject = 0xE02C,

    /// <summary>Volume Mute</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcVolumeMute = 0xE020,

    /// <summary>Volume Down</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcVolumeDown = 0xE030,

    /// <summary>Volume Up</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcVolumeUp = 0xE02E,

    /// <summary>Launch app 1</summary>
    /// <remarks>Available on: Windows</remarks>
    VcApp1 = 0xE026,

    /// <summary>Launch app 2</summary>
    /// <remarks>Available on: Windows</remarks>
    VcApp2 = 0xE027,

    /// <summary>Launch browser</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAppBrowser = 0xE025,

    /// <summary>Launch calculator</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAppCalculator = 0xE021,

    /// <summary>Launch mail</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcAppMail = 0xE06C,

    /// <summary>Launch music</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAppMusic = 0xE03C,

    /// <summary>Launch pictures</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAppPictures = 0xE064,

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

    /// <summary>^</summary>
    /// <remarks>Available on: Linux</remarks>
    VcCircumflex = 0x0202,

    /// <summary>` (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadGrave = 0x0080,

    /// <summary>´ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadAcute = 0x0081,

    /// <summary>^ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadCircumflex = 0x0082,

    /// <summary>~ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadTilde = 0x0083,

    /// <summary>¯ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadMacron = 0x0084,

    /// <summary>˘ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadBreve = 0x0085,

    /// <summary>˙ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadAboveDot = 0x0086,

    /// <summary>¨ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadDiaeresis = 0x0087,

    /// <summary>˚ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadAboveRing = 0x0088,

    /// <summary>˝ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadDoubleAcute = 0x0089,

    /// <summary>ˇ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadCaron = 0x008A,

    /// <summary>¸ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadCedilla = 0x008B,

    /// <summary>˛ (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadOgonek = 0x008C,

    /// <summary>Iota (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadIota = 0x008D,

    /// <summary>Voiced Sound (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadVoicedSound = 0x008E,

    /// <summary>Semivoiced Sound (dead key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcDeadSemivoicedSound = 0x008F,

    /// <summary>IME Katakana mode</summary>
    /// <remarks>Available on: Linux</remarks>
    VcKatakana = 0x00F1,

    /// <summary>IME Kana mode</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcKana = 0x0015,

    /// <summary>IME Kana Lock</summary>
    /// <remarks>Available on: Linux</remarks>
    VcKanaLock = 0x0106,

    /// <summary>IME Kanji mode</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcKanji = 0x0019,

    /// <summary>IME Hiragana mode</summary>
    /// <remarks>Available on: Linux</remarks>
    VcHiragana = 0x00F2,

    /// <summary>IME Hangul mode</summary>
    /// <remarks>Available on: Windows</remarks>
    VcHangul = 0x00E9,

    /// <summary>IME Junja mode</summary>
    /// <remarks>Available on: Windows</remarks>
    VcJunja = 0x00E8,

    /// <summary>IME Final mode</summary>
    /// <remarks>Available on: Windows</remarks>
    VcFinal = 0x00E7,

    /// <summary>IME Hanja mode</summary>
    /// <remarks>Available on: Windows</remarks>
    VcHanja = 0x00E6,

    /// <summary>IME Accept</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcAccept = 0x001E,

    /// <summary>IME Convert (henkan)</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcConvert = 0x001C,

    /// <summary>IME Non-Convert (muhenkan)</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcNonConvert = 0x001D,

    /// <summary>IME Compose (multi-key)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcCompose = 0xFF20,

    /// <summary>IME On</summary>
    /// <remarks>Available on: Windows</remarks>
    VcImeOn = 0x0109,

    /// <summary>IME Off</summary>
    /// <remarks>Available on: Windows</remarks>
    VcImeOff = 0x0108,

    /// <summary>IME Mode Change</summary>
    /// <remarks>Available on: Windows</remarks>
    VcModeChange = 0x0107,

    /// <summary>IME Process</summary>
    /// <remarks>Available on: Windows</remarks>
    VcProcess = 0x0105,

    /// <summary>IME All Candidates (zen koho)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAllCandidates = 0x0100,

    /// <summary>IME Alphanumeric mode (eisū)</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcAlphanumeric = 0x00F0,

    /// <summary>IME Code Input (Kanji bangou)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcCodeInput = 0x0102,

    /// <summary>IME Wull Width (zenkaku)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcFullWidth = 0x00F3,

    /// <summary>IME Half Width (hankaku)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcHalfWidth = 0x00F4,

    /// <summary>IME Previous Candidate (mae koho)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcPreviousCandidate = 0x0101,

    /// <summary>IME Roman Characters (rōmaji)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcRomanCharacters = 0x00F5,

    /// <summary>_</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcUnderscore = 0x020B,

    /// <summary>Yen</summary>
    /// <remarks>Available on: macOS</remarks>
    VcYen = 0x020C,

    /// <summary>Stop (Sun keyboard)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSunStop = 0xFF78,

    /// <summary>Props (Sun keyboard)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSunProps = 0xFF76,

    /// <summary>Front (Sun keyboard)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSunFront = 0xFF77,

    /// <summary>Open (Sun keyboard)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSunOpen = 0xFF74,

    /// <summary>Find (Sun keyboard)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSunFind = 0xFF70,

    /// <summary>Again (Sun keyboard)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSunAgain = 0xFF79,

    /// <summary>Undo (Sun keyboard)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSunUndo = 0xFF7A,

    /// <summary>Copy (Sun keyboard)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSunCopy = 0xFF7C,

    /// <summary>Paste (Sun keyboard)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSunPaste = 0xFF7D,

    /// <summary>Cut (Sun keyboard)</summary>
    /// <remarks>Available on: Linux</remarks>
    VcSunCut = 0xFF7B
}
