using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class PlayerController : CharacterBase
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public override void Damage(float damageValue)
        {
            if (!_isLive)
            {
                return;
            }

            base.Damage(damageValue);

            if (_currentHealth <= 0)
            {
                base.Dead();
            }
        }
    }
}