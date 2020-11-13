using strange.extensions.mediation.impl;
using UnityEngine.UI;

namespace UI
{
    public class ScoreView : View
    {
        public Text SlicedScore;
        public Text MissedScore;

        public void SetScore(int sliced, int missed)
        {
            MissedScore.text = missed.ToString();
            SlicedScore.text = sliced.ToString();
        }
    }
}