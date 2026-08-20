namespace SharpHook.Data;

/// <summary>
/// Represents a virtual key code.
/// </summary>
/// <seealso cref="KeyboardEventData" />
public enum KeyCode : ushort
{
    /// <summary>Undefined key</summary>
    VcUndefined = 0x00,

    /// <summary>Escape</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcEscape = 0x01,

    /// <summary>F1</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF1 = 0x02,

    /// <summary>F2</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF2 = 0x03,

    /// <summary>F3</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF3 = 0x04,

    /// <summary>F4</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF4 = 0x05,

    /// <summary>F5</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF5 = 0x06,

    /// <summary>F6</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF6 = 0x07,

    /// <summary>F7</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF7 = 0x08,

    /// <summary>F8</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF8 = 0x09,

    /// <summary>F9</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF9 = 0x0A,

    /// <summary>F10</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF10 = 0x0B,

    /// <summary>F11</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF11 = 0x0C,

    /// <summary>F12</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF12 = 0x0D,

    /// <summary>F13</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF13 = 0x0E,

    /// <summary>F14</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF14 = 0x0F,

    /// <summary>F15</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF15 = 0x10,

    /// <summary>F16</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF16 = 0x11,

    /// <summary>F17</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF17 = 0x12,

    /// <summary>F18</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF18 = 0x13,

    /// <summary>F19</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF19 = 0x14,

    /// <summary>F20</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF20 = 0x15,

    /// <summary>F21</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcF21 = 0x16,

    /// <summary>F22</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcF22 = 0x17,

    /// <summary>F23</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcF23 = 0x18,

    /// <summary>F24</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcF24 = 0x19,

    /// <summary>`</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcBackQuote = 0x20,

    /// <summary>1</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc1 = 0x21,

    /// <summary>2</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc2 = 0x22,

    /// <summary>3</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc3 = 0x23,

    /// <summary>4</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc4 = 0x24,

    /// <summary>5</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc5 = 0x25,

    /// <summary>6</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc6 = 0x26,

    /// <summary>7</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc7 = 0x27,

    /// <summary>8</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc8 = 0x28,

    /// <summary>9</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc9 = 0x29,

    /// <summary>0</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    Vc0 = 0x2A,

    /// <summary>-</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMinus = 0x2B,

    /// <summary>=</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcEquals = 0x2C,

    /// <summary>
    /// Backspace (on Windows and Linux) or Delete (on macOS)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcBackspace = 0x2D,

    /// <summary>Tab</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcTab = 0x2E,

    /// <summary>Caps Lock</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcCapsLock = 0x2F,

    /// <summary>A</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcA = 0x30,

    /// <summary>B</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcB = 0x31,

    /// <summary>C</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcC = 0x32,

    /// <summary>D</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcD = 0x33,

    /// <summary>E</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcE = 0x34,

    /// <summary>F</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcF = 0x35,

    /// <summary>G</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcG = 0x36,

    /// <summary>H</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcH = 0x37,

    /// <summary>I</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcI = 0x38,

    /// <summary>J</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcJ = 0x39,

    /// <summary>K</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcK = 0x3A,

    /// <summary>L</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcL = 0x3B,

    /// <summary>M</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcM = 0x3C,

    /// <summary>N</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcN = 0x3D,

    /// <summary>O</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcO = 0x3E,

    /// <summary>P</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcP = 0x3F,

    /// <summary>Q</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcQ = 0x40,

    /// <summary>R</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcR = 0x41,

    /// <summary>S</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcS = 0x42,

    /// <summary>T</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcT = 0x43,

    /// <summary>U</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcU = 0x44,

    /// <summary>V</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcV = 0x45,

    /// <summary>W</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcW = 0x46,

    /// <summary>X</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcX = 0x47,

    /// <summary>Y</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcY = 0x48,

    /// <summary>Z</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcZ = 0x49,

    /// <summary>[</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcOpenBracket = 0x4A,

    /// <summary>]</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcCloseBracket = 0x4B,

    /// <summary>\</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcBackslash = 0x4C,

    /// <summary>;</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcSemicolon = 0x4D,

    /// <summary>'</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcQuote = 0x4E,

    /// <summary>Enter</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcEnter = 0x4F,

    /// <summary>,</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcComma = 0x50,

