using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Magic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using WorkWork.Memory;




namespace WorkWork.Bot
{
    class Other
    {
        //Release, retrieve, mount, walk, left, right, loot, drink, eat
        public Other(Main bot, BlackMagic magic, KeyboardSim keyboardSim, ObjectManager objectManager, Settings settings)
        {
            this.bot = bot;
            this.magic = magic;
            this.keyboardSim = keyboardSim;
            this.objectManager = objectManager;
            release = settings.GetKey("release");
            retrieve = settings.GetKey("retrieve");
            mount = settings.GetKey("mount");
            walk = settings.GetKey("walk");
            turnleft = settings.GetKey("turnleft");
            turnright = settings.GetKey("turnright");
            loot = settings.GetKey("loot");
            drink = settings.GetKey("drink");
            eat = settings.GetKey("eat");
            handle = FindWindow(null, "World of Warcraft");
        }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern IntPtr GetForegroundWindow();

        BlackMagic magic;
        IntPtr handle;
        Main bot;
        KeyboardSim keyboardSim;
        ObjectManager objectManager;

        private bool halt = false;

        private int release, retrieve, mount, walk, turnleft, turnright, loot, drink, eat;

        public void Interact()
        {
            
            var screen = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            var width = screen.Width;
            var height = screen.Height;
            bool found = false;
            int X = Cursor.Position.X;
            int Y = Cursor.Position.Y;
            IntPtr activeWindow = GetForegroundWindow();
            for (int i = 0; i < width; i += 40)
            {
                for (int j = 0; j < height; j += 40)
                {

                    Cursor.Position = new Point(i, j);
                    short cursor = magic.ReadShort((uint)TbcOffsets.General.Cursor);
                    if (cursor == 16 || cursor == 18 || cursor == 8 || cursor == 12)
                    {
                        SetForegroundWindow(handle);
                        keyboardSim.MouseButtonDown();
                        keyboardSim.MouseButtonUp();
                        keyboardSim.MouseButtonDown();
                        keyboardSim.MouseButtonUp();
                        Thread.Sleep(1000);
                        keyboardSim.KeyDown(loot);
                        keyboardSim.KeyUp(loot);
                        found = true;
                    }
                    else if (cursor == 13 || cursor == 11)
                    {
                        SetForegroundWindow(handle);
                        keyboardSim.MouseButtonDown();
                        keyboardSim.MouseButtonUp();
                        keyboardSim.MouseButtonDown();
                        keyboardSim.MouseButtonUp();
                        Thread.Sleep(3000);
                        keyboardSim.KeyDown(loot);
                        keyboardSim.KeyUp(loot);
                        found = true;
                    }
                    Thread.Sleep(1);
                    if (found)
                    {
                        break;
                    }
                }
                if (found)
                {
                    break;
                }
            }
            Cursor.Position = new Point(X, Y);
            SetForegroundWindow(activeWindow);

        }
        public bool Drink()
        {
            keyboardSim.KeyUp(walk);
            keyboardSim.KeyUp(turnleft);
            keyboardSim.KeyUp(turnright);
            keyboardSim.KeyDown(drink);

            keyboardSim.KeyUp(drink);
            return true;
        }
        public bool Eat()
        {
            keyboardSim.KeyUp(walk);
            keyboardSim.KeyUp(turnleft);
            keyboardSim.KeyUp(turnright);
            keyboardSim.KeyDown(eat);

            keyboardSim.KeyUp(eat);
            return true;
        }
        public void Rebuff()
        {
            bool drinking = false;
            bool eating = Eat();
            if (objectManager.GetPlayer().MaxMana > 0)
            {
                drinking = Drink();

            }
            if (eating && !drinking)
            {
                while (!halt && (objectManager.GetPlayer().Health < objectManager.GetPlayer().MaxHealth && magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) != (int)TbcOffsets.CombatState.InCombat))
                {
                    //We wait.
                }
            }
            else if (drinking && !eating)
            {
                while (!halt && (objectManager.GetPlayer().Mana < objectManager.GetPlayer().MaxMana && magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) != (int)TbcOffsets.CombatState.InCombat))
                {
                    //Again, we wait.
                }
            }
            else if (eating && drinking)
            {
                while (magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) != (int)TbcOffsets.CombatState.InCombat && !halt && (objectManager.GetPlayer().Mana < objectManager.GetPlayer().MaxMana || objectManager.GetPlayer().Health < objectManager.GetPlayer().MaxHealth))
                {
                    //Again, we wait.
                }
            }
            bot.GetCombat.afterCombatSpells(0, "aftercombat");
        }
        public void ReleaseScript()
        {
            keyboardSim.KeyDown(release);
            keyboardSim.KeyUp(release);
        }
        public void RetrieveScript()
        {
            keyboardSim.KeyDown(retrieve);
            keyboardSim.KeyUp(retrieve);
        }
        public void Mount(bool value)
        {
            keyboardSim.KeyDown(mount);
            keyboardSim.KeyUp(mount);
            if (value)
            {
                var watch = Stopwatch.StartNew();
                while (!halt && watch.Elapsed.Seconds < 3)
                {
                    //We wait to mount up.
                }
                watch.Stop();
            }
        }
        public void Regen()
        {
            bool eating = false;
            bool drinking = false;
            if (objectManager.GetPlayer().Health < bot.AverageHealth && objectManager.GetPlayer().Health < objectManager.GetPlayer().MaxHealth)
            {
                eating = Eat();

            }
            if (objectManager.GetPlayer().MaxMana > 0 && objectManager.GetPlayer().Mana < bot.AverageMana && objectManager.GetPlayer().Mana < objectManager.GetPlayer().MaxMana)
            {
                drinking = Drink();

            }
            if (eating && !drinking)
            {
                while (!halt && (objectManager.GetPlayer().Health < objectManager.GetPlayer().MaxHealth && magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) != (int)TbcOffsets.CombatState.InCombat))
                {
                    //We wait.
                }
            }
            else if (drinking && !eating)
            {
                while (!halt && (objectManager.GetPlayer().Mana < objectManager.GetPlayer().MaxMana && magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) != (int)TbcOffsets.CombatState.InCombat))
                {
                    //Again, we wait.
                }
            }
            else if (eating && drinking)
            {
                while (magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) != (int)TbcOffsets.CombatState.InCombat && !halt && (objectManager.GetPlayer().Mana < objectManager.GetPlayer().MaxMana || objectManager.GetPlayer().Health < objectManager.GetPlayer().MaxHealth))
                {
                    //Again, we wait.
                }
            }
        }
        public void Halt()
        {
            halt = true;
        }
    }
}
