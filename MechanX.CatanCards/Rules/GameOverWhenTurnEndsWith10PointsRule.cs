using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

public class GameOverWhenTurnEndsWith10PointsRule : EventRule<CatanCardsGameState>
{
    public override void ApplyEvent<TEvent>(Game<CatanCardsGameState> game, TEvent? evnt) where TEvent : class
    {
        if(game.State.GetPoints(game.State.CurrentPlayer) >= 10)
        {
            game.Winners.Add(game.State.CurrentPlayer);
            game.GameOver = true;
        }
    }

    public override Result IsValidEvent<TEvent>(Game<CatanCardsGameState> game, TEvent? evnt) where TEvent : class
    {
        return Result.Ok();
    }
}