using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class BuildCityHallRule : ActionRule<CatanCardsGameState, BuildAction>
{
    private static readonly List<ResourceOkToBuild> ResourcesToBuild = new()
    {
        new ResourceOkToBuild { ResourceType = ResourceType.Sheep },
        new ResourceOkToBuild { ResourceType = ResourceType.Sheep },
        new ResourceOkToBuild { ResourceType = ResourceType.Sheep },
        new ResourceOkToBuild { ResourceType = ResourceType.Ore },
        new ResourceOkToBuild { ResourceType = ResourceType.Ore },
        new ResourceOkToBuild { ResourceType = ResourceType.Ore }
    };

    public override void ApplyAction(Game<CatanCardsGameState> game, BuildAction action)
    {
        if (action.CardToBuild == CardType.CityHall)
        {
            var cityHall = game.State.CityHallAvailable.First();
            var playerCards = game.State.PlayerCards[action.Player];
            var city = playerCards.First(x => x.CardType == CardType.City);
            var index = playerCards.IndexOf(city);
            playerCards.Remove(city);
            playerCards.Insert(index, cityHall);
            game.State.CityHallAvailable.Remove(cityHall);
            BuildRuleHelper.RemoveUsedResources(game, action.ResourceToUse);
        }
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, BuildAction action)
    {
        var result = Result.Ok();
        if (action.CardToBuild == CardType.CityHall)
        {
            //must have city
            var cards = game.State.PlayerCards[action.Player];
            if (!cards.Any(x => x.CardType == CardType.City))
            {
                result.WithReason(new Reason { Message = "No city available that can be expanded." });
                return result;
            }
            //must be available
            if (!game.State.CityHallAvailable.Any())
            {
                result.WithReason(new Reason { Message = "No city hall available as expansion." });
                return result;
            }
            //already build this expansion
            if (cards.Any(x => x.CardType == CardType.CityHall))
            {
                result.WithReason(new Reason { Message = "You already built a city hall." });
                return result;
            }
            //are resources ok
            var copy = action.ResourceToUse.ToList();
            var copyResourcesToBuild = ResourcesToBuild.ToList();
            var isUniversityOk = BuildRuleHelper.AreTheResourcesSufficient(copy, copyResourcesToBuild, action.UseUniversity);

            if (!isUniversityOk)
            {
                result.WithReason(new Reason { Message = "University is used but not necessary." });
                return result;
            }
            if (copyResourcesToBuild.Any(x => !x.IsOk))
            {
                result.WithReason(new Reason { Message = "Not enough resources to build a city hall." });
                return result;
            }
            if (copy.Any())
            {
                result.WithReason(new Reason { Message = "Too many resources to build a city hall." });
                return result;
            }
            //do we have resources?
            var copyOfResourcesOfPlayer = game.State.PlayerResources[action.Player].ToList();
            foreach (var resource in action.ResourceToUse)
            {
                if (copyOfResourcesOfPlayer.Contains(resource))
                {
                    copyOfResourcesOfPlayer.Remove(resource);
                }
                else
                {
                    result.WithReason(new Reason { Message = "You don't have the resources to build a city hall." });
                    return result;
                }
            }

        }
        return result;
    }
}