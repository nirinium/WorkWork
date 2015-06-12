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
    class Main
    {
        public Main(Profile profile, Spells spells, Settings settings, BlackMagic magic, Label mobs, Label died, Label averageHealth, Label averageMana, Label loopLabel)
        {
            this.magic = magic;
            this.profile = profile;
            this.spells = spells;
            this.settings = settings;
            wayPoints = profile.getWaypoints();
            ghostPoints = profile.getGhostpoints();
            sellPoints = profile.getSellpoints();

            objectManager = new ObjectManager(magic);

            statistics = new Statistics(this, mobs, died, averageHealth, averageMana, loopLabel);
            Thread statisticsThread = new Thread(statistics.DoWork);
            statisticsThread.Start();


            keyboardSim = new KeyboardSim();
            other = new Other(this, magic, keyboardSim, objectManager, settings);
            combat = new Combat(this, keyboardSim, objectManager, magic, settings, spells, statistics, profile);
            pathing = new Pathing(this, magic, keyboardSim, objectManager, settings, profile);
            gathering = new Gathering(this, settings, objectManager, magic);

            walk = settings.GetKey("walk");
            turnleft = settings.GetKey("turnleft");
            turnright = settings.GetKey("turnright");

            if (settings.Sell)
            {
                sell = 0;
            }

        }
        //walk, turnleft, turnright.
        private ObjectManager objectManager;
        private List<float[]> wayPoints, ghostPoints, sellPoints;

        private Settings settings;
        private Spells spells;

        private KeyboardSim keyboardSim;
        private Other other;
        private Pathing pathing;
        private Combat combat;
        private Gathering gathering;

        private Profile profile;

        private BlackMagic magic;


        private Statistics statistics;

        private bool halt;

        private int sell = -1;
        private int sellLoops = -1;

        private int loops = -1;
        private int point;

        private bool endGhostPoints = false;

        private double averageHealth = 0;

        private double averageMana = 0;

        private bool mount = false;

        private int ghostpoint = 0;

        private int timesDied = 0;

        private int mobsKilled = 0;

        private int walk, turnleft, turnright;

        public void DoWork()
        {
            objectManager.PopulateList();
            while (!halt)
            {

                if (pathing.Dead || magic.ReadFloat((uint)TbcOffsets.General.PlayerCorpseX) != 0)//Dead
                {
                    if (profile.IsGhostSet)
                    {
                        timesDied++;
                        statistics.Changed = true;
                        if (!FirstPoint(ghostPoints, 1))
                        {

                            endGhostPoints = FirstPoint(wayPoints, 2);
                        }
                        while (!halt && pathing.Dead)
                        {
                            if (!endGhostPoints)
                            {
                                endGhostPoints = NextPoint(ghostPoints, 1);
                            }
                            else
                            {
                                NextPoint(wayPoints, 2);
                            }


                        }

                    }
                    else
                    {
                        Halt();
                    }
                }
                else //Alive
                {
                    endGhostPoints = false;
                    if (profile.IsWaySet)
                    {
                        other.Regen();
                        if (!FirstPoint(wayPoints, 0))
                        {
                            Halt();
                        }
                        bool combat = false;
                        while (!halt && !pathing.Dead)
                        {
                            if (sell > 0 && profile.IsSellSet)
                            {
                                FirstPoint(sellPoints, 0);
                                while (sell > 0 && !pathing.Dead && !halt)
                                {
                                    NextPoint(sellPoints, 3);
                                    if (point == sellPoints.Count-1)
                                    {
                                        other.Interact();
                                    }
                                }
                            }
                            else
                            {
                                other.Regen();
                                if (combat && !pathing.Dead && settings.Linear)
                                {
                                    NextPoint(wayPoints, 3);
                                    combat = false;
                                }
                                else if (combat && !pathing.Dead)
                                {
                                    FirstPoint(wayPoints, 0);
                                    combat = false;
                                }
                                else if (!combat && !pathing.Dead)
                                {
                                    combat = NextPoint(wayPoints, 0);
                                }
                            }

                        }
                    }

                    else
                    {
                        Halt();
                    }
                }
            }

            keyboardSim.KeyUp(walk);
            keyboardSim.KeyUp(turnleft);
            keyboardSim.KeyUp(turnright);



        }
        public bool FirstPoint(List<float[]> points, int j)
        {
            float startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
            float startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
            float startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
            if ((!pathing.Dead || magic.ReadFloat((uint)TbcOffsets.General.PlayerCorpseX) == 0) && sell <= 0)
            {
                gathering.Search(startingX, startingY, startingZ);
            }
            float[] shortestPoint = points[0];
            point = 0;
            float deltaX = shortestPoint[0] - startingX;
            float deltaY = shortestPoint[1] - startingY;
            float deltaZ = shortestPoint[2] - startingZ;
            float shortestDistance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
            int i = 1;
            float[] nextPoint;
            while (!halt && i < points.Count)
            {

                nextPoint = points[i];
                deltaX = nextPoint[0] - startingX;
                deltaY = nextPoint[1] - startingY;
                deltaZ = nextPoint[2] - startingZ;
                float nextDistance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ);
                if (nextDistance < shortestDistance)
                {
                    shortestDistance = nextDistance;
                    shortestPoint = nextPoint;
                    point = i;
                }
                i++;

            }
            if (shortestDistance <= 100f)
            {

                if (j == 0)
                {
                    if (point == 0)
                    {
                        if (sell == 0)
                        {
                            sellLoops++;
                            loops++;
                            statistics.Changed = true;
                            if (sellLoops == settings.SellLoops)
                            {
                                sell = 1;
                            }
                        }
                    }
                    
                    pathing.WalkToPoint(shortestPoint[0], shortestPoint[1], shortestPoint[2], true, startingX, startingY, startingZ);
                    if (shortestPoint[3] == 1 && !mount)
                    {
                        other.Mount(true);
                    }
                    else if (shortestPoint[3] == 1 && mount)
                    {
                        other.Mount(false);
                    }
                    return true;
                }
                else if (j == 1)
                {
                    pathing.WalkToPoint(shortestPoint[0], shortestPoint[1], shortestPoint[2], true, startingX, startingY, startingZ);
                    ghostpoint = (int)shortestPoint[3];
                    return true;
                }
                else
                {
                    pathing.WalkToPoint(shortestPoint[0], shortestPoint[1], shortestPoint[2], true, startingX, startingY, startingZ);
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public bool NextPoint(List<float[]> points, int j)
        {
            if (sell == 2)
            {
                point--;
            }
            else
            {
                point++;
            }
            
            if (point == points.Count() && profile.Loop && (j == 0 || j == 3))
            {
                
                
                if (sell == 0 || sell == -1)
                {
                    point = 0;
                    sellLoops++;
                    loops++;
                    statistics.Changed = true;
                    if (sellLoops == settings.SellLoops)
                    {
                        sell = 1;
                    }
                }
                else if (sell == 1)
                {
                    sell = 2;
                    point--;
                }

            }
            else if (point == points.Count() && !profile.Loop && (j == 0 || j == 3))
            {
                Halt();
            }
            else if (point == points.Count() && j == 1)
            {

                return FirstPoint(wayPoints, 2);
            }
            else if (point == -1 && sell == 2)
            {
                sell = 0;
                point = 0;
                sellLoops = 0;
            }
            float[] nextPoint;
            if (!halt && ((point < points.Count() && sell!=2) || (point >-1 && sell==2)))
            {
                float startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                float startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                float startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                if ((!pathing.Dead || magic.ReadFloat((uint)TbcOffsets.General.PlayerCorpseX) == 0) && sell <= 0)
                {
                    gathering.Search(startingX, startingY, startingZ);
                }
                nextPoint = points[point];
                float deltaX = nextPoint[0] - startingX;
                float deltaY = nextPoint[1] - startingY;
                float deltaZ = nextPoint[2] - startingZ;
                if (j == 0 && sell == 0 || sell == -1) //Alive
                {
                    bool combat;
                    combat = pathing.WalkToPoint(nextPoint[0], nextPoint[1], nextPoint[2], false, startingX, startingY, startingZ);
                    if (nextPoint[3] == 1 && !mount)
                    {
                        other.Mount(true);
                    }
                    else if (nextPoint[3] == 1 && mount)
                    {
                        other.Mount(false); ;
                    }
                    return combat;
                }
                else if (j == 1) //Dead, Ghostpoints
                {
                    if (ghostpoint == nextPoint[3])
                    {
                        pathing.WalkToPoint(nextPoint[0], nextPoint[1], nextPoint[2], true, startingX, startingY, startingZ);
                        return false;
                    }
                    else
                    {

                        return FirstPoint(wayPoints, 2);
                    }
                }
                else if (j == 2) //Dead, Waypoints
                {
                    pathing.WalkToPoint(nextPoint[0], nextPoint[1], nextPoint[2], true, startingX, startingY, startingZ);
                    return false; //test
                }
                else if (j == 3 || sell == 1 || sell == 2) //Alive, no combat
                {

                    pathing.WalkToPoint(nextPoint[0], nextPoint[1], nextPoint[2], true, startingX, startingY, startingZ);
                    if (nextPoint[3] == 1 && !mount)
                    {
                        other.Mount(true);
                    }
                    else if (nextPoint[3] == 1 && mount)
                    {
                        other.Mount(false);
                    }
                    return false;
                }



            }
            return false;
        }


        public void Halt()
        {
            halt = true;
            statistics.Halt();
            other.Halt();
            combat.Halt();
            pathing.Halt();
        }
        public int TimesDied
        {
            get { return timesDied; }
        }

        public double AverageHealth
        {
            get { return averageHealth; }
            set { averageHealth = value; }
        }
        public double AverageMana
        {
            get { return averageMana; }
            set { averageMana = value; }
        }
        public int MobsKilled
        {
            get { return mobsKilled; }
            set { mobsKilled = value; }
        }
        public Combat GetCombat
        {
            get { return combat; }
        }
        public Other GetOther
        {
            get { return other; }
        }
        public Pathing GetPathing
        {
            get { return pathing; }
        }
        public int Loops
        {
            get { return loops; }
        }
        public Gathering GetGathering
        {
            get { return gathering; }
        }
    }

}
