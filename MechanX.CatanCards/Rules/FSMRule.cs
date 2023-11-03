using MechanX.CatanCards.Model;
namespace MechanX.CatanCards.Rules;

public class FSMRule : Rule<CatanCardsGameState>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, IAction action)
    {
        //do nothing
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, IAction action)
    {
        var result = Result.Ok();
        var found = false;
        foreach (var taken in action.GetType().GetCustomAttributes(typeof(TakenInStateAttribute<Phase>), true).Cast<TakenInStateAttribute<Phase>>())
        {
            if (taken.State == game.State.FSM.CurrentState)
            {
                found = true;
                break;
            }
        }
        if (!found)
        {
            result.WithReason(new Reason { Message = "This action can not be taken in the current phase of the game." });
        }
        return result;
    }
}