using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LevelEditor
{
    public class MultiDataView : View
    {
        public TMP_Text SelectectCount;
        public Button LayerUp;
        public Button LayerDown;
        public TMP_InputField LayerOffset;
        [FormerlySerializedAs("ObstaclesOffset")] public TMP_InputField ObstaclesOffsetX;
        [FormerlySerializedAs("RecalculateOffset")] public Button RecalculateOffsetX;
        public TMP_InputField ObstaclesOffsetY; 
        public Button RecalculateOffsetY;
        public Button FlipXPos;
        public Button FlipYPos;
        
        
        public TMP_InputField AddSpaceInput;
        public Button ExpandSpace;
        public Button ShrinkSpace;

        
        
        public Button MoveLayerOffset;
        public Button RemoveButton;
        public Button AddToLibraryButton;
    
    }
}