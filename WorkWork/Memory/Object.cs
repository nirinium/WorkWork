using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magic;

namespace WorkWork.Memory
{
    class Object
    {
        
        private BlackMagic magic;
        private uint baseAddress;
        public Object(uint baseAddress, BlackMagic magic)
        {
            this.baseAddress = baseAddress;
            this.magic = magic;
        }
        public uint BaseAddress
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }
        public int Type
        {
            get { return magic.ReadInt(BaseAddress + (uint)TbcOffsets.ObjectOffsets.TypeOffset); }
        }
        public virtual ulong Guid
        {
            get { return magic.ReadUInt64(BaseAddress + (uint)TbcOffsets.ObjectOffsets.GuidOffset); }
            set { return; }
        }
        public virtual float XPosition
        {
            get { return magic.ReadFloat(BaseAddress + (uint)TbcOffsets.ObjectOffsets.XPositionOffset); }
        }
        public virtual float YPosition
        {
            get { return magic.ReadFloat(BaseAddress + (uint)TbcOffsets.ObjectOffsets.YPositionOffset); }
        }
        public virtual float ZPosition
        {
            get { return magic.ReadFloat(BaseAddress + (uint)TbcOffsets.ObjectOffsets.ZPositionOffset); }
        }
        public float Rotation
        {
            get { return magic.ReadFloat(BaseAddress + (uint)TbcOffsets.ObjectOffsets.RotationOffset); }
        }
        public int Health
        {
            get { return magic.ReadInt(magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.Health); }
        }
        public int MaxHealth
        {
            get { return magic.ReadInt(magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.MaxHealth); }
        }
        public int Mana
        {
            get { return magic.ReadInt(magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.Mana); }
        }
        public int MaxMana
        {
            get { return magic.ReadInt(magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.MaxMana); }
        }
        public int Energy
        {
            get { return magic.ReadInt(magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.Energy); }
        }
        public int Rage
        {
            get { return magic.ReadInt(magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.Rage); }
        }
        public string Name
        {
            get { return magic.ReadASCIIString(magic.ReadUInt((magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectOffsets.NameOffset1) + (uint)TbcOffsets.ObjectOffsets.NameOffset2)), 40); }
        }
    }
}
