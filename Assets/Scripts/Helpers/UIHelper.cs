using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Scripts.Helpers
{
    public static class UIHelper
    {
        #region Button

        public static void AddButtonOnClick(this Button button, UnityAction onClickAction)
            => button.onClick.AddListener(onClickAction);

        public static void SetButtonOnClick(this Button button, UnityAction onClickAction)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onClickAction);
        }

        public static void RemoveButtonOnClick(this Button button)
            => button.onClick.RemoveAllListeners();

        #endregion

        #region Slider

        public static void AddSliderOnValueChanged(this Slider slider, UnityAction<float> onValueChangedAction)
            => slider.onValueChanged.AddListener(onValueChangedAction);

        public static void SetSliderOnValueChanged(this Slider slider, UnityAction<float> onValueChangedAction)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(onValueChangedAction);
        }

        public static void RemoveSliderOnValueChanged(this Slider slider)
            => slider.onValueChanged.RemoveAllListeners();

        #endregion

        #region TMP_InputField

        public static void AddTMPInputFieldOnValueChanged(this TMP_InputField inpuField, UnityAction<string> onValueChangedAction)
            => inpuField.onValueChanged.AddListener(onValueChangedAction);

        public static void SetTMPInputFieldOnValueChanged(this TMP_InputField inpuField, UnityAction<string> onValueChangedAction)
        {
            inpuField.onValueChanged.RemoveAllListeners();
            inpuField.onValueChanged.AddListener(onValueChangedAction);
        }

        public static void RemoveTMPInputFieldOnValueChanged(this TMP_InputField slider)
            => slider.onValueChanged.RemoveAllListeners();

        #endregion

        #region Toggle

        public static void AddToggleOnValueChanged(this Toggle toggle, UnityAction<bool> onValueChangedAction)
            => toggle.onValueChanged.AddListener(onValueChangedAction);

        public static void SetToggleOnValueChanged(this Toggle toggle, UnityAction<bool> onValueChangedAction)
        {
            toggle.onValueChanged.RemoveAllListeners();
            toggle.onValueChanged.AddListener(onValueChangedAction);
        }

        public static void RemoveToggleOnValueChanged(this Toggle toggle)
            => toggle.onValueChanged.RemoveAllListeners();

        #endregion
    }
}
