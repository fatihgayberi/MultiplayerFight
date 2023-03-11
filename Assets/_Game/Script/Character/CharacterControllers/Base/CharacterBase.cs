using System;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public abstract class CharacterBase : MonoBehaviour, IDamageable
    {
        public delegate void CharacterBaseCharacterDamaged(float maxHealth, float currentHealth, bool isDamagedMasterClient);
        public static event CharacterBaseCharacterDamaged CharacterDamaged;

        public delegate void CharacterBaseCharacterDead(bool isDeadMasterClient);
        public static event CharacterBaseCharacterDead CharacterDead;

        public static event CharacterManagers.CharacterManagersCharacterGenerated CharacterGenerated;

        [SerializeField] public PhotonView playerPhotonView;
        [SerializeField] public OutlineController outlineController;
        [SerializeField] protected CharacterData characterData;
        [SerializeField] private SpriteRenderer characterSpriteRenderer;

        protected float _currentHealth;
        protected bool _isLive;

        private const string functionName_PunRPC_Damage = "PunRPC_Damage";
        private const string functionName_PunRPC_Dead = "PunRPC_Dead";
        private const string functionName_PunRPC_OutlineActivator = "PunRPC_OutlineActivator";


        private void Start()
        {
            OutlineEditor();
        }


        private void OutlineEditor()
        {
            outlineController.OutlineActivator(false);

            if (outlineController == null)
            {
                return;
            }

            if (playerPhotonView == null)
            {
                return;
            }

            if (playerPhotonView.IsMine)
            {
                outlineController.OutlineActivator(true);
            }
        }


        public virtual void Damage(float damageValue)
        {
            if (playerPhotonView == null)
            {
                return;
            }

            playerPhotonView.RPC(functionName_PunRPC_Damage, RpcTarget.All, damageValue);

            CharacterDamaged?.Invoke(characterData.GetMaxHealth(), _currentHealth, PhotonNetwork.IsMasterClient);
        }

        [PunRPC]
        public void PunRPC_Damage(float damageValue)
        {
            _currentHealth -= damageValue;

            Debug.Log("_currentHealth:::" + _currentHealth, gameObject);

            if (characterData == null)
            {
                return;
            }
        }


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

            if (stream.IsWriting)
            {
                stream.SendNext(_currentHealth);

            }
            else
            {
                _currentHealth = (float)stream.ReceiveNext();
            }
        }


        public virtual void Dead()
        {
            if (playerPhotonView == null)
            {
                return;
            }

            playerPhotonView.RPC(functionName_PunRPC_Dead, RpcTarget.All, PhotonNetwork.IsMasterClient);
        }


        [PunRPC]
        public virtual void PunRPC_Dead(bool isDeadedMasterClient)
        {
            _isLive = false;

            CharacterDead?.Invoke(isDeadedMasterClient);
        }


        protected virtual void OnEnable()
        {
            GameManager.TourPrepare += OnTourPrepare;
            InputManager.InputFinish += OnInputFinish;
        }
        protected virtual void OnDisable()
        {
            GameManager.TourPrepare -= OnTourPrepare;
            InputManager.InputFinish -= OnInputFinish;
        }


        private void OnTourPrepare()
        {
            if (characterData != null)
            {
                _currentHealth = characterData.GetMaxHealth();
            }

            _isLive = true;
        }


        private void OnInputFinish(float horizontalInput, float verticalInput)
        {
            if (!TourController.Instance.IsMyTurn)
            {
                return;
            }

            BallBase ballBase = BallManager.Instance.GetBall(BallManager.BallType.BallClassic);

            if (ballBase == null)
            {
                return;
            }

            ballBase.gameObject.SetActive(true);
            ballBase.BallSetActive(true);

            ballBase.Throw(horizontalInput, verticalInput);

            TourController.Instance.TurnChange();
        }


        public virtual void CharacterInitialize()
        {
            if (playerPhotonView == null)
            {
                return;
            }

            if (PhotonNetwork.IsMasterClient)
            {
                transform.position = new Vector3(8, 0, 0);
            }
            else
            {
                transform.position = new Vector3(-8, 0, 0);
            }


            // CharacterSpriteRendererEnable(true);

            CharacterGenerated?.Invoke(playerPhotonView.ViewID);
        }


        public void CharacterSpriteRendererEnable(bool isEnabled)
        {
            if (characterSpriteRenderer == null)
            {
                return;
            }

            characterSpriteRenderer.enabled = isEnabled;
        }
    }
}