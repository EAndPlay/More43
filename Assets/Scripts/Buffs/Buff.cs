namespace Buffs
{
    public abstract class Buff
    {
        public Character Owner;
        
        public float MaxTime;

        public float RemainingTime;
        // public delegate void BuffAffection(float spendTime);
        //
        // public BuffAffection OnTick;

        public virtual void Tick() { }
    }
}