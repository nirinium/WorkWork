using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WorkWork.Settings
{
    //On-start settings, saved on-exit


    class Settings
    {
        private bool _changed;
        private string[,] generalKeybinds = new string[13, 2];
        private string[] keyNames = {
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
        private UInt16[] keyValues =
        {
            0x01,0x02,0x03,0x04,0x05,0x06,0x08,0x09,0x0C,0x0D,0x10,0x11,0x12,0x13,0x14,0x15,0x15,0x17,0x18,0x19,0x19,0x1b,0x1c,0x1d,0x1e,0x1f,0x20,0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2a,
            0x2b,0x2c,0x2d,0x2e,0x2f,0x30,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0x4a,0x4b,0x4c,0x4d,0x4e,0x4f,0x50,0x51,0x52,0x53,0x54,0x55,0x56,
            0x57,0x58,0x59,0x5a,0x5b,0x5c,0x5d,0x5f,0x60,0x61,0x62,0x63,0x64,0x65,0x66,0x67,0x68,0x69,0x6a,0x6b,0x6c,0x6d,0x6e,0x6f,0x70,0x71,0x72,0x73,0x74,0x75,0x76,0x77,
            0x78,0x79,0x7a,0x7b,0x7c,0x7d,0x7e,0x7f,0x80,0x81,0x82,0x83,0x84,0x85,0x86,0x87,0x90,0x91,0xA0,0xA1,0xA2,0xA3,0xA4,0xA5,0xA6,0xA7,0xa8,0xa9,0xaa,0xab,0xac,0xad,
            0xae,0xaf,0xb0,0xb1,0xb2,0xb3,0xb4,0xb5,0xb6,0xb7,0xba,0xbb,0xbc,0xbd,0xbe,0xbf,0xc0,0xdb,0xdc,0xdd,0xde,0xdf,0xe2,0xe5,0xe7,0xf6,0xf7,0xf8,0xf9,0xfa,0xfb,0xfc,0xfd,0xfe
        };
        private string
        _lastProfile,
        _lastSpells;
        private bool _skinning;
        private bool _ignoreMobs;
        private bool _ignorePlayers;
        private int _xresolution;
        private int _yresolution;
        private bool _looting;
        private bool _linear;
        private bool _sell;
        private int _sellLoops = 20;
        private bool _mining;
        private bool _herbing;
        public void Load()
        {
            string[] lines = File.ReadAllLines("../Settings.cfg");
            foreach (string line in lines)
            {
                if (line.ToLower().Contains("skinning"))
                {
                    _skinning = true;
                }
                else if (line.ToLower().Contains("ignoremobs"))
                {
                    _ignoreMobs = true;
                }
                else if (line.ToLower().Contains("ignoreplayers"))
                {
                    _ignorePlayers = true;
                }
                else if (line.ToLower().Contains("lastprofile"))
                {
                    string[] temp = line.Split('_');
                    _lastProfile = temp[1];
                }
                else if (line.ToLower().Contains("lastspells"))
                {
                    string[] temp = line.Split('_');
                    _lastSpells = temp[1];
                }
                else if (line.ToLower().Contains("mount"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[0, 1] = temp[1];
                    generalKeybinds[0, 0] = "mount";
                }
                else if (line.ToLower().Contains("walk"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[1, 1] = temp[1];
                    generalKeybinds[1, 0] = "walk";
                }
                else if (line.ToLower().Contains("turnleft"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[2, 1] = temp[1];
                    generalKeybinds[2, 0] = "turnleft";
                }
                else if (line.ToLower().Contains("turnright"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[3, 1] = temp[1];
                    generalKeybinds[3, 0] = "turnright";
                }
                else if (line.ToLower().Contains("godown"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[4, 1] = temp[1];
                    generalKeybinds[4, 0] = "godown";
                }
                else if (line.ToLower().Contains("goup"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[5, 1] = temp[1];
                    generalKeybinds[5, 0] = "goup";
                }
                else if (line.ToLower().Contains("target"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[6, 1] = temp[1];
                    generalKeybinds[6, 0] = "target";
                }
                else if (line.ToLower().Contains("loot"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[7, 1] = temp[1];
                    generalKeybinds[7, 0] = "loot";
                }
                else if (line.ToLower().Contains("drink"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[8, 1] = temp[1];
                    generalKeybinds[8, 0] = "drink";
                }
                else if (line.ToLower().Contains("eat"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[9, 1] = temp[1];
                    generalKeybinds[9, 0] = "eat";
                }
                else if (line.ToLower().Contains("enter"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[10, 1] = temp[1];
                    generalKeybinds[10, 0] = "enter";
                }
                else if (line.ToLower().Contains("xresolution"))
                {
                    string[] temp = line.Split('_');
                    _xresolution = Convert.ToInt32(temp[1]);
                }
                else if (line.ToLower().Contains("yresolution"))
                {
                    string[] temp = line.Split('_');
                    _yresolution = Convert.ToInt32(temp[1]);
                }
                else if (line.ToLower().Contains("gather"))
                {
                    _looting = true;
                }
                else if (line.ToLower().Contains("release"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[11, 1] = temp[1];
                    generalKeybinds[11, 0] = "release";
                }
                else if (line.ToLower().Contains("retrieve"))
                {
                    string[] temp = line.Split('_');
                    generalKeybinds[12, 1] = temp[1];
                    generalKeybinds[12, 0] = "retrieve";
                }
                else if (line.ToLower().Contains("linear"))
                {
                    _linear = true;
                }
                else if (line.ToLower().Contains("sell"))
                {
                    _sell = true;
                }
                else if (line.ToLower().Contains("sloops"))
                {
                    string[] temp = line.Split('_');
                    _sellLoops = Convert.ToInt32(temp[1]);
                }
                else if (line.ToLower().Contains("herbing"))
                {
                    _herbing = true;
                }
                else if (line.ToLower().Contains("mining"))
                {
                    _mining = true;
                }
            }
        }
        
        public void Save()
        {
            TextWriter tW = new StreamWriter("../Settings.cfg");
            tW.WriteLine("[Settings]");
            if (_looting)
            {
                tW.WriteLine("gather");
            }
            if (_skinning)
            {
                tW.WriteLine("skinning");
            }
            if (_herbing)
            {
                tW.WriteLine("herbing");
            }
            if (_mining)
            {
                tW.WriteLine("mining");
            }
            if (_linear)
            {
                tW.WriteLine("linear");
            }
            if (_sell)
            {
                tW.WriteLine("sell");
            }
            if (_sellLoops > -1)
            {
                tW.WriteLine("sloops_" + _sellLoops);
            }
            if (_lastSpells != null)
            {
                tW.WriteLine("lastspells_" + _lastSpells);
            }
            if (_lastProfile != null)
            {
                tW.WriteLine("lastprofile_" + _lastProfile);
            }
            tW.WriteLine("xresolution_" + _xresolution);
            tW.WriteLine("yresolution_" + _yresolution);
            tW.WriteLine("[Keybinds]");
            for (int i = 0; i < generalKeybinds.GetLength(0); i++)
            {
                tW.WriteLine(generalKeybinds[i, 0] + "_" + generalKeybinds[i, 1]);
            }
            tW.Close();
        }
        
        public string Profile
        {
          
            get { return _lastProfile; }
            
            set { _lastProfile = value; }
        }
        public string Spells
        {
            
            get { return _lastSpells; }
            
            set { _lastSpells = value; }
        }
        public bool Skinning
        {
            
            get { return _skinning; }
            
            set { _skinning = value; }
        }
        public UInt16 GetVKey(string value)
        {
            for (int i = 0; i < generalKeybinds.GetLength(0); i++)
            {
                if (value == generalKeybinds[i, 0])
                {
                    value = generalKeybinds[i, 1];
                    break;
                }
            }
            for (int i = 0; i < keyNames.Count(); i++)
            {
                if (value == keyNames[i])
                {
                    UInt16 vk = keyValues[i];
                    return vk;
                }
            }
            return 0x0;
        }
        
        public int GetKey(string value)
        {
            for (int i = 0; i < generalKeybinds.GetLength(0); i++)
            {
                if (value == generalKeybinds[i, 0])
                {
                    value = generalKeybinds[i, 1];
                    break;
                }
            }
            Keys key;
            Enum.TryParse(value, true, out key);
            return (int)key;
        }
        public bool IgnoreMobs
        {
            
            get { return _ignoreMobs; }
            
            set { _ignoreMobs = value; }
        }
        public bool IgnorePlayers
        {
            
            get { return _ignorePlayers; }
            
            set { _ignorePlayers = value; }
           
        }
        public int XResolution
        {
           
            get { return _xresolution; }
            
            set { _xresolution = value; }
        }
        public int YResolution
        {
            
            get { return _yresolution; }
            
            set { _yresolution = value; }
        }
        public string[,] GeneralKeybinds
        {
            
            get { return generalKeybinds; }
        }
        public bool Looting
        {
            
            get { return _looting; }
            
            set { _looting = value; }
        }
        public bool Changed
        {
            
            get { return _changed; }
            
            set { _changed = value; }
        }
        public bool Linear
        {
            get { return _linear; }
            set { _linear = value; }
        }
        public bool Sell
        {
            get { return _sell; }
            set { _sell = value; }
        }
        public int SellLoops
        {
            get { return _sellLoops; }
            set { _sellLoops = value; }
        }
        public bool Mining
        {
            get { return _mining; }
            set { _mining = value; }

        }
        public bool Herbing
        {
            get { return _herbing; }
            set { _herbing = value; }
        }
    }
}
