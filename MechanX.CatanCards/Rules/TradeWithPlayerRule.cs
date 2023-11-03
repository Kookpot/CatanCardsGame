using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class TradeWithPlayerRule : ActionRule<CatanCardsGameState, TradeWithPlayerAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, TradeWithPlayerAction tradeAction)
    {
        var otherResources = game.State.PlayerResources[tradeAction.OtherPlayer];
        var k = ThreadSafeRandom.ThisThreadsRandom.Next(otherResources.Count);
        var item = otherResources[k];
        otherResources.Remove(item);
        game.State.PlayerResources[tradeAction.Player].Add(item);
        game.State.TradedThisRound++;
        game.State.FSM.PerformTransition(PhaseTransition.SingleTradeStarted);
        game.State.TradingWith = tradeAction.OtherPlayer;
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, TradeWithPlayerAction tradeAction)
    {
        var result = Result.Ok();
        var cards = game.State.PlayerCards[tradeAction.Player];
        if (game.State.TradedThisRound > cards.Count(x => x.CardType == CardType.Street && !x.BackSide))
        {
            result.WithReason(new Reason { Message = "You cannot trade anymore." });
            return result;
        }
        if (!cards.Any(x => x.CardType == CardType.Street && !x.BackSide))
        {
            result.WithReason(new Reason { Message = "You cannot trade with a player." });
            return result;
        }
        if (!game.State.PlayerResources[tradeAction.OtherPlayer].Any())
        {
            result.WithReason(new Reason { Message = "You cannot trade with that player as he has no resources." });
            return result;
        }
        return result;
    }
}