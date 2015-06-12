using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic;

namespace WorkWork.Memory
{
    class ObjectManager
    {
        private Object player;
        private bool isPlayer;
        private BlackMagic magic;
        public ObjectManager(BlackMagic magic)
        {
            this.magic = magic;
        }
        private List<Object> Objects = new List<Object>();
        public void PopulateList()
        {
            uint ClientConnection = magic.ReadUInt((uint)TbcOffsets.General.ClientConnection);
            ClientConnection = magic.ReadUInt(ClientConnection + (uint)TbcOffsets.ObjectManagerOffsets.ObjectManagerOffset);
            uint nextObject = magic.ReadUInt(ClientConnection + (uint)TbcOffsets.ObjectManagerOffsets.FirstObject);
            Objects.Clear();
            isPlayer = false;
            while ((nextObject != 0) && ((nextObject & 1) == 0))
            {

                Object obj = new Object(nextObject, magic);
                Objects.Add(obj);
                nextObject = magic.ReadUInt(nextObject + (uint)TbcOffsets.ObjectManagerOffsets.NextObject);
                if (obj.Type == 4 && !isPlayer)
                {
                    player = obj;
                    isPlayer = true;
                }
            }
        }
        public List<Object> GetObjects()
        {
            return Objects;
        }
        public Object GetPlayer()
        {
            return player;
        }
    }
}
