using MechanX.CatanCards.Model;

namespace MechanX.CatanCards.Rules;

internal static class BuildRuleHelper
{
    internal static void RemoveUsedResources(Game<CatanCardsGameState> game, List<ResourceType> resources)
    {
        foreach (var resource in resources)
        {
            game.State.PlayerResources[game.State.CurrentPlayer].Remove(resource);
            game.State.DropStack.Add(resource);
        }
    }

    internal static bool AreTheResourcesSufficient(List<ResourceType> resources, List<ResourceOkToBuild> resourcesOkToBuild, bool useUniversity)
    {
        foreach(var resource in resourcesOkToBuild)
        {
            if (resources.Contains(resource.ResourceType))
            {
                resources.Remove(resource.ResourceType);
                resource.IsOk = true;
            }
        }
        foreach(var resource in resourcesOkToBuild)
        {
            if (!resource.IsOk && resources.Any())
            {
                resource.IsOk = true;
                var f = resources.First();
                for (var i = 0; i < 3; i++)
                {
                    if (resources.Contains(f))
                    {
                        resources.Remove(f);
                    }
                    else
                    {
                        resource.IsOk = false;
                    }
                }
            }
        }
        if (useUniversity)
        {
            var used = false;
            foreach(var resource in resourcesOkToBuild)
            {
                if (!resource.IsOk)
                {
                    resource.IsOk = true;
                    used = true;
                    break;
                }
            }
            if (!used)
            {
                return false;
            }
        }
        return true;
    }
}