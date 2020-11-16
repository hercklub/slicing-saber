using strange.extensions.mediation.impl;
using TMPro;

namespace LevelEditor
{
    public class LevelInfoDataView : View
    {
        public TMP_InputField LevelNameInput;
        public TMP_InputField DisplayNameInput;
        public TMP_InputField BpmInput;
        public TMP_InputField EventsPerMinuteInput;
        public TMP_InputField DurationInput;
        public TMP_Dropdown DiffcultyDropdown;
        public TMP_Dropdown LevelTypeDropdown;
    }
}