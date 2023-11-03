using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Actions;

[TakenInState<Phase>(Phase.Hijack)]
public record class DropResourcesAction(Player Player, List<ResourceType> ResourceToDrop) : PlayerAction(Player) { }