using UnityEngine;

namespace Wonnasmith
{
    public class UIWinPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject winPanel;

        private void OnEnable()
        {
            GameManager.LevelWin += OnLevelWin;
        }
        private void OnDisable()
        {
            GameManager.LevelWin -= OnLevelWin;
        }


        private void OnLevelWin()
        {
            winPanel.SetActiveNullCheck(true);
        }
    }
}