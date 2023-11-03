using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class BuildCityRule : ActionRule<CatanCardsGameState, BuildAction>
{
    private static readonly List<ResourceOkToBuild> ResourcesToBuild = new()
    {
        new ResourceOkToBuild { ResourceType = ResourceType.Grain },
        new ResourceOkToBuild { ResourceType = ResourceType.Grain },
        new ResourceOkToBuild { ResourceType = ResourceType.Ore },
        new ResourceOkToBuild { ResourceType = ResourceType.Ore },
        new ResourceOkToBuild { ResourceType = ResourceType.Ore }
    };

    public override void ApplyAction(Game<CatanCardsGameState> game, BuildAction buildAction)
    {
        if (buildAction.CardToBuild == CardType.City)
        {
            var firstVillage = game.State.PlayerCards[buildAction.Player].First(x => x.CardType == CardType.Village) as Village;
            firstVillage!.BackSide = true;
            BuildRuleHelper.RemoveUsedResources(game, buildAction.ResourceToUse);

            if (firstVillage.IsHijack)
            {
                if (game.State.PlayerResources.Any(x => x.Value.Count > 7))
                {
                    game.State.FSM.PerformTransition(PhaseTransition.HijackStared);
                } 
                else if (game.Players.Count > 2)
                {
                    game.State.PlayerOrder.Reverse();
                    game.State.ReverseOrder = !game.State.ReverseOrder;
                }
            } 
            else
            {
                foreach(var card in game.State.Market)
                {
                    game.State.DropStack.Add(card);
                }
                game.State.Market.Clear();
                for (var i = 0; i < 5; i++)
                {
                    if (!game.State.DrawStack.Any())
                    {
                        game.State.DrawStack.AddRange(game.State.DropStack);
                        game.State.DropStack.Clear();
                        game.State.DrawStack.Shuffle();
                    }
                    var first = game.State.DrawStack.First();
                    game.State.Market.Add(first);
                    game.State.DrawStack.Remove(first);
                }
            }
        }
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, BuildAction buildAction)
    {
        var result = Result.Ok();
        if (buildAction.CardToBuild is CardType.City)
        {
            //must have village
            if (!game.State.PlayerCards[buildAction.Player].Any(x => x is Village && !x.BackSide))
            {
                result.WithReason(new Reason { Message = "No village available that can be converted to city." });
                return result;
            }
            //already build city
            if (game.State.BuiltThisTurn.Any(x => x is CardType.City))
            {
                result.WithReason(new Reason { Message = "You already build a city this turn." });
                return result;
            }
            //are resources ok for city?
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
                    result.WithReason(new Reason { Message = "You don't have the resources to build a city." });
                    return result;
                }
            }
        }
        return result;
    }
}