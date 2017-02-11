using Magic;

namespace WorkWork.Memory
{
    internal class Object
    {
        
        private readonly BlackMagic _magic;

        public Object(uint baseAddress, BlackMagic magic)
        {
            BaseAddress = baseAddress;
            _magic = magic;
        }
        public uint BaseAddress { get; set; }

        public int Type => _magic.ReadInt(BaseAddress + (uint)TbcOffsets.ObjectOffsets.TypeOffset);

        public virtual ulong Guid => _magic.ReadUInt64(BaseAddress + (uint)TbcOffsets.ObjectOffsets.GuidOffset);

        public virtual float XPosition => _magic.ReadFloat(BaseAddress + (uint)TbcOffsets.ObjectOffsets.XPositionOffset);

        public virtual float YPosition => _magic.ReadFloat(BaseAddress + (uint)TbcOffsets.ObjectOffsets.YPositionOffset);

        public virtual float ZPosition => _magic.ReadFloat(BaseAddress + (uint)TbcOffsets.ObjectOffsets.ZPositionOffset);

        public float Rotation => _magic.ReadFloat(BaseAddress + (uint)TbcOffsets.ObjectOffsets.RotationOffset);

        public int Health => _magic.ReadInt(_magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.Health);

        public int MaxHealth => _magic.ReadInt(_magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.MaxHealth);

        public int Mana => _magic.ReadInt(_magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.Mana);

        public int MaxMana => _magic.ReadInt(_magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.MaxMana);

        public int Energy => _magic.ReadInt(_magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.Energy);

        public int Rage => _magic.ReadInt(_magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectManagerOffsets.FieldOffset) + (uint)TbcOffsets.ObjectOffsets.Rage);

        public string Name => _magic.ReadASCIIString(_magic.ReadUInt((_magic.ReadUInt(BaseAddress + (uint)TbcOffsets.ObjectOffsets.NameOffset1) + (uint)TbcOffsets.ObjectOffsets.NameOffset2)), 40);
    }
}
