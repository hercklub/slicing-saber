using Common;
namespace Definitions
{

    public enum BladeDataType
    {
        RedBlade,
        BlueBlade,
        GreenBlade,
        YellowBlade
    }

    public interface IBladesDataDefinitions
    {
        BladeDataDefinitionSO GetDefinitionById(BladeDataType id);
        BladeDataDefinitionSO[] GetAllDefinitions();
    }

    public class BladesDataDefinitions : ScriptableObjectDefintions<BladeDataDefinitionSO, BladeDataType>, IBladesDataDefinitions
    {
        protected override string Path => ResourcePath.DEFINITIONS_BLADES;
    }
}