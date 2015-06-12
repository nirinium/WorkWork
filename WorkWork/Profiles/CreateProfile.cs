using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Magic;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using WorkWork.Memory;

namespace WorkWork
{
    class CreateProfile
    {
        private volatile bool halt;
        private volatile bool isWayPoints;
        private volatile bool isGhostPoints;
        private volatile bool isSellPoints;
        private float startingX, startingY, startingZ, currentX, currentY, currentZ, deltaX, deltaY, deltaZ;
        private BlackMagic magic;
        private Profile profile= new Profile();
        private string filename;
        private RichTextBox richTextBox;
        public CreateProfile(BlackMagic magic, RichTextBox richTextBox)
        {
            this.magic = magic;
            this.richTextBox = richTextBox;
        }
        
        public void DoWork()
        {
            
            while (!halt)
            {
                if (!halt && isWayPoints)
                {
                    startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                    startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                    startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                    profile.AddWayPoint(startingX, startingY, startingZ);
                    if (richTextBox.InvokeRequired)
                    {
                        richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added waypoint at x: " + startingX + ", y: " + startingY + ", z: " + startingZ + Environment.NewLine); }));
                    }
                    
                    currentX = startingX;
                    currentY = startingY;
                    currentZ = startingZ;
                    deltaX = 0;
                    deltaY = 0;
                    deltaZ = 0;
                    while (!halt && isWayPoints)
                    {
                        while (!halt && (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ) < 15 && isWayPoints)
                        {
                            currentX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                            currentY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                            currentZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                            deltaX = currentX - startingX;
                            deltaY = currentY - startingY;
                            deltaZ = currentZ - startingZ;
                        }
                        profile.AddWayPoint(currentX, currentY, currentZ);
                        startingX = currentX;
                        startingY = currentY;
                        startingZ = currentZ;
                        if (richTextBox.InvokeRequired)
                        {
                            richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added waypoint at x: " + startingX + ", y: " + startingY + ", z: " + startingZ + Environment.NewLine); }));
                        }
                        deltaX = 0;
                        deltaY = 0;
                        deltaZ = 0;
                    }
                }
                else if (!halt && isGhostPoints)
                {
                    startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                    startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                    startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                    profile.AddGhostPoint(startingX, startingY, startingZ);
                    if (richTextBox.InvokeRequired)
                    {
                        richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added ghostpoint at x: " + startingX + ", y: " + startingY + ", z: " + startingZ + Environment.NewLine); }));
                    }
                    currentX = startingX;
                    currentY = startingY;
                    currentZ = startingZ;
                    deltaX = 0;
                    deltaY = 0;
                    deltaZ = 0;
                    while (!halt && isGhostPoints)
                    {
                        while (!halt && (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ) < 15 && isGhostPoints)
                        {
                            currentX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                            currentY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                            currentZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                            deltaX = currentX - startingX;
                            deltaY = currentY - startingY;
                            deltaZ = currentZ - startingZ;
                        }
                        profile.AddGhostPoint(currentX, currentY, currentZ);
                        startingX = currentX;
                        startingY = currentY;
                        startingZ = currentZ;
                        if (richTextBox.InvokeRequired)
                        {
                            richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added ghostpoint at x: " + startingX + ", y: " + startingY + ", z: " + startingZ + Environment.NewLine); }));
                        }
                        deltaX = 0;
                        deltaY = 0;
                        deltaZ = 0;
                    }
                }
                else if (!halt && isSellPoints)
                {
                    startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                    startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                    startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                    profile.AddSellPoint(startingX, startingY, startingZ);
                    if (richTextBox.InvokeRequired)
                    {
                        richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added sellpoint at x: " + startingX + ", y: " + startingY + ", z: " + startingZ + Environment.NewLine); }));
                    }
                    currentX = startingX;
                    currentY = startingY;
                    currentZ = startingZ;
                    deltaX = 0;
                    deltaY = 0;
                    deltaZ = 0;
                    while (!halt && isSellPoints)
                    {
                        while (!halt && (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY + deltaZ * deltaZ) < 15 && isSellPoints)
                        {
                            currentX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                            currentY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                            currentZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                            deltaX = currentX - startingX;
                            deltaY = currentY - startingY;
                            deltaZ = currentZ - startingZ;
                        }
                        profile.AddSellPoint(currentX, currentY, currentZ);
                        startingX = currentX;
                        startingY = currentY;
                        startingZ = currentZ;
                        if (richTextBox.InvokeRequired)
                        {
                            richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added sellpoint at x: " + startingX + ", y: " + startingY + ", z: " + startingZ + Environment.NewLine); }));
                        }
                        deltaX = 0;
                        deltaY = 0;
                        deltaZ = 0;
                    }
                }
            }
            

        }
        public void SetMode(int value)
        {
            if (value == 0)
            {
                isWayPoints = !isWayPoints;
            }
            else if (value == 1)
            {
                isGhostPoints = !isGhostPoints;
            }
            else
            {
                isSellPoints = !isSellPoints;
            }
        }
        public void Halt()
        {
            halt = true;
            isWayPoints = false;
            isGhostPoints = false;
            isSellPoints = false;            
        }
        public void save()
        {
            profile.Save(filename);
        }
        public bool Loop
        {
            get { return profile.Loop; }
            set { profile.Loop = value; }
        }
        public bool IgnoreZ
        {
            get { return profile.IgnoreZ; }
            set { profile.IgnoreZ = value; }
        }
        public void AddMountPoint()
        {
            startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
            startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
            startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
            if (isWayPoints)
            {
                profile.AddMountPoint(startingX, startingY, startingZ,0);
            }
            else if (isSellPoints)
            {
                profile.AddMountPoint(startingX, startingY, startingZ, 1);
            }
            
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added mountpoint at x: " + startingX + ", y: " + startingY + ", z: " + startingZ + Environment.NewLine); }));
            }
        }
        public void AddIgnoredMob(string value)
        {
            profile.AddIgnoredMob(value);
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added mob: "+value + Environment.NewLine); }));
            }
        }
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }
        public int Ghostpoint
        {
            get { return profile.Ghostpaths; }
            set { profile.Ghostpaths = value; }
        }
        public void Load(string value)
        {
            profile.Load(value);

        }
        public void AddIgnoredMobGuid(ulong value)
        {
            profile.AddIgnoredMobGuid(value);
        }
    }
}
