using UnityEngine;

namespace Buffs
{
    public abstract class Buff
    {
        public BuffType Type;
        public Character Owner;
        public float MaxTime;
        public float RemainingTime;
        public int Stacks;
        // public delegate void BuffAffection(float spendTime);
        //
        // public BuffAffection OnTick;

        public virtual void Tick() { }
        
        public virtual void OnActivate() {}
        public virtual void OnDeactivate() {}

        public enum BuffType
        {
            Regeneration,
            Burning
        }
    }
}