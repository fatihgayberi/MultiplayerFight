using UnityEngine;

namespace Wonnasmith
{
    public class ClassicBallController : BallBase
    {
        private void Start()
        {
            BallGeneratedEvent(BallManager.BallType.BallClassic);
        }


        public override void BulletLifeFinised(GameObject damagedObje)
        {
            if (!_isBallLifeActive)
            {
                return;
            }

            _isBallLifeActive = false;

            BallReset();

            TourController.Instance.TurnChange();

            BallSetActive(false);
        }
    }
}