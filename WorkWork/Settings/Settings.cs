using System;
using System.IO;
using System.Windows.Forms;

namespace WorkWork.Settings
{
    //On-start settings, saved on-exit


    internal class Settings
    {
        private readonly string[] _keyNames = {
                                "lbutton","rbutton","cancel","mbutton","xbutton1","xbutton2","back","tab","clear","return","shift","control","menu","pause","capital","kana","hangeul","hangul","junja","final",
                                "hanja","kanji","escape","convert","nonconvert","accept","modechange","space","prior","next","end","home","left","up","right","down","select","print","execute","snapshot",
                                "insert","delete","help","0","1","2","3","4","5","6","7","8","9","a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s",
                                "t","u","v","w","x","y","z","lwin","rwin","apps","sleep","numpad0","numpad1","numpad2","numpad3","numpad4","numpad5","numpad6","numpad7","numpad8",
                                "numpad9","multiply","add","separator","subtract","decimal","divide","f1","f2","f3","f4","f5","f6","f7","f8","f9","f10","f11","f12","f13","f14","f15","f16","f17","f18",
                                "f19","f20","f21","f22","f23","f24","numlock","scroll","lshift","rshift","lcontrol","rcontrol","lmenu","rmenu","browser_back","browser_forward","browser_refresh",
                                "browser_stop","browser_search","browser_favorites","browser_home","volume_mute","volume_down","volume_up","media_next_track","media_prev_track","media_stop","media_play_pause","launch_mail",
                                "launch_media_select","launch_app1","launch_app2","oem_plus","oem_comma","oem_minus","oem_period","oem_2","oem_3","oem_4","oem_5","oem_6","oem_7","oem_8","oem_102","processkey","packet",
                                "attn","crsel","exsel","ereof","play","zoom","noname","pa1","oem_clear"
                            };
        private readonly ushort[] _keyValues =
        {
            0x01,0x02,0x03,0x04,0x05,0x06,0x08,0x09,0x0C,0x0D,0x10,0x11,0x12,0x13,0x14,0x15,0x15,0x17,0x18,0x19,0x19,0x1b,0x1c,0x1d,0x1e,0x1f,0x20,0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2a,
            0x2b,0x2c,0x2d,0x2e,0x2f,0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0x4a,0x4b,0x4c,0x4d,0x4e,0x4f,0x50,0x51,0x52,0x53,0x54,0x55,0x56,
            0x57,0x58,0x59,0x5a,0x5b,0x5c,0x5d,0x5f,0x60,0x61,0x62,0x63,0x64,0x65,0x66,0x67,0x68,0x69,0x6a,0x6b,0x6c,0x6d,0x6e,0x6f,0x70,0x71,0x72,0x73,0x74,0x75,0x76,0x77,
            0x78,0x79,0x7a,0x7b,0x7c,0x7d,0x7e,0x7f,0x80,0x81,0x82,0x83,0x84,0x85,0x86,0x87,0x90,0x91,0xA0,0xA1,0xA2,0xA3,0xA4,0xA5,0xA6,0xA7,0xa8,0xa9,0xaa,0xab,0xac,0xad,
            0xae,0xaf,0xb0,0xb1,0xb2,0xb3,0xb4,0xb5,0xb6,0xb7,0xba,0xbb,0xbc,0xbd,0xbe,0xbf,0xc0,0xdb,0xdc,0xdd,0xde,0xdf,0xe2,0xe5,0xe7,0xf6,0xf7,0xf8,0xf9,0xfa,0xfb,0xfc,0xfd,0xfe
        };