    /// <summary>.</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcPeriod = 0x51,

    /// <summary>/</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcSlash = 0x52,

    /// <summary>Space</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcSpace = 0x53,

    /// <summary>
    /// The &lt;&gt; key on the US standard keyboard, or the \| key on the non-US 102-key keyboard,
    /// or the Section key (§) on the macOS ISO keyboard
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcSection = 0x54,

    /// <summary>Miscellaneous OEM-specific key</summary>
    /// <remarks>Available on: Windows</remarks>
    VcMisc = 0x55,

    /// <summary>Print Screen</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcPrintScreen = 0x60,

    /// <summary>Scroll Lock</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcScrollLock = 0x61,

    /// <summary>Pause</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcPause = 0x62,

    /// <summary>Cancel</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcCancel = 0x63,

    /// <summary>Help</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcHelp = 0x64,

    /// <summary>Insert</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcInsert = 0x65,

    /// <summary>
    /// Delete (on Windows and Linux) or Forward Delete (on macOS)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcDelete = 0x66,

    /// <summary>Home</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcHome = 0x67,

    /// <summary>End</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcEnd = 0x68,

    /// <summary>Page Up</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcPageUp = 0x69,

    /// <summary>Page Down</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcPageDown = 0x6A,

    /// <summary>Up Arrow</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcUp = 0x6B,

    /// <summary>Left Arrow</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcLeft = 0x6C,

    /// <summary>Right Arrow</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcRight = 0x6D,

    /// <summary>Down Arrow</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcDown = 0x6E,

    /// <summary>Num Lock</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcNumLock = 0x70,

    /// <summary>Num-Pad 1</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad1 = 0x71,

    /// <summary>Num-Pad 2</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad2 = 0x72,

    /// <summary>Num-Pad 3</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad3 = 0x73,

    /// <summary>Num-Pad 4</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad4 = 0x74,

    /// <summary>Num-Pad 5</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad5 = 0x75,

    /// <summary>Num-Pad 6</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad6 = 0x76,

    /// <summary>Num-Pad 7</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad7 = 0x77,

    /// <summary>Num-Pad 8</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad8 = 0x78,

    /// <summary>Num-Pad 9</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad9 = 0x79,

    /// <summary>Num-Pad 0</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPad0 = 0x7A,

    /// <summary>Num-Pad Clear</summary>
    /// <remarks>Available on: Windows, macOS</remarks>
    VcNumPadClear = 0x7B,

    /// <summary>Num-Pad /</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadDivide = 0x7C,

    /// <summary>Num-Pad *</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadMultiply = 0x7D,

    /// <summary>Num-Pad -</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadSubtract = 0x7E,

    /// <summary>Num-Pad =</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadEquals = 0x7F,

    /// <summary>Num-Pad +</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadAdd = 0x80,

    /// <summary>Num-Pad Enter</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadEnter = 0x81,

    /// <summary>Num-Pad Decimal</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcNumPadDecimal = 0x82,

    /// <summary>Num-Pad Separator</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcNumPadSeparator = 0x83,

    /// <summary>Left Shift</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcLeftShift = 0x90,

    /// <summary>Right Shift</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcRightShift = 0x91,

    /// <summary>Left Control</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcLeftControl = 0x92,

    /// <summary>Right Control</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcRightControl = 0x93,

    /// <summary>
    /// Left Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcLeftAlt = 0x94,

    /// <summary>
    /// Right Alt (on Windows and Linux) or Option (on macOS)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcRightAlt = 0x95,

    /// <summary>
    /// Left Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcLeftMeta = 0x96,

    /// <summary>
    /// Right Win (on Windows), Command (on macOS), or Super/Meta (on Linux)
    /// </summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcRightMeta = 0x97,

    /// <summary>Context Menu</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcContextMenu = 0x98,

    /// <summary>Function</summary>
    /// <remarks>Available on: macOS</remarks>
    VcFunction = 0x99,

    /// <summary>
    /// Function key when used to change an input source on macOS
    /// </summary>
    /// <remarks>Available on: macOS</remarks>
    VcChangeInputSource = 0x9A,

    /// <summary>Power</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcPower = 0xA0,

    /// <summary>Sleep</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcSleep = 0xA1,

    /// <summary>Play/Pause Media</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMediaPlay = 0xA2,

    /// <summary>Stop Media</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcMediaStop = 0xA3,

    /// <summary>Previous Media</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMediaPrevious = 0xA4,

    /// <summary>Next Media</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcMediaNext = 0xA5,

    /// <summary>Select Media</summary>
    /// <remarks>Available on: Windows</remarks>
    VcMediaSelect = 0xA6,

    /// <summary>Eject Media</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcMediaEject = 0xA7,

    /// <summary>Volume Mute</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcVolumeMute = 0xA8,

    /// <summary>Volume Down</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcVolumeDown = 0xA9,

    /// <summary>Volume Up</summary>
    /// <remarks>Available on: Windows, macOS, Linux</remarks>
    VcVolumeUp = 0xAA,

    /// <summary>Launch app 1</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcApp1 = 0xAB,

    /// <summary>Launch app 2</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcApp2 = 0xAC,

    /// <summary>Launch app 3</summary>
    /// <remarks>Available on: Linux</remarks>
    VcApp3 = 0xAD,

    /// <summary>Launch app 4</summary>
    /// <remarks>Available on: Linux</remarks>
    VcApp4 = 0xAE,

    /// <summary>Launch browser</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAppBrowser = 0xAF,

    /// <summary>Launch calculator</summary>
    /// <remarks>Available on: Linux</remarks>
    VcAppCalculator = 0xB0,

    /// <summary>Launch mail</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcAppMail = 0xB1,

    /// <summary>Browser Search</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserSearch = 0xB2,

    /// <summary>Browser Home</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserHome = 0xB3,

    /// <summary>Browser Back</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserBack = 0xB4,

    /// <summary>Browser Forward</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserForward = 0xB5,

    /// <summary>Browser Stop</summary>
    /// <remarks>Available on: Windows</remarks>
    VcBrowserStop = 0xB6,

    /// <summary>Browser Refresh</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserRefresh = 0xB7,

    /// <summary>Browser Favorites</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcBrowserFavorites = 0xB8,

    /// <summary>IME Katakana/Hiragana toggle</summary>
    /// <remarks>Available on: Linux</remarks>
    VcKatakanaHiragana = 0xC0,

    /// <summary>IME Katakana mode</summary>
    /// <remarks>Available on: Linux</remarks>
    VcKatakana = 0xC1,

    /// <summary>IME Hiragana mode</summary>
    /// <remarks>Available on: Linux</remarks>
    VcHiragana = 0xC2,

    /// <summary>IME Kana mode</summary>
    /// <remarks>Available on: Windows, macOS</remarks>
    VcKana = 0xC3,

    /// <summary>IME Junja mode</summary>
    /// <remarks>Available on: Windows</remarks>
    VcJunja = 0xC4,

    /// <summary>IME Final mode</summary>
    /// <remarks>Available on: Windows</remarks>
    VcFinal = 0xC5,

    /// <summary>IME Hanja mode</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcHanja = 0xC6,

    /// <summary>IME Accept</summary>
    /// <remarks>Available on: Windows</remarks>
    VcAccept = 0xC7,

    /// <summary>IME Convert (henkan)</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcConvert = 0xC8,

    /// <summary>IME Non-Convert (muhenkan)</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcNonConvert = 0xC9,

    /// <summary>IME On</summary>
    /// <remarks>Available on: Windows</remarks>
    VcImeOn = 0xCA,

    /// <summary>IME Off</summary>
    /// <remarks>Available on: Windows</remarks>
    VcImeOff = 0xCB,

    /// <summary>IME Mode Change</summary>
    /// <remarks>Available on: Windows, Linux</remarks>
    VcModeChange = 0xCC,

    /// <summary>IME Process</summary>
    /// <remarks>Available on: Windows</remarks>
    VcProcess = 0xCD,

    /// <summary>IME Alphanumeric mode (eisū)</summary>
    /// <remarks>Available on: macOS</remarks>
    VcAlphanumeric = 0xCE,

    /// <summary>_</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcUnderscore = 0xCF,

    /// <summary>Yen</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcYen = 0xD1,

    /// <summary>JP Comma</summary>
    /// <remarks>Available on: macOS, Linux</remarks>
    VcJpComma = 0xD2
}
