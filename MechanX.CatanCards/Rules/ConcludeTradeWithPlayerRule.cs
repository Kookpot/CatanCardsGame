using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class ConcludeTradeWithPlayerRule : ActionRule<CatanCardsGameState, ConcludeTradeWithPlayerAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, ConcludeTradeWithPlayerAction tradeAction)
    {
        game.State.PlayerResources[game.State.TradingWith!].Add(tradeAction.PlayerResource);
        game.State.PlayerResources[tradeAction.Player].Remove(tradeAction.PlayerResource);
        game.State.FSM.PerformTransition(PhaseTransition.SingleTradeConcluded);
        game.State.TradingWith = null;
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, ConcludeTradeWithPlayerAction tradeAction)
    {
        var result = Result.Ok();
        if (!game.State.PlayerResources[tradeAction.Player].Any(x => x == tradeAction.PlayerResource))
        {
            result.WithReason(new Reason { Message = "You cannot trade that resource as you don't have it." });
            return result;
        }
        return result;
    }
}