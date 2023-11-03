namespace MechanX.CatanCards.Actions;

public record class TradeWithPlayerUsingLibraryAction(Player Player, Player OtherPlayer) : TradeAction(Player) { }