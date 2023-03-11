using System;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Wonnasmith
{
    public abstract class BallBase : MonoBehaviour
    {
        [SerializeField] private BallData ballData;

        [SerializeField] private Rigidbody2D ballRigidbody2D;
        [SerializeField] private PhotonView ballPhotonView;


        public abstract void ThrowStarted();
        public abstract void ThrowFinished();


        public void Throw(float horizontalInput, float verticalInput)
        {
            if (ballData == null)
            {
                return;
            }

            if (ballRigidbody2D == null)
            {
                return;
            }

            ThrowStarted();

            ballRigidbody2D.simulated = true;

            ballRigidbody2D.velocity = new Vector2(-horizontalInput, -verticalInput) * ballData.Power;
        }


        public void BallSetActive(bool isActive)
        {
            if (ballPhotonView == null)
            {
                return;
            }

            ballPhotonView.RPC("RPC_BallSetActive", RpcTarget.All, isActive);
        }


        [PunRPC]
        protected virtual void RPC_BallSetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!GameManager.Instance.GameIsPlaying())
            {
                return;
            }


            if (other == null)
            {
                return;
            }


            if (LayerManager.Instance.IsLayerEquals(other.gameObject.layer, LayerManager.LayerType.Character_LAYER))
            {
                if (ballData == null)
                {
                    return;
                }

                IDamageable iDamageable = other.GetComponent<IDamageable>();

                if (iDamageable != null)
                {
                    Debug.Log("other:::", other.gameObject);

                    iDamageable.Damage(ballData.Damage);
                }
            }
        }
    }
}