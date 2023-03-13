using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wonnasmith
{
    [Serializable]
    public class AIController : CharacterBase
    {
        [Space, Space]
        [WonnasmithEditor.HelpBox("AI hedefi periodik olarak vurur.\nHer totalThrowPeriod atis sayisinda minHitPeriod kez kesinlikle hedefi vurur.", WonnasmithEditor.HelpBoxMessageType.Info)]
        [Space, Space]


        [SerializeField, Min(0)] private float ballThrowWaitTime;
        [SerializeField, Min(0)] private int totalThrowPeriod;
        [SerializeField, Min(0)] private int minHitPeriod;


        public static event CharacterBaseCharacterDamaged CharacterDamaged;
        public static event CharacterBaseCharacterDead CharacterDead;
        public static event CharacterBaseCharacterThrowed CharacterThrowed;

        private Transform _masterClientCharactreClientTR;
        private WaitForSeconds _ballThrowWaitForSeconds;
        private Coroutine _ballThrowCoroutine;

        private List<bool> _randomHitList;

        public override void OnEnable()
        {
            base.OnEnable();

            TourController.TurnChanged += OnTurnChanged;

            PlayerController.MineMasterClientCharacter += OnMineMasterClientCharacter;
        }
        public override void OnDisable()
        {
            base.OnDisable();

            TourController.TurnChanged -= OnTurnChanged;

            PlayerController.MineMasterClientCharacter -= OnMineMasterClientCharacter;
        }


        private void Start()
        {
            RandomHitListInitialize();
        }


        private bool GetIsHit()
        {
            if (_randomHitList == null)
            {
                _randomHitList = new List<bool>();
            }

            if (_randomHitList.Count == 0)
            {
                RandomHitListInitialize();
            }

            if (_randomHitList.Count == 0)
            {
                // eğer periyod ataması yapılmadıysa

                return false;
            }

            bool isHit = _randomHitList[0];
            _randomHitList.RemoveAt(0);

            return isHit;
        }


        private void RandomHitListInitialize()
        {
            if (_randomHitList == null)
            {
                _randomHitList = new List<bool>();
            }

            if (totalThrowPeriod <= 0)
            {
                return;
            }

            for (int i = 0; i < minHitPeriod; i++)
            {
                _randomHitList.Add(true);
            }

            for (int i = 0; i < totalThrowPeriod - minHitPeriod; i++)
            {
                _randomHitList.Add(false);
            }

            _randomHitList.Shuffle<bool>();
        }


        private void OnMineMasterClientCharacter(Transform masterClientCharactreClientTR)
        {
            _masterClientCharactreClientTR = masterClientCharactreClientTR;
        }


        private void OnTurnChanged(bool isTurnOfMasterClient)
        {
            if (isTurnOfMasterClient)
            {
                return;
            }

            BallBase ballBase = BallManager.Instance.GetBall(BallManager.BallType.BallClassic);

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

            if (_ballThrowCoroutine != null)
            {
                StopCoroutine(_ballThrowCoroutine);
            }

            _ballThrowCoroutine = StartCoroutine(ThrowIEnumerator(ballBase));
        }


        private IEnumerator ThrowIEnumerator(BallBase ballBase)
        {
            if (ballBase == null)
            {
                yield break;
            }

            if (_ballThrowWaitForSeconds == null)
            {
                _ballThrowWaitForSeconds = new WaitForSeconds(ballThrowWaitTime);
            }

            yield return _ballThrowWaitForSeconds;

            if (characterData == null)
            {
                yield break;
            }

            float curve = UnityEngine.Random.Range(characterData.RangeCurve.min, characterData.RangeCurve.max);
            float duration = UnityEngine.Random.Range(characterData.RangeDuration.min, characterData.RangeDuration.max);

            if (GetIsHit())
            {
                ballBase.ThrowInitialize(_masterClientCharactreClientTR.position, curve, duration);
            }
            else
            {
                Vector3 randomHitPosPos = Vector3.zero;

                randomHitPosPos.x = UnityEngine.Random.Range(characterData.RangeThrowPosX.min, characterData.RangeThrowPosX.max);
                randomHitPosPos.y = UnityEngine.Random.Range(characterData.RangeThrowPosY.min, characterData.RangeThrowPosY.max);

                ballBase.ThrowInitialize(transform.position + randomHitPosPos, curve, duration);
            }

            CharacterThrowed?.Invoke();
        }


        public override void Damage(float damageValue)
        {
            _currentHealth -= damageValue;

            if (characterData == null)
            {
                return;
            }

            CharacterDamaged?.Invoke(characterData.MaxHealth, _currentHealth, false);

            base.Damage(damageValue);

            if (_currentHealth <= 0)
            {
                Dead();
            }
        }


        public override void Dead()
        {
            CharacterDead?.Invoke(false);

            base.Dead();
        }


        public void AiInitialize()
        {
            transform.position = new Vector3(-8, 0, 0);
        }
    }
}