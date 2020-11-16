using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LevelEditor
{
    public class AddtoLibraryPopupView : View
    {
        public TMP_InputField BundleName;
        public TMP_InputField DirectoryName;
        public Button SaveButton;
        public Button CloseButton;
    }
}