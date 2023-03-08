using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Wonnasmith
{
    public class WonnaTest3 : MonoBehaviour
    {
        public Transform arrowTR;

        private void OnEnable()
        {
            InputManager.InputChange += OnInputChange;
        }
        private void OnDisable()
        {
            InputManager.InputChange += OnInputChange;
        }


        private void OnInputChange(float horizontalInput, float verticalInput)
        {
            float AngleRad = Mathf.Atan2(verticalInput, horizontalInput);
            float angle = (180 / Mathf.PI) * AngleRad;

            Debug.Log("horizontalInput::" + horizontalInput + "   verticalInput::" + verticalInput);

            arrowTR.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
