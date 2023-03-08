using System;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public abstract class CharacterBase : MonoBehaviour, IAttackable, IDamageable
    {
        [SerializeField] protected CharacterData characterData;

        protected int _curentHealth;
        protected bool _isDead;


        public abstract void Attack();

        public abstract void Damage();


        protected virtual void OnEnable()
        {
            GameManager.LevelPrepare += OnLevelPrepare;
        }
        protected virtual void OnDisable()
        {
            GameManager.LevelPrepare -= OnLevelPrepare;
        }


        private void OnLevelPrepare()
        {
            if (characterData != null)
            {
                _curentHealth = characterData.GetMaxHealth();
            }

            _isDead = false;
        }
    }
}