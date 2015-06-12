using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Windows.Input;


namespace WorkWork
{
    class SetKeys
    {
        Key pressedKey;
        private volatile bool halt = false;
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
            pressedKey = new Key();
            while (!halt)
            {
                foreach (Key key in Enum.GetValues(typeof(Key)))
                {
                    if (key != 0)
                    {
                        if (Keyboard.IsKeyDown(key))
                        {
                            pressedKey = key;
                            break;
                        }
                    }


                }
                if (pressedKey != Key.None)
                {
                    break;
                }
            }
            if (!halt)
            {
                if (textBox.InvokeRequired)
                {
                    textBox.Invoke(new MethodInvoker(delegate { textBox.Text = pressedKey.ToString(); }));

                }
                if (value < 13)
                {
                    settings.GeneralKeybinds[value, 1] = pressedKey.ToString();
                    settings.Save();
                    settings.Changed = true;
                }

            }
        }
        public void Halt()
        {
            halt = true;
        }
    }
}
