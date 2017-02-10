using System;
using System.Windows.Forms;
using System.Windows.Input;

namespace WorkWork.Settings
{
    class SetKeys
    {
        private Key _pressedKey;
        private volatile bool _halt;
        TextBox textBox;
        int value;
        Settings settings;
        public SetKeys(TextBox textBox, int value, Settings settings)
        {
            this.textBox = textBox;
            this.value = value;
            this.settings = settings;
        }
        public void DoWork()
        {
            _pressedKey = new Key();
            while (!_halt)
            {
                foreach (Key key in Enum.GetValues(typeof(Key)))
                {
                    if (key != 0)
                    {
                        if (Keyboard.IsKeyDown(key))
                        {
                            _pressedKey = key;
                            break;
                        }
                    }


                }
                if (_pressedKey != Key.None)
                {
                    break;
                }
            }
            if (!_halt)
            {
                if (textBox.InvokeRequired)
                {
                    textBox.Invoke(new MethodInvoker(delegate { textBox.Text = _pressedKey.ToString(); }));

                }
                if (value < 13)
                {
                    settings.GeneralKeybinds[value, 1] = _pressedKey.ToString();
                    settings.Save();
                    settings.Changed = true;
                }

            }
        }
        public void Halt()
        {
            _halt = true;
        }
    }
}
