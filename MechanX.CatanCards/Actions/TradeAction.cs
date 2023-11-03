using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Actions;

[TakenInState<Phase>(Phase.Trade)]
public record class TradeAction(Player Player) : PlayerAction(Player) { }
