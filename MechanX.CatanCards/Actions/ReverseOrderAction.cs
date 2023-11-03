using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Actions;

[TakenInState<Phase>(Phase.CanReverseOrder)]
public record class ReverseOrderAction(Player Player, bool Yes) : PlayerAction(Player) { }