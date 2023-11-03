using MechanX.CatanCards.Actions;
using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal class StopTradeRule : ActionRule<CatanCardsGameState, StopTradeAction>
{ 
    public override void ApplyAction(Game<CatanCardsGameState> game, StopTradeAction stopTradeAction)
    {
        game.State.FSM.PerformTransition(PhaseTransition.TradeDone);
    }

    public override Result IsValidAction(Game<CatanCardsGameState> game, StopTradeAction stopTradeAction)
    {
        return Result.Ok();
    }
}