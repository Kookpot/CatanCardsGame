using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class UseGuildRule : ActionRule<CatanCardsGameState, UseGuildAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, UseGuildAction useGuildAction)
    {
        var otherResources = game.State.PlayerResources[useGuildAction.OtherPlayer];
        int k = ThreadSafeRandom.ThisThreadsRandom.Next(otherResources.Count);
        var res = otherResources[k];
        otherResources.Remove(res);
        game.State.PlayerResources[useGuildAction.Player].Add(res);
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, UseGuildAction useGuildAction)
    {
        var result = Result.Ok();
        if (game.State.GetPoints(useGuildAction.Player) > game.State.GetPoints(useGuildAction.OtherPlayer))
        {
            result.WithReason(new Reason { Message = "You cannot use the guild action on a player with less points." });
            return result;
        }
        if (!game.State.PlayerResources[useGuildAction.OtherPlayer].Any())
        {
            result.WithReason(new Reason { Message = "You cannot use the guild action on a player with no resource cards." });
            return result;
        }
        return result;
    }
}