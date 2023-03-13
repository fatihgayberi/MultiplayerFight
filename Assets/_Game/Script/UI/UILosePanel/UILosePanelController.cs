using UnityEngine;

namespace Wonnasmith
{
    public class UILosePanelController : MonoBehaviour
    {
        [SerializeField] private GameObject losePanel;

        private void OnEnable()
        {
            GameManager.TourLose += OnTourLose;
        }
        private void OnDisable()
        {
            GameManager.TourLose -= OnTourLose;
        }


        private void OnTourLose()
        {
            losePanel.SetActiveNullCheck(true);
        }
    }
}