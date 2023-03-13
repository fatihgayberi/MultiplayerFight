using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Wonnasmith
{
    public class PlayerMaterialController : MonoBehaviour
    {
        [SerializeField] private PhotonView playerPhotonView;

        private SpriteRenderer _spriteRenderer;

        private MaterialPropertyBlock _materialPropertyBlock;

        private const string strOutlineActive = "_isOutlineActive";
        private const string strSpecialActive = "_isSpecialActive";

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

            float outlineFlag = isOutlineActive ? 1 : 0;

            _materialPropertyBlock.SetFloat(strOutlineActive, outlineFlag);

            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }


        public void SpecialActivator(bool isSpecialActive)
        {
            if (playerPhotonView != null)
            {
                playerPhotonView.RPC("PunRPC_SpecialActivator", RpcTarget.All, isSpecialActive);
            }
            else
            {
                PunRPC_SpecialActivator(isSpecialActive);
            }
        }



        [PunRPC]
        public void PunRPC_SpecialActivator(bool isSpecialActive)
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (_materialPropertyBlock == null)
            {
                _materialPropertyBlock = new MaterialPropertyBlock();
            }

            float specialFlag = isSpecialActive ? 1 : 0;

            _materialPropertyBlock.SetFloat(strSpecialActive, specialFlag);

            _spriteRenderer.SetPropertyBlock(_materialPropertyBlock);
        }
    }
}