using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Actions;

[TakenInState<Phase>(Phase.Build)]
public record class BuildAction(Player Player, CardType CardToBuild, List<ResourceType> ResourceToUse, bool UseUniversity) : PlayerAction(Player) { }