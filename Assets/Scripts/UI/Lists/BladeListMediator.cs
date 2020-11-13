using System.Collections;
using System.Collections.Generic;
using Blades;
using Definitions;
using Framewerk.UI.List;
using UnityEngine;

public class BladeListMediator : ListMediator<BladeListView, BladeDataProvider>
{
    [Inject] public IBladesDataDefinitions BladesDataDefinitions { get; set; }
    [Inject] public BladeSelectSignal BladeSelectSignal { get; set; }
    public override void OnRegister()
    {
        base.OnRegister();
        var bladeDefinitions = BladesDataDefinitions.GetAllDefinitions();
        var items = new List<BladeDataProvider>();
        
        for (int i = 0; i < bladeDefinitions.Length; i++)
        {
            items.Add(new BladeDataProvider(bladeDefinitions[i]));
        }
        
        SetData(items);
    }

    protected override void ListItemSelected(int index, BladeDataProvider dataProvider)
    {
        base.ListItemSelected(index, dataProvider);
        BladeSelectSignal.Dispatch(dataProvider.BladeData);
    }
}
