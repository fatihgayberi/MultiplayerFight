using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using WonnasmithTools;

namespace Wonnasmith
{
    [Serializable]
    public class BallManager : Singleton<BallManager>
    {
        public enum BallType
        {
            NONE,

            BallClassic,
            BallSpecial
        }

        [Serializable]
        public class BallPoolDatas
        {
            public BallType poolBallType;
            public GameObject poolBallPrefab;
            public int poolBallCount;
        }

        [SerializeField] private PhotonView ballManaherPhotonView;
        [SerializeField] private BallPoolDatas[] ballPoolDatasArray;

        Dictionary<BallType, List<BallBase>> _ballPoolDictionary = new Dictionary<BallType, List<BallBase>>();

        public List<BallBase> testLIST = new List<BallBase>();

        private void OnEnable()
        {
            GameManager.TourPrepare += OnTourPrepare;
        }
        private void OnDisable()
        {
            GameManager.TourPrepare -= OnTourPrepare;
        }


        private void OnTourPrepare()
        {
            BallPoolInitialize();
        }


        private void BallPoolInitialize()
        {
            if (ballPoolDatasArray == null)
            {
                return;
            }

            int balCount = 0;
            BallType ballType;
            string ballPrefabName;

            foreach (BallPoolDatas ballPoolData in ballPoolDatasArray)
            {
                if (ballPoolData == null)
                {
                    continue;
                }

                if (ballPoolData.poolBallPrefab == null)
                {
                    continue;
                }

                balCount = ballPoolData.poolBallCount;
                ballType = ballPoolData.poolBallType;
                ballPrefabName = ballPoolData.poolBallPrefab.name;

                if (!_ballPoolDictionary.ContainsKey(ballType))
                {
                    _ballPoolDictionary.Add(ballType, new List<BallBase>());
                }

                for (int i = 0; i < balCount; i++)
                {
                    GameObject createdBall = PhotonNetwork.Instantiate(ballPrefabName, Vector3.zero, Quaternion.identity);

                    if (createdBall != null)
                    {
                        BallBase ballBase = createdBall.GetComponent<BallBase>();

                        _ballPoolDictionary[ballType].Add(ballBase);

                        testLIST.Add(ballBase);
                        ballBase.BallSetActive(false);
                    }
                }
            }
        }


        public BallBase GetBall(BallType ballType)
        {
            if (_ballPoolDictionary == null)
            {
                Debug.Log("return;::", gameObject);

                return null;
            }

            if (!_ballPoolDictionary.ContainsKey(ballType))
            {
                Debug.Log("return;::", gameObject);

                return null;
            }

            foreach (BallBase ballBase in _ballPoolDictionary[ballType])
            {
                if (ballBase == null)
                {
                    continue;
                }

                if (ballBase.gameObject.activeSelf)
                {
                    continue;
                }

                return ballBase;
            }

            Debug.Log("return;::", gameObject);
            return null;
        }
    }
}