using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Actions;

[TakenInState<Phase>(Phase.Build), TakenInState<Phase>(Phase.Trade)]
public record class UseGuildAction(Player Player, Player OtherPlayer) : PlayerAction(Player) { }