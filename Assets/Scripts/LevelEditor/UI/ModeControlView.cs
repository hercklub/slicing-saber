using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class ModeControlView : View
    {
        public Button AddButton;
        public Button EditButton;
        public Button SaveButton;
        
        public Button PathModeButton;
        public Button ObstaclesVisible;
        
        public Color ToggleOnColor;
        public Color ToggleOffColor;
        public Slider LevelSlider;
        public TMP_Text SliderVal;
        
        public TMP_InputField LayerInput;
        public Button GotoLayer;
        
    }
}