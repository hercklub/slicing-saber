using Framewerk.UI.List;
using UnityEngine;
using UnityEngine.Serialization;

namespace Definitions
{
    [CreateAssetMenu(menuName = "Custom/Blade Color Definition")]
    public class BladeDataDefinitionSO : DefinitionSO<BladeDataType>, IListItemDataProvider
    {
        [SerializeField]
        [ColorUsage(true, true)]
        private Color _bladeColor;
        
        public Color BladeColor => _bladeColor;

    }
    
}