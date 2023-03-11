using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wonnasmith
{
    public class OutlineController : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;

        private MaterialPropertyBlock _materialPropertyBlock;

        private const string strOutlineActive = "_isOutlineActive";

        public void OutlineActivator(bool isOutlineActive)
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (_materialPropertyBlock == null)
            {
                _materialPropertyBlock = new MaterialPropertyBlock();
            }

            float otulineFlag = isOutlineActive ? 1 : 0;

            Debug.Log("OutlineActivator::" + otulineFlag, gameObject);

            _materialPropertyBlock.SetFloat(strOutlineActive, otulineFlag);

            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}