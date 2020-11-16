using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class EnemyDataView : View
    {
        public TMP_Text Index;
        [Header("Start")]
        public Button LayerUp;
        public Button LayerDown;
        public TMP_InputField StartPositionInput;
        public TMP_InputField StartHeightInput;
        public TMP_Dropdown StartTypeDropdown;
        
        [Header("Middle")]
        public TMP_InputField MidPositionInput;
        public TMP_InputField MidHeightInput;
        public TMP_Dropdown MidRotationDropDown;

        [Header("End")]
        public TMP_Dropdown EndPosDropwDown;
        public TMP_Dropdown EndRotationDropDown;

        [Header("CutDit")] 
        public Button LeftButton;
        public Button RightButton;
        public Button UpButton;
        public Button DownButton;
        
        
        public GameObject LeftCheckmark;
        public GameObject RightChekmark;
        public GameObject UpCheckmark;
        public GameObject DownCheckmark;
        
        public Button RemoveButton;


    }
}