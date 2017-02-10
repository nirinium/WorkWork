using System.Collections.Generic;
using Magic;

namespace WorkWork.Memory
{
    class ObjectManager
    {
        private Object _player;
        private bool _isPlayer;
        private BlackMagic magic;
        public ObjectManager(BlackMagic magic)
        {
            this.magic = magic;
        }
        private List<Object> Objects = new List<Object>();
        public void PopulateList()
        {
            uint clientConnection = magic.ReadUInt((uint)TbcOffsets.General.ClientConnection);
            clientConnection = magic.ReadUInt(clientConnection + (uint)TbcOffsets.ObjectManagerOffsets.ObjectManagerOffset);
            uint nextObject = magic.ReadUInt(clientConnection + (uint)TbcOffsets.ObjectManagerOffsets.FirstObject);
            Objects.Clear();
            _isPlayer = false;
            while ((nextObject != 0) && ((nextObject & 1) == 0))
            {

                Object obj = new Object(nextObject, magic);
                Objects.Add(obj);
                nextObject = magic.ReadUInt(nextObject + (uint)TbcOffsets.ObjectManagerOffsets.NextObject);
                if (obj.Type == 4 && !_isPlayer)
                {
                    _player = obj;
                    _isPlayer = true;
                }
            }
        }
        public List<Object> GetObjects()
        {
            return Objects;
        }
        public Object GetPlayer()
        {
            return _player;
        }
    }
}
