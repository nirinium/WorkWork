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
    class Pathing
    {
        //Left, right, up, down
        public Pathing(Main bot, BlackMagic magic, KeyboardSim keyboardSim, ObjectManager objectManager, Settings settings, Profile profile)
        {
            this.magic = magic;
            this.keyboardSim = keyboardSim;
            this.objectManager = objectManager;
            this.settings = settings;
            this.profile = profile;
            this.bot = bot;
            turnleft = settings.GetKey("turnleft");
            turnright = settings.GetKey("turnright");
            walk = settings.GetKey("walk");
            goup = settings.GetKey("goup");
            godown = settings.GetKey("godown");
        }

        BlackMagic magic;
        KeyboardSim keyboardSim;
        ObjectManager objectManager;
        Settings settings;
        Profile profile;
        Main bot;

        private bool dead = false;

        private bool halt = false;

        private int turnleft, turnright, goup, godown, walk;

        public bool WalkToPoint(float targetX, float targetY, float targetZ, bool value, float currentX, float currentY, float currentZ)
        {
            float bodyx = magic.ReadFloat((uint)TbcOffsets.General.PlayerCorpseX);
            float bodyy = 0;
            float bodyz = 0;
            dead = false;
            if (bodyx != 0)
            {

                bodyy = magic.ReadFloat((uint)TbcOffsets.General.PlayerCorpseY);
                bodyz = magic.ReadFloat((uint)TbcOffsets.General.PlayerCorpseZ);
                dead = true;
            }

            float startingX = currentX;
            float startingY = currentY;
            float startingZ = currentZ;
            bool inCombat = value;
            float dX = targetX - startingX;
            float dY = targetY - startingY;
            float dZ = targetZ - startingZ;
            float rotation = (float)Math.Atan2(targetY - startingY, targetX - startingX);
            if (rotation < 0)
            {
                rotation = rotation + (2f * (float)Math.PI);
            }
            float myRotation = magic.ReadFloat((uint)TbcOffsets.General.PlayerRotation);
            myRotation = float.Parse(myRotation.ToString("0.0"));
            rotation = float.Parse(rotation.ToString("0.0"));
            float angle1;
            float angle2;
            float distance = (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
            while ((distance > 3f && distance <= 100f) && !halt)
            {
                
                if ((!settings.IgnoreMobs || !settings.IgnorePlayers) && (!inCombat || magic.ReadUInt(magic.ReadUInt((uint)TbcOffsets.General.PlayerBase) + (uint)TbcOffsets.General.CombatStateOffset) == (int)TbcOffsets.CombatState.InCombat && magic.ReadUInt64((uint)TbcOffsets.General.TargetGuid) != 0))
                {
                    if (inCombat)
                    {
                        inCombat = bot.GetCombat.CombatMode();
                    }
                    else
                    {
                        inCombat = bot.GetCombat.CombatMode();
                        if (inCombat)
                        {
                            return true;
                        }
                    }

                }
                int playerHealth = objectManager.GetPlayer().Health;
                if (playerHealth <= 0 && profile.IsGhostSet)
                {
                    bot.GetOther.ReleaseScript();
                    dead = true;
                    Thread.Sleep(1000);
                    return true;
                }
                else if (playerHealth <= 0 && !profile.IsGhostSet)
                {
                    bot.Halt();
                }

                if (dead)
                {

                    float gX = bodyx - startingX;
                    float gY = bodyy - startingY;
                    float gZ = bodyz - startingZ;
                    if ((float)Math.Sqrt(gX * gX + gY * gY + gZ * gZ) < 30)
                    {
                        keyboardSim.KeyUp(walk);
                        keyboardSim.KeyUp(turnleft);
                        keyboardSim.KeyUp(turnright);
                        while (magic.ReadFloat((uint)TbcOffsets.General.PlayerCorpseX) != 0)
                        {
                            bot.GetOther.RetrieveScript();
                        }
                        
                        bot.GetOther.Rebuff();
                        dead = false;
                        break;
                    }

                }
                if (!profile.IgnoreZ && startingZ > targetZ)
                {
                    if (startingZ - targetZ > 0.3f)
                    {
                        keyboardSim.KeyDown(godown);
                    }
                    else
                    {
                        keyboardSim.KeyUp(godown);
                    }

                }
                else if (!profile.IgnoreZ && startingZ < targetZ)
                {
                    if (targetZ - startingZ > 0.3f)
                    {
                        keyboardSim.KeyDown(goup);
                    }
                    else
                    {
                        keyboardSim.KeyUp(goup);
                    }

                }

                if (myRotation != rotation)
                {
                    if (myRotation >= rotation)
                    {
                        angle1 = myRotation - rotation;
                        angle2 = rotation + (2 * (float)Math.PI - myRotation);
                        if (angle1 < angle2)
                        {
                            if (angle1 > 0.3)
                            {
                                keyboardSim.KeyDown(turnright);
                                keyboardSim.KeyUp(turnleft);
                            }
                            else
                            {
                                keyboardSim.KeyUp(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }

                            if (angle1 < Math.PI / 2 && distance > 3)
                            {
                                keyboardSim.KeyDown(walk);
                            }
                        }
                        else
                        {
                            if (angle2 > 0.3)
                            {
                                keyboardSim.KeyDown(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }
                            else
                            {
                                keyboardSim.KeyUp(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }

                            if (angle2 < Math.PI / 2 && distance > 3)
                            {
                                keyboardSim.KeyDown(walk);
                            }
                        }

                    }
                    else if (myRotation < rotation)
                    {
                        angle1 = rotation - myRotation;
                        angle2 = myRotation + (2 * (float)Math.PI - rotation);
                        if (angle1 < angle2)
                        {
                            if (angle1 > 0.3)
                            {
                                keyboardSim.KeyDown(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }
                            else
                            {
                                keyboardSim.KeyUp(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }

                            if (angle1 < Math.PI / 2 && distance > 3)
                            {
                                keyboardSim.KeyDown(walk);
                            }
                        }
                        else
                        {
                            if (angle2 > 0.3)
                            {
                                keyboardSim.KeyDown(turnright);
                                keyboardSim.KeyUp(turnleft);
                            }
                            else
                            {
                                keyboardSim.KeyUp(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }

                            if (angle2 < Math.PI / 2 && distance > 3)
                            {
                                keyboardSim.KeyDown(walk);
                            }
                        }

                    }
                }

                else
                {
                    keyboardSim.KeyDown(walk);
                    keyboardSim.KeyUp(turnleft);
                    keyboardSim.KeyUp(turnright);
                    Random random = new Random();
                    if (random.Next(10) == 0)
                    {
                        keyboardSim.KeyDown(goup);
                        keyboardSim.KeyUp(goup);
                    }

                }
                startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                dX = targetX - startingX;
                dY = targetY - startingY;
                dZ = targetZ - startingZ;
                myRotation = magic.ReadFloat((uint)TbcOffsets.General.PlayerRotation);
                rotation = (float)Math.Atan2(targetY - startingY, targetX - startingX);
                if (rotation < 0)
                {
                    rotation = rotation + (2f * (float)Math.PI);
                }
                myRotation = float.Parse(myRotation.ToString("0.0"));
                rotation = float.Parse(rotation.ToString("0.0"));
                float temp = distance;
                distance = (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
                if (temp - distance == 0)
                {
                    Antistuck();
                }
            }
            keyboardSim.KeyUp(walk);
            keyboardSim.KeyUp(turnleft);
            keyboardSim.KeyUp(turnright);
            return false;


        }
        public void WalkToMob(WorkWork.Memory.Object obj, int range)
        {
            float startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
            float startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
            float startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
            float targetX = obj.XPosition;
            float targetY = obj.YPosition;
            float targetZ = obj.ZPosition;
            float dX = targetX - startingX;
            float dY = targetY - startingY;
            float dZ = targetZ - startingZ;
            float rotation = (float)Math.Atan2(targetY - startingY, targetX - startingX);
            if (rotation < 0)
            {
                rotation = rotation + (2f * (float)Math.PI);
            }

            float myRotation = magic.ReadFloat((uint)TbcOffsets.General.PlayerRotation);
            myRotation = float.Parse(myRotation.ToString("0.0"));
            rotation = float.Parse(rotation.ToString("0.0"));
            float angle1;
            float angle2;
            float distance = (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
            while ((distance > range - 1 && distance <= 100f) && !halt)
            {
                int playerHealth = objectManager.GetPlayer().Health;
                if (playerHealth <= 0 && profile.IsGhostSet)
                {
                    bot.GetOther.ReleaseScript();
                    dead = true;
                    Thread.Sleep(1000);
                    break;
                }
                else if (playerHealth <= 0 && !profile.IsGhostSet)
                {
                    bot.Halt();
                }
                if (!profile.IgnoreZ && startingZ > targetZ)
                {
                    if (startingZ - targetZ > 0.3f)
                    {
                        keyboardSim.KeyDown(godown);
                    }
                    else
                    {
                        keyboardSim.KeyUp(godown);
                    }
                }
                else if (!profile.IgnoreZ && startingZ < targetZ)
                {
                    if (targetZ - startingZ > 0.3f)
                    {
                        keyboardSim.KeyDown(goup);
                    }
                    else
                    {
                        keyboardSim.KeyUp(goup);
                    }
                }

                if (myRotation != rotation)
                {
                    if (myRotation >= rotation)
                    {
                        angle1 = myRotation - rotation;
                        angle2 = rotation + (2 * (float)Math.PI - myRotation);
                        if (angle1 < angle2)
                        {
                            if (angle1 > 0.3)
                            {
                                keyboardSim.KeyDown(turnright);
                                keyboardSim.KeyUp(turnleft);
                            }
                            else
                            {
                                keyboardSim.KeyUp(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }
                            if (angle1 < Math.PI / 2 && distance > range - 1)
                            {
                                keyboardSim.KeyDown(walk);
                            }
                        }
                        else
                        {
                            if (angle2 > 0.3)
                            {
                                keyboardSim.KeyDown(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }
                            else
                            {
                                keyboardSim.KeyUp(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }

                            if (angle2 < Math.PI / 2 && distance > range - 1)
                            {
                                keyboardSim.KeyDown(walk);
                            }
                        }
                    }
                    else if (myRotation < rotation)
                    {
                        angle1 = rotation - myRotation;
                        angle2 = myRotation + (2 * (float)Math.PI - rotation);
                        if (angle1 < angle2)
                        {
                            if (angle1 > 0.3)
                            {
                                keyboardSim.KeyDown(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }
                            else
                            {
                                keyboardSim.KeyUp(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }
                            if (angle1 < Math.PI / 2 && distance > range - 1)
                            {
                                keyboardSim.KeyDown(walk);
                            }
                        }
                        else
                        {
                            if (angle2 > 0.3)
                            {
                                keyboardSim.KeyDown(turnright);
                                keyboardSim.KeyUp(turnleft);
                            }
                            else
                            {
                                keyboardSim.KeyUp(turnleft);
                                keyboardSim.KeyUp(turnright);
                            }
                            if (angle2 < Math.PI / 2 && distance > range - 1)
                            {
                                keyboardSim.KeyDown(walk);
                            }
                        }
                    }
                }

                else
                {
                    keyboardSim.KeyDown(walk);
                    keyboardSim.KeyUp(turnleft);
                    keyboardSim.KeyUp(turnright);
                }
                startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                targetX = obj.XPosition;
                targetY = obj.YPosition;
                targetZ = obj.ZPosition;
                myRotation = magic.ReadFloat((uint)TbcOffsets.General.PlayerRotation);
                rotation = (float)Math.Atan2(targetY - startingY, targetX - startingX);
                if (rotation < 0)
                {
                    rotation = rotation + (2f * (float)Math.PI);
                }
                myRotation = float.Parse(myRotation.ToString("0.0"));
                rotation = float.Parse(rotation.ToString("0.0"));
                dX = targetX - startingX;
                dY = targetY - startingY;
                dZ = targetZ - startingZ;
                float temp = distance;
                distance = (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);
                if (temp - distance == 0)
                {
                    Antistuck();
                }

            }
            keyboardSim.KeyUp(walk);
            keyboardSim.KeyUp(turnleft);
            keyboardSim.KeyUp(turnright);


        }

        public void Rotate(WorkWork.Memory.Object obj)
        {

            float startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
            float startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
            float startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);

            float angle1;
            float angle2;
            float rotation = (float)Math.Atan2(obj.YPosition - startingY, obj.XPosition - startingX);
            if (rotation < 0)
            {
                rotation = rotation + (2f * (float)Math.PI);
            }
            float myRotation = magic.ReadFloat((uint)TbcOffsets.General.PlayerRotation);
            myRotation = float.Parse(myRotation.ToString("0.0"));
            rotation = float.Parse(rotation.ToString("0.0"));
            if (myRotation != rotation)
            {
                if (myRotation > rotation)
                {
                    angle1 = myRotation - rotation;
                    angle2 = rotation + (2 * (float)Math.PI - myRotation);
                    if (angle1 < angle2)
                    {
                        if (angle1 > 0.1)
                        {
                            keyboardSim.KeyDown(turnright);
                            keyboardSim.KeyUp(turnleft);
                        }
                        else
                        {
                            keyboardSim.KeyUp(turnleft);
                            keyboardSim.KeyUp(turnright);
                        }
                    }
                    else
                    {
                        if (angle2 > 0.1)
                        {
                            keyboardSim.KeyDown(turnleft);
                            keyboardSim.KeyUp(turnright);
                        }
                        else
                        {
                            keyboardSim.KeyUp(turnleft);
                            keyboardSim.KeyUp(turnright);
                        }
                    }
                }
                else if (myRotation < rotation)
                {
                    angle1 = rotation - myRotation;
                    angle2 = myRotation + (2 * (float)Math.PI - rotation);
                    if (angle1 < angle2)
                    {
                        if (angle1 > 0.1)
                        {
                            keyboardSim.KeyDown(turnleft);
                            keyboardSim.KeyUp(turnright);
                        }
                        else
                        {
                            keyboardSim.KeyUp(turnleft);
                            keyboardSim.KeyUp(turnright);
                        }

                    }
                    else
                    {
                        if (angle2 > 0.1)
                        {
                            keyboardSim.KeyDown(turnright);
                            keyboardSim.KeyUp(turnleft);
                        }
                        else
                        {
                            keyboardSim.KeyUp(turnleft);
                            keyboardSim.KeyUp(turnright);
                        }
                    }
                }
            }
            else
            {
                keyboardSim.KeyUp(turnleft);
                keyboardSim.KeyUp(turnright);
            }
        }
        public void Halt()
        {
            halt = true;
        }
        public bool Dead
        {
            get { return dead; }
        }
        public void Antistuck()
        {
            //keyboardSim.KeyDown(goup);
            //keyboardSim.KeyUp(goup);
        }
    }
}
