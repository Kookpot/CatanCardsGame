using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class DropResourcesRule : ActionRule<CatanCardsGameState, DropResourcesAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, DropResourcesAction dropResourceAction)
    {
        BuildRuleHelper.RemoveUsedResources(game, dropResourceAction.ResourceToDrop);
        if (!game.State.PlayerResources.Any(x => x.Value.Count > 7))
        {
            game.State.FSM.PerformTransition(PhaseTransition.HijackOver);
            if (game.Players.Count > 2)
            {
                game.State.PlayerOrder.Reverse();
                game.State.ReverseOrder = !game.State.ReverseOrder;
            }
        }
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, DropResourcesAction dropResourceAction)
    {
        var result = Result.Ok();
        var resources = game.State.PlayerResources[dropResourceAction.Player];
        if (dropResourceAction.ResourceToDrop.Count > (resources.Count - 7))
        {
            result.WithReason(new Reason { Message = "Too many resources are being dropped." });
        }
        //do we have resources?
        var copyOfResourcesOfPlayer = resources.ToList();
        foreach (var resource in dropResourceAction.ResourceToDrop)
        {
            if (copyOfResourcesOfPlayer.Contains(resource))
            {
                copyOfResourcesOfPlayer.Remove(resource);
            }
            else
            {
                result.WithReason(new Reason { Message = "You don't have the resources to perform this drop." });
                return result;
            }
        }
        return result;
    }
}