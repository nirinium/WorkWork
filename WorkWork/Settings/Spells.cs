using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WorkWork.Settings
{
    internal class Spells
    {
        public void Load(string value)
        {
            var lines = File.ReadAllLines(value);
            foreach (var line in lines)
            {
                if (line.ToLower().Contains("example") || line.ToLower().Contains("[spells]"))
                {
                    //Ignore
                }
                else
                {
                    var temp = line.Split('_');
                    SpellNames.Add(temp[0]);                    
                    Health.Add(Convert.ToInt32(temp[1]));
                    SpellKeys.Add(temp[2]);
                    CastTime.Add(Convert.ToInt32(temp[3]));
                    Mana.Add(Convert.ToInt32(temp[4]));
                    Range.Add(Convert.ToInt32(temp[5]));
                    Type.Add(temp[6]);
                    Combo.Add(Convert.ToInt32(temp[7]));
                    MinDistance.Add(Convert.ToInt32(temp[8]));
                    EnemyHealth.Add(Convert.ToInt32(temp[9]));
                }
            }
        }
        public void Save(string value)
        {
            TextWriter tW = new StreamWriter(value);
            tW.WriteLine("[Spells]");
            for (var i = 0; i < SpellNames.Count; i++)
            {
                tW.WriteLine(SpellNames[i] + "_" + Health[i] + "_" + SpellKeys[i] + "_" + CastTime[i]+ "_" + Mana[i]+ "_" +Range[i]+ "_" +Type[i]+"_"+Combo[i]+"_"+MinDistance[i]+"_"+EnemyHealth[i]);
            }
            tW.Close();
        }
        public int GetKey(string value)
        {
            for (var i = 0; i < SpellNames.Count; i++)
            {
                if (value != SpellNames[i]) continue;
                value = SpellKeys[i];
                break;
            }
            Keys key;
            Enum.TryParse(value, out key);
            return (int)key;
        }
        public List<string> SpellNames { get; } = new List<string>();

        public List<int> Health { get; } = new List<int>();

        public List<int> CastTime { get; } = new List<int>();

        public List<int> Mana { get; } = new List<int>();

        public List<int> Range { get; } = new List<int>();

        public List<string> Type { get; } = new List<string>();

        public List<int> Combo { get; } = new List<int>();

        public List<int> MinDistance { get; } = new List<int>();

        public List<int> EnemyHealth { get; } = new List<int>();

        public List<string> SpellKeys { get; } = new List<string>();
    }
}
