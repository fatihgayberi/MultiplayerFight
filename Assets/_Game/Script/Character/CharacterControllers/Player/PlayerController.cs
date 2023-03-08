using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class PlayerController : CharacterBase
    {
        [SerializeField] private BallClassicController ballClassicController;

        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }


        public override void Attack()
        {

        }

        public override void Damage()
        {

        }
    }
}