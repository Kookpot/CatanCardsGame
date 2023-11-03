using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class ChooseResourceFromPlayerRule : ActionRule<CatanCardsGameState, ChooseResourceFromPlayerAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, ChooseResourceFromPlayerAction tradeAction)
    {
        var item = game.State.PlayerResources[game.State.TradingWith!].First(x => x == tradeAction.PlayerResource);
        game.State.PlayerResources[game.State.TradingWith!].Remove(item);
        game.State.PlayerResources[tradeAction.Player].Add(item);
        game.State.FSM.PerformTransition(PhaseTransition.SingleTradeStarted);
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, ChooseResourceFromPlayerAction tradeAction)
    {
        var result = Result.Ok();
        if (!game.State.PlayerResources[game.State.TradingWith!].Any(x => x == tradeAction.PlayerResource))
        {
            result.WithReason(new Reason { Message = "You cannot trade that resource with that player as he has not that resources." });
            return result;
        }
        return result;
    }
}