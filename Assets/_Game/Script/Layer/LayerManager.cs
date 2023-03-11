using UnityEngine;
using System;
using WonnasmithTools;

namespace Wonnasmith
{
    public class LayerManager : Singleton<LayerManager>
    {
        public enum LayerType
        {
            NONE,

            Character_LAYER,
            Ball_LAYER
        }

        [Serializable]
        private class LayerDatas
        {
            public LayerType layerType;
            public LayerMask layerMask;
        }

        [SerializeField] private LayerDatas[] layerDatasArray;


        public bool IsLayerEquals(int objectLayer, LayerType layerType)
        {
            LayerDatas layerDatas = GetLayerDatas(layerType);

            if (layerDatas == null)
            {
                return false;
            }

            int layerNum = LayerMaskExtensionMethods.LayerMask2Int(layerDatas.layerMask);

            return objectLayer.Equals(layerNum);
        }


        private LayerDatas GetLayerDatas(LayerType layerType)
        {
            if (layerType == LayerType.NONE)
            {
                return null;
            }

            if (layerDatasArray == null)
            {
                return null;
            }

            int layerDatasArrayLength = layerDatasArray.Length;

            for (int i = 0; i < layerDatasArrayLength; i++)
            {
                if (layerDatasArray[i] != null)
                {
                    if (layerDatasArray[i].layerType == layerType)
                    {
                        return layerDatasArray[i];
                    }
                }
            }

            return null;
        }
    }
}