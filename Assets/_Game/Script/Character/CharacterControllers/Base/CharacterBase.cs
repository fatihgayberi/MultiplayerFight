using System;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public abstract class CharacterBase : MonoBehaviour, IDamageable
    {
        public delegate void CharacterBaseCharacterDamaged(float maxHealth, float currentHealth, bool isDamagedMasterClient);

        public delegate void CharacterBaseCharacterDead(bool isDeadMasterClient);

        public delegate void CharacterBaseCharacterThrowed();

        [SerializeField] protected SpriteRenderer characterSpriteRenderer;
        [SerializeField] protected CharacterData characterData;
        [SerializeField] protected Transform ballThrowTR;
        [SerializeField] protected PlayerMaterialController playerMaterialController;


        protected float _currentHealth;
        protected bool _isLive;


        public virtual void OnEnable()
        {
            GameManager.TourPrepare += OnTourPrepare;
        }
        public virtual void OnDisable()
        {
            GameManager.TourPrepare -= OnTourPrepare;
        }


        private void OnTourPrepare()
        {
            if (characterData != null)
            {
                _currentHealth = characterData.MaxHealth;
            }

            _isLive = true;
        }


        public virtual void Damage(float damageValue)
        {

        }


        public virtual void Dead()
        {
            _isLive = false;
        }


        protected void CharacterSpriteRendererEnable(bool isEnabled)
        {
            if (characterSpriteRenderer == null)
            {
                return;
            }

            characterSpriteRenderer.enabled = isEnabled;
        }
    }
}