using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Actions;

public record class TradeWithStackAction(Player Player, ResourceType PlayerResource) : TradeAction(Player) { }