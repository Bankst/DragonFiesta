namespace DragonFiesta.Utils.Config.Section
{
    public class DatabaseSection
    {
        public virtual int DatabaseClientLifeTime { get; set; } = 10;

        public virtual int MaxPoolSize { get; set; } = 10;

        public int MinPoolSize { get; set; } = 5;

        public virtual string SQLHost { get; set; } = "SQL_HOST";

        public virtual string SQLUser { get; set; } = "sa";

        public virtual string SQLPassword { get; set; } = "123456";

        public virtual string SQLName { get; set; } = "auth";
    }
}