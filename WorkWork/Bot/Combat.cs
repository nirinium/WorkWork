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
using WorkWork.Profiles;

namespace WorkWork.Bot
{
    class Combat
    {
        //tab, escape
        public Combat(Main bot, KeyboardSim keyboardSim, ObjectManager objectManager, BlackMagic magic, Settings settings, Spells spells, Statistics statistics, Profile profile)
        {

            this.bot = bot;
            this.keyboardSim = keyboardSim;
            this.objectManager = objectManager;
            this.magic = magic;
            this.settings = settings;
            this.spells = spells;
            this.statistics = statistics;
            this.profile = profile;
            targetKey = settings.GetKey("target");
            escape = settings.GetKey("escape");
        }
        Main bot;
        KeyboardSim keyboardSim;
        ObjectManager objectManager;
        BlackMagic magic;
        Settings settings;
        Spells spells;
        Statistics statistics;
        Profile profile;

        private List<WorkWork.Memory.Object> ignoredMobs = new List<WorkWork.Memory.Object>();


        private int[] healthPerMob = new int[10];
        private int hpCounter = 0;



        private long[] timePerMob = new long[10];
        private int timeCounter = 0;
        private long averageTime = 60000;

        private int[] manaPerMob = new int[10];
        private int mpCounter = 0;

        private bool halt = false;

        private int targetKey, escape;

        public bool CombatMode()
        {
            ulong target = magic.ReadUInt64((uint)TbcOffsets.General.TargetGuid);
            bool stuck = false;
            //Investigate
            bool combatSuccessful = false;
            if (target == 0)
            {
                keyboardSim.KeyDown(targetKey);

                keyboardSim.KeyUp(targetKey);

            }
            else
            {
                objectManager.PopulateList();
                List<WorkWork.Memory.Object> objs = objectManager.GetObjects();
                for (int i = 0; i < objs.Count; i++)
                {
                    WorkWork.Memory.Object obj = objs[i];
                    int type = obj.Type;
                    if ((type == 4 && !settings.IgnorePlayers) || (type == 3 && !settings.IgnoreMobs))
                    {
                        ulong guid = obj.Guid;
                        if (target == guid && obj.Health > 0 && !stuck)
                        {
                            if (type == 3)
                            {
                                string name = obj.Name;
                                foreach (String ignoredMob in profile.GetIgnoredMobs())
                                {
                                    if (name == ignoredMob)
                                    {
                                        stuck = true;
                                        keyboardSim.KeyDown(targetKey);

                                        keyboardSim.KeyUp(targetKey);
                                    }
                                }
                            }
                            foreach (WorkWork.Memory.Object ignoredObjects in ignoredMobs)
                            {
                                if (ignoredObjects.Guid == guid)
                                {
                                    stuck = true;
                                    keyboardSim.KeyDown(targetKey);

                                    keyboardSim.KeyUp(targetKey);
                                }
                                else
                                {
                                    foreach (ulong ignoredMobGuid in profile.getIgnoredMobsGuid)
                                    {
                                        if (guid == ignoredMobGuid)
                                        {
                                            stuck = true;
                                            keyboardSim.KeyDown(targetKey);

                                            keyboardSim.KeyUp(targetKey);
                                        }
                                    }
                                }
                            }

                            var watch = Stopwatch.StartNew();
                            int startingHP = objectManager.GetPlayer().Health;
                            int startingMP = objectManager.GetPlayer().Mana;
                            int currentCombo = 0;
                            while (obj.Health > 0 && !halt && !stuck)
                            {
                                currentCombo = DpsRotation(obj, currentCombo);
                                if (AutoAttack() && objectManager.GetPlayer().Health > 0)
                                {
                                    bot.GetPathing.WalkToMob(obj, 5);
                                }
                                if (watch.ElapsedMilliseconds > averageTime * 3 && averageTime > 0)
                                {
                                    ignoredMobs.Add(obj);
                                    stuck = true;
                                }
                                if (halt || (objectManager.GetPlayer().Health <= 0) || bot.GetPathing.Dead || magic.ReadFloat((uint)TbcOffsets.General.PlayerCorpseX) != 0)
                                {
                                    stuck = true;
                                }
                            }
                            watch.Stop();
                            if (!stuck)
                            {
                                int counter;
                                int endingHP = objectManager.GetPlayer().Health;
                                if (endingHP < startingHP)
                                {
                                    endingHP = startingHP - endingHP;
                                    endingHP += endingHP / 2;
                                    healthPerMob[hpCounter] = endingHP;
                                    hpCounter++;
                                    if (hpCounter == 10)
                                    {
                                        hpCounter = 0;
                                    }
                                    counter = 0;
                                    bot.AverageHealth = 0;
                                    foreach (int hp in healthPerMob)
                                    {
                                        if (hp > 0)
                                        {
                                            counter++;
                                            bot.AverageHealth += hp;
                                        }

                                    }
                                    if (counter > 0) { bot.AverageHealth /= counter; }

                                }
                                if (objectManager.GetPlayer().MaxMana > 0)
                                {
                                    int endingMP = objectManager.GetPlayer().Mana;
                                    if (endingMP < startingMP)
                                    {
                                        endingMP = startingMP - endingMP;
                                        endingMP += endingMP / 2;
                                        manaPerMob[mpCounter] = endingMP;
                                        mpCounter++;
                                        if (mpCounter == 10)
                                        {
                                            mpCounter = 0;
                                        }
                                        counter = 0;
                                        bot.AverageMana = 0;
                                        foreach (int mp in manaPerMob)
                                        {
                                            if (mp > 0)
                                            {
                                                counter++;
                                                bot.AverageMana += mp;
                                            }

                                        }
                                        if (counter > 0) { bot.AverageMana /= counter; }

                                    }
                                }
                                if (magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) == (int)TbcOffsets.CombatState.InCombat && magic.ReadUInt64((uint)TbcOffsets.General.TargetGuid) != 0)
                                {
                                    CombatMode();
                                }

                                timePerMob[timeCounter] = watch.ElapsedMilliseconds;
                                timeCounter++;
                                if (timeCounter == 10)
                                {
                                    timeCounter = 0;
                                }
                                averageTime = 0;
                                counter = 0;
                                foreach (long temp in timePerMob)
                                {
                                    if (temp > 0)
                                    {
                                        counter++;
                                        averageTime += temp;
                                    }


                                }
                                if (counter > 0) { averageTime /= counter; }
                                if (objectManager.GetPlayer().Health > 0)
                                {
                                    if (settings.Looting)
                                    {
                                        bot.GetPathing.WalkToMob(obj, 3);
                                        bot.GetOther.Interact();
                                        if (settings.Skinning)
                                        {
                                            bot.GetOther.Interact();
                                        }
                                    }
                                    combatSuccessful = true;
                                    bot.MobsKilled++;
                                    statistics.Changed = true;
                                    bot.GetOther.Regen();
                                    afterCombatSpells(currentCombo, "aftercombat");
                                }

                            }
                            break;
                        }
                        else if (target == guid && obj.Health <= 0)
                        {
                            keyboardSim.KeyDown(targetKey);
                            keyboardSim.KeyUp(targetKey);
                            break;
                        }
                    }




                }
            }


