using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class BuildVillageRule : ActionRule<CatanCardsGameState, BuildAction>
{
    private static readonly List<ResourceOkToBuild> ResourcesToBuild = new()
    {
        new ResourceOkToBuild { ResourceType = ResourceType.Grain },
        new ResourceOkToBuild { ResourceType = ResourceType.Sheep },
        new ResourceOkToBuild { ResourceType = ResourceType.Brick },
        new ResourceOkToBuild { ResourceType = ResourceType.Wood }
    };

    public override void ApplyAction(Game<CatanCardsGameState> game, BuildAction buildAction)
    {
        if (buildAction.CardToBuild == CardType.Village)
        {
            var village = game.State.VillageAvailable.First();
            game.State.PlayerCards[buildAction.Player].Add(village);
            game.State.VillageAvailable.Remove(village);
            BuildRuleHelper.RemoveUsedResources(game, buildAction.ResourceToUse);
        }
        if (game.Players.Count > 2)
        {
            game.State.FSM.PerformTransition(PhaseTransition.CanOrderBeReversed);
        }
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, BuildAction buildAction)
    {
        var result = Result.Ok();
        if (buildAction.CardToBuild == CardType.Village)
        {
            if (!game.State.VillageAvailable.Any())
            {
                result.WithReason(new Reason { Message = "All villages have been build." });
                return result;
            }
            //already build village
            if (game.State.BuiltThisTurn.Any(x => x == CardType.Village))
            {
                result.WithReason(new Reason { Message = "You already build a village this turn." });
                return result;
            }
            //are resources ok for village?
            var copy = buildAction.ResourceToUse.ToList();
            var copyResourcesToBuild = ResourcesToBuild.ToList();
            var isUniversityOk = BuildRuleHelper.AreTheResourcesSufficient(copy, copyResourcesToBuild, buildAction.UseUniversity);

            if (!isUniversityOk)
            {
                result.WithReason(new Reason { Message = "University is used but not necessary." });
                return result;
            }
            if (copyResourcesToBuild.Any(x => !x.IsOk))
            {
                result.WithReason(new Reason { Message = "Not enough resources to build a village." });
                return result;
            }
            if (copy.Any())
            {
                result.WithReason(new Reason { Message = "Too many resources to build a village." });
                return result;
            }
            //do we have resources?
            var copyOfResourcesOfPlayer = game.State.PlayerResources[buildAction.Player].ToList();
            foreach (var resource in buildAction.ResourceToUse)
            {
                if (copyOfResourcesOfPlayer.Contains(resource))
                {
                    copyOfResourcesOfPlayer.Remove(resource);
                }
                else
                {
                    result.WithReason(new Reason { Message = "You don't have the resources to build a village." });
                    return result;
                }
            }
        }
        return result;
    }
}