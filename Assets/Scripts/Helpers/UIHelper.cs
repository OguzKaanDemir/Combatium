using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Scripts.Helpers
{
    public static class UIHelper
    {
        public static void AddButtonOnClick(this Button button, UnityAction onClickAction)
            => button.onClick.AddListener(onClickAction);

        public static void SetButtonOnClick(this Button button, UnityAction onClickAction)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClickAction);
        }

        public static void RemoveButtonOnClick(this Button button)
            => button.onClick.RemoveAllListeners();
    }
}
