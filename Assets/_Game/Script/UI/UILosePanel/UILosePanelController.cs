using UnityEngine;

namespace Wonnasmith
{
    public class UILosePanelController : MonoBehaviour
    {
        [SerializeField] private GameObject losePanel;

        private void OnEnable()
        {
            GameManager.LevelLose += OnLevelLose;
        }
        private void OnDisable()
        {
            GameManager.LevelLose -= OnLevelLose;
        }


        private void OnLevelLose()
        {
            losePanel.SetActiveNullCheck(true);
        }
    }
}