using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace WorkWork.Profiles
{
    internal class Profile
    {
        private readonly List<float[]> _wayPoints = new List<float[]>();
        private readonly List<float[]> _ghostPoints = new List<float[]>();
        private readonly List<float[]> _sellPoints = new List<float[]>();
        private readonly List<string> _ignoredMobs = new List<string>();
        private readonly List<ulong> _ignoredMobsGuid = new List<ulong>();

        public void Load(string value)
        {
            var lines = File.ReadAllLines(value);
            foreach (var line in lines)
            {
                if (line.ToLower().Contains("w_"))
                {
                    var words = line.Split('_');
                    var temp = new float[4];
                    temp[0] = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
                    temp[1] = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
                    temp[2] = float.Parse(words[4], CultureInfo.InvariantCulture.NumberFormat);
                    temp[3] = float.Parse(words[5], CultureInfo.InvariantCulture.NumberFormat);
                    _wayPoints.Add(temp);
                    IsWaySet = true;
                }
                else if (line.ToLower().Contains("g_"))
                {
                    var words = line.Split('_');
                    var temp = new float[4];
                    temp[0] = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
                    temp[1] = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
                    temp[2] = float.Parse(words[4], CultureInfo.InvariantCulture.NumberFormat);
                    temp[3] = float.Parse(words[5], CultureInfo.InvariantCulture.NumberFormat);
                    _ghostPoints.Add(temp);
                    IsGhostSet = true;
                }
                else if (line.ToLower().Contains("s_"))
                {
                    var words = line.Split('_');
                    var temp = new float[4];
                    temp[0] = float.Parse(words[2], CultureInfo.InvariantCulture.NumberFormat);
                    temp[1] = float.Parse(words[3], CultureInfo.InvariantCulture.NumberFormat);
                    temp[2] = float.Parse(words[4], CultureInfo.InvariantCulture.NumberFormat);
                    temp[3] = float.Parse(words[5], CultureInfo.InvariantCulture.NumberFormat);
                    _sellPoints.Add(temp);
                    IsSellSet = true;
                }
                else if (line.ToLower().Contains("loop"))
                {
                    Loop = true;
                }
                else if (line.ToLower().Contains("ignorez"))
                {
                    IgnoreZ = true;
                }
                else if (line.ToLower().Contains("ignoredmob_"))
                {
                    var words = line.Split('_');
                    _ignoredMobs.Add(words[1]);
                }
                else if (line.ToLower().Contains("ghostpath_"))
                {
                    var words = line.Split('_');
                    Ghostpaths = Convert.ToInt32(words[1]);
                }
                else if (line.ToLower().Contains("ignoredmobguid_"))
                {
                    var words = line.Split('_');
                    _ignoredMobsGuid.Add(Convert.ToUInt64(words[1]));
                }
            }
        }
        public void Save(string value)
        {
            TextWriter tW = new StreamWriter(value);
            tW.WriteLine("[Profile]");
            if (Loop)
            {
                tW.WriteLine("loop");
            }
            if (IgnoreZ)
            {
                tW.WriteLine("ignorez");
            }
            tW.WriteLine("ghostpath_" + Ghostpaths);
            tW.WriteLine("[IgnoredMobs]");
            if (_ignoredMobs.Count > 0)
            {
                foreach (var ignoredMob in _ignoredMobs)
                {
                    tW.WriteLine("ignoredmob_" + ignoredMob);
                }
            }
            tW.WriteLine("[IgnoredMobsGuid]");
            if (_ignoredMobsGuid.Count>0)
            {
                foreach (var ignoredMobGuid in _ignoredMobsGuid)
                {
                    tW.WriteLine("ignoredmobguid_" + ignoredMobGuid);
                }
            }
            tW.WriteLine("[Waypoints]");
            if (_wayPoints.Count > 0)
            {
                for (var i = 0; i < _wayPoints.Count; i++)
                {
                    var temp = _wayPoints[i];
                    tW.WriteLine("w_" + i + "_" + temp[0] + "_" + temp[1] + "_" + temp[2] + "_" + temp[3]);
                }
            }
            tW.WriteLine("[Ghostpoints]");
            if (_ghostPoints.Count > 0)
            {
                for (var i = 0; i < _ghostPoints.Count; i++)
                {
                    var temp = _ghostPoints[i];
                    tW.WriteLine("g_" + i + "_" + temp[0] + "_" + temp[1] + "_" + temp[2] + "_" + temp[3]);
                }
            }

            tW.WriteLine("[Sellpoints]");
            if (_sellPoints.Count > 0)
            {
                for (var i = 0; i < _sellPoints.Count; i++)
                {
                    var temp = _sellPoints[i];
                    tW.WriteLine("s_" + i + "_" + temp[0] + "_" + temp[1] + "_" + temp[2] + "_" + temp[3]);
                }
            }

            tW.Close();
        }
        public void AddWayPoint(float x, float y, float z)
        {
            float[] temp = { x, y, z, 0 };
            _wayPoints.Add(temp);
        }
        public void AddMountPoint(float x, float y, float z, int value)
        {
            float[] temp = { x, y, z, 1 };
            if (value == 0)
            {
                _wayPoints.Add(temp);
            }
            else
            {
                _sellPoints.Add(temp);
            }
            
        }
        public void AddGhostPoint(float x, float y, float z)
        {
           
            float[] temp = { x, y, z, Ghostpaths };
            _ghostPoints.Add(temp);
        }
        public void AddSellPoint(float x, float y, float z)
        {
            float[] temp = { x, y, z, 0 };
            _sellPoints.Add(temp);
        }
        public void AddIgnoredMob(string value)
        {
            _ignoredMobs.Add(value);
        }
        public void AddIgnoredMobGuid(ulong value)
        {
            _ignoredMobsGuid.Add(value);
        }
        public bool Loop { get; set; }

        public bool IsWaySet { get; set; }

        public bool IsGhostSet { get; set; }

        public bool IsSellSet { get; set; }

        public bool IgnoreZ { get; set; } = true;

        public List<float[]> GetWaypoints()
        {
            return _wayPoints;
        }
        public List<float[]> GetGhostpoints()
        {
            return _ghostPoints;
        }
        public List<float[]> GetSellpoints()
        {
            return _sellPoints;
        }
        public List<string> GetIgnoredMobs()
        {
            return _ignoredMobs;
        }
        public int Ghostpaths { get; set; }

        public List<ulong> getIgnoredMobsGuid => _ignoredMobsGuid;
    }
}
