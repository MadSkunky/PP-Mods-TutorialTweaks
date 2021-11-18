namespace MadSkunky.TutorialTweaks
{
    // config class.
    internal class ModConfig
    {
        // config fields
        // If true Jacob get the modified 'Ares SAR-1 mod' as main weapon (see below), otherwise a standard Ares AR-1
        public bool GiveJacobAssaultRifleProficiency = true;
        public bool GiveJacobNewSpecialAres = true;
        // Modification to Jacobs 'Ares SAR-1 mod'
        public NewSpecialAresSetStruct NewSpecialAresSettings = new NewSpecialAresSetStruct(
            true,
            35,
            1,
            0,
            4,
            40,
            false,
            6,
            95,
            65
            );
        // DebugLevel (0: nothing, 1: error, 2: debug, 3: info)
        public int Debug = 1;
    }
    internal struct NewSpecialAresSetStruct
    {
        public NewSpecialAresSetStruct(
            bool nopenalty,
            int damage,
            int shred,
            int pierce,
            int burst,
            int er,
            bool manufacturable,
            int tech,
            int mat,
            int tp
            )
        {
            NoPenaltyWithoutProficiency = nopenalty;
            Damage = damage;
            Shred = shred;
            Pierce = pierce;
            BurstCount = burst;
            EffectiveRange = er;
            Manufacturable = manufacturable;
            ManufacturingCost = new ManufacureCost(tech, mat, tp);
        }

        public bool NoPenaltyWithoutProficiency;
        public int Damage;
        public int Shred;
        public int Pierce;
        public int BurstCount;
        public int EffectiveRange;
        public bool Manufacturable;
        public ManufacureCost ManufacturingCost;
        internal struct ManufacureCost
        {
            public ManufacureCost(int tech, int materials, int tp) : this()
            {
                Tech = tech;
                Materials = materials;
                TimePoints = tp;
            }

            public int Tech;
            public int Materials;
            public int TimePoints;
        }
    }
}
