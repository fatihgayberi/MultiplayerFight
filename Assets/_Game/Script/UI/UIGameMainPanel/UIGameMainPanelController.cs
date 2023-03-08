using UnityEngine;

namespace Wonnasmith
{
    public class UIGameMainPanelController : MonoBehaviour
    {
        public delegate void UIGameMainPanelControllerBack2LobyButtonClick();
        public static event /*UIGameMainPanelController.*/UIGameMainPanelControllerBack2LobyButtonClick Back2LobyButtonClick;

        public void _BUTTON_Back2Loby()
        {
            Back2LobyButtonClick?.Invoke();
        }
    }
}