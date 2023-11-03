using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class TradeWithMarketRule : ActionRule<CatanCardsGameState, TradeWithMarketAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, TradeWithMarketAction tradeAction)
    {
        var resources = game.State.PlayerResources[tradeAction.Player];
        resources.Remove(tradeAction.PlayerResource);
        game.State.Market.Remove(tradeAction.MarketResource);

        resources.Add(tradeAction.MarketResource);
        game.State.Market.Remove(tradeAction.PlayerResource);

        game.State.TradedThisRound++;
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, TradeWithMarketAction tradeAction)
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
            result.WithReason(new Reason { Message = "You cannot trade with the market." });
            return result;
        }
        if (!game.State.PlayerResources[tradeAction.Player].Any(x => x == tradeAction.PlayerResource))
        {
            result.WithReason(new Reason { Message = "You cannot trade a resource you don't have." });
            return result;
        }
        if (!game.State.Market.Any(x => x == tradeAction.MarketResource))
        {
            result.WithReason(new Reason { Message = "You cannot trade a resource that the market doesn't have." });
            return result;
        }
        return result;
    }
}