namespace WorkWork.Memory
{
    internal class TbcOffsets
    {
        public enum CombatState
        {
            InCombat = 524296,
        }
        public enum ObjectOffsets
        {
            GuidOffset = 0x30,
            NextObjectOffset = 0x3C,
            TypeOffset = 0x14,
            XPositionOffset = 0xBF0,
            YPositionOffset = 0xBF4,
            ZPositionOffset = 0xBF8,
            RotationOffset = 0xBFC,
            Health = 0x40,
            MaxHealth = 0x58,
            Mana = 0x44,
            MaxMana = 0x5C,
            Energy = 0x50,
            Rage = 0x48,
            NameOffset1 = 0xDB8,
            NameOffset2 = 0x40,
            GameObjectNameOffset1 = 0x224,
            GameObjectNameOffset2 = 0x78,
            GameObjectOffset = 0xEC,
            GameObjectOffsetX = 0x6C,
            GameObjectOffsetY = 0x70,
            GameObjectOffsetZ = 0x74,




        };
        public enum ObjectManagerOffsets
        {
            ObjectManagerOffset = 0x2218,
            LocalGuid = 0xC0,
            FirstObject = 0xAC,
            NextObject = 0x3C,
            Guid = 0x30,
            Type = 0x14,
            X = 0xBF0,
            Y = 0xBF4,
            Z = 0xBF8,
            R = 0xBFC,
            FieldOffset = 0x120,
        };
        public enum ObjectTypes
        {
            Object = 0,
            Item = 1,
            Container = 2,
            Unit = 3,
            Player = 4,
            Gameobject = 5,
            Dynamicobject = 6,
            Corpse = 7,
            Areatrigger = 8,
            Sceneobject = 9,
            NumClientObjectTypes = 0xA
        };
        public enum General
        {
            ClientConnection = 0x00D43318,
            PlayerBase = 0x00E29D28,
            PlayerCorpseX = 0x00C6EA80,
            PlayerCorpseY = 0x00C6EA84,
            PlayerCorpseZ = 0x00C6EA88,
            PlayerX = 0x00E18DF4,
            PlayerY = 0x00E18DF8,
            PlayerZ = 0x00E18DFC,
            PlayerRotation = 0x00E18E24,
            CombatStateOffset = 0x26F8,
            TargetGuid = 0x00C6E960,
            Cursor = 0x00CF5750,

        };
    }
}
