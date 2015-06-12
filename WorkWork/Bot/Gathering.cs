using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkWork.Memory;
using Magic;

namespace WorkWork.Bot
{
    class Gathering
    {
        public Gathering(Main bot, Settings settings, ObjectManager objectManager, BlackMagic magic)
        {
            this.settings = settings;
            this.objectManager = objectManager;
            this.magic = magic;
            this.bot = bot;
        }
        private ObjectManager objectManager;
        private Settings settings;
        private Main bot;
        private BlackMagic magic;
        public string[] Herbs = {
                                    "Bloodthistle","Peacebloom","Silverleaf","Earthroot","Mageroyal","Briarthorn","Stranglekelp","Bruiseweed","Wild Steelbloom","Grave Moss","Kingsblood","Liferoot","Fadeleaf","Goldthorn","Khadgar's Whisker","Wintersbite","Firebloom","Purple Lotus","Arthas' Tears","Sungrass","Blindweed","Ghost Mushroom","Gromsblood","Golden Sansam","Dreamfoil","Mountain Silversage","Plaguebloom","Icecap","Black Lotus","Felweed","Dreaming Glory","Ragveil","Terocone","Flame Cap","Ancient Lichen","Netherbloom","Netherdust Bush","Nightmare Vine","Mana Thistle"                                
                                };
        public string[] Nodes = {
                                    "Copper Vein","Indendicite Mineral Vein","Tin Vein","Lesser Bloodstone Deposit","Ooze Covered Silver Vein","Silver Vein","Iron Deposit","Indurium Mineral Vein","Gold Vein","Ooze Covered Gold Vein","Mithril Deposit","Ooze Covered Mithril Deposit","Dark Iron Deposit","Ooze Covered Truesilver Deposit","Truesilver Deposit","Ooze Covered Thorium Vein","Small Thorium Vein","Hakkari Thorium Vein","Ooze Covered Rich Thorium Vein","Rich Thorium Vein","Fel Iron Deposit","Large Obsidian Chunk","Small Obsidian Chunk","Adamantite Deposit","Nethercite Deposit","Rich Adamantite Deposit","Ancient Gem Vein","Khorium Vein"
                                };
        public void Search(float startingX, float startingY, float startingZ)
        {
            float targetX, targetY, targetZ, dX, dY, dZ, distance;
            if (settings.Mining || settings.Herbing)
            {

                objectManager.PopulateList();
                foreach (WorkWork.Memory.Object obj in objectManager.GetObjects())
                {
                    if (obj.Type == 5)
                    {
                        string name = magic.ReadASCIIString(magic.ReadUInt(magic.ReadUInt(obj.BaseAddress + (uint)TbcOffsets.ObjectOffsets.GameObjectNameOffset1) + (uint)TbcOffsets.ObjectOffsets.GameObjectNameOffset2), 40);
                        if (settings.Herbing)
                        {
                            foreach (string herb in Herbs)
                            {
                                if (name.ToLower().Contains(herb))
                                {
                                    targetX = magic.ReadFloat(magic.ReadUInt(obj.BaseAddress + (uint)TbcOffsets.ObjectOffsets.GameObjectOffset) +  (uint)TbcOffsets.ObjectOffsets.GameObjectOffsetX);
                                    targetY = magic.ReadFloat(magic.ReadUInt(obj.BaseAddress + (uint)TbcOffsets.ObjectOffsets.GameObjectOffset) +  (uint)TbcOffsets.ObjectOffsets.GameObjectOffsetY);
                                    targetZ = magic.ReadFloat(magic.ReadUInt(obj.BaseAddress + (uint)TbcOffsets.ObjectOffsets.GameObjectOffset) + (uint)TbcOffsets.ObjectOffsets.GameObjectOffsetZ);
                                    dX = targetX - startingX;
                                    dY = targetY - startingY;
                                    dZ = targetZ - startingZ;
                                    distance = (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
                                    if (distance <= 30)
                                    {
                                        bot.GetPathing.WalkToPoint(targetX, targetY, targetZ, true, startingX, startingY, startingZ);
                                        bot.GetOther.Interact();
                                        bot.GetPathing.WalkToPoint(startingX, startingY, startingZ, true, targetX, targetY, targetZ);
                                        return;
                                    }
                                }
                            }
                        }
                        if (settings.Mining)
                        {
                            foreach (string node in Nodes)
                            {
                                if (name.Contains(node))
                                {
                                    targetX = magic.ReadFloat(magic.ReadUInt(obj.BaseAddress + (uint)TbcOffsets.ObjectOffsets.GameObjectOffset) + (uint)TbcOffsets.ObjectOffsets.GameObjectOffsetX);
                                    targetY = magic.ReadFloat(magic.ReadUInt(obj.BaseAddress + (uint)TbcOffsets.ObjectOffsets.GameObjectOffset) + (uint)TbcOffsets.ObjectOffsets.GameObjectOffsetY);
                                    targetZ = magic.ReadFloat(magic.ReadUInt(obj.BaseAddress + (uint)TbcOffsets.ObjectOffsets.GameObjectOffset) + (uint)TbcOffsets.ObjectOffsets.GameObjectOffsetZ);
                                    dX = targetX - startingX;
                                    dY = targetY - startingY;
                                    dZ = targetZ - startingZ;
                                    distance = (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
                                    if (distance <= 30)
                                    {
                                        bot.GetPathing.WalkToPoint(targetX, targetY, targetZ, true, startingX, startingY, startingZ);
                                        bot.GetOther.Interact();
                                        bot.GetPathing.WalkToPoint(startingX, startingY, startingZ, true, targetX, targetY, targetZ);
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
