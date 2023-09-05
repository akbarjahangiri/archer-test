using UnityEngine;

namespace Archer.UI
{
    public abstract class UIBase : MonoBehaviour
    {
        // This method should be called to show the UI element
        public abstract void Show();

        // This method should be called to hide the UI element
        public abstract void Hide();
    }
}