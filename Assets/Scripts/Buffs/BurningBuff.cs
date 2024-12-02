using UnityEngine;

namespace Buffs
{
    public class BurningBuff : Buff
    {
        private const float BurningDamage = 0.4f;
        private const float AdditionalBurningDamage = 0.2f;

        private ParticleSystem _particleSystem;
        
        public BurningBuff(float maxTime = 5)
        {
            Type = BuffType.Burning;
            MaxTime = maxTime;
        }

        public override void Tick()
        {
            Owner.Health -= BurningDamage + (Stacks - 1) * AdditionalBurningDamage;
        }

        public override void OnActivate()
        {
            _particleSystem = Owner.AddParticles(GameGlobals.BurningParticles);
        }

        public override void OnDeactivate()
        {
            Owner.RemoveParticles(_particleSystem);
        }
    }
}