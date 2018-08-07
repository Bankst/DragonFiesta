using System;

namespace DragonFiesta.Utils.Config.Section
{
    public class CommonSection
    {
        public TimeSpan AttackSpeed { get; set; } = TimeSpan.FromMilliseconds(125);
        public byte ShoutLevel { get; set; } = 0;
        public TimeSpan ShoutDelay { get; set; } = TimeSpan.FromSeconds(130);
    }
}