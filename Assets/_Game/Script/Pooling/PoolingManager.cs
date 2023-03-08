using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WonnasmithTools;

namespace Wonnasmith
{
    [Serializable]
    public class PoolingManager : Singleton<PoolingManager>
    {
        public enum PoolInObjectType
        {
            NONE,

        }

        /// <summary>
        /// Pool yapılacak objenin datalarını saklar
        /// </summary>
        [Serializable]
        public class PoolingObject
        {
            // pool yapılacak obje
            public GameObject poolObjectPrefab;
            // poolObjesinin tipi
            public PoolInObjectType poolInObjectType;
            // pool yapılacak objenin sayisi
            public int objectCount;
        }

        [SerializeField]
        private List<PoolingObject> poolingObjectList = new List<PoolingObject>();

        private List<List<GameObject>> createdPoolObjectList = new List<List<GameObject>>();

        private const string strPoolObjectNum = "PoolObjectNum_";

        private GameObject allPoolObjectParent;

        private void Awake()
        {
            PoolObjectGenerator();
        }


        /// <summary>
        /// Pool objelerini olusturur
        /// </summary>
        private void PoolObjectGenerator()
        {
            allPoolObjectParent = new GameObject("PoolingObjectsParent");

            DontDestroyOnLoad(allPoolObjectParent);

            int poolingObjectListCount = poolingObjectList.Count;

            for (int i = 0; i < poolingObjectListCount; i++)
            {
                PoolListInObjectGenerator(poolingObjectList[i], i);
            }
        }


        /// <summary>
        /// listeden pool objelerini ceker
        /// </summary>
        /// <param name="poolingObject"></param>
        /// <param name="idx"></param>
        private void PoolListInObjectGenerator(PoolingObject poolingObject, int idx)
        {
            int nCount = poolingObject.objectCount;

            string strObjectName = strPoolObjectNum + poolingObject.poolInObjectType.ToString() + idx;//poolingObject.nObjectNum;

            GameObject prefabObject = poolingObject.poolObjectPrefab;
            GameObject parentPoolObject = new GameObject(strObjectName);

            DontDestroyOnLoad(parentPoolObject);

            GameObject generatedObject;
            List<GameObject> generatedObjectList = new List<GameObject>();

            Transform trParentPoolObject = parentPoolObject.transform;

            trParentPoolObject.SetParent(allPoolObjectParent.transform.transform);

            for (int i = 0; i < nCount; i++)
            {
                generatedObject = Instantiate(prefabObject, Vector3.zero, Quaternion.identity, trParentPoolObject);

                if (generatedObject != null)
                {
                    DontDestroyOnLoad(generatedObject);
                    generatedObject.SetActive(false);
                    generatedObjectList.Add(generatedObject);
                }
            }

            createdPoolObjectList.Add(generatedObjectList);
        }


        public GameObject GetPoolObject(PoolInObjectType poolInObjectType, Vector3 position, Quaternion quaternion)
        {
            GameObject poolObject = GetPoolObject(poolInObjectType);

            if (poolObject == null)
            {
                return null;
            }
            else
            {
                poolObject.transform.position = position;
                poolObject.transform.rotation = quaternion;

                return poolObject;
            }
        }

        public GameObject GetPoolObject(PoolInObjectType poolInObjectType, Vector3 position)
        {
            GameObject poolObject = GetPoolObject(poolInObjectType);

            if (poolObject == null)
            {
                return null;
            }
            else
            {
                poolObject.transform.position = position;

                return poolObject;
            }
        }


        /// <summary>
        /// pool objelerini return eder
        /// </summary>
        /// <param name="poolObjectListIdxNum"> pool' dan cekilmesini istedigin objenin indexini sorar </param>
        /// <returns></returns>
        public GameObject GetPoolObject(PoolInObjectType poolInObjectType)
        {
            int poolObjectListIdxNum = PoolInObjectFinder(poolInObjectType);
            int createdPoolObjectListCount = createdPoolObjectList.Count;
            GameObject usingPoolObject = null;

            if (poolObjectListIdxNum < createdPoolObjectListCount)
            {
                if (0 < createdPoolObjectList[poolObjectListIdxNum].Count)
                {
                    usingPoolObject = createdPoolObjectList[poolObjectListIdxNum][0];
                    createdPoolObjectList[poolObjectListIdxNum].RemoveAt(0);
                    createdPoolObjectList[poolObjectListIdxNum].Add(usingPoolObject);
                }
            }

            if (usingPoolObject != null && !usingPoolObject.activeSelf)
            {
                return usingPoolObject;
            }
            else
            {
                usingPoolObject = Instantiate(poolingObjectList[poolObjectListIdxNum].poolObjectPrefab, Vector3.zero, Quaternion.identity);
                DontDestroyOnLoad(usingPoolObject);

                usingPoolObject.SetActive(false);
                createdPoolObjectList[poolObjectListIdxNum].Add(usingPoolObject);

                return usingPoolObject;
            }
        }

        private int PoolInObjectFinder(PoolInObjectType poolInObjectType)
        {
            int createdPoolObjectListCount = createdPoolObjectList.Count;

            for (int i = 0; i < createdPoolObjectListCount; i++)
            {
                if (poolingObjectList[i].poolInObjectType.Equals(poolInObjectType))
                {
                    return i;
                }
            }

            return -1;
        }


        /// <summary>
        /// Kullanılan pool objelerini tekrardan pool a kazandirir
        /// </summary>
        public void RelasePoolObjects()
        {
            int createdPoolObjectListCount = createdPoolObjectList.Count;
            int createdPoolObjectListInListCount;

            for (int i = 0; i < createdPoolObjectListCount; i++)
            {
                createdPoolObjectListInListCount = createdPoolObjectList[i].Count;

                for (int j = 0; j < createdPoolObjectListInListCount; j++)
                {
                    if (createdPoolObjectList[i][j] == null)
                    {
                        createdPoolObjectList[i].RemoveAt(j);
                    }
                    else
                    {
                        createdPoolObjectList[i][j].SetActive(false);
                    }
                }
            }
        }
    }
}