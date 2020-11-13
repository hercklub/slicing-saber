using System.Collections;
using System.Collections.Generic;
using Blades;
using Contexts;
using Definitions;
using Framewerk.UI.List;
using UnityEngine;

public class BladeListMediator : ListMediator<BladeListView, BladeDataProvider>
{
    [Inject] public IBladesDataDefinitions BladesDataDefinitions { get; set; }
    [Inject] public BladeSelectedSignal BladeSelectSignal { get; set; }
    
    [Inject] public RestartGameSignal RestartGameSignal { get; set; }
    public override void OnRegister()
    {
        base.OnRegister();
        AddButtonListener(View.RestartButton, RestartButtonHandler);
        var bladeDefinitions = BladesDataDefinitions.GetAllDefinitions();
        var items = new List<BladeDataProvider>();
        
        for (int i = 0; i < bladeDefinitions.Length; i++)
        {
            items.Add(new BladeDataProvider(bladeDefinitions[i]));
        }
        
        SetData(items);
        SelectItemAt(0);

    }
    
    private void RestartButtonHandler()
    {
        RestartGameSignal.Dispatch();
    }


    protected override void ListItemSelected(int index, BladeDataProvider dataProvider)
    {
        base.ListItemSelected(index, dataProvider);
        BladeSelectSignal.Dispatch(dataProvider.BladeData);
    }
}
