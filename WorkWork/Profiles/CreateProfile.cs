using System;
using System.Windows.Forms;
using Magic;
using WorkWork.Memory;

namespace WorkWork.Profiles
{
    internal class CreateProfile
    {
        private volatile bool _halt;
        private volatile bool _isWayPoints;
        private volatile bool _isGhostPoints;
        private volatile bool _isSellPoints;
        private float _startingX, _startingY, _startingZ, _currentX, _currentY, _currentZ, _deltaX, _deltaY, _deltaZ;
        private readonly BlackMagic _magic;
        private readonly Profile _profile= new Profile();
        private readonly RichTextBox _richTextBox;
        public CreateProfile(BlackMagic magic, RichTextBox richTextBox)
        {
            _magic = magic;
            _richTextBox = richTextBox;
        }
        
        public void DoWork()
        {
            
            while (!_halt)
            {
                if (!_halt && _isWayPoints)
                {
                    _startingX = _magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                    _startingY = _magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                    _startingZ = _magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                    _profile.AddWayPoint(_startingX, _startingY, _startingZ);
                    if (_richTextBox.InvokeRequired)
                    {
                        _richTextBox.Invoke(new MethodInvoker(delegate { _richTextBox.AppendText("Added waypoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
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
                            _currentX = _magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                            _currentY = _magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                            _currentZ = _magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                            _deltaX = _currentX - _startingX;
                            _deltaY = _currentY - _startingY;
                            _deltaZ = _currentZ - _startingZ;
                        }
                        _profile.AddWayPoint(_currentX, _currentY, _currentZ);
                        _startingX = _currentX;
                        _startingY = _currentY;
                        _startingZ = _currentZ;
                        if (_richTextBox.InvokeRequired)
                        {
                            _richTextBox.Invoke(new MethodInvoker(delegate { _richTextBox.AppendText("Added waypoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
                        }
                        _deltaX = 0;
                        _deltaY = 0;
                        _deltaZ = 0;
                    }
                }
                else if (!_halt && _isGhostPoints)
                {
                    _startingX = _magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                    _startingY = _magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                    _startingZ = _magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                    _profile.AddGhostPoint(_startingX, _startingY, _startingZ);
                    if (_richTextBox.InvokeRequired)
                    {
                        _richTextBox.Invoke(new MethodInvoker(delegate { _richTextBox.AppendText("Added ghostpoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
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
                            _currentX = _magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                            _currentY = _magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                            _currentZ = _magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                            _deltaX = _currentX - _startingX;
                            _deltaY = _currentY - _startingY;
                            _deltaZ = _currentZ - _startingZ;
                        }
                        _profile.AddGhostPoint(_currentX, _currentY, _currentZ);
                        _startingX = _currentX;
                        _startingY = _currentY;
                        _startingZ = _currentZ;
                        if (_richTextBox.InvokeRequired)
                        {
                            _richTextBox.Invoke(new MethodInvoker(delegate { _richTextBox.AppendText("Added ghostpoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
                        }
                        _deltaX = 0;
                        _deltaY = 0;
                        _deltaZ = 0;
                    }
                }
                else if (!_halt && _isSellPoints)
                {
                    _startingX = _magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                    _startingY = _magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                    _startingZ = _magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                    _profile.AddSellPoint(_startingX, _startingY, _startingZ);
                    if (_richTextBox.InvokeRequired)
                    {
                        _richTextBox.Invoke(new MethodInvoker(delegate { _richTextBox.AppendText("Added sellpoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
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
                            _currentX = _magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
                            _currentY = _magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
                            _currentZ = _magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
                            _deltaX = _currentX - _startingX;
                            _deltaY = _currentY - _startingY;
                            _deltaZ = _currentZ - _startingZ;
                        }
                        _profile.AddSellPoint(_currentX, _currentY, _currentZ);
                        _startingX = _currentX;
                        _startingY = _currentY;
                        _startingZ = _currentZ;
                        if (_richTextBox.InvokeRequired)
                        {
                            _richTextBox.Invoke(new MethodInvoker(delegate { _richTextBox.AppendText("Added sellpoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
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
            switch (value)
            {
                case 0:
                    _isWayPoints = !_isWayPoints;
                    break;
                case 1:
                    _isGhostPoints = !_isGhostPoints;
                    break;
                default:
                    _isSellPoints = !_isSellPoints;
                    break;
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
            _profile.Save(FileName);
        }
        public bool Loop
        {
            get { return _profile.Loop; }
            set { _profile.Loop = value; }
        }
        public bool IgnoreZ
        {
            get { return _profile.IgnoreZ; }
            set { _profile.IgnoreZ = value; }
        }
        public void AddMountPoint()
        {
            _startingX = _magic.ReadFloat((uint)TbcOffsets.General.PlayerX);
            _startingY = _magic.ReadFloat((uint)TbcOffsets.General.PlayerY);
            _startingZ = _magic.ReadFloat((uint)TbcOffsets.General.PlayerZ);
            if (_isWayPoints)
            {
                _profile.AddMountPoint(_startingX, _startingY, _startingZ,0);
            }
            else if (_isSellPoints)
            {
                _profile.AddMountPoint(_startingX, _startingY, _startingZ, 1);
            }
            
            if (_richTextBox.InvokeRequired)
            {
                _richTextBox.Invoke(new MethodInvoker(delegate { _richTextBox.AppendText("Added mountpoint at x: " + _startingX + ", y: " + _startingY + ", z: " + _startingZ + Environment.NewLine); }));
            }
        }
        public void AddIgnoredMob(string value)
        {
            _profile.AddIgnoredMob(value);
            if (_richTextBox.InvokeRequired)
            {
                _richTextBox.Invoke(new MethodInvoker(delegate { _richTextBox.AppendText("Added mob: "+value + Environment.NewLine); }));
            }
        }
        public string FileName { get; set; }

        public int Ghostpoint
        {
            get { return _profile.Ghostpaths; }
            set { _profile.Ghostpaths = value; }
        }
        public void Load(string value)
        {
            _profile.Load(value);

        }
        public void AddIgnoredMobGuid(ulong value)
        {
            _profile.AddIgnoredMobGuid(value);
        }
    }
}
