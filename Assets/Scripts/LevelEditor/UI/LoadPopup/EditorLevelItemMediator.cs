using Framewerk.UI.List;

namespace LevelEditor.UI.LoadPopup
{
    public class EditorLevelItemMediator : ListItemMediator<EditorLevelItemView, EditorLevelItemDataProvider>
    {
        public override void SetData(EditorLevelItemDataProvider dataProvider, int index)
        {
            base.SetData(dataProvider, index);
            View.LevelName.text = dataProvider.LevelInfoData.LevelName;
        }
    }
}