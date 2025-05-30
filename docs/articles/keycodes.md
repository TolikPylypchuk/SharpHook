# Key Code Mappings

The following table contains the mapping of virtual key codes defined in the `SharpHook.Data.KeyCode` enum to
OS-specific key code definitions. Note that several key codes are not available on all 3 OSes.

> [!IMPORTANT]
> Key code values in the `KeyCode` enum are meaningless and may change between major versions. Only the enum constant
> names should be taken into consideration (e.g. when persisting key codes).

Sources:
- Windows: [Microsoft Docs](https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes).
- macOS: `HIToolbox/Events.h` as defined in macOS 13.5 with some additional key codes included in libuiohook.
- X11: `/usr/share/X11/xkb/keycodes/evdev` as defined in Kubuntu 22.04
- Evdev: `input-event-codes.h` from the Linux source code.

<table>
  <thead>
    <tr>
      <th>SharpHook Key Code</th>
      <th>Windows Key Code</th>
      <th>macOS Key Code</th>
      <th>X11 Key Name</th>
      <th>Evdev Key Definition</th>
    <tr>
  </thead>
  <tbody>
    <tr>
      <td><code>VcUndefined</code></td>
      <td><em>Undefined</em></td>
      <td><em>Undefined</em></td>
      <td><em>Undefined</em></td>
      <td><em>Undefined</em></td>
    </tr>
    <tr>
      <td><code>VcEscape</code></td>
      <td><code>VK_ESCAPE</code></td>
      <td><code>kVK_Escape</code></td>
      <td><code>ESC</code></td>
      <td><code>KEY_ESC</code></td>
    </tr>
    <tr>
      <td><code>VcF1</code></td>
      <td><code>VK_F1</code></td>
      <td><code>kVK_F1</code></td>
      <td><code>FK01</code></td>
      <td><code>KEY_F1</code></td>
    </tr>
    <tr>
      <td><code>VcF2</code></td>
      <td><code>VK_F2</code></td>
      <td><code>kVK_F2</code></td>
      <td><code>FK02</code></td>
      <td><code>KEY_F2</code></td>
    </tr>
    <tr>
      <td><code>VcF3</code></td>
      <td><code>VK_F3</code></td>
      <td><code>kVK_F3</code></td>
      <td><code>FK03</code></td>
      <td><code>KEY_F3</code></td>
    </tr>
    <tr>
      <td><code>VcF4</code></td>
      <td><code>VK_F4</code></td>
      <td><code>kVK_F4</code></td>
      <td><code>FK04</code></td>
      <td><code>KEY_F4</code></td>
    </tr>
    <tr>
      <td><code>VcF5</code></td>
      <td><code>VK_F5</code></td>
      <td><code>kVK_F5</code></td>
      <td><code>FK05</code></td>
      <td><code>KEY_F5</code></td>
    </tr>
    <tr>
      <td><code>VcF6</code></td>
      <td><code>VK_F6</code></td>
      <td><code>kVK_F6</code></td>
      <td><code>FK06</code></td>
      <td><code>KEY_F6</code></td>
    </tr>
    <tr>
      <td><code>VcF7</code></td>
      <td><code>VK_F7</code></td>
      <td><code>kVK_F7</code></td>
      <td><code>FK07</code></td>
      <td><code>KEY_F7</code></td>
    </tr>
    <tr>
      <td><code>VcF8</code></td>
      <td><code>VK_F8</code></td>
      <td><code>kVK_F8</code></td>
      <td><code>FK08</code></td>
      <td><code>KEY_F8</code></td>
    </tr>
    <tr>
      <td><code>VcF9</code></td>
      <td><code>VK_F9</code></td>
      <td><code>kVK_F9</code></td>
      <td><code>FK09</code></td>
      <td><code>KEY_F9</code></td>
    </tr>
    <tr>
      <td><code>VcF10</code></td>
      <td><code>VK_F10</code></td>
      <td><code>kVK_F10</code></td>
      <td><code>FK010</code></td>
      <td><code>KEY_F10</code></td>
    </tr>
    <tr>
      <td><code>VcF11</code></td>
      <td><code>VK_F11</code></td>
      <td><code>kVK_F11</code></td>
      <td><code>FK011</code></td>
      <td><code>KEY_F11</code></td>
    </tr>
    <tr>
      <td><code>VcF12</code></td>
      <td><code>VK_F12</code></td>
      <td><code>kVK_F12</code></td>
      <td><code>FK012</code></td>
      <td><code>KEY_F12</code></td>
    </tr>
    <tr>
      <td><code>VcF13</code></td>
      <td><code>VK_F13</code></td>
      <td><code>kVK_F13</code></td>
      <td><code>FK013</code></td>
      <td><code>KEY_F13</code></td>
    </tr>
    <tr>
      <td><code>VcF14</code></td>
      <td><code>VK_F14</code></td>
      <td><code>kVK_F14</code></td>
      <td><code>FK014</code></td>
      <td><code>KEY_F14</code></td>
    </tr>
    <tr>
      <td><code>VcF15</code></td>
      <td><code>VK_F15</code></td>
      <td><code>kVK_F15</code></td>
      <td><code>FK015</code></td>
      <td><code>KEY_F15</code></td>
    </tr>
    <tr>
      <td><code>VcF16</code></td>
      <td><code>VK_F16</code></td>
      <td><code>kVK_F16</code></td>
      <td><code>FK016</code></td>
      <td><code>KEY_F16</code></td>
    </tr>
    <tr>
      <td><code>VcF17</code></td>
      <td><code>VK_F17</code></td>
      <td><code>kVK_F17</code></td>
      <td><code>FK017</code></td>
      <td><code>KEY_F17</code></td>
    </tr>
    <tr>
      <td><code>VcF18</code></td>
      <td><code>VK_F18</code></td>
      <td><code>kVK_F18</code></td>
      <td><code>FK018</code></td>
      <td><code>KEY_F18</code></td>
    </tr>
    <tr>
      <td><code>VcF19</code></td>
      <td><code>VK_F19</code></td>
      <td><code>kVK_F19</code></td>
      <td><code>FK019</code></td>
      <td><code>KEY_F19</code></td>
    </tr>
    <tr>
      <td><code>VcF20</code></td>
      <td><code>VK_F20</code></td>
      <td><code>kVK_F20</code></td>
      <td><code>FK020</code></td>
      <td><code>KEY_F20</code></td>
    </tr>
    <tr>
      <td><code>VcF21</code></td>
      <td><code>VK_F21</code></td>
      <td>-</td>
      <td><code>FK021</code></td>
      <td><code>KEY_F21</code></td>
    </tr>
    <tr>
      <td><code>VcF22</code></td>
      <td><code>VK_F22</code></td>
      <td>-</td>
      <td><code>FK022</code></td>
      <td><code>KEY_F22</code></td>
    </tr>
    <tr>
      <td><code>VcF23</code></td>
      <td><code>VK_F23</code></td>
      <td>-</td>
      <td><code>FK023</code></td>
      <td><code>KEY_F23</code></td>
    </tr>
    <tr>
      <td><code>VcF24</code></td>
      <td><code>VK_F24</code></td>
      <td>-</td>
      <td><code>FK024</code></td>
      <td><code>KEY_F24</code></td>
    </tr>
    <tr>
      <td><code>VcBackQuote</code></td>
      <td><code>VK_OEM_3</code></td>
      <td><code>kVK_ANSI_Grave</code></td>
      <td><code>TLDE</code></td>
      <td><code>KEY_GRAVE</code></td>
    </tr>
    <tr>
      <td><code>Vc0</code></td>
      <td><code>0x30</code>, <code>'0'</code></td>
      <td><code>kVK_ANSI_0</code></td>
      <td><code>AE10</code></td>
      <td><code>KEY_0</code></td>
    </tr>
    <tr>
      <td><code>Vc1</code></td>
      <td><code>0x31</code>, <code>'1'</code></td>
      <td><code>kVK_ANSI_1</code></td>
      <td><code>AE01</code></td>
      <td><code>KEY_1</code></td>
    </tr>
    <tr>
      <td><code>Vc2</code></td>
      <td><code>0x32</code>, <code>'2'</code></td>
      <td><code>kVK_ANSI_2</code></td>
      <td><code>AE02</code></td>
      <td><code>KEY_2</code></td>
    </tr>
    <tr>
      <td><code>Vc3</code></td>
      <td><code>0x33</code>, <code>'3'</code></td>
      <td><code>kVK_ANSI_3</code></td>
      <td><code>AE03</code></td>
      <td><code>KEY_3</code></td>
    </tr>
    <tr>
      <td><code>Vc4</code></td>
      <td><code>0x34</code>, <code>'4'</code></td>
      <td><code>kVK_ANSI_4</code></td>
      <td><code>AE04</code></td>
      <td><code>KEY_4</code></td>
    </tr>
    <tr>
      <td><code>Vc5</code></td>
      <td><code>0x35</code>, <code>'5'</code></td>
      <td><code>kVK_ANSI_5</code></td>
      <td><code>AE05</code></td>
      <td><code>KEY_5</code></td>
    </tr>
    <tr>
      <td><code>Vc6</code></td>
      <td><code>0x36</code>, <code>'6'</code></td>
      <td><code>kVK_ANSI_6</code></td>
      <td><code>AE06</code></td>
      <td><code>KEY_6</code></td>
    </tr>
    <tr>
      <td><code>Vc7</code></td>
      <td><code>0x37</code>, <code>'7'</code></td>
      <td><code>kVK_ANSI_7</code></td>
      <td><code>AE07</code></td>
      <td><code>KEY_7</code></td>
    </tr>
    <tr>
      <td><code>Vc8</code></td>
      <td><code>0x38</code>, <code>'8'</code></td>
      <td><code>kVK_ANSI_8</code></td>
      <td><code>AE08</code></td>
      <td><code>KEY_8</code></td>
    </tr>
    <tr>
      <td><code>Vc9</code></td>
      <td><code>0x39</code>, <code>'9'</code></td>
      <td><code>kVK_ANSI_9</code></td>
      <td><code>AE09</code></td>
      <td><code>KEY_9</code></td>
    </tr>
    <tr>
      <td><code>VcMinus</code></td>
      <td><code>VK_OEM_MINUS</code></td>
      <td><code>kVK_ANSI_Minus</code></td>
      <td><code>AE11</code></td>
      <td><code>KEY_MINUS</code></td>
    </tr>
    <tr>
      <td><code>VcEquals</code></td>
      <td><code>VK_OEM_PLUS</code></td>
      <td><code>kVK_ANSI_Equal</code></td>
      <td><code>AE12</code></td>
      <td><code>KEY_EQUAL</code></td>
    </tr>
    <tr>
      <td><code>VcBackspace</code></td>
      <td><code>VK_BACK</code></td>
      <td><code>kVK_Delete</code></td>
      <td><code>BKSP</code></td>
      <td><code>KEY_BACKSPACE</code></td>
    </tr>
    <tr>
      <td><code>VcTab</code></td>
      <td><code>VK_TAB</code></td>
      <td><code>kVK_Tab</code></td>
      <td><code>TAB</code></td>
      <td><code>KEY_TAB</code></td>
    </tr>
    <tr>
      <td><code>VcCapsLock</code></td>
      <td><code>VK_CAPITAL</code></td>
      <td><code>kVK_CapsLock</code></td>
      <td><code>CAPS</code></td>
      <td><code>KEY_CAPSLOCK</code></td>
    </tr>
    <tr>
      <td><code>VcA</code></td>
      <td><code>0x41</code>, <code>'A'</code></td>
      <td><code>kVK_ANSI_A</code></td>
      <td><code>AC01</code></td>
      <td><code>KEY_A</code></td>
    </tr>
    <tr>
      <td><code>VcB</code></td>
      <td><code>0x42</code>, <code>'B'</code></td>
      <td><code>kVK_ANSI_B</code></td>
      <td><code>AB05</code></td>
      <td><code>KEY_B</code></td>
    </tr>
    <tr>
      <td><code>VcC</code></td>
      <td><code>0x43</code>, <code>'C'</code></td>
      <td><code>kVK_ANSI_C</code></td>
      <td><code>AB03</code></td>
      <td><code>KEY_C</code></td>
    </tr>
    <tr>
      <td><code>VcD</code></td>
      <td><code>0x44</code>, <code>'D'</code></td>
      <td><code>kVK_ANSI_D</code></td>
      <td><code>AC03</code></td>
      <td><code>KEY_D</code></td>
    </tr>
    <tr>
      <td><code>VcE</code></td>
      <td><code>0x45</code>, <code>'E'</code></td>
      <td><code>kVK_ANSI_E</code></td>
      <td><code>AD03</code></td>
      <td><code>KEY_E</code></td>
    </tr>
    <tr>
      <td><code>VcF</code></td>
      <td><code>0x46</code>, <code>'F'</code></td>
      <td><code>kVK_ANSI_F</code></td>
      <td><code>AC04</code></td>
      <td><code>KEY_F</code></td>
    </tr>
    <tr>
      <td><code>VcG</code></td>
      <td><code>0x47</code>, <code>'G'</code></td>
      <td><code>kVK_ANSI_G</code></td>
      <td><code>AC05</code></td>
      <td><code>KEY_G</code></td>
    </tr>
    <tr>
      <td><code>VcH</code></td>
      <td><code>0x48</code>, <code>'H'</code></td>
      <td><code>kVK_ANSI_H</code></td>
      <td><code>AC06</code></td>
      <td><code>KEY_H</code></td>
    </tr>
    <tr>
      <td><code>VcI</code></td>
      <td><code>0x49</code>, <code>'I'</code></td>
      <td><code>kVK_ANSI_I</code></td>
      <td><code>AD08</code></td>
      <td><code>KEY_I</code></td>
    </tr>
    <tr>
      <td><code>VcJ</code></td>
      <td><code>0x4A</code>, <code>'J'</code></td>
      <td><code>kVK_ANSI_J</code></td>
      <td><code>AC07</code></td>
      <td><code>KEY_J</code></td>
    </tr>
    <tr>
      <td><code>VcK</code></td>
      <td><code>0x4B</code>, <code>'K'</code></td>
      <td><code>kVK_ANSI_K</code></td>
      <td><code>AC08</code></td>
      <td><code>KEY_K</code></td>
    </tr>
    <tr>
      <td><code>VcL</code></td>
      <td><code>0x4C</code>, <code>'L'</code></td>
      <td><code>kVK_ANSI_L</code></td>
      <td><code>AC09</code></td>
      <td><code>KEY_L</code></td>
    </tr>
    <tr>
      <td><code>VcM</code></td>
      <td><code>0x4D</code>, <code>'M'</code></td>
      <td><code>kVK_ANSI_M</code></td>
      <td><code>AB07</code></td>
      <td><code>KEY_M</code></td>
    </tr>
    <tr>
      <td><code>VcN</code></td>
      <td><code>0x4E</code>, <code>'N'</code></td>
      <td><code>kVK_ANSI_N</code></td>
      <td><code>AB06</code></td>
      <td><code>KEY_N</code></td>
    </tr>
    <tr>
      <td><code>VcO</code></td>
      <td><code>0x4F</code>, <code>'O'</code></td>
      <td><code>kVK_ANSI_O</code></td>
      <td><code>AD09</code></td>
      <td><code>KEY_O</code></td>
    </tr>
    <tr>
      <td><code>VcP</code></td>
      <td><code>0x50</code>, <code>'P'</code></td>
      <td><code>kVK_ANSI_P</code></td>
      <td><code>AD10</code></td>
      <td><code>KEY_P</code></td>
    </tr>
    <tr>
      <td><code>VcQ</code></td>
      <td><code>0x51</code>, <code>'Q'</code></td>
      <td><code>kVK_ANSI_Q</code></td>
      <td><code>AD01</code></td>
      <td><code>KEY_Q</code></td>
    </tr>
    <tr>
      <td><code>VcR</code></td>
      <td><code>0x52</code>, <code>'R'</code></td>
      <td><code>kVK_ANSI_R</code></td>
      <td><code>AD04</code></td>
      <td><code>KEY_R</code></td>
    </tr>
    <tr>
      <td><code>VcS</code></td>
      <td><code>0x53</code>, <code>'S'</code></td>
      <td><code>kVK_ANSI_S</code></td>
      <td><code>AC02</code></td>
      <td><code>KEY_S</code></td>
    </tr>
    <tr>
      <td><code>VcT</code></td>
      <td><code>0x54</code>, <code>'T'</code></td>
      <td><code>kVK_ANSI_T</code></td>
      <td><code>AD05</code></td>
      <td><code>KEY_T</code></td>
    </tr>
    <tr>
      <td><code>VcU</code></td>
      <td><code>0x55</code>, <code>'U'</code></td>
      <td><code>kVK_ANSI_U</code></td>
      <td><code>AD07</code></td>
      <td><code>KEY_U</code></td>
    </tr>
    <tr>
      <td><code>VcV</code></td>
      <td><code>0x56</code>, <code>'V'</code></td>
      <td><code>kVK_ANSI_V</code></td>
      <td><code>AB04</code></td>
      <td><code>KEY_V</code></td>
    </tr>
    <tr>
      <td><code>VcW</code></td>
      <td><code>0x57</code>, <code>'W'</code></td>
      <td><code>kVK_ANSI_W</code></td>
      <td><code>AD02</code></td>
      <td><code>KEY_W</code></td>
    </tr>
    <tr>
      <td><code>VcX</code></td>
      <td><code>0x58</code>, <code>'X'</code></td>
      <td><code>kVK_ANSI_X</code></td>
      <td><code>AB02</code></td>
      <td><code>KEY_X</code></td>
    </tr>
    <tr>
      <td><code>VcY</code></td>
      <td><code>0x59</code>, <code>'Y'</code></td>
      <td><code>kVK_ANSI_Y</code></td>
      <td><code>AD06</code></td>
      <td><code>KEY_Y</code></td>
    </tr>
    <tr>
      <td><code>VcZ</code></td>
      <td><code>0x5A</code>, <code>'Z'</code></td>
      <td><code>kVK_ANSI_Z</code></td>
      <td><code>AB01</code></td>
      <td><code>KEY_Z</code></td>
    </tr>
    <tr>
      <td><code>VcOpenBracket</code></td>
      <td><code>VK_OEM_4</code></td>
      <td><code>kVK_ANSI_LeftBracket</code></td>
      <td><code>AD11</code></td>
      <td><code>KEY_LEFTBRACE</code></td>
    </tr>
    <tr>
      <td><code>VcCloseBracket</code></td>
      <td><code>VK_OEM_6</code></td>
      <td><code>kVK_ANSI_RightBracket</code></td>
      <td><code>AD12</code></td>
      <td><code>KEY_RIGHTBRACE</code></td>
    </tr>
    <tr>
      <td><code>VcBackslash</code></td>
      <td><code>VK_OEM_5</code></td>
      <td><code>kVK_ANSI_Backslash</code></td>
      <td><code>AC12</code> and <code>BKSL</code></td>
      <td><code>KEY_BACKSLASH</code></td>
    </tr>
    <tr>
      <td><code>VcSemicolon</code></td>
      <td><code>VK_OEM_1</code></td>
      <td><code>kVK_ANSI_Semicolon</code></td>
      <td><code>AC10</code></td>
      <td><code>KEY_SEMICOLON</code></td>
    </tr>
    <tr>
      <td><code>VcQuote</code></td>
      <td><code>VK_OEM_7</code></td>
      <td><code>kVK_ANSI_Quote</code></td>
      <td><code>AC11</code></td>
      <td><code>KEY_APOSTROPHE</code></td>
    </tr>
    <tr>
      <td><code>VcEnter</code></td>
      <td><code>VK_RETURN</code></td>
      <td><code>kVK_Return</code></td>
      <td><code>RTRN</code></td>
      <td><code>KEY_ENTER</code></td>
    </tr>
    <tr>
      <td><code>VcComma</code></td>
      <td><code>VK_OEM_COMMA</code></td>
      <td><code>kVK_ANSI_Comma</code></td>
      <td><code>AB08</code></td>
      <td><code>KEY_COMMA</code></td>
    </tr>
    <tr>
      <td><code>VcPeriod</code></td>
      <td><code>VK_OEM_PERIOD</code></td>
      <td><code>kVK_ANSI_Period</code></td>
      <td><code>AB09</code></td>
      <td><code>KEY_DOT</code></td>
    </tr>
    <tr>
      <td><code>VcSlash</code></td>
      <td><code>VK_OEM_2</code></td>
      <td><code>kVK_ANSI_Slash</code></td>
      <td><code>AB10</code></td>
      <td><code>KEY_DOT</code></td>
    </tr>
    <tr>
      <td><code>VcSpace</code></td>
      <td><code>VK_SPACE</code></td>
      <td><code>kVK_Space</code></td>
      <td><code>SPCE</code></td>
      <td><code>KEY_SPACE</code></td>
    </tr>
    <tr>
      <td><code>Vc102</code></td>
      <td><code>VK_OEM_102</code></td>
      <td><code>kVK_ISO_Section</code></td>
      <td><code>LSGT</code></td>
      <td><code>KEY_102ND</code></td>
    </tr>
    <tr>
      <td><code>VcMisc</code></td>
      <td><code>VK_OEM_8</code></td>
      <td>-</td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcPrintScreen</code></td>
      <td><code>VK_SNAPSHOT</code></td>
      <td>-</td>
      <td><code>PRSC</code></td>
      <td><code>KEY_SYSRQ</code></td>
    </tr>
    <tr>
      <td><code>VcScrollLock</code></td>
      <td><code>VK_SCROLL</code></td>
      <td>-</td>
      <td><code>SCLK</code></td>
      <td><code>KEY_SCROLLLOCK</code></td>
    </tr>
    <tr>
      <td><code>VcPause</code></td>
      <td><code>VK_PAUSE</code></td>
      <td>-</td>
      <td><code>PAUS</code></td>
      <td><code>KEY_PAUSE</code></td>
    </tr>
    <tr>
      <td><code>VcCancel</code></td>
      <td><code>VK_CANCEL</code></td>
      <td>-</td>
      <td><code>I231</code></td>
      <td><code>KEY_CANCEL</code></td>
    </tr>
    <tr>
      <td><code>VcHelp</code></td>
      <td><code>VK_HELP</code></td>
      <td><code>kVK_Help</code></td>
      <td><code>HELP</code></td>
      <td><code>KEY_HELP</code></td>
    </tr>
    <tr>
      <td><code>VcInsert</code></td>
      <td><code>VK_INSERT</code></td>
      <td>-</td>
      <td><code>INS</code></td>
      <td><code>KEY_INSERT</code></td>
    </tr>
    <tr>
      <td><code>VcDelete</code></td>
      <td><code>VK_DELETE</code></td>
      <td><code>kVK_ForwardDelete</code></td>
      <td><code>DELE</code></td>
      <td><code>KEY_DELETE</code></td>
    </tr>
    <tr>
      <td><code>VcHome</code></td>
      <td><code>VK_HOME</code></td>
      <td><code>kVK_Home</code></td>
      <td><code>HOME</code></td>
      <td><code>KEY_HOME</code></td>
    </tr>
    <tr>
      <td><code>VcEnd</code></td>
      <td><code>VK_END</code></td>
      <td><code>kVK_End</code></td>
      <td><code>END</code></td>
      <td><code>KEY_END</code></td>
    </tr>
    <tr>
      <td><code>VcPageUp</code></td>
      <td><code>VK_PRIOR</code></td>
      <td><code>kVK_PageUp</code></td>
      <td><code>PGUP</code></td>
      <td><code>KEY_PAGEUP</code></td>
    </tr>
    <tr>
      <td><code>VcPageDown</code></td>
      <td><code>VK_NEXT</code></td>
      <td><code>kVK_PageDown</code></td>
      <td><code>PGDN</code></td>
      <td><code>KEY_PAGEDOWN</code></td>
    </tr>
    <tr>
      <td><code>VcUp</code></td>
      <td><code>VK_UP</code></td>
      <td><code>kVK_UpArrow</code></td>
      <td><code>UP</code></td>
      <td><code>KEY_UP</code></td>
    </tr>
    <tr>
      <td><code>VcLeft</code></td>
      <td><code>VK_LEFT</code></td>
      <td><code>kVK_LeftArrow</code></td>
      <td><code>LEFT</code></td>
      <td><code>KEY_LEFT</code></td>
    </tr>
    <tr>
      <td><code>VcRight</code></td>
      <td><code>VK_RIGHT</code></td>
      <td><code>kVK_RightArrow</code></td>
      <td><code>RGHT</code></td>
      <td><code>KEY_RIGHT</code></td>
    </tr>
    <tr>
      <td><code>VcDown</code></td>
      <td><code>VK_DOWN</code></td>
      <td><code>kVK_DownArrow</code></td>
      <td><code>DOWN</code></td>
      <td><code>KEY_DOWN</code></td>
    </tr>
    <tr>
      <td><code>VcNumLock</code></td>
      <td><code>VK_NUMLOCK</code></td>
      <td>-</td>
      <td><code>NMLK</code></td>
      <td><code>KEY_NUMLOCK</code></td>
    </tr>
    <tr>
      <td><code>VcNumPadClear</code></td>
      <td><code>VK_CLEAR</code> and <code>VK_OEM_CLEAR</code></td>
      <td><code>kVK_ANSI_KeypadClear</code></td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcNumPadDivide</code></td>
      <td><code>VK_DIVIDE</code></td>
      <td><code>kVK_ANSI_KeypadDivide</code></td>
      <td><code>KPDV</code></td>
      <td><code>KEY_KPASTERISK</code></td>
    </tr>
    <tr>
      <td><code>VcNumPadMultiply</code></td>
      <td><code>VK_MULTIPLY</code></td>
      <td><code>kVK_ANSI_KeypadMultiply</code></td>
      <td><code>KPMU</code></td>
      <td><code>KEY_KPSLASH</code></td>
    </tr>
    <tr>
      <td><code>VcNumPadSubtract</code></td>
      <td><code>VK_SUBTRACT</code></td>
      <td><code>kVK_ANSI_KeypadMinus</code></td>
      <td><code>KPSU</code></td>
      <td><code>KEY_KPMINUS</code></td>
    </tr>
    <tr>
      <td><code>VcNumPadEquals</code></td>
      <td><code>0x92</code></td>
      <td><code>kVK_ANSI_KeypadEquals</code></td>
      <td><code>KPEQ</code></td>
      <td><code>KEY_KPEQUAL</code></td>
    </tr>
    <tr>
      <td><code>VcNumPadAdd</code></td>
      <td><code>VK_ADD</code></td>
      <td><code>kVK_ANSI_KeypadPlus</code></td>
      <td><code>KPAD</code></td>
      <td><code>KEY_KPPLUS</code></td>
    </tr>
    <tr>
      <td><code>VcNumPadEnter</code></td>
      <td><code>VK_RETURN</code> with <code>KEYEVENTF_EXTENDEDKEY</code></td>
      <td><code>kVK_ANSI_KeypadEnter</code></td>
      <td><code>KPEN</code></td>
      <td><code>KEY_KPENTER</code></td>
    </tr>
    <tr>
      <td><code>VcNumPadDecimal</code></td>
      <td><code>VK_DECIMAL</code></td>
      <td><code>kVK_ANSI_KeypadDecimal</code></td>
      <td><code>KPDL</code></td>
      <td><code>KEY_KPDOT</code></td>
    </tr>
    <tr>
      <td><code>VcNumPadSeparator</code></td>
      <td><code>VK_SEPARATOR</code></td>
      <td>-</td>
      <td><code>I129</code></td>
      <td><code>KEY_KPCOMMA</code></td>
    </tr>
    <tr>
      <td><code>VcNumPad0</code></td>
      <td><code>VK_NUMPAD0</code></td>
      <td><code>kVK_ANSI_Keypad0</code></td>
      <td><code>KP0</code></td>
      <td><code>KEY_KP0</code></td>
    </tr>
    <tr>
      <td><code>VcNumPad1</code></td>
      <td><code>VK_NUMPAD1</code></td>
      <td><code>kVK_ANSI_Keypad1</code></td>
      <td><code>KP1</code></td>
      <td><code>KEY_KP1</code></td>
    </tr>
    <tr>
      <td><code>VcNumPad2</code></td>
      <td><code>VK_NUMPAD2</code></td>
      <td><code>kVK_ANSI_Keypad2</code></td>
      <td><code>KP2</code></td>
      <td><code>KEY_KP2</code></td>
    </tr>
    <tr>
      <td><code>VcNumPad3</code></td>
      <td><code>VK_NUMPAD3</code></td>
      <td><code>kVK_ANSI_Keypad3</code></td>
      <td><code>KP3</code></td>
      <td><code>KEY_KP3</code></td>
    </tr>
    <tr>
      <td><code>VcNumPad4</code></td>
      <td><code>VK_NUMPAD4</code></td>
      <td><code>kVK_ANSI_Keypad4</code></td>
      <td><code>KP4</code></td>
      <td><code>KEY_KP4</code></td>
    </tr>
    <tr>
      <td><code>VcNumPad5</code></td>
      <td><code>VK_NUMPAD5</code></td>
      <td><code>kVK_ANSI_Keypad6</code></td>
      <td><code>KP6</code></td>
      <td><code>KEY_KP6</code></td>
    </tr>
    <tr>
      <td><code>VcNumPad6</code></td>
      <td><code>VK_NUMPAD6</code></td>
      <td><code>kVK_ANSI_Keypad6</code></td>
      <td><code>KP6</code></td>
      <td><code>KEY_KP6</code></td>
    </tr>
    <tr>
      <td><code>VcNumPad7</code></td>
      <td><code>VK_NUMPAD7</code></td>
      <td><code>kVK_ANSI_Keypad7</code></td>
      <td><code>KP7</code></td>
      <td><code>KEY_KP7</code></td>
    </tr>
    <tr>
      <td><code>VcNumPad8</code></td>
      <td><code>VK_NUMPAD8</code></td>
      <td><code>kVK_ANSI_Keypad8</code></td>
      <td><code>KP8</code></td>
      <td><code>KEY_KP8</code></td>
    </tr>
    <tr>
      <td><code>VcNumPad9</code></td>
      <td><code>VK_NUMPAD9</code></td>
      <td><code>kVK_ANSI_Keypad9</code></td>
      <td><code>KP9</code></td>
      <td><code>KEY_KP9</code></td>
    </tr>
    <tr>
      <td><code>VcLeftShift</code></td>
      <td><code>VK_LSHIFT</code> and <code>VK_SHIFT</code></td>
      <td><code>kVK_Shift</code></td>
      <td><code>LFSH</code></td>
      <td><code>KEY_LEFTSHIFT</code></td>
    </tr>
    <tr>
      <td><code>VcRightShift</code></td>
      <td><code>VK_RSHIFT</code></td>
      <td><code>kVK_RightShift</code></td>
      <td><code>RTSH</code></td>
      <td><code>KEY_RIGHTSHIFT</code></td>
    </tr>
    <tr>
      <td><code>VcLeftControl</code></td>
      <td><code>VK_LCONTROL</code> and <code>VK_CONTROL</code></td>
      <td><code>kVK_Control</code></td>
      <td><code>LCTL</code></td>
      <td><code>KEY_LEFTCTRL</code></td>
    </tr>
    <tr>
      <td><code>VcRightControl</code></td>
      <td><code>VK_RCONTROL</code></td>
      <td><code>kVK_RightControl</code></td>
      <td><code>RCTL</code></td>
      <td><code>KEY_RIGHTCTRL</code></td>
    </tr>
    <tr>
      <td><code>VcLeftAlt</code></td>
      <td><code>VK_LMENU</code> and <code>VK_MENU</code></td>
      <td><code>kVK_Option</code></td>
      <td><code>LALT</code></td>
      <td><code>KEY_LEFTALT</code></td>
    </tr>
    <tr>
      <td><code>VcRightAlt</code></td>
      <td><code>VK_RMENU</code></td>
      <td><code>kVK_RightOption</code></td>
      <td><code>RALT</code></td>
      <td><code>KEY_RIGHTALT</code></td>
    </tr>
    <tr>
      <td><code>VcLeftMeta</code></td>
      <td><code>VK_LWIN</code></td>
      <td><code>kVK_Command</code></td>
      <td><code>LWIN</code> and <code>LMTA</code></td>
      <td><code>KEY_LEFTMETA</code></td>
    </tr>
    <tr>
      <td><code>VcRightMeta</code></td>
      <td><code>VK_RWIN</code></td>
      <td><code>kVK_RightCommand</code></td>
      <td><code>RWIN</code> and <code>RMTA</code></td>
      <td><code>KEY_RIGHTMETA</code></td>
    </tr>
    <tr>
      <td><code>VcContextMenu</code></td>
      <td><code>VK_APPS</code></td>
      <td><code>0x6E</code></td>
      <td><code>COMP</code> and <code>MENU</code></td>
      <td><code>KEY_COMPOSE</code></td>
    </tr>
    <tr>
      <td><code>VcFunction</code></td>
      <td>-</td>
      <td><code>kVK_Function</code></td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcChangeInputSource</code></td>
      <td>-</td>
      <td><code>0xB3</code></td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcPower</code></td>
      <td>-</td>
      <td><code>0xE6</code></td>
      <td><code>POWR</code></td>
      <td><code>KEY_POWER</code></td>
    </tr>
    <tr>
      <td><code>VcSleep</code></td>
      <td><code>VK_SLEEP</code></td>
      <td>-</td>
      <td><code>I150</code></td>
      <td><code>KEY_SLEEP</code></td>
    </tr>
    <tr>
      <td><code>VcMediaPlay</code></td>
      <td><code>VK_MEDIA_PLAY_PAUSE</code></td>
      <td><code>0xF0</code></td>
      <td><code>I172</code></td>
      <td><code>KEY_PLAYPAUSE</code></td>
    </tr>
    <tr>
      <td><code>VcMediaStop</code></td>
      <td><code>VK_MEDIA_STOP</code></td>
      <td>-</td>
      <td><code>I174</code></td>
      <td><code>KEY_STOPCD</code></td>
    </tr>
    <tr>
      <td><code>VcMediaPrevious</code></td>
      <td><code>VK_MEDIA_PREV_TRACK</code></td>
      <td><code>0xF2</code></td>
      <td><code>I173</code></td>
      <td><code>KEY_PREVIOUSSONG</code></td>
    </tr>
    <tr>
      <td><code>VcMediaNext</code></td>
      <td><code>VK_MEDIA_NEXT_TRACK</code></td>
      <td><code>0xF1</code></td>
      <td><code>I171</code></td>
      <td><code>KEY_NEXTSONG</code></td>
    </tr>
    <tr>
      <td><code>VcMediaSelect</code></td>
      <td><code>VK_LAUNCH_MEDIA_SELECT</code></td>
      <td>-</td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcMediaEject</code></td>
      <td>-</td>
      <td><code>0xEE</code></td>
      <td><code>I169</code></td>
      <td><code>KEY_EJECTCD</code></td>
    </tr>
    <tr>
      <td><code>VcVolumeMute</code></td>
      <td><code>VK_VOLUME_MUTE</code></td>
      <td><code>kVK_Mute</code></td>
      <td><code>MUTE</code></td>
      <td><code>KEY_MUTE</code></td>
    </tr>
    <tr>
      <td><code>VcVolumeDown</code></td>
      <td><code>VK_VOLUME_DOWN</code></td>
      <td><code>kVK_VolumeDown</code></td>
      <td><code>VOL-</code></td>
      <td><code>KEY_VOLUMEDOWN</code></td>
    </tr>
    <tr>
      <td><code>VcVolumeUp</code></td>
      <td><code>VK_VOLUME_UP</code></td>
      <td><code>kVK_VolumeUp</code></td>
      <td><code>VOL+</code></td>
      <td><code>KEY_VOLUMEUP</code></td>
    </tr>
    <tr>
      <td><code>VcApp1</code></td>
      <td><code>VK_LAUNCH_APP1</code></td>
      <td>-</td>
      <td><code>I156</code></td>
      <td><code>KEY_PROG1</code></td>
    </tr>
    <tr>
      <td><code>VcApp2</code></td>
      <td><code>VK_LAUNCH_APP2</code></td>
      <td>-</td>
      <td><code>I157</code></td>
      <td><code>KEY_PROG2</code></td>
    </tr>
    <tr>
      <td><code>VcApp3</code></td>
      <td>-</td>
      <td>-</td>
      <td><code>I210</code></td>
      <td><code>KEY_PROG3</code></td>
    </tr>
    <tr>
      <td><code>VcApp4</code></td>
      <td>-</td>
      <td>-</td>
      <td><code>I211</code></td>
      <td><code>KEY_PROG4</code></td>
    </tr>
    <tr>
      <td><code>VcAppBrowser</code></td>
      <td>-</td>
      <td>-</td>
      <td><code>I158</code></td>
      <td><code>KEY_WWW</code></td>
    </tr>
    <tr>
      <td><code>VcAppCalculator</code></td>
      <td>-</td>
      <td>-</td>
      <td><code>I148</code></td>
      <td><code>KEY_CALC</code></td>
    </tr>
    <tr>
      <td><code>VcAppMail</code></td>
      <td><code>VK_LAUNCH_MAIL</code></td>
      <td>-</td>
      <td><code>I163</code></td>
      <td><code>KEY_MAIL</code></td>
    </tr>
    <tr>
      <td><code>VcBrowserSearch</code></td>
      <td><code>VK_BROWSER_SEARCH</code></td>
      <td>-</td>
      <td><code>I225</code></td>
      <td><code>KEY_SEARCH</code></td>
    </tr>
    <tr>
      <td><code>VcBrowserHome</code></td>
      <td><code>VK_BROWSER_HOME</code></td>
      <td>-</td>
      <td><code>I180</code></td>
      <td><code>KEY_HOMEPAGE</code></td>
    </tr>
    <tr>
      <td><code>VcBrowserBack</code></td>
      <td><code>VK_BROWSER_BACK</code></td>
      <td>-</td>
      <td><code>I166</code></td>
      <td><code>KEY_BACK</code></td>
    </tr>
    <tr>
      <td><code>VcBrowserForward</code></td>
      <td><code>VK_BROWSER_FORWARD</code></td>
      <td>-</td>
      <td><code>I167</code></td>
      <td><code>KEY_FORWARD</code></td>
    </tr>
    <tr>
      <td><code>VcBrowserStop</code></td>
      <td><code>VK_BROWSER_STOP</code></td>
      <td>-</td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcBrowserRefresh</code></td>
      <td><code>VK_BROWSER_REFRESH</code></td>
      <td>-</td>
      <td><code>I181</code></td>
      <td><code>KEY_REFRESH</code></td>
    </tr>
    <tr>
      <td><code>VcBrowserFavorites</code></td>
      <td><code>VK_BROWSER_FAVORITES</code></td>
      <td>-</td>
      <td><code>I164</code></td>
      <td><code>KEY_BOOKMARKS</code></td>
    </tr>
    <tr>
      <td><code>VcKatakanaHiragana</code></td>
      <td>-</td>
      <td>-</td>
      <td><code>HKTG</code></td>
      <td><code>KEY_KATAKANAHIRAGANA</code></td>
    </tr>
    <tr>
      <td><code>VcKatakana</code></td>
      <td>-</td>
      <td>-</td>
      <td><code>KATA</code></td>
      <td><code>KEY_KATAKANA</code></td>
    </tr>
    <tr>
      <td><code>VcHiragana</code></td>
      <td>-</td>
      <td>-</td>
      <td><code>HIRA</code></td>
      <td><code>KEY_HIRAGANA</code></td>
    </tr>
    <tr>
      <td><code>VcKana</code></td>
      <td><code>VK_KANA</code></td>
      <td><code>kVK_JIS_Kana</code></td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcKanji</code></td>
      <td><code>VK_KANJI</code></td>
      <td>-</td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcHangul</code></td>
      <td><code>VK_HANGUL</code></td>
      <td>-</td>
      <td><code>HNGL</code></td>
      <td><code>KEY_HANGEUL</code></td>
    </tr>
    <tr>
      <td><code>VcJunja</code></td>
      <td><code>VK_JUNJA</code></td>
      <td>-</td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcFinal</code></td>
      <td><code>VK_FINAL</code></td>
      <td>-</td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcHanja</code></td>
      <td><code>VK_HANJA</code></td>
      <td>-</td>
      <td><code>HJCV</code></td>
      <td><code>KEY_HANJA</code></td>
    </tr>
    <tr>
      <td><code>VcAccept</code></td>
      <td><code>VK_ACCEPT</code></td>
      <td>-</td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcConvert</code></td>
      <td><code>VK_CONVERT</code></td>
      <td>-</td>
      <td><code>HENK</code></td>
      <td><code>KEY_HENKAN</code></td>
    </tr>
    <tr>
      <td><code>VcNonConvert</code></td>
      <td><code>VK_NONCONVERT</code></td>
      <td>-</td>
      <td><code>MUHE</code></td>
      <td><code>KEY_MUHENKAN</code></td>
    </tr>
    <tr>
      <td><code>VcImeOn</code></td>
      <td><code>VK_IME_ON</code></td>
      <td>-</td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcImeOff</code></td>
      <td><code>VK_IME_OFF</code></td>
      <td>-</td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcModeChange</code></td>
      <td><code>VK_MODECHANGE</code></td>
      <td>-</td>
      <td><code>I155</code></td>
      <td><code>KEY_XFER</code></td>
    </tr>
    <tr>
      <td><code>VcProcess</code></td>
      <td><code>VK_PROCESSKEY</code></td>
      <td>-</td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcAlphanumeric</code></td>
      <td>-</td>
      <td><code>kVK_JIS_Eisu</code></td>
      <td>-</td>
      <td>-</td>
    </tr>
    <tr>
      <td><code>VcUnderscore</code></td>
      <td>-</td>
      <td><code>kVK_JIS_Underscore</code></td>
      <td><code>AB11</code></td>
      <td><code>KEY_RO</code></td>
    </tr>
    <tr>
      <td><code>VcYen</code></td>
      <td>-</td>
      <td><code>kVK_JIS_Yen</code></td>
      <td><code>AE13</code></td>
      <td><code>KEY_YEN</code></td>
    </tr>
    <tr>
      <td><code>VcJpComma</code></td>
      <td>-</td>
      <td><code>kVK_JIS_KeypadComma</code></td>
      <td><code>JPCM</code></td>
      <td><code>KEY_KPJPCOMMA</code></td>
    </tr>
  <tbody>
</table>
