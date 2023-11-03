using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class TradeWithPlayerUsingLibraryRule : ActionRule<CatanCardsGameState, TradeWithPlayerUsingLibraryAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, TradeWithPlayerUsingLibraryAction tradeAction)
    {
        game.State.TradedThisRound++;
        game.State.FSM.PerformTransition(PhaseTransition.SingleTradeLibraryStarted);
        game.State.TradingWith = tradeAction.OtherPlayer;

        var lib = game.State.PlayerCards[tradeAction.Player].First(x => x.CardType == CardType.Library);
        lib.Tapped = true;
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, TradeWithPlayerUsingLibraryAction tradeAction)
    {
        var result = Result.Ok();
        var cards = game.State.PlayerCards[tradeAction.Player];
        if (!cards.Any(x => x.CardType == CardType.Library && !x.Tapped))
        {
            result.WithReason(new Reason { Message = "You cannot trade using the library as you don't have an untapped library card." });
            return result;
        }
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