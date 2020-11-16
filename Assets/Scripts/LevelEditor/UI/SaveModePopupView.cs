using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class SaveModePopupView : View
    {
        public Button NewButton;
        public Button SaveAsButton;
        public Button LoadButton;
        public Button LoadBundleButton;

        public void SetEnabled(bool isEnabled)
        {
            gameObject.SetActive(isEnabled);
        }

    }
}