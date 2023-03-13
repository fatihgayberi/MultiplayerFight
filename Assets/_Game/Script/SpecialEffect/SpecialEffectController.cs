using UnityEngine;

namespace Wonnasmith
{
    public class SpecialEffectController : MonoBehaviour
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

            _materialPropertyBlock.SetFloat(strOutlineActive, otulineFlag);

            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}