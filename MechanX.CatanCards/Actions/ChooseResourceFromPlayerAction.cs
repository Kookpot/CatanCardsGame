using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Actions;

[TakenInState<Phase>(Phase.NeedsSelectionResource)]
public record class ChooseResourceFromPlayerAction(Player Player, ResourceType PlayerResource) : PlayerAction(Player) { }