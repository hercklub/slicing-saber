using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LevelEditor
{
    public class AddModePopupView : View
    {
        public Button Normal;
        public Button Collectable;
        public Button Bullet;
        public Button SideBomb;
        public Button Target;
        public Button Obstacle;
        public Button Grass;

        
        public Color ToggleOnColor;
        public Color ToggleOffColor;
        
        public void SetEnabled(bool enabled)
        {
            this.gameObject.SetActive(enabled);
        }
    }
}