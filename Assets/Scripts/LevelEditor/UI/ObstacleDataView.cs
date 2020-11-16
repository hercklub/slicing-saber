using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class ObstacleDataView : View
    {
        public TMP_Text Index;
        [Header("Start")]
        public Button LayerUp;
        public Button LayerDown;
        
        public TMP_InputField ScaleX;
        public TMP_InputField ScaleY;
        
        public TMP_InputField StartPositionInput;
        public TMP_InputField StartHeightInput;

        [Header("Middle")]
        public TMP_InputField MidPositionInput;
        public TMP_InputField MidHeightInput;

        [Header("End")]
        public TMP_Dropdown EndPosDropwDown;
        public TMP_InputField EndRotationDropDown;

        
        public TMP_Dropdown PivotDropDown;
        public Image IsPortalImage;
        public Button IsPortal;
        public Button RemoveButton;
    }
}