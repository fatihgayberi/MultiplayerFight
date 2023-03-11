using System;
using UnityEngine;

namespace Wonnasmith
{
    [CreateAssetMenu(fileName = "BallData", menuName = "MultiplayerFight/BallData", order = 0)]
    public class BallData : ScriptableObject
    {
        [Tooltip("Topun temel firlatma gucu"), SerializeField] private float power;
        [Tooltip("Topun verecegi damage degeri"), SerializeField] private float damage;


        public float Power { get => power; }
        public float Damage { get => damage; }
    }
}