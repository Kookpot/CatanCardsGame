using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class BuildStreetRule : ActionRule<CatanCardsGameState, BuildAction>
{
    private static readonly List<ResourceOkToBuild> ResourcesToBuild = new()
    {
        new ResourceOkToBuild { ResourceType = ResourceType.Wood },
        new ResourceOkToBuild { ResourceType = ResourceType.Brick }
    };

    public override void ApplyAction(Game<CatanCardsGameState> game, BuildAction buildAction)
    {
        if (buildAction.CardToBuild == CardType.Street)
        {
            var cards = game.State.PlayerCards[buildAction.Player];
            if (game.State.StreetAvailable.Any())
            {
                var street = game.State.StreetAvailable.First();
                cards.Add(street);
                if (cards.Count(x => x.CardType == CardType.Street) % 2 == 0)
                {
                    street.BackSide = true;
                }
                game.State.StreetAvailable.Remove(street);
            }
            else
            {
                var street = GetNextStreetFromPlayer(game.State);
                cards.Add(street!);
                if (cards.Count(x => x.CardType == CardType.Street) % 2 == 0)
                {
                    street!.BackSide = true;
                }
                else
                {
                    street!.BackSide = false;
                }

            }
            BuildRuleHelper.RemoveUsedResources(game, buildAction.ResourceToUse);
        }
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, BuildAction buildAction)
    {
        var result = Result.Ok();
        if (buildAction.CardToBuild == CardType.Street)
        {
            //must have street available
            if (!game.State.StreetAvailable.Any() && GetNextStreetFromPlayer(game.State) == null)
            {
                result.WithReason(new Reason { Message = "No streets available." });
                return result;
            }
            //already build a street
            if (game.State.PlayerCards[buildAction.Player].Any(x => x.CardType == CardType.Street))
            {
                result.WithReason(new Reason { Message = "You already build a street this turn." });
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
                result.WithReason(new Reason { Message = "Not enough resources to build a street." });
                return result;
            }
            if (copy.Any())
            {
                result.WithReason(new Reason { Message = "Too many resources to build a street." });
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
                    result.WithReason(new Reason { Message = "You don't have the resources to build a street." });
                    return result;
                }
            }

        }
        return result;
    }

    private static Street? GetNextStreetFromPlayer(CatanCardsGameState state)
    {
        var indexPlayer = state.PlayerOrder.IndexOf(state.CurrentPlayer);
        var nextPlayer = indexPlayer;
        nextPlayer++;
        nextPlayer %= state.PlayerOrder.Count;
        while (indexPlayer != nextPlayer)
        {
            var otherPlayer = state.PlayerOrder[nextPlayer];
            var otherCards = state.PlayerCards[otherPlayer];
            var countStreet = otherCards.Count(x => x.CardType == CardType.Street);
            if ((countStreet > 3 && otherCards.Any(x => x.CardType == CardType.CityWall)) || countStreet > 0)
            {
                var street = otherCards.Last(x => x.CardType == CardType.Street) as Street;
                otherCards.Remove(street!);
                return street;
            }
            nextPlayer++;
            nextPlayer %= state.PlayerOrder.Count;
        }
        return null;
    }
}
