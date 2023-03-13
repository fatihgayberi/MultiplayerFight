using System;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Wonnasmith
{
    public abstract class BallBase : MonoBehaviour
    {
        public delegate void BallBaseBallGenerated(BallBase ballBase, BallManager.BallType ballType);
        public static event /*BallBase.*/BallBaseBallGenerated BallGenerated;

        [SerializeField] private BallData ballData;

        [SerializeField] private Rigidbody2D ballRigidbody2D;
        [SerializeField] protected PhotonView ballPhotonView;
        [SerializeField] private Vector2 ballMaxDist;
        [SerializeField] private Vector2 ballMinDist;


        private float _speedY;
        private float _speedX;
        private float _distanceX;
        private float _distanceY;
        private float _posConstValue_1;
        private float _posConstValue_2;
        private float _tempThrowDuration;
        private float _throwDuration;
        private float _angle;
        private float _minCurve;
        private float _maxCurve;
        private float _curve;

        private float _tempLifeDuration;

        private int _horizontalDirection = 1;

        private Vector3 _newPos = Vector3.zero;
        private Vector3 _firstPos = Vector3.zero;
        private Vector3 _calculatePos = Vector3.zero;

        private bool _isThrow;
        private bool _isVerticalThrow;

        protected bool _isBallLifeActive;

        public abstract void BulletLifeFinised(GameObject damagedObje);


        public PhotonView GetBallPhotonView()
        {
            return ballPhotonView;
        }


        public void BallGeneratedEvent(BallManager.BallType ballType)
        {
            if (ballPhotonView == null)
            {
                return;
            }

            ballPhotonView.RPC("PunRPC_BallGeneratedEvent", RpcTarget.All, ballType);
        }


        [PunRPC]
        public void PunRPC_BallGeneratedEvent(BallManager.BallType ballType)
        {
            BallGenerated?.Invoke(this, ballType);
        }


        public void BallReset()
        {
            _isThrow = false;

            if (ballRigidbody2D != null)
            {
                ballRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                ballRigidbody2D.velocity = Vector2.zero;
            }
        }


        public void ThrowInitialize(float horizontalInput, float verticalInput)
        {
            _isBallLifeActive = true;

            if (ballData == null)
            {
                return;
            }

            if (ballRigidbody2D == null)
            {
                return;
            }

            ballRigidbody2D.bodyType = RigidbodyType2D.Dynamic;

            ballRigidbody2D.velocity = new Vector2(-horizontalInput, -verticalInput) * ballData.Power;
        }


        public void ThrowInitialize(Vector3 targetPos, float curve, float duration)
        {
            if (ballRigidbody2D == null)
            {
                return;
            }

            ballRigidbody2D.bodyType = RigidbodyType2D.Kinematic;

            _isThrow = true;
            _isBallLifeActive = true;

            _curve = curve;
            _throwDuration = duration;

            _firstPos = transform.position;

            _distanceX = Mathf.Abs(targetPos.x - transform.position.x);
            _distanceY = Mathf.Abs(targetPos.y - transform.position.y);

            _isVerticalThrow = false;//_distanceY > _distanceX ? true : false;

            _horizontalDirection = 1;

            if (targetPos.x < transform.position.x)
            {
                // yatay konumda hedef noktası negatif tarafta kalıyor

                _horizontalDirection = -1;
            }

            if (_isVerticalThrow)
            {
                // dikey yonde bir atis yapiliyor

                _speedY = (_distanceY / duration);

                _angle = Mathf.Atan2(_curve, _distanceY);

                _speedX = Mathf.Abs(_speedY * Mathf.Tan(_angle));

                _posConstValue_1 = ((-2 * Mathf.Abs(Mathf.Tan(_angle))) / _distanceY);
                _posConstValue_2 = _speedX * (duration / 2);
            }
            else
            {
                // yatay yonde bir atis yapiliyor

                _speedX = (_distanceX / duration);

                _angle = Mathf.Atan2(_curve, _distanceX);

                _speedY = Mathf.Abs(_speedX * Mathf.Tan(_angle));

                _posConstValue_1 = ((-2 * Mathf.Abs(Mathf.Tan(_angle))) / _distanceX);
                _posConstValue_2 = _speedY * (duration / 2);
            }

            _tempThrowDuration = 0;
        }


        private void Throw()
        {
            if (!_isThrow)
            {

                return;
            }


            if (_isVerticalThrow)
            {
                _newPos.y = GetPositionY();
                _newPos.x = GetPositionX(_newPos.y);
            }
            else
            {
                _newPos.x = GetPositionX();
                _newPos.y = GetPositionY(_newPos.x);
            }

            _calculatePos.x = _firstPos.x + (_newPos.x * _horizontalDirection);
            _calculatePos.y = _firstPos.y + _newPos.y;

            transform.position = _calculatePos;

            _tempThrowDuration += Time.fixedDeltaTime;

            // if (_tempThrowDuration > _throwDuration)
            // {
            //     // degdigi anda bir sey olmasini istiyorsan buraya yazabilirsin
            // }
        }


        private void FixedUpdate()
        {
            if (!GameManager.Instance.GameIsPlaying())
            {
                return;
            }

            Throw();

            BallLifeControl();
        }

        private void BallLifeControl()
        {
            if (!_isBallLifeActive)
            {
                return;
            }

            if (!(transform.position.x < ballMaxDist.x && transform.position.x > ballMinDist.x))
            {
                BulletLifeFinised(null);
            }
            if (!(transform.position.y < ballMaxDist.y && transform.position.y > ballMinDist.y))
            {
                BulletLifeFinised(null);
            }
        }


        private float GetPositionX(float posY)
        {
            return _posConstValue_1 * (posY - _distanceY / 2) * (posY - _distanceY / 2) + _posConstValue_2 + ((_distanceX * _tempThrowDuration) / _throwDuration);
        }
        private float GetPositionX()
        {
            return _speedX * _tempThrowDuration;
        }


        private float GetPositionY(float posX)
        {
            return _posConstValue_1 * (posX - _distanceX / 2) * (posX - _distanceX / 2) + _posConstValue_2 + ((_distanceY * _tempThrowDuration) / _throwDuration);
        }
        private float GetPositionY()
        {
            return _speedY * _tempThrowDuration;
        }


        public void BallSetActive(bool isActive)
        {
            if (ballPhotonView == null)
            {
                return;
            }

            ballPhotonView.RPC("RPC_BallSetActive", RpcTarget.All, isActive);
        }


        [PunRPC]
        protected virtual void RPC_BallSetActive(bool isActive)
        {
            // BallReset();
            gameObject.SetActive(isActive);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!GameManager.Instance.GameIsPlaying())
            {
                return;
            }

            if (other == null)
            {
                return;
            }

            if (!_isBallLifeActive)
            {
                return;
            }

            if (LayerManager.Instance.IsLayerEquals(other.gameObject.layer, LayerManager.LayerType.Character_LAYER))
            {
                if (ballData == null)
                {
                    return;
                }

                IDamageable iDamageable = other.GetComponent<IDamageable>();

                if (iDamageable != null)
                {
                    iDamageable.Damage(ballData.Damage);

                    BulletLifeFinised(other.gameObject);
                }
            }
        }
    }
}