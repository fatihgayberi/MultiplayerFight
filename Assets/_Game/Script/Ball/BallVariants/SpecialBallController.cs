using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class SpecialBallController : BallBase
    {
        [SerializeField] private float ballFinishWaitTime;

        private WaitForSeconds _ballFinishWaitForSeconds;
        private Coroutine _ballFinishCoroutine;


        private void Start()
        {
            BallGeneratedEvent(BallManager.BallType.BallSpecial);
        }


        public override void BulletLifeFinised(GameObject damagedObje)
        {
            if (!_isBallLifeActive)
            {
                return;
            }

            _isBallLifeActive = false;

            if (damagedObje != null)
            {
                PlayerMaterialController playerMaterialController = damagedObje.GetComponent<PlayerMaterialController>();

                if (_ballFinishCoroutine != null)
                {
                    StopCoroutine(_ballFinishCoroutine);
                }

                _ballFinishCoroutine = StartCoroutine(BulletLifeFinisedIenumerator(playerMaterialController));
            }
            else
            {
                TourController.Instance.TurnChange();
                BallReset();
                BallSetActive(false);
            }
        }


        private IEnumerator BulletLifeFinisedIenumerator(PlayerMaterialController playerMaterialController)
        {
            if (playerMaterialController != null)
            {
                playerMaterialController.SpecialActivator(true);
            }

            if (_ballFinishWaitForSeconds == null)
            {
                _ballFinishWaitForSeconds = new WaitForSeconds(ballFinishWaitTime);
            }

            yield return _ballFinishWaitForSeconds;

            if (playerMaterialController != null)
            {
                playerMaterialController.SpecialActivator(false);
            }

            TourController.Instance.TurnChange();
            BallReset();
            BallSetActive(false);
        }
    }
}