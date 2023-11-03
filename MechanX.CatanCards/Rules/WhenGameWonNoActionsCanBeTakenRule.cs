using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

public class WhenGameWonNoActionsCanBeTakenRule : Rule<CatanCardsGameState>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, IAction action)
    {
        //do nothing
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, IAction action)
    {
        var result = Result.Ok();
        if (action is PlayerAction && game.GameOver)
        {
            result.Reasons.Add(new Reason { Message = "You cannot perform this action when the game is over." });
        }
        return result;
    }
}