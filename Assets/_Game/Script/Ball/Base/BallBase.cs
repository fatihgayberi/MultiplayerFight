using UnityEngine;

namespace Wonnasmith
{
    public abstract class BallBase : MonoBehaviour, IThrowable
    {
        public abstract void ThrowStarted();
        public abstract void ThrowFinished();

        [Space(), Space()]
        [WonnasmithEditor.HelpBox("Bu mermi hiz kullanmiyor, onun yerine duration kullaniyor.", WonnasmithEditor.HelpBoxMessageType.Info)]
        [Space(), Space()]

        [SerializeField] private float minCurve;
        [SerializeField] private float maxCurve;
        [SerializeField] private float duration;
        [SerializeField][Range(0, 1)] private float scalePercentRate;



        private float _angle;
        private float _speedY;
        private float _speedX;
        private float _curve;
        private float _distanceX;
        private float _distanceY;
        private float _tempDuration;
        private float _tempScaleRate;
        private float _posConstValue_1;
        private float _posConstValue_2;

        private int _horizontalDirection = 1;
        private int _verticalDirection = 1;

        private Vector3 _newPos = Vector3.zero;
        private Vector3 _firstPos = Vector3.zero;
        private Vector3 _calculatePos = Vector3.zero;

        private bool _isThrow;
        private bool _isVerticalThrow;

        public Transform targetTR;
        public bool isTest;


        private void Update()
        {
            if (isTest)
            {
                isTest = false;

                // ThrowInitialize(targetTR.position);
            }
        }


        public void ThrowStart(float throwPower, float throwAngle, float horizontalInput, float verticalInput)
        {
            ThrowInitialize();
        }


        private void ThrowInitialize(float angle, float distanceHorizantal, Vector3 targetPos)
        {
            _isThrow = true;

            _firstPos = transform.position;

            _distanceX = Mathf.Abs(targetPos.x - transform.position.x);
            _distanceY = Mathf.Abs(targetPos.y - transform.position.y);

            _isVerticalThrow = _distanceY > _distanceX ? true : false;

            _horizontalDirection = 1;
            _verticalDirection = 1;

            if (targetPos.x < transform.position.x)
            {
                // yatay konumda hedef noktasi negatif tarafta kaliyor

                _horizontalDirection = -1;
            }

            if (targetPos.y < transform.position.y)
            {
                // dikey konumda hedef noktasi negatif tarafta kaliyor

                _verticalDirection = -1;
            }

            _curve = UnityEngine.Random.Range(minCurve, maxCurve);

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

            _tempDuration = 0;
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


            if (_tempDuration <= duration / 2)
            {
                // yukari çikiyor

                _tempScaleRate = _tempDuration.FloatRemap(0, duration / 2, 1, scalePercentRate);
            }
            else
            {
                // aşagi düşüyor

                _tempScaleRate = _tempDuration.FloatRemap(duration / 2, duration, scalePercentRate, 1);
            }

            _calculatePos.x = _firstPos.x + (_newPos.x * _horizontalDirection);
            _calculatePos.y = _firstPos.y + (_newPos.y * _verticalDirection);

            transform.position = _calculatePos;
            transform.localScale = Vector3.one * _tempScaleRate;

            _tempDuration += Time.fixedDeltaTime;

            if (_tempDuration > duration)
            {
                // hedefe ulasildi
                _isThrow = false;
            }
        }


        private void FixedUpdate()
        {
            Throw();
        }


        private float GetPositionX(float posY)
        {
            return _posConstValue_1 * (posY - _distanceY / 2) * (posY - _distanceY / 2) + _posConstValue_2 + ((_distanceX * _tempDuration) / duration);
        }
        private float GetPositionX()
        {
            return _speedX * _tempDuration;
        }


        private float GetPositionY(float posX)
        {
            return _posConstValue_1 * (posX - _distanceX / 2) * (posX - _distanceX / 2) + _posConstValue_2 + ((_distanceY * _tempDuration) / duration);
        }
        private float GetPositionY()
        {
            return _speedY * _tempDuration;
        }

    }
}