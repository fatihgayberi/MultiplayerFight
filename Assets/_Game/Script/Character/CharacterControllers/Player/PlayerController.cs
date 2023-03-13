using System;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class PlayerController : CharacterBase
    {
        public delegate void PlayerControllerMineMasterClientCharacter(Transform masterClientCharactreClientTR);
        public static event /*PlayerController.*/PlayerControllerMineMasterClientCharacter MineMasterClientCharacter;

        public static event CharacterBaseCharacterDamaged CharacterDamaged;
        public static event CharacterBaseCharacterDead CharacterDead;
        public static event CharacterBaseCharacterThrowed CharacterThrowed;
        public static event CharacterManagers.CharacterManagersCharacterGenerated CharacterGenerated;

        [SerializeField] public PhotonView playerPhotonView;

        private const string functionName_PunRPC_Throwed = "PunRPC_Throwed";
        private const string functionName_PunRPC_Damage = "PunRPC_Damage";
        private const string functionName_PunRPC_IsMasterClientCharacterInitialize = "PunRPC_IsMasterClientCharacterInitialize";

        private bool _isMasterClientCharacter;

        private bool _isThrowable = true;

        private BallManager.BallType throwballType = BallManager.BallType.BallClassic;

        public override void OnEnable()
        {
            base.OnEnable();

            InputManager.InputFinish += OnInputFinish;
            TourController.TurnChanged += OnTurnChanged;
            TourController.TourCountDownFinish += OnTourCountDownFinish;
            UIPowerUpButtonController.BallButtonSelect += OnBallButtonSelect;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            InputManager.InputFinish -= OnInputFinish;
            TourController.TurnChanged -= OnTurnChanged;
            TourController.TourCountDownFinish -= OnTourCountDownFinish;
            UIPowerUpButtonController.BallButtonSelect -= OnBallButtonSelect;
        }


        private void Start()
        {
            PlayerMaterialEditor();

            IsMasterClientCharacterInitialize();

            BallThrowTRInitialize();
        }


        private void PlayerMaterialEditor()
        {
            playerMaterialController.SpecialActivator(false);
            playerMaterialController.OutlineActivator(false);

            if (playerMaterialController == null)
            {
                return;
            }

            if (playerPhotonView == null)
            {
                return;
            }

            if (playerPhotonView.IsMine)
            {
                playerMaterialController.OutlineActivator(true);
            }
        }


        private void IsMasterClientCharacterInitialize()
        {
            if (playerPhotonView != null)
            {
                if (PhotonNetwork.IsMasterClient && playerPhotonView.IsMine)
                {
                    playerPhotonView.RPC(functionName_PunRPC_IsMasterClientCharacterInitialize, RpcTarget.All);
                }
            }
        }


        private void BallThrowTRInitialize()
        {
            if (!_isMasterClientCharacter)
            {
                if (ballThrowTR != null)
                {
                    Vector3 ballThrowTRposition = ballThrowTR.localPosition;

                    ballThrowTRposition.x = -ballThrowTRposition.x;

                    ballThrowTR.localPosition = ballThrowTRposition;
                }
            }
        }


        [PunRPC]
        public void PunRPC_IsMasterClientCharacterInitialize()
        {
            _isMasterClientCharacter = true;

            MineMasterClientCharacter?.Invoke(transform);
        }


        private void OnInputFinish(float horizontalInput, float verticalInput)
        {
            if (!_isThrowable)
            {
                return;
            }

            if (TourController.Instance.IsTurnOfMasterClient)
            {
                if (_isMasterClientCharacter)
                {
                    BallThrower(horizontalInput, verticalInput);
                }
            }
            else
            {
                if (!_isMasterClientCharacter)
                {
                    BallThrower(horizontalInput, verticalInput);
                }
            }
        }


        private void OnTurnChanged(bool isTurnOfMasterClient)
        {
            _isThrowable = true;
        }


        private void OnTourCountDownFinish(float currentCountDownSecond, float targetCountDownSecond)
        {
            if (!_isThrowable)
            {
                return;
            }

            if (TourController.Instance.IsTurnOfMasterClient)
            {
                if (_isMasterClientCharacter)
                {
                    BallThrowerRandom();
                }
            }
            else
            {
                if (!_isMasterClientCharacter)
                {
                    BallThrowerRandom();
                }
            }
        }


        private void OnBallButtonSelect(BallManager.BallType ballType, bool isMasterClientButton)
        {
            if (isMasterClientButton == _isMasterClientCharacter)
            {
                throwballType = ballType;
            }
        }


        private void BallThrowerRandom()
        {
            _isThrowable = false;

            if (characterData == null)
            {
                return;
            }

            BallBase ballBase = BallManager.Instance.GetBall(throwballType);
            throwballType = BallManager.BallType.BallClassic;

            if (ballBase == null)
            {
                return;
            }

            if (ballThrowTR != null)
            {
                ballBase.transform.position = ballThrowTR.position;
            }

            ballBase.gameObject.SetActive(true);
            ballBase.BallSetActive(true);
            ballBase.BallReset();

            float curve = UnityEngine.Random.Range(characterData.RangeCurve.min, characterData.RangeCurve.max);
            float duration = UnityEngine.Random.Range(characterData.RangeDuration.min, characterData.RangeDuration.max);

            Vector3 randomHitPosPos = Vector3.zero;

            randomHitPosPos.x = UnityEngine.Random.Range(characterData.RangeThrowPosX.min, characterData.RangeThrowPosX.max);
            randomHitPosPos.y = UnityEngine.Random.Range(characterData.RangeThrowPosY.min, characterData.RangeThrowPosY.max);

            if (_isMasterClientCharacter)
            {
                ballBase.ThrowInitialize(transform.position - randomHitPosPos, curve, duration);
            }
            else
            {
                ballBase.ThrowInitialize(transform.position + randomHitPosPos, curve, duration);
            }

            Throwed();
        }


        private void BallThrower(float horizontalInput, float verticalInput)
        {
            BallBase ballBase = BallManager.Instance.GetBall(throwballType);
            throwballType = BallManager.BallType.BallClassic;

            if (ballBase == null)
            {
                return;
            }

            _isThrowable = false;

            ballBase.GetBallPhotonView().RequestOwnership();

            ballBase.transform.position = ballThrowTR.position;

            ballBase.gameObject.SetActive(true);
            ballBase.BallSetActive(true);
            ballBase.BallReset();

            ballBase.ThrowInitialize(horizontalInput, verticalInput);

            Throwed();
        }


        public override void Damage(float damageValue)
        {
            if (playerPhotonView == null)
            {
                return;
            }

            playerPhotonView.RPC(functionName_PunRPC_Damage, RpcTarget.All, damageValue);

            if (characterData == null)
            {
                return;
            }

            CharacterDamaged?.Invoke(characterData.MaxHealth, _currentHealth, _isMasterClientCharacter);

            base.Damage(damageValue);
        }

        [PunRPC]
        public void PunRPC_Damage(float damageValue)
        {
            _currentHealth -= damageValue;

            if (_currentHealth <= 0)
            {
                Dead();
            }
        }


        private void Throwed()
        {
            if (playerPhotonView == null)
            {
                return;
            }

            playerPhotonView.RPC(functionName_PunRPC_Throwed, RpcTarget.All);
        }

        [PunRPC]
        public void PunRPC_Throwed()
        {
            CharacterThrowed?.Invoke();
        }


        public override void Dead()
        {
            CharacterDead?.Invoke(_isMasterClientCharacter);

            base.Dead();
        }


        public void CharacterInitialize()
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

            CharacterGenerated?.Invoke(playerPhotonView.ViewID);
        }
    }
}