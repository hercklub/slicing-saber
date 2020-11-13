using System.Collections;
using System.Collections.Generic;
using Framewerk.UI.List;
using UnityEngine;

public class BladeListItemMediator : ListItemMediator<BladeListItemView, BladeDataProvider>
{
    public override void SetData(BladeDataProvider dataProvider, int index)
    {
        base.SetData(dataProvider, index);
        View.LabelText.text = dataProvider.BladeData.Id.ToString();
        View.SelectButton.image.color = Color.gray;
    }
    
    public override void SetSelected(bool selected)
    {
        base.SetSelected(selected);
        View.SelectButton.image.color = selected ? Color.red : Color.grey;
    }
    
}
