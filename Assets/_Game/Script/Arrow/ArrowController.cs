using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Wonnasmith
{
    public class ArrowController : MonoBehaviour
    {
        [SerializeField] private Image arrowImage;
        [SerializeField] private Transform arrowTR;

        [SerializeField] private float maxScale;

        [SerializeField] private Color startColor;
        [SerializeField] private Color endColor;

        private float _throwPower;
        private float _throwAngle;

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


        private void OnInputStart(Vector2 firstPos)
        {
            _throwAngle = 0;
            _throwPower = 0;

            ArrrowPosChange(firstPos);
            ArrrowColorChange();
            ArrrowRotationChange();
            ArrrowScaleChange();
            ArrrowImageActivator(true);
        }


        private void OnInputChange(float horizontalInput, float verticalInput)
        {
            _throwAngle = (180 / Mathf.PI) * Mathf.Atan2(verticalInput, horizontalInput);
            _throwPower = Mathf.Clamp01(MathF.Abs(horizontalInput) + MathF.Abs(verticalInput));

            ArrrowColorChange();
            ArrrowRotationChange();
            ArrrowScaleChange();
        }


        private void OnInputFinish(float horizontalInput, float verticalInput)
        {
            ArrrowImageActivator(false);
        }


        private void ArrrowPosChange(Vector3 pos)
        {
            if (arrowTR == null)
            {
                return;
            }

            arrowTR.position = pos;
        }


        private void ArrrowScaleChange()
        {
            if (arrowTR == null)
            {
                return;
            }

            arrowTR.localScale = new Vector3(_throwPower.FloatRemap(0, 1, 0, maxScale), 1, 1);
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
            if (arrowImage == null)
            {
                return;
            }

            arrowImage.color = (_throwPower * 100).ColorPercentRate(startColor, endColor);
        }


        private void ArrrowImageActivator(bool isEnable)
        {
            if (arrowImage == null)
            {
                return;
            }

            arrowImage.enabled = isEnable;
        }
    }
}
