using System;
using ProtoModels;

namespace GGMatch3
{
    [Serializable]
    public class BoosterConfig
    {
        public ChipType chipType
        {
            get
            {
                return BoosterConfig.BoosterToChipType(this.boosterType);
            }
        }

        public static ChipType BoosterToChipType(BoosterType booster)
        {
            if (booster == BoosterType.BombBooster)
            {
                return ChipType.Bomb;
            }
            if (booster == BoosterType.DiscoBooster)
            {
                return ChipType.DiscoBall;
            }
            if (booster == BoosterType.VerticalRocketBooster)
            {
                return ChipType.VerticalRocket;
            }
            if (booster == BoosterType.SeekingMissle)
            {
                return ChipType.SeekingMissle;
            }
            return ChipType.VerticalRocket;
        }

        public static ProtoModels.BoosterType BoosterToProtoType(BoosterType booster)
        {
            if (booster == BoosterType.BombBooster)
            {
                return ProtoModels.BoosterType.BombBooster;
            }
            if (booster == BoosterType.DiscoBooster)
            {
                return ProtoModels.BoosterType.DiscoBooster;
            }
            return ProtoModels.BoosterType.VericalRocketBooster;
        }

        public GGMatch3.BoosterType boosterType;
    }
}
