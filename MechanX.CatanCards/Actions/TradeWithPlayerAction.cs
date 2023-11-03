namespace MechanX.CatanCards.Actions;

public record class TradeWithPlayerAction(Player Player, Player OtherPlayer) : TradeAction(Player) { }