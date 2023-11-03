using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Actions;

[TakenInState<Phase>(Phase.Build)]
public record class EndBuildPhaseAction(Player Player) : PlayerAction(Player) { }