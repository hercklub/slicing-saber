using Framewerk.UI.List;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevelEditor
{
    public class BundleListMenuView : ListView
    {
        public Button LoadButton;
        public Button RemoveButton;
        public Button CloseButton;

        public GameObject StatsContainer;
        public TMP_Text BundleNameText;
        public TMP_Text EnemyCountText;
        public TMP_Text GrassCountText;
        public TMP_Text FishCountText;
    } 
}