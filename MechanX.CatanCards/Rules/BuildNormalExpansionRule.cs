using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class BuildNormalExpansionRule : ActionRule<CatanCardsGameState, BuildAction>
{
    private static readonly List<ResourceOkToBuild> ResourcesToBuild = new()
    {
        new ResourceOkToBuild { ResourceType = ResourceType.Sheep },
        new ResourceOkToBuild { ResourceType = ResourceType.Sheep },
        new ResourceOkToBuild { ResourceType = ResourceType.Sheep },
        new ResourceOkToBuild { ResourceType = ResourceType.Ore }
    };

    public override void ApplyAction(Game<CatanCardsGameState> game, BuildAction buildAction)
    {
        if (buildAction.CardToBuild == CardType.Church || buildAction.CardToBuild == CardType.CityWall || 
            buildAction.CardToBuild == CardType.Library || buildAction.CardToBuild == CardType.University ||
            buildAction.CardToBuild == CardType.Guild)
        {
            var lst = GetAvailableList(buildAction.CardToBuild, game.State);
            var card = lst.First();
            var playerCards = game.State.PlayerCards[buildAction.Player];
            var city = playerCards.First(x => x.CardType == CardType.City);
            var index = playerCards.IndexOf(city);
            playerCards.Remove(city);
            playerCards.Insert(index, card);
            RemoveFirstFromAvailableList(buildAction.CardToBuild, game.State);
            BuildRuleHelper.RemoveUsedResources(game, buildAction.ResourceToUse);
        }
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, BuildAction buildAction)
    {
        var result = Result.Ok();
        if (buildAction.CardToBuild == CardType.Church || buildAction.CardToBuild == CardType.CityWall ||
            buildAction.CardToBuild == CardType.Library || buildAction.CardToBuild == CardType.University || 
            buildAction.CardToBuild == CardType.Guild)
        {
            var cards = game.State.PlayerCards[buildAction.Player];
            //must have city
            if (!cards.Any(x => x.CardType == CardType.City))
            {
                result.WithReason(new Reason { Message = "No city available that can be expanded." });
                return result;
            }
            //must be available
            if (!GetAvailableList(buildAction.CardToBuild, game.State).Any())
            {
                result.WithReason(new Reason { Message = $"No {buildAction.CardToBuild.ToString().ToLowerInvariant() } available as expansion." });
                return result;
            }
            //already build this expansion
            if (cards.Any(x => x.CardType == buildAction.CardToBuild))
            {
                result.WithReason(new Reason { Message = $"You already built a { buildAction.CardToBuild.ToString().ToLowerInvariant() }." });
                return result;
            }
            //are resources ok
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
                result.WithReason(new Reason { Message = $"Not enough resources to build a { buildAction.CardToBuild.ToString().ToLowerInvariant() }." });
                return result;
            }
            if (copy.Any())
            {
                result.WithReason(new Reason { Message = $"Too many resources to build a { buildAction.CardToBuild.ToString().ToLowerInvariant() }." });
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
                    result.WithReason(new Reason { Message = $"You don't have the resources to build a { buildAction.CardToBuild.ToString().ToLowerInvariant() }." });
                    return result;
                }
            }
        }
        return result;
    }

    private static List<Card> GetAvailableList(CardType cardType, CatanCardsGameState state)
    {
        return cardType switch
        {
            CardType.Library => state.LibraryAvailable.Select(x => x as Card).ToList(),
            CardType.University => state.UniversityAvailable.Select(x => x as Card).ToList(),
            CardType.CityWall => state.CityWallAvailable.Select(x => x as Card).ToList(),
            CardType.Guild => state.GuildAvailable.Select(x => x as Card).ToList(),
            _ => state.ChurchAvailable.Select(x => x as Card).ToList(),
        };
    }

    private static void RemoveFirstFromAvailableList(CardType cardType, CatanCardsGameState state)
    {
        switch (cardType)
        {
            case CardType.Library:
                state.LibraryAvailable.RemoveAt(0);
                break;
            case CardType.University:
                state.UniversityAvailable.RemoveAt(0);
                break;
            case CardType.CityWall:
                state.CityWallAvailable.RemoveAt(0);
                break;
            case CardType.Guild:
                state.GuildAvailable.RemoveAt(0);
                break;
            default:
                state.ChurchAvailable.RemoveAt(0);
                break;
        }
    }
}