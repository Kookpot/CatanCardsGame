using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

public class OnlyCurrentPlayerCanPerformActionsRule : Rule<CatanCardsGameState>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, IAction action)
    {
        //do nothing
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, IAction action)
    {
        var result = Result.Ok();
        if (action is PlayerAction playerAction)
        {
            if (game.State.CurrentPlayer != playerAction.Player && action is not DropResourcesAction)
            {
                result.Reasons.Add(new Reason { Message = "Only the current player can perform actions." });
            }
        }
        return result;
    }
}