using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WorkWork.Settings
{
    class Spells
    {
        private List<string> spellNames = new List<string>();
        private List<string> spellKeys = new List<string>();
        private List<int> health = new List<int>();
        private List<int> castTime = new List<int>();
        private List<int> mana = new List<int>();
        private List<int> range = new List<int>();
        private List<string> type = new List<string>();
        private List<int> combo = new List<int>();
        private List<int> minDistance = new List<int>();
        private List<int> enemyHealth = new List<int>();
        public void Load(string value)
        {
            string[] lines = File.ReadAllLines(value);
            foreach (string line in lines)
            {
                if (line.ToLower().Contains("example") || line.ToLower().Contains("[spells]"))
                {
                    //Ignore
                }
                else
                {
                    string[] temp = line.Split('_');
                    spellNames.Add(temp[0]);                    
                    health.Add(Convert.ToInt32(temp[1]));
                    spellKeys.Add(temp[2]);
                    castTime.Add(Convert.ToInt32(temp[3]));
                    mana.Add(Convert.ToInt32(temp[4]));
                    range.Add(Convert.ToInt32(temp[5]));
                    type.Add(temp[6]);
                    combo.Add(Convert.ToInt32(temp[7]));
                    minDistance.Add(Convert.ToInt32(temp[8]));
                    enemyHealth.Add(Convert.ToInt32(temp[9]));
                }
            }
        }
        public void Save(string value)
        {
            TextWriter tW = new StreamWriter(value);
            tW.WriteLine("[Spells]");
            for (int i = 0; i < spellNames.Count; i++)
            {
                tW.WriteLine(spellNames[i] + "_" + health[i] + "_" + spellKeys[i] + "_" + castTime[i]+ "_" + mana[i]+ "_" +range[i]+ "_" +type[i]+"_"+combo[i]+"_"+minDistance[i]+"_"+enemyHealth[i]);
            }
            tW.Close();
        }
        public int GetKey(string value)
        {
            for (int i = 0; i < spellNames.Count; i++)
            {
                if (value == spellNames[i])
                {
                    value = spellKeys[i];
                    break;
                }
            }
            Keys key;
            Enum.TryParse(value, out key);
            return (int)key;
        }
        public List<string> SpellNames
        {
            get { return spellNames; }
        }
        public List<int> Health
        {
            get { return health; }
        }
        public List<int> CastTime
        {
            get { return castTime; }
        }
        public List<int> Mana
        {
            get { return mana; }
        }
        public List<int> Range
        {
            get { return range; }
        }
        public List<string> Type
        {
            get { return type; }
        }
        public List<int> Combo
        {
            get { return combo; }
        }
        public List<int> MinDistance
        {
            get { return minDistance; }
        }
        public List<int> EnemyHealth
        {
            get { return enemyHealth; }
        }
        public List<string> SpellKeys
        {
            get { return spellKeys; }
        }
    }
}
