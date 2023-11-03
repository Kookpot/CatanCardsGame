using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class ResourceOkToBuild
{
    internal ResourceType ResourceType { get; set; }
    internal bool IsOk { get; set; }
}