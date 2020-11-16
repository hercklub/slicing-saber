using Framewerk.UI.List;

namespace LevelEditor.UI.LoadPopup
{
    public class EditorLevelItemDataProvider : IListItemDataProvider
    {
        public LevelInfoData LevelInfoData;
        public EditorLevelItemDataProvider(LevelInfoData levelInfoData)
        {
            LevelInfoData = levelInfoData;
        }
    }
}