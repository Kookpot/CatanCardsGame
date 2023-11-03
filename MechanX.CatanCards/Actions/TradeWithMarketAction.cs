using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Actions;

public record class TradeWithMarketAction(Player Player, ResourceType PlayerResource, ResourceType MarketResource) : TradeAction(Player) { }