using MechanX.CatanCards.Events;
using MechanX.CatanCards.Model;
using MechanX.CatanCards.Rules;

namespace MechanX.CatanCards;

public class CatanCardsGame : Game<CatanCardsGameState>
{
    //main entry point
    public CatanCardsGame(List<Player> players) : base(players, new CatanCardsGameState(players))
    {
        AddRule(new FSMRule());
        AddRule(new OnlyCurrentPlayerCanPerformActionsRule());
        AddRule(new WhenGameWonNoActionsCanBeTakenRule());

        AddActionRule(new AfterBuildDrawCardsAndEndTurnRule());
        AddActionRule(new StopTradeRule());
        AddActionRule(new BuildActionRule());
        AddActionRule(new BuildVillageRule());
        AddActionRule(new BuildCityRule());
        AddActionRule(new BuildCityHallRule());
        AddActionRule(new BuildKnightRule());
        AddActionRule(new BuildStreetRule());
        AddActionRule(new BuildNormalExpansionRule());
        AddActionRule(new ReverseOrderRule());
        AddActionRule(new DropResourcesRule());
        AddActionRule(new TradeWithStackRule());
        AddActionRule(new TradeWithMarketRule());
        AddActionRule(new TradeWithPlayerRule());
        AddActionRule(new ConcludeTradeWithPlayerRule());
        AddActionRule(new TradeWithPlayerUsingLibraryRule());
        AddActionRule(new ChooseResourceFromPlayerRule());
        AddActionRule(new UseGuildRule());

        AddEventRule<TurnEndedEvent>(new GameOverWhenTurnEndsWith10PointsRule());
    }
}