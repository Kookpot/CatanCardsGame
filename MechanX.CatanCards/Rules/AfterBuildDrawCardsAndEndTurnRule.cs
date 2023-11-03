using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Events;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class AfterBuildDrawCardsAndEndTurnRule : ActionRule<CatanCardsGameState, EndBuildPhaseAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, EndBuildPhaseAction action)
    {
        var knights = game.State.PlayerCards[action.Player].Count(x => x.CardType == CardType.Knight && !x.BackSide);
        DrawFromStackForCurrentPlayer(game, knights + 2);
        game.RaiseEvent<TurnEndedEvent>();
        foreach(var card in game.State.PlayerCards[action.Player])
        {
            card.Tapped = false;
        }
        game.State.TradedThisRound = 0;
        game.State.FSM.PerformTransition(PhaseTransition.BuildDone);
        var currentIndex = game.State.PlayerOrder.IndexOf(action.Player);
        currentIndex++;
        currentIndex %= game.State.PlayerOrder.Count;
        game.State.CurrentPlayer = game.State.PlayerOrder[currentIndex];
        game.State.BuiltThisTurn.Clear();
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, EndBuildPhaseAction action)
    {
        return Result.Ok();
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