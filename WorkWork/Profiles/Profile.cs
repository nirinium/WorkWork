using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace WorkWork.Profiles
{
    class Profile
    {
        private List<float[]> wayPoints = new List<float[]>();
        private List<float[]> ghostPoints = new List<float[]>();
        private List<float[]> sellPoints = new List<float[]>();
        private bool _loop;
        private bool _isWaySet;
        private bool _isGhostSet;
        private bool _isSellSet;
        private bool _ignoreZ = true;
        private List<string> ignoredMobs = new List<string>();
        private List<ulong> ignoredMobsGuid = new List<ulong>();
        private int _ghostpaths;
        public void Load(string value)
        {
            string[] lines = File.ReadAllLines(value);
            foreach (string line in lines)
            {
                if (line.ToLower().Contains("w_"))
                {
                    string[] words = line.Split('_');
                    float[] temp = new float[4];
                    temp[0] = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
                    temp[1] = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
                    temp[2] = float.Parse(words[4], CultureInfo.InvariantCulture.NumberFormat);
                    temp[3] = float.Parse(words[5], CultureInfo.InvariantCulture.NumberFormat);
                    wayPoints.Add(temp);
                    _isWaySet = true;
                }
                else if (line.ToLower().Contains("g_"))
                {
                    string[] words = line.Split('_');
                    float[] temp = new float[4];
                    temp[0] = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
                    temp[1] = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
                    temp[2] = float.Parse(words[4], CultureInfo.InvariantCulture.NumberFormat);
                    temp[3] = float.Parse(words[5], CultureInfo.InvariantCulture.NumberFormat);
                    ghostPoints.Add(temp);
                    _isGhostSet = true;
                }
                else if (line.ToLower().Contains("s_"))
                {
                    string[] words = line.Split('_');
                    float[] temp = new float[4];
                    temp[0] = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
                    temp[1] = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
                    temp[2] = float.Parse(words[4], CultureInfo.InvariantCulture.NumberFormat);
                    temp[3] = float.Parse(words[5], CultureInfo.InvariantCulture.NumberFormat);
                    sellPoints.Add(temp);
                    _isSellSet = true;
                }
                else if (line.ToLower().Contains("loop"))
                {
                    _loop = true;
                }
                else if (line.ToLower().Contains("ignorez"))
                {
                    _ignoreZ = true;
                }
                else if (line.ToLower().Contains("ignoredmob_"))
                {
                    string[] words = line.Split('_');
                    ignoredMobs.Add(words[1]);
                }
                else if (line.ToLower().Contains("ghostpath_"))
                {
                    string[] words = line.Split('_');
                    _ghostpaths = Convert.ToInt32(words[1]);
                }
                else if (line.ToLower().Contains("ignoredmobguid_"))
                {
                    string[] words = line.Split('_');
                    ignoredMobsGuid.Add(Convert.ToUInt64(words[1]));
                }
            }
        }
        public void Save(string value)
        {
            TextWriter tW = new StreamWriter(value);
            tW.WriteLine("[Profile]");
            if (_loop)
            {
                tW.WriteLine("loop");
            }
            if (_ignoreZ)
            {
                tW.WriteLine("ignorez");
            }
            tW.WriteLine("ghostpath_" + _ghostpaths);
            tW.WriteLine("[IgnoredMobs]");
            if (ignoredMobs.Count > 0)
            {
                for (int i = 0; i < ignoredMobs.Count; i++)
                {
                    tW.WriteLine("ignoredmob_" + ignoredMobs[i]);
                }
            }
            tW.WriteLine("[IgnoredMobsGuid]");
            if (ignoredMobsGuid.Count>0)
            {
                for (int i = 0; i < ignoredMobsGuid.Count; i++)
                {
                    tW.WriteLine("ignoredmobguid_" + ignoredMobsGuid[i]);
                }
            }
            tW.WriteLine("[Waypoints]");
            if (wayPoints.Count > 0)
            {
                for (int i = 0; i < wayPoints.Count; i++)
                {
                    float[] temp = wayPoints[i];
                    tW.WriteLine("w_" + i + "_" + temp[0] + "_" + temp[1] + "_" + temp[2] + "_" + temp[3]);
                }
            }
            tW.WriteLine("[Ghostpoints]");
            if (ghostPoints.Count > 0)
            {
                for (int i = 0; i < ghostPoints.Count; i++)
                {
                    float[] temp = ghostPoints[i];
                    tW.WriteLine("g_" + i + "_" + temp[0] + "_" + temp[1] + "_" + temp[2] + "_" + temp[3]);
                }
            }

            tW.WriteLine("[Sellpoints]");
            if (sellPoints.Count > 0)
            {
                for (int i = 0; i < sellPoints.Count; i++)
                {
                    float[] temp = sellPoints[i];
                    tW.WriteLine("s_" + i + "_" + temp[0] + "_" + temp[1] + "_" + temp[2] + "_" + temp[3]);
                }
            }

            tW.Close();
        }
        public void AddWayPoint(float x, float y, float z)
        {
            float[] temp = { x, y, z, 0 };
            wayPoints.Add(temp);
        }
        public void AddMountPoint(float x, float y, float z, int value)
        {
            float[] temp = { x, y, z, 1 };
            if (value == 0)
            {
                wayPoints.Add(temp);
            }
            else
            {
                sellPoints.Add(temp);
            }
            
        }
        public void AddGhostPoint(float x, float y, float z)
        {
           
            float[] temp = { x, y, z, _ghostpaths };
            ghostPoints.Add(temp);
        }
        public void AddSellPoint(float x, float y, float z)
        {
            float[] temp = { x, y, z, 0 };
            sellPoints.Add(temp);
        }
        public void AddIgnoredMob(string value)
        {
            ignoredMobs.Add(value);
        }
        public void AddIgnoredMobGuid(ulong value)
        {
            ignoredMobsGuid.Add(value);
        }
        public bool Loop
        {
            get { return _loop; }
            set { _loop = value; }
        }
        public bool IsWaySet
        {
            get { return _isWaySet; }
            set { _isWaySet=value;}
        }
        public bool IsGhostSet
        {
            get { return _isGhostSet; }
            set { _isGhostSet=value;}
        }
        public bool IsSellSet
        {
            get { return _isSellSet; }
            set { _isSellSet=value;}
        }
        public bool IgnoreZ
        {
            get { return _ignoreZ; }
            set { _ignoreZ = value; }
        }
        public List<float[]> GetWaypoints()
        {
            return wayPoints;
        }
        public List<float[]> GetGhostpoints()
        {
            return ghostPoints;
        }
        public List<float[]> GetSellpoints()
        {
            return sellPoints;
        }
        public List<string> GetIgnoredMobs()
        {
            return ignoredMobs;
        }
        public int Ghostpaths
        {
            get { return _ghostpaths; }
            set { _ghostpaths = value; }
        }
        public List<ulong> getIgnoredMobsGuid
        {
            get { return ignoredMobsGuid; }
        }
    }
}
