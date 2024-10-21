namespace Skills
{
    public interface ISkill
    {
        public string Name { get; }
        
        public int Level { get; set; }
        public int MaxLevel { get; }

        // health (/mana?)
        public int Cost { get; set; }
        
        public bool Activated { get; set; }

        public void Activate();
        // if holdable
        public void Deactivate();
    }
}