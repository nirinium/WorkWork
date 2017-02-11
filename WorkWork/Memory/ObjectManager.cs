using System.Collections.Generic;
using Magic;

namespace WorkWork.Memory
{
    internal class ObjectManager
    {
        private Object _player;
        private bool _isPlayer;
        private readonly BlackMagic _magic;
        public ObjectManager(BlackMagic magic)
        {
            _magic = magic;
        }
        private readonly List<Object> _objects = new List<Object>();
        public void PopulateList()
        {
            var clientConnection = _magic.ReadUInt((uint)TbcOffsets.General.ClientConnection);
            clientConnection = _magic.ReadUInt(clientConnection + (uint)TbcOffsets.ObjectManagerOffsets.ObjectManagerOffset);
            var nextObject = _magic.ReadUInt(clientConnection + (uint)TbcOffsets.ObjectManagerOffsets.FirstObject);
            _objects.Clear();
            _isPlayer = false;
            while ((nextObject != 0) && ((nextObject & 1) == 0))
            {

                var obj = new Object(nextObject, _magic);
                _objects.Add(obj);
                nextObject = _magic.ReadUInt(nextObject + (uint)TbcOffsets.ObjectManagerOffsets.NextObject);
                if (obj.Type != 4 || _isPlayer) continue;
                _player = obj;
                _isPlayer = true;
            }
        }
        public List<Object> GetObjects()
        {
            return _objects;
        }
        public Object GetPlayer()
        {
            return _player;
        }
    }
}
