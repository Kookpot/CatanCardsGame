using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class BuildKnightRule : ActionRule<CatanCardsGameState, BuildAction>
{
    private static readonly List<ResourceOkToBuild> ResourcesToBuild = new()
    {
        new ResourceOkToBuild { ResourceType = ResourceType.Grain },
        new ResourceOkToBuild { ResourceType = ResourceType.Sheep },
        new ResourceOkToBuild { ResourceType = ResourceType.Ore }
    };

    public override void ApplyAction(Game<CatanCardsGameState> game, BuildAction buildAction)
    {
        if (buildAction.CardToBuild == CardType.Knight)
        {
            var cards = game.State.PlayerCards[buildAction.Player];
            if (game.State.KnightAvailable.Any())
            {
                var knight = game.State.KnightAvailable.First();
                cards.Add(knight);
                if (cards.Count(x => x.CardType == CardType.Knight) % 2 == 0)
                {
                    knight.BackSide = true;
                }
                game.State.KnightAvailable.Remove(knight);
            }
            else
            {
                var knight = GetNextKnightFromPlayer(game.State);
                cards.Add(knight!);
                if (cards.Count(x => x.CardType == CardType.Knight) % 2 == 0)
                {
                    knight!.BackSide = true;
                }
                else
                {
                    knight!.BackSide = false;
                }

            }
            BuildRuleHelper.RemoveUsedResources(game, buildAction.ResourceToUse);
        }
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, BuildAction buildAction)
    {
        var result = Result.Ok();
        if (buildAction.CardToBuild == CardType.Knight)
        {
            //must have knight available
            if (!game.State.KnightAvailable.Any() && GetNextKnightFromPlayer(game.State) == null)
            {
                result.WithReason(new Reason { Message = "No knights available." });
                return result;
            }
            //already build a knight
            if (game.State.PlayerCards[buildAction.Player].Any(x => x.CardType == CardType.Knight))
            {
                result.WithReason(new Reason { Message = "You already build a knight this turn." });
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
                result.WithReason(new Reason { Message = "Not enough resources to build a knight." });
                return result;
            }
            if (copy.Any())
            {
                result.WithReason(new Reason { Message = "Too many resources to build a knight." });
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
                    result.WithReason(new Reason { Message = "You don't have the resources to build a knight." });
                    return result;
                }
            }

        }
        return result;
    }

    private static Knight? GetNextKnightFromPlayer(CatanCardsGameState state)
    {
        var indexPlayer = state.PlayerOrder.IndexOf(state.CurrentPlayer);
        var nextPlayer = indexPlayer;
        nextPlayer++;
        nextPlayer %= state.PlayerOrder.Count;
        while (indexPlayer != nextPlayer)
        {
            var otherPlayer = state.PlayerOrder[nextPlayer];
            var otherCards = state.PlayerCards[otherPlayer];
            var countKnight = otherCards.Count(x => x.CardType == CardType.Knight);
            if ((countKnight > 1 && otherCards.Any(x => x.CardType == CardType.Church)) || countKnight > 0)
            {
                var knight = otherCards.Last(x => x.CardType == CardType.Knight) as Knight;
                otherCards.Remove(knight!);
                return knight;
            }
            nextPlayer++;
            nextPlayer %= state.PlayerOrder.Count;
        }
        return null;
    }
}
