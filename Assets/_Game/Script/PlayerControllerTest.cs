using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Wonnasmith
{
    public class PlayerControllerTest : MonoBehaviour
    {
        // [SerializeField] private PhotonView playerPhotonView;
        // [SerializeField] private MeshRenderer meshRenderer;

        // private void Update()
        // {
        //     if (!playerPhotonView.IsMine)
        //     {
        //         return;
        //     }


        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         playerPhotonView.RPC("ColorChange", RpcTarget.All);
        //         // meshRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        //     }
        // }


        // [PunRPC]
        // private void ColorChange()
        // {
        //     meshRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        // }
    }
}
