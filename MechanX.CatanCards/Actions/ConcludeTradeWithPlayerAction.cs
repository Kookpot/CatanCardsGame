using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Actions;

[TakenInState<Phase>(Phase.NeedsConcludeTrade)]
public record class ConcludeTradeWithPlayerAction(Player Player, ResourceType PlayerResource) : PlayerAction(Player) { }