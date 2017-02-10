using System;
using System.Windows.Forms;
using Magic;
using WorkWork.Memory;

namespace WorkWork.Profiles
{
    class CreateProfile
    {
        private volatile bool _halt;
        private volatile bool _isWayPoints;
        private volatile bool _isGhostPoints;
        private volatile bool _isSellPoints;
        private float _startingX, _startingY, _startingZ, _currentX, _currentY, _currentZ, _deltaX, _deltaY, _deltaZ;
        private BlackMagic magic;
        private Profile profile= new Profile();
        private string _filename;
        private RichTextBox richTextBox;
        public CreateProfile(BlackMagic magic, RichTextBox richTextBox)
        {
            this.magic = magic;
            this.richTextBox = richTextBox;
        }
        
        public void DoWork()
        {
            
            while (!_halt)
            {
                if (!_halt && _isWayPoints)
                {
                    _startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                    _startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                    _startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                    profile.AddWayPoint(_startingX, _startingY, _startingZ);
                    if (richTextBox.InvokeRequired)
                    {
                        richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added waypoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
                    }
                    
                    _currentX = _startingX;
                    _currentY = _startingY;
                    _currentZ = _startingZ;
                    _deltaX = 0;
                    _deltaY = 0;
                    _deltaZ = 0;
                    while (!_halt && _isWayPoints)
                    {
                        while (!_halt && (float)Math.Sqrt(_deltaX * _deltaX + _deltaY * _deltaY + _deltaZ * _deltaZ) < 15 && _isWayPoints)
                        {
                            _currentX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                            _currentY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                            _currentZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                            _deltaX = _currentX - _startingX;
                            _deltaY = _currentY - _startingY;
                            _deltaZ = _currentZ - _startingZ;
                        }
                        profile.AddWayPoint(_currentX, _currentY, _currentZ);
                        _startingX = _currentX;
                        _startingY = _currentY;
                        _startingZ = _currentZ;
                        if (richTextBox.InvokeRequired)
                        {
                            richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added waypoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
                        }
                        _deltaX = 0;
                        _deltaY = 0;
                        _deltaZ = 0;
                    }
                }
                else if (!_halt && _isGhostPoints)
                {
                    _startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                    _startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                    _startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                    profile.AddGhostPoint(_startingX, _startingY, _startingZ);
                    if (richTextBox.InvokeRequired)
                    {
                        richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added ghostpoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
                    }
                    _currentX = _startingX;
                    _currentY = _startingY;
                    _currentZ = _startingZ;
                    _deltaX = 0;
                    _deltaY = 0;
                    _deltaZ = 0;
                    while (!_halt && _isGhostPoints)
                    {
                        while (!_halt && (float)Math.Sqrt(_deltaX * _deltaX + _deltaY * _deltaY + _deltaZ * _deltaZ) < 15 && _isGhostPoints)
                        {
                            _currentX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                            _currentY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                            _currentZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                            _deltaX = _currentX - _startingX;
                            _deltaY = _currentY - _startingY;
                            _deltaZ = _currentZ - _startingZ;
                        }
                        profile.AddGhostPoint(_currentX, _currentY, _currentZ);
                        _startingX = _currentX;
                        _startingY = _currentY;
                        _startingZ = _currentZ;
                        if (richTextBox.InvokeRequired)
                        {
                            richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added ghostpoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
                        }
                        _deltaX = 0;
                        _deltaY = 0;
                        _deltaZ = 0;
                    }
                }
                else if (!_halt && _isSellPoints)
                {
                    _startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                    _startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                    _startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                    profile.AddSellPoint(_startingX, _startingY, _startingZ);
                    if (richTextBox.InvokeRequired)
                    {
                        richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added sellpoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
                    }
                    _currentX = _startingX;
                    _currentY = _startingY;
                    _currentZ = _startingZ;
                    _deltaX = 0;
                    _deltaY = 0;
                    _deltaZ = 0;
                    while (!_halt && _isSellPoints)
                    {
                        while (!_halt && (float)Math.Sqrt(_deltaX * _deltaX + _deltaY * _deltaY + _deltaZ * _deltaZ) < 15 && _isSellPoints)
                        {
                            _currentX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                            _currentY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                            _currentZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                            _deltaX = _currentX - _startingX;
                            _deltaY = _currentY - _startingY;
                            _deltaZ = _currentZ - _startingZ;
                        }
                        profile.AddSellPoint(_currentX, _currentY, _currentZ);
                        _startingX = _currentX;
                        _startingY = _currentY;
                        _startingZ = _currentZ;
                        if (richTextBox.InvokeRequired)
                        {
                            richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added sellpoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
                        }
                        _deltaX = 0;
                        _deltaY = 0;
                        _deltaZ = 0;
                    }
                }
            }
            

        }
        public void SetMode(int value)
        {
            if (value == 0)
            {
                _isWayPoints = !_isWayPoints;
            }
            else if (value == 1)
            {
                _isGhostPoints = !_isGhostPoints;
            }
            else
            {
                _isSellPoints = !_isSellPoints;
            }
        }
        public void Halt()
        {
            _halt = true;
            _isWayPoints = false;
            _isGhostPoints = false;
            _isSellPoints = false;            
        }
        public void Save()
        {
            profile.Save(_filename);
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
            _startingX = magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
            _startingY = magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
            _startingZ = magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
            if (_isWayPoints)
            {
                profile.AddMountPoint(_startingX, _startingY, _startingZ,0);
            }
            else if (_isSellPoints)
            {
                profile.AddMountPoint(_startingX, _startingY, _startingZ, 1);
            }
            
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new MethodInvoker(delegate { richTextBox.AppendText("Added mountpoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
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
            get { return _filename; }
            set { _filename = value; }
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
