namespace Tirajii.Models.Trucker
{
    public class ExperienceModel
    {
        public bool WasLeveledUp { get; set; } = false;

        public bool GainedXp { get; set; } = false;

        public int Xp { get; set; }
        public int Level { get; set; }
    }
}
