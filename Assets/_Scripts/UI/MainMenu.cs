using Archer.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class MainMenu : UIBase
    {
        [SerializeField] private GameObject scoreboardUI;
        [SerializeField] private Button newGameButton;

        [SerializeField] private Button loadGameButton;

        [SerializeField] private UIAnimation[] _uiAnimations;

        // Start is called before the first frame update
        void Start()
        {
            newGameButton.onClick.AddListener(OnNewGameClick);
            loadGameButton.onClick.AddListener(OnLoadGameClick);
        }

        void OnDestroy()
        {
            newGameButton.onClick.RemoveListener(OnNewGameClick);
            loadGameButton.onClick.RemoveListener(OnLoadGameClick);
        }

        // This method is called to show the main menu UI
        public override void Show()
        {
            // Implement logic to show the main menu UI
            gameObject.SetActive(true);
            // Play animations for each button
            foreach (var buttonAnimation in _uiAnimations)
            {
                buttonAnimation.PlayAnimation();
            }
        }

        // This method is called to hide the main menu UI
        public override void Hide()
        {
            // Implement logic to hide the main menu UI
            gameObject.SetActive(false);
        }

        public void OnNewGameClick()
        {
            Debug.Log("OnNewGameClick");
            GameManager.Instance.StartNewGame();
        }

        public void OnLoadGameClick()
        {
            Debug.Log("OnLoadGameClick");
        }
    }
}