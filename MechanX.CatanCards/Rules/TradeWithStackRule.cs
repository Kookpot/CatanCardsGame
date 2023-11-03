using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class TradeWithStackRule : ActionRule<CatanCardsGameState, TradeWithStackAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, TradeWithStackAction tradeAction)
    {
        DrawFromStackForCurrentPlayer(game, 1);
        game.State.DropStack.Add(tradeAction.PlayerResource);
        game.State.PlayerResources[tradeAction.Player].Remove(tradeAction.PlayerResource);
        game.State.TradedThisRound++;
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, TradeWithStackAction tradeAction)
    {
        var result = Result.Ok();
        if (game.State.TradedThisRound != 0 && game.State.TradedThisRound > game.State.PlayerCards[tradeAction.Player].Count(x => x.CardType == CardType.Street && !x.BackSide))
        {
            result.WithReason(new Reason { Message = "You cannot trade anymore." });
            return result;
        }
        if (!game.State.PlayerResources[tradeAction.Player].Any(x => x == tradeAction.PlayerResource))
        {
            result.WithReason(new Reason { Message = "You cannot trade a resource you don't have." });
            return result;
        }
        return result;
    }

    private static void DrawFromStackForCurrentPlayer(Game<CatanCardsGameState> game, int nr)
    {
        for (var i = 0; i < nr; i++)
        {
            if (!game.State.DrawStack.Any() && !game.State.DropStack.Any())
            {
                return;
            }
            else if (!game.State.DrawStack.Any() && game.State.DropStack.Any())
            {
                game.State.DrawStack.AddRange(game.State.DropStack);
                game.State.DropStack.Clear();
                game.State.DrawStack.Shuffle();
            }
            var first = game.State.DrawStack.First();
            game.State.PlayerResources[game.State.CurrentPlayer].Add(first);
            game.State.DrawStack.Remove(first);
        }
    }
}