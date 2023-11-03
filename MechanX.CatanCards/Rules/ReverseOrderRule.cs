using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class ReverseOrderRule : ActionRule<CatanCardsGameState, ReverseOrderAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, ReverseOrderAction reverseOrderAction)
    {
        if (reverseOrderAction.Yes)
        {
            game.State.ReverseOrder = !game.State.ReverseOrder;
            game.State.PlayerOrder.Reverse();
        }
        game.State.FSM.PerformTransition(PhaseTransition.OrderReversed);
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, ReverseOrderAction reverseOrderAction)
    {
        var result = Result.Ok();
        if (game.Players.Count == 2)
        {
            result.WithReason(new Reason { Message = "Reverse order has no meaning when playing with 2 players." });
        }
        return result;
    }
}