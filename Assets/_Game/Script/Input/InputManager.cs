using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class InputManager : MonoBehaviour
    {
        public delegate void InputManagerInputChange(float horizontalInput, float verticalInput);
        public static event /*InputManager.*/ InputManagerInputChange InputChange;
        public static event /*InputManager.*/ InputManagerInputChange InputFinish;

        public delegate void InputManagerInputAction(Vector2 firstPos);
        public static event /*InputManager.*/ InputManagerInputAction InputStart;

        [SerializeField, Range(0f, 100f)] float joystickRange;
        [SerializeField, Range(0f, 100f)] float joystickDiscardRange;

        private float _width;
        private float _height;
        private float _pixelDistance;
        private float _pixelDiscardDistance;
        private float _horizontalInput;
        private float _verticalInput;

        private Vector3 _firstPos;
        private Vector3 _endPos;
        private Vector3 _posDist;


        private void Start()
        {
            _width = Screen.width;
            _height = Screen.height;

            if (_width > _height)
            {
                _pixelDistance = _width * joystickRange / 200;
                _pixelDiscardDistance = _width * joystickDiscardRange / 200;
            }
            else
            {
                _pixelDistance = _height * joystickRange / 200;
                _pixelDiscardDistance = _height * joystickDiscardRange / 200;
            }
        }


        private void Update()
        {
            if (!GameManager.Instance.GameIsPlaying())
            {
                return;
            }

            if (Input.touchCount > 1)
            {
                // ekrana fazla barnak atmasın

                return;
            }

            if (TourController.Instance.IsTurnOfMasterClient && !PhotonNetwork.IsMasterClient)
            {
                return;
            }


            if (!TourController.Instance.IsTurnOfMasterClient && PhotonNetwork.IsMasterClient)
            {
                return;
            }


            if (Input.GetMouseButtonDown(0))
            {
                _firstPos = Input.mousePosition;

                InputStart?.Invoke(_firstPos);
            }

            if (Input.GetMouseButton(0))
            {
                _endPos = Input.mousePosition;

                if ((_endPos - _firstPos).magnitude >= _pixelDiscardDistance)
                {
                    Touching();
                    InputChange?.Invoke(_horizontalInput, _verticalInput);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Touching();

                if ((_endPos - _firstPos).magnitude >= _pixelDiscardDistance)
                {
                    InputFinish?.Invoke(_horizontalInput, _verticalInput);
                }
            }
        }


        private void Touching()
        {
            _posDist = _endPos - _firstPos;

            _horizontalInput = _posDist.x.FloatRemap(-_pixelDistance, _pixelDistance, -1, 1);

            _verticalInput = _posDist.y.FloatRemap(-_pixelDistance, _pixelDistance, -1, 1);
        }
    }
}
