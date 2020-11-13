using System.Collections;
using System.Collections.Generic;
using Definitions;
using Framewerk.UI.List;
using UnityEngine;

public class BladeDataProvider :  IListItemDataProvider
{

    public BladeDataDefinitionSO BladeData;

    public BladeDataProvider(BladeDataDefinitionSO bladeData)
    {
        BladeData = bladeData;
    }
}
