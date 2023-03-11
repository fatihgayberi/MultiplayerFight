using UnityEngine;

namespace Wonnasmith
{
    public class UIGameMainPanelController : MonoBehaviour
    {
        public static event LobyManager.LobyManagerBackToLoby BackToLoby;

        public void _BUTTON_Back2Loby()
        {
            BackToLoby?.Invoke();
        }
    }
}