namespace MadSkunky.TutorialTweaks
{
    // config class.
    internal class ModConfig
    {
        // config fields
        // If true Jacob get the modified 'Ares SAR-1 mod' as main weapon (see below), otherwise a standard Ares AR-1
        public bool UseNewAresSAR1forJacob = true;
        // Modification to Jacobs 'Ares SAR-1 mod'
        public AresSAR1SetStruct NewAresSAR1Settings = new AresSAR1SetStruct(
            35,
            1,
            4,
            40
            );
        // DebugLevel (0: nothing, 1: error, 2: debug, 3: info)
        public int Debug = 1;

    }
    internal struct AresSAR1SetStruct
    {
        public AresSAR1SetStruct(int damage, int shred, int burst, int er)
        {
            Damage = damage;
            Shred = shred;
            BurstCount = burst;
            EffectiveRange = er;
        }

        public int Damage;
        public int Shred;
        public int BurstCount;
        public int EffectiveRange;
    }
}