            return combatSuccessful;
        }
        public int DpsRotation(WorkWork.Memory.Object obj, int currentCombo)
        {
            int playerEnergy = objectManager.GetPlayer().Energy;
            int playerMana = objectManager.GetPlayer().Mana;
            int playerRage = objectManager.GetPlayer().Rage / 10;
            double playerHealth = objectManager.GetPlayer().Health;
            double playerMaxHealth = objectManager.GetPlayer().MaxHealth;
            double objHealth = obj.Health;
            double objMaxHealth = obj.MaxHealth;
            for (int i = 0; i < spells.SpellNames.Count; i++)
            {

                string name = spells.SpellNames[i];
                int cast = spells.CastTime[i];
                int health = spells.Health[i];
                int mana = spells.Mana[i];
                int range = spells.Range[i];
                string type = spells.Type[i];
                int combo = spells.Combo[i];
                int minDistance = spells.MinDistance[i];
                int enemyHPPercentage = spells.EnemyHealth[i];
                if (name.ToLower().Contains("heal"))
                {
                    currentCombo = damageInDetail(obj, combo, minDistance, type, name, cast, health, mana, range, currentCombo, enemyHPPercentage, playerEnergy, playerMana, playerRage, playerHealth, playerMaxHealth, objHealth, objMaxHealth);
                }
                else if (name.ToLower().Contains("defensive"))
                {
                    currentCombo = damageInDetail(obj, combo, minDistance, type, name, cast, health, mana, range, currentCombo, enemyHPPercentage, playerEnergy, playerMana, playerRage, playerHealth, playerMaxHealth, objHealth, objMaxHealth);
                }
                else if (name.ToLower().Contains("charge"))
                {
                    currentCombo = damageInDetail(obj, combo, minDistance, type, name, cast, health, mana, range, currentCombo, enemyHPPercentage, playerEnergy, playerMana, playerRage, playerHealth, playerMaxHealth, objHealth, objMaxHealth);
                }
                else if (name.ToLower().Contains("execute"))
                {
                    currentCombo = damageInDetail(obj, combo, minDistance, type, name, cast, health, mana, range, currentCombo, enemyHPPercentage, playerEnergy, playerMana, playerRage, playerHealth, playerMaxHealth, objHealth, objMaxHealth);
                }
                else if (name.ToLower().Contains("combo"))
                {
                    currentCombo = damageInDetail(obj, combo, minDistance, type, name, cast, health, mana, range, currentCombo, enemyHPPercentage, playerEnergy, playerMana, playerRage, playerHealth, playerMaxHealth, objHealth, objMaxHealth);
                }
                else if (name.ToLower().Contains("spam"))
                {
                    currentCombo = damageInDetail(obj, combo, minDistance, type, name, cast, health, mana, range, currentCombo, enemyHPPercentage, playerEnergy, playerMana, playerRage, playerHealth, playerMaxHealth, objHealth, objMaxHealth);
                }
            }
            return currentCombo;
        }
        public int damageInDetail(WorkWork.Memory.Object obj, int combo, int minDistance, string type, string name, int cast, int playerHPPercentage, int mana, int range, int currentCombo, int enemyHPPercentage, int playerEnergy, int playerMana, int playerRage, double playerHealth, double playerMaxHealth, double objHealth, double objMaxHealth)
        {
            float startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
            float startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
            float startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
            float dX = obj.XPosition - startingX;
            float dY = obj.YPosition - startingY;
            float dZ = obj.ZPosition - startingZ;

            bot.GetPathing.Rotate(obj);
            if ((float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ) > minDistance)
            {

                if ((objHealth / objMaxHealth * 100) <= enemyHPPercentage)
                {

                    if (playerHealth / playerMaxHealth * 100 <= playerHPPercentage)
                    {
                        if (type == "energy")
                        {
                            if (playerEnergy >= mana)
                            {
                                if (currentCombo % combo == 0)
                                {
                                    if (range >= 0)
                                    {
                                        bot.GetPathing.WalkToMob(obj, range);
                                    }
                                    keyboardSim.KeyDown(spells.GetKey(name));
                                    keyboardSim.KeyUp(spells.GetKey(name));
                                    if (name.ToLower().Contains("spam"))
                                    {
                                        currentCombo++;

                                    }
                                    var watch = Stopwatch.StartNew();
                                    while (!halt && watch.ElapsedMilliseconds <= cast && (((float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ) <= range && (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ) > minDistance && obj.Health > 0 && objectManager.GetPlayer().Health > 0) || range == -1))
                                    { //REALLY SMALL THREAD SLEEP HERE MAYBE
                                        startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                                        startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                                        startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                                        dX = obj.XPosition - startingX;
                                        dY = obj.YPosition - startingY;
                                        dZ = obj.ZPosition - startingZ;
                                        bot.GetPathing.Rotate(obj);
                                    }
                                    watch.Stop();
                                }

                            }
                        }
                        else if (type == "mana")
                        {
                            if (playerMana >= mana)
                            {
                                if (currentCombo % combo == 0)
                                {
                                    if (range >= 0)
                                    {
                                        bot.GetPathing.WalkToMob(obj, range);
                                    }
                                    keyboardSim.KeyDown(spells.GetKey(name));
                                    keyboardSim.KeyUp(spells.GetKey(name));
                                    if (name.ToLower().Contains("spam"))
                                    {
                                        currentCombo++;

                                    }
                                    var watch = Stopwatch.StartNew();
                                    while (!halt && watch.ElapsedMilliseconds <= cast && (((float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ) <= range && (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ) > minDistance && obj.Health > 0 && objectManager.GetPlayer().Health > 0) || range == -1))
                                    {
                                        startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                                        startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                                        startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                                        dX = obj.XPosition - startingX;
                                        dY = obj.YPosition - startingY;
                                        dZ = obj.ZPosition - startingZ;
                                        bot.GetPathing.Rotate(obj);
                                    }
                                    watch.Stop();
                                }
                            }
                        }
                        else
                        {
                            if (playerRage >= mana)
                            {
                                if (currentCombo % combo == 0)
                                {
                                    if (range >= 0)
                                    {
                                        bot.GetPathing.WalkToMob(obj, range);
                                    }
                                    keyboardSim.KeyDown(spells.GetKey(name));
                                    keyboardSim.KeyUp(spells.GetKey(name));
                                    if (name.ToLower().Contains("spam"))
                                    {
                                        currentCombo++;

                                    }
                                    var watch = Stopwatch.StartNew();
                                    while (!halt && watch.ElapsedMilliseconds <= cast && (((float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ) <= range && (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ) > minDistance && obj.Health > 0 && objectManager.GetPlayer().Health > 0) || range == -1))
                                    {
                                        startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                                        startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                                        startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                                        dX = obj.XPosition - startingX;
                                        dY = obj.YPosition - startingY;
                                        dZ = obj.ZPosition - startingZ;
                                        bot.GetPathing.Rotate(obj);
                                    }
                                    watch.Stop();
                                }
                            }
                        }
                    }
                }
            }
            return currentCombo;
        }
        public void afterCombatSpells(int currentCombo, string afterCombat)
        {
            if (magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) != (int)TbcOffsets.CombatState.InCombat)
            {
                for (int i = 0; i < spells.SpellNames.Count; i++)
                {

                    string name = spells.SpellNames[i];
                    if (name.ToLower().Contains(afterCombat))
                    {
                        int cast = spells.CastTime[i];
                        int health = spells.Health[i];
                        int mana = spells.Mana[i];
                        string type = spells.Type[i];
                        int combo = spells.Combo[i];
                        int playerEnergy = objectManager.GetPlayer().Energy;
                        int playerMana = objectManager.GetPlayer().Mana;
                        int playerRage = objectManager.GetPlayer().Rage / 10;

                        if (objectManager.GetPlayer().Health / objectManager.GetPlayer().MaxHealth * 100 <= health)
                        {
                            if (type == "energy")
                            {
                                if (playerEnergy >= mana)
                                {
                                    if (currentCombo % combo == 0)
                                    {
                                        keyboardSim.KeyDown(spells.GetKey(name));
                                        keyboardSim.KeyUp(spells.GetKey(name));
                                        var watch = Stopwatch.StartNew();
                                        while (!halt && watch.ElapsedMilliseconds <= cast && objectManager.GetPlayer().Health > 0 && magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) != (int)TbcOffsets.CombatState.InCombat)
                                        {

                                        }
                                        watch.Stop();
                                    }

                                }
                            }
                            else if (type == "mana")
                            {
                                if (playerMana >= mana)
                                {
                                    if (currentCombo % combo == 0)
                                    {
                                        keyboardSim.KeyDown(spells.GetKey(name));
                                        keyboardSim.KeyUp(spells.GetKey(name));
                                        var watch = Stopwatch.StartNew();
                                        while (!halt && watch.ElapsedMilliseconds <= cast && objectManager.GetPlayer().Health > 0 && magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) != (int)TbcOffsets.CombatState.InCombat)
                                        {

                                        }
                                        watch.Stop();
                                    }
                                }
                            }
                            else
                            {
                                if (playerRage >= mana)
                                {
                                    if (currentCombo % combo == 0)
                                    {
                                        keyboardSim.KeyDown(spells.GetKey(name));
                                        keyboardSim.KeyUp(spells.GetKey(name));
                                        var watch = Stopwatch.StartNew();
                                        while (!halt && watch.ElapsedMilliseconds <= cast && objectManager.GetPlayer().Health > 0 && magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) != (int)TbcOffsets.CombatState.InCombat)
                                        {

                                        }
                                        watch.Stop();
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }
        public bool AutoAttack()
        {
            int playerMana = objectManager.GetPlayer().Mana;
            for (int i = 0; i < spells.SpellNames.Count; i++)
            {
                string name = spells.SpellNames[i];
                if (name.ToLower().Contains("autoattack"))
                {
                    if (playerMana <= spells.Mana[i] || spells.Mana[i] == -1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void Halt()
        {
            halt = true;
        }
    }
}
