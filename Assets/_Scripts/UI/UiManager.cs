using UnityEngine;
using UnityEngine.UI;

namespace Archer.UI
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;

    

        // Start is called before the first frame update
        void Start()
        {
            mainMenu.Show();
        }

        // Update is called once per frame
        void Update()
        {
        }
        
        public void ShowMainMenu()
        {
            mainMenu.Show();
        }

        public void HideMainMenu()
        {
            mainMenu.Hide();
        }
    }
}