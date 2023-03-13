using UnityEngine;

namespace Wonnasmith
{
    public class UIWinPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject winPanel;

        private void OnEnable()
        {
            GameManager.TourWin += OnTourWin;
        }
        private void OnDisable()
        {
            GameManager.TourWin -= OnTourWin;
        }


        private void OnTourWin()
        {
            winPanel.SetActiveNullCheck(true);
        }
    }
}