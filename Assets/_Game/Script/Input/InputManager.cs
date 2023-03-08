using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wonnasmith
{
    public class InputManager : MonoBehaviour
    {
        public delegate void InputManagerInputChange(float horizontalInput, float verticalInput);
        public static event /*InputManager.*/ InputManagerInputChange InputChange;

        public delegate void InputManagerInputAction();
        public static event /*InputManager.*/ InputManagerInputAction InputStart;
        public static event /*InputManager.*/ InputManagerInputAction InputFinish;


        [SerializeField, Range(0f, 100f)] float joystickRange;

        private float _width;
        private float _height;

        private float pixelDistance;

        private Vector3 _firstPos;
        private Vector3 _endPos;

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

            if (Input.GetMouseButtonDown(0))
            {
                InputStart?.Invoke();

                _firstPos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                _endPos = Input.mousePosition;

                Touching();
            }

            if (Input.GetMouseButtonUp(0))
            {
                InputFinish?.Invoke();
            }
        }


        private void Touching()
        {
            Vector3 posDist = _endPos - _firstPos;

            float horizontalInput = posDist.x.FloatRemap(-pixelDistance, pixelDistance, -1, 1);
            // horizontalInput = Mathf.Clamp(horizontalInput, -1, 1);

            float verticalInput = posDist.y.FloatRemap(-pixelDistance, pixelDistance, -1, 1);
            // verticalInput = Mathf.Clamp(verticalInput, -1, 1);

            InputChange?.Invoke(horizontalInput, verticalInput);
        }
    }
}
