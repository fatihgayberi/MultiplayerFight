using UnityEngine;

namespace Wonnasmith
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "MultiplayerFight/CharacterData", order = 0)]
    public class CharacterData : ScriptableObject
    {
        [SerializeField] private int maxHealth;

        public int GetMaxHealth() { return maxHealth; }
    }
}