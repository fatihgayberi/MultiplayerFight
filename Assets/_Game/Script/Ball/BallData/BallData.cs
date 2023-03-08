using UnityEngine;

namespace Wonnasmith
{
    [CreateAssetMenu(fileName = "BallData", menuName = "MultiplayerFight/BallData", order = 0)]
    public class BallData : ScriptableObject
    {
        [SerializeField] private float minCurve;
        [SerializeField] private float maxCurve;
        [SerializeField] private float duration;
        [SerializeField][Range(0, 100)] private float scalePercentRate;
    }
}