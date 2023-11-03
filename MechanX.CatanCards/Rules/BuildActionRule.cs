using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class BuildActionRule : ActionRule<CatanCardsGameState, BuildAction>
{
    public override void ApplyAction(Game<CatanCardsGameState> game, BuildAction action)
    {
        if (action.UseUniversity)
        {
            var uni = game.State.PlayerCards[action.Player].First(x => x.CardType == CardType.University);
            uni.Tapped = true;
        }
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, BuildAction action)
    {
        var result = Result.Ok();
        if (action.UseUniversity)
        {
            var cards = game.State.PlayerCards[action.Player];
            if (!cards.Any(x => x.CardType == CardType.University))
            {
                result.WithReason(new Reason { Message = "You don't have a university so you can't use that option." });
                return result;
            }
            if (cards.Any(x => x.CardType == CardType.University && x.Tapped))
            {
                result.WithReason(new Reason { Message = "The university is already used this turn so you can't use that option." });
            }
        }
        return result;
    }
}