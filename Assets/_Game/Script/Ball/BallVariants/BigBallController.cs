using UnityEngine;

namespace Wonnasmith
{
    public class BigBallController : BallBase
    {
        private void Start()
        {
            BallGeneratedEvent(BallManager.BallType.BallBig);
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