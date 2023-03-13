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
            BallBig,
            BallSpecial
        }

        [Serializable]
        public class BallPoolDatas
        {
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

            BallBase.BallGenerated += OnBallGenerated;
        }
        private void OnDisable()
        {
            GameManager.TourPrepare -= OnTourPrepare;

            BallBase.BallGenerated -= OnBallGenerated;
        }


        private void OnTourPrepare()
        {
            BallPoolInitialize();
        }


        private void OnBallGenerated(BallBase ballBase, BallType ballType)
        {
            if (ballBase == null)
            {
                return;
            }

            PoolAdd(ballBase, ballType);
        }


        private void BallPoolInitialize()
        {
            if (ballPoolDatasArray == null)
            {
                return;
            }

            int balCount = 0;
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
                ballPrefabName = ballPoolData.poolBallPrefab.name;

                for (int i = 0; i < balCount; i++)
                {
                    GameObject createdBall = PhotonNetwork.Instantiate(ballPrefabName, Vector3.zero, Quaternion.identity);
                }
            }
        }


        private void PoolAdd(BallBase ballBase, BallType ballType)
        {
            if (ballBase == null)
            {
                return;
            }

            if (!_ballPoolDictionary.ContainsKey(ballType))
            {
                _ballPoolDictionary.Add(ballType, new List<BallBase>());
            }

            if (_ballPoolDictionary[ballType].Contains(ballBase))
            {
                return;
            }

            _ballPoolDictionary[ballType].Add(ballBase);
            testLIST.Add(ballBase);
            ballBase.BallSetActive(false);
        }


        public BallBase GetBall(BallType ballType)
        {
            if (_ballPoolDictionary == null)
            {
                return null;
            }

            if (!_ballPoolDictionary.ContainsKey(ballType))
            {
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

            return null;
        }
    }
}