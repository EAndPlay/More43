namespace Buffs
{
    public class RegenerationBuff : Buff
    {
        public float HealthAdd;
        
        public RegenerationBuff(float healthAdd = 0.25f, float maxTime = 10)
        {
            Type = BuffType.Regeneration;
            HealthAdd = healthAdd;
            MaxTime = maxTime;
        }

        public override void Tick()
        {
            Owner.Health += HealthAdd;
        }
    }
}