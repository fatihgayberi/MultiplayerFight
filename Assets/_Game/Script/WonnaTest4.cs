using System;
using UnityEngine;

namespace Wonnasmith
{
    public class WonnaTest4 : MonoBehaviour
    {
        public Rigidbody2D ballRB;
        public float time;
        public float angle;
        public Transform targetTR;
        public Transform ballTR;

        public bool isThrow;

        private void Update()
        {
            if (isThrow)
            {
                isThrow = false;

                Throw();
            }
        }

        public Vector2 speed;
        public Vector2 dist;
        private void Throw()
        {
            dist = ballTR.position - targetTR.position;

            speed.x = dist.x / time;

            speed.y = dist.y - ((Physics2D.gravity.y / 2) * time * time);

            ballRB.velocity = speed;
        }
    }
}
