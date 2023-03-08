using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wonnasmith
{
    public class ThrowInputController : MonoBehaviour
    {
        public delegate void ThrowInputControllerThrowInputListener(float throwPower, float throwAngle, float horizontalInput, float verticalInput);
        public static event /*ThrowInputController.*/ ThrowInputControllerThrowInputListener ThrowInputListener;

        [SerializeField] private SpriteRenderer arrowSpriteRenderer;
        [SerializeField] private Transform arrowTR;

        [SerializeField] private float minScale;
        [SerializeField] private float maxScale;

        [SerializeField] private Color startColor;
        [SerializeField] private Color endColor;

        private float _throwPower;
        private float _throwAngle;
        private float _horizontalInput;
        private float _verticalInput;

        private void OnEnable()
        {
            InputManager.InputStart += OnInputStart;
            InputManager.InputChange += OnInputChange;
            InputManager.InputFinish += OnInputFinish;
        }
        private void OnDisable()
        {
            InputManager.InputStart -= OnInputStart;
            InputManager.InputChange -= OnInputChange;
            InputManager.InputFinish -= OnInputFinish;
        }


        private void OnInputStart()
        {
            _throwAngle = 0;
            _throwPower = 0;
            _horizontalInput = 0;
            _verticalInput = 0;

            ArrrowColorChange();
            ArrrowRotationChange();
            ArrrowScaleChange();
            ArrrowSpriteActivator(true);
        }


        private void OnInputChange(float horizontalInput, float verticalInput)
        {
            _horizontalInput = horizontalInput;
            _verticalInput = verticalInput;

            _throwAngle = (180 / Mathf.PI) * Mathf.Atan2(_verticalInput, _horizontalInput);
            _throwPower = Mathf.Clamp01(MathF.Abs(_horizontalInput) + MathF.Abs(_verticalInput));

            ArrrowColorChange();
            ArrrowRotationChange();
            ArrrowScaleChange();
        }


        private void OnInputFinish()
        {
            ArrrowSpriteActivator(false);

            ThrowInputListener?.Invoke(_throwPower, _throwAngle, _horizontalInput, _verticalInput);
        }


        private void ArrrowScaleChange()
        {
            if (arrowTR == null)
            {
                return;
            }

            arrowTR.localScale = new Vector3(_throwPower.FloatRemap(0, 1, minScale, maxScale), 1, 1);
        }


        private void ArrrowRotationChange()
        {
            if (arrowTR == null)
            {
                return;
            }

            arrowTR.rotation = Quaternion.Euler(0, 0, _throwAngle);
        }


        private void ArrrowColorChange()
        {
            if (arrowSpriteRenderer == null)
            {
                return;
            }

            arrowSpriteRenderer.color = (_throwPower * 100).ColorPercentRate(startColor, endColor);
        }


        private void ArrrowSpriteActivator(bool isEnable)
        {
            if (arrowSpriteRenderer == null)
            {
                return;
            }

            arrowSpriteRenderer.enabled = isEnable;
        }
    }
}