        public void Load()
        {
            var lines = File.ReadAllLines("../Settings.cfg");
            foreach (var line in lines)
            {
                if (line.ToLower().Contains("skinning"))
                {
                    Skinning = true;
                }
                else if (line.ToLower().Contains("ignoremobs"))
                {
                    IgnoreMobs = true;
                }
                else if (line.ToLower().Contains("ignoreplayers"))
                {
                    IgnorePlayers = true;
                }
                else if (line.ToLower().Contains("lastprofile"))
                {
                    var temp = line.Split('_');
                    Profile = temp[1];
                }
                else if (line.ToLower().Contains("lastspells"))
                {
                    var temp = line.Split('_');
                    Spells = temp[1];
                }
                else if (line.ToLower().Contains("mount"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[0, 1] = temp[1];
                    GeneralKeybinds[0, 0] = "mount";
                }
                else if (line.ToLower().Contains("walk"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[1, 1] = temp[1];
                    GeneralKeybinds[1, 0] = "walk";
                }
                else if (line.ToLower().Contains("turnleft"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[2, 1] = temp[1];
                    GeneralKeybinds[2, 0] = "turnleft";
                }
                else if (line.ToLower().Contains("turnright"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[3, 1] = temp[1];
                    GeneralKeybinds[3, 0] = "turnright";
                }
                else if (line.ToLower().Contains("godown"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[4, 1] = temp[1];
                    GeneralKeybinds[4, 0] = "godown";
                }
                else if (line.ToLower().Contains("goup"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[5, 1] = temp[1];
                    GeneralKeybinds[5, 0] = "goup";
                }
                else if (line.ToLower().Contains("target"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[6, 1] = temp[1];
                    GeneralKeybinds[6, 0] = "target";
                }
                else if (line.ToLower().Contains("loot"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[7, 1] = temp[1];
                    GeneralKeybinds[7, 0] = "loot";
                }
                else if (line.ToLower().Contains("drink"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[8, 1] = temp[1];
                    GeneralKeybinds[8, 0] = "drink";
                }
                else if (line.ToLower().Contains("eat"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[9, 1] = temp[1];
                    GeneralKeybinds[9, 0] = "eat";
                }
                else if (line.ToLower().Contains("enter"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[10, 1] = temp[1];
                    GeneralKeybinds[10, 0] = "enter";
                }
                else if (line.ToLower().Contains("xresolution"))
                {
                    var temp = line.Split('_');
                    XResolution = Convert.ToInt32(temp[1]);
                }
                else if (line.ToLower().Contains("yresolution"))
                {
                    var temp = line.Split('_');
                    YResolution = Convert.ToInt32(temp[1]);
                }
                else if (line.ToLower().Contains("gather"))
                {
                    Looting = true;
                }
                else if (line.ToLower().Contains("release"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[11, 1] = temp[1];
                    GeneralKeybinds[11, 0] = "release";
                }
                else if (line.ToLower().Contains("retrieve"))
                {
                    var temp = line.Split('_');
                    GeneralKeybinds[12, 1] = temp[1];
                    GeneralKeybinds[12, 0] = "retrieve";
                }
                else if (line.ToLower().Contains("linear"))
                {
                    Linear = true;
                }
                else if (line.ToLower().Contains("sell"))
                {
                    Sell = true;
                }
                else if (line.ToLower().Contains("sloops"))
                {
                    var temp = line.Split('_');
                    SellLoops = Convert.ToInt32(temp[1]);
                }
                else if (line.ToLower().Contains("herbing"))
                {
                    Herbing = true;
                }
                else if (line.ToLower().Contains("mining"))
                {
                    Mining = true;
                }
            }
        }
        
        public void Save()
        {
            TextWriter tW = new StreamWriter("../Settings.cfg");
            tW.WriteLine("[Settings]");
            if (Looting)
            {
                tW.WriteLine("gather");
            }
            if (Skinning)
            {
                tW.WriteLine("skinning");
            }
            if (Herbing)
            {
                tW.WriteLine("herbing");
            }
            if (Mining)
            {
                tW.WriteLine("mining");
            }
            if (Linear)
            {
                tW.WriteLine("linear");
            }
            if (Sell)
            {
                tW.WriteLine("sell");
            }
            if (SellLoops > -1)
            {
                tW.WriteLine("sloops_" + SellLoops);
            }
            if (Spells != null)
            {
                tW.WriteLine("lastspells_" + Spells);
            }
            if (Profile != null)
            {
                tW.WriteLine("lastprofile_" + Profile);
            }
            tW.WriteLine("xresolution_" + XResolution);
            tW.WriteLine("yresolution_" + YResolution);
            tW.WriteLine("[Keybinds]");
            for (var i = 0; i < GeneralKeybinds.GetLength(0); i++)
            {
                tW.WriteLine(GeneralKeybinds[i, 0] + "_" + GeneralKeybinds[i, 1]);
            }
            tW.Close();
        }
        
        public string Profile { get; set; }

        public string Spells { get; set; }

        public bool Skinning { get; set; }

        public ushort GetVKey(string value)
        {
            for (var i = 0; i < GeneralKeybinds.GetLength(0); i++)
            {
                if (value != GeneralKeybinds[i, 0]) continue;
                value = GeneralKeybinds[i, 1];
                break;
            }
            for (var i = 0; i < _keyNames.Length; i++)
            {
                if (value != _keyNames[i]) continue;
                var vk = _keyValues[i];
                return vk;
            }
            return 0x0;
        }
        
        public int GetKey(string value)
        {
            for (var i = 0; i < GeneralKeybinds.GetLength(0); i++)
            {
                if (value != GeneralKeybinds[i, 0]) continue;
                value = GeneralKeybinds[i, 1];
                break;
            }
            Keys key;
            Enum.TryParse(value, true, out key);
            return (int)key;
        }
        public bool IgnoreMobs { get; set; }

        public bool IgnorePlayers { get; set; }

        public int XResolution { get; set; }

        public int YResolution { get; set; }

        public string[,] GeneralKeybinds { get; } = new string[13, 2];

        public bool Looting { get; set; }

        public bool Changed { get; set; }

        public bool Linear { get; set; }

        public bool Sell { get; set; }

        public int SellLoops { get; set; } = 20;

        public bool Mining { get; set; }

        public bool Herbing { get; set; }
    }
}
