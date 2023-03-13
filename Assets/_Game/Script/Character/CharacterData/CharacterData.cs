using UnityEngine;
using System;

namespace Wonnasmith
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "MultiplayerFight/CharacterData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] private int maxHealth;

        [SerializeField] private WonnRangeDatas rangeDuration;
        [SerializeField] private WonnRangeDatas rangeCurve;
        [SerializeField] private WonnRangeDatas rangeThrowPosX;
        [SerializeField] private WonnRangeDatas rangeThrowPosY;


        public int MaxHealth { get => maxHealth; }

        public WonnRangeDatas RangeThrowPosY { get => rangeThrowPosY; }
        public WonnRangeDatas RangeThrowPosX { get => rangeThrowPosX; }
        public WonnRangeDatas RangeCurve { get => rangeCurve; }
        public WonnRangeDatas RangeDuration { get => rangeDuration; }
    }
}