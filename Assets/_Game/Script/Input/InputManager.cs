using System.Collections;
using System.Collections.Generic;
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

        private float _width;
        private float _height;
        private float pixelDistance;
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
                pixelDistance = _width * joystickRange / 200;
            }
            else
            {
                pixelDistance = _height * joystickRange / 200;
            }
        }


        private void Update()
        {
            if (Input.touchCount > 1)
            {
                // ekrana fazla barnak atmasın

                return;
            }

            if (!TourController.Instance.IsMyTurn)
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

                Touching();
                InputChange?.Invoke(_horizontalInput, _verticalInput);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Touching();
                InputFinish?.Invoke(_horizontalInput, _verticalInput);
                Debug.Log("GetMouseButtonUp:::", gameObject);
            }
        }


        private void Touching()
        {
            _posDist = _endPos - _firstPos;

            _horizontalInput = _posDist.x.FloatRemap(-pixelDistance, pixelDistance, -1, 1);

            _verticalInput = _posDist.y.FloatRemap(-pixelDistance, pixelDistance, -1, 1);
        }
    }
}
