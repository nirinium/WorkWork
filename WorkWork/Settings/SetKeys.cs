using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace WorkWork.Settings
{
    internal class SetKeys
    {
        private Key _pressedKey;
        private volatile bool _halt;
        private readonly TextBox _textBox;
        private readonly int _value;
        private readonly Settings _settings;
        public SetKeys(TextBox textBox, int value, Settings settings)
        {
            _textBox = textBox;
            _value = value;
            _settings = settings;
        }
        public void DoWork()
        {
            _pressedKey = new Key();
            while (!_halt)
            {
                foreach (Key key in Enum.GetValues(typeof(Key)))
                {
                    if (key == 0) continue;
                    if (!Keyboard.IsKeyDown(key)) continue;
                    _pressedKey = key;
                    break;
                }
                if (_pressedKey != Key.None)
                {
                    break;
                }
            }
            if (_halt) return;
            if (_textBox.InvokeRequired)
            {
                _textBox.Invoke(new MethodInvoker(delegate { _textBox.Text = _pressedKey.ToString(); }));

            }
            if (_value >= 13) return;
            _settings.GeneralKeybinds[_value, 1] = _pressedKey.ToString();
            _settings.Save();
            _settings.Changed = true;
        }
        public void Halt()
        {
            _halt = true;
        }
    }
}
