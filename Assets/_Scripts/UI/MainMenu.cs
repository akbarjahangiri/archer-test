using Archer.Managers;
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

        public override void Show()
        {
            gameObject.SetActive(true);
            // Play animations for each button
            foreach (var buttonAnimation in _uiAnimations)
            {
                buttonAnimation.PlayAnimation();
            }
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }

        public void OnNewGameClick()
        {
            GameManager.Instance.StartNewGame();
        }

        public void OnLoadGameClick()
        {
            GameManager.Instance.LoadGame();

            Debug.Log("OnLoadGameClick");
        }
    }
}