namespace MechanX.CatanCards.Model;

//hold state + begin state
public class CatanCardsGameState : IState
{
    public Dictionary<Player, List<Card>> PlayerCards { get; } = new();
    public Dictionary<Player, List<ResourceType>> PlayerResources { get; } = new();
    internal FSM<Phase, PhaseTransition> FSM { get; }

    public List<ResourceType> Market { get; } = new();

    public List<ResourceType> DrawStack { get; } = new();

    public List<ResourceType> DropStack { get; } = new();

    public Player CurrentPlayer { get; internal set; }

    public List<Player> PlayerOrder { get; } = new();

    public List<CardType> BuiltThisTurn { get; } = new();

    public List<Village> VillageAvailable { get; } = new();
    public List<Street> StreetAvailable { get; } = new();
    public List<Knight> KnightAvailable { get; } = new();
    public List<Library> LibraryAvailable { get; } = new();
    public List<CityHall> CityHallAvailable { get; } = new();
    public List<CityWall> CityWallAvailable { get; } = new();
    public List<Church> ChurchAvailable { get; } = new();
    public List<University> UniversityAvailable { get; } = new();
    public List<Guild> GuildAvailable { get; } = new();

    public bool ReverseOrder { get; internal set; }

    public int TradedThisRound { get; internal set; }

    public Player? TradingWith {get;internal set; }

    internal CatanCardsGameState(List<Player> players)
    {
        FSM = new(Phase.Trade);
        FSM.AddTransition(Phase.Trade, Phase.Build, PhaseTransition.TradeDone);
        FSM.AddTransition(Phase.Build, Phase.Trade, PhaseTransition.BuildDone);
        FSM.AddTransition(Phase.Build, Phase.Hijack, PhaseTransition.HijackStared);
        FSM.AddTransition(Phase.Hijack, Phase.Build, PhaseTransition.HijackOver);
        FSM.AddTransition(Phase.NeedsConcludeTrade, Phase.Trade, PhaseTransition.SingleTradeConcluded);
        FSM.AddTransition(Phase.Trade, Phase.NeedsConcludeTrade, PhaseTransition.SingleTradeStarted);
        FSM.AddTransition(Phase.Trade, Phase.NeedsSelectionResource, PhaseTransition.SingleTradeLibraryStarted);
        FSM.AddTransition(Phase.NeedsSelectionResource, Phase.NeedsConcludeTrade, PhaseTransition.SingleTradeStarted);
        FSM.AddTransition(Phase.CanReverseOrder, Phase.Build, PhaseTransition.OrderReversed);
        FSM.AddTransition(Phase.Build, Phase.CanReverseOrder, PhaseTransition.CanOrderBeReversed);

        for (var i = 0; i < (players.Count >= 5 ? 32 : 16); i++)
        {
            DrawStack.Add(ResourceType.Ore);
        }
        for (var i = 0; i < (players.Count >= 5 ? 22 : 11); i++)
        {
            DrawStack.Add(ResourceType.Brick);
        }
        for (var i = 0; i < (players.Count >= 5 ? 22 : 11); i++)
        {
            DrawStack.Add(ResourceType.Wood);
        }
        for (var i = 0; i < (players.Count >= 5 ? 30 : 15); i++)
        {
            DrawStack.Add(ResourceType.Sheep);
        }
        for (var i = 0; i < (players.Count >= 5 ? 28 : 14); i++)
        {
            DrawStack.Add(ResourceType.Grain);
        }
        DrawStack.Shuffle();


        var street = 5;
        var knight = 2;
        var village = 7;
        var hijack = 5;
        var cityHall = 1;
        var cityWall = 1;
        var library = 1;
        var university = 1;
        var church = 1;
        var guild = 0;
        if (players.Count >= 3)
        {
            street += 2;
            knight += 2;
            village += 4;
            hijack += 3;
            guild += 1;
            church += 1;
        }
        if (players.Count >= 4)
        {
            street += 2;
            knight += 1;
            village += 4;
            hijack += 3;
            guild += 1;
            cityWall += 1;
        }
        if (players.Count >= 5)
        {
            street += 2;
            knight += 2;
            village += 4;
            hijack += 3;
            guild += 1;
            church += 1;
        }
        if (players.Count >= 6)
        {
            street += 2;
            knight += 1;
            village += 4;
            hijack += 3;
            guild += 1;
            cityWall += 1;
        }

        for (var i = 0; i < cityHall; i++)
        {
            CityHallAvailable.Add(new CityHall());
        }
        for (var i = 0; i < cityWall; i++)
        {
            CityWallAvailable.Add(new CityWall());
        }
        for (var i = 0; i < library; i++)
        {
            LibraryAvailable.Add(new Library());
        }
        for (var i = 0; i < church; i++)
        {
            ChurchAvailable.Add(new Church());
        }
        for (var i = 0; i < university; i++)
        {
            UniversityAvailable.Add(new University());
        }
        for (var i = 0; i < village; i++)
        {
            VillageAvailable.Add(new Village());
        }
        for (var i = 0; i < street; i++)
        {
            StreetAvailable.Add(new Street());
        }
        for (var i = 0; i < knight; i++)
        {
            KnightAvailable.Add(new Knight());
        }
        for (var i = 0; i < guild; i++)
        {
            GuildAvailable.Add(new Guild());
        }
        for (var i = 0; i < hijack; i++)
        {
            var needsHijack = ThreadSafeRandom.ThisThreadsRandom.Next(VillageAvailable.Count);
            while (VillageAvailable[needsHijack].IsHijack)
            {
                needsHijack++;
                needsHijack %= VillageAvailable.Count;
            }
            VillageAvailable[needsHijack].IsHijack = true;
        }
        for(var i = 0; i < 5; i++)
        {
            var firstResource = DrawStack.First();
            DrawStack.Remove(firstResource);
            Market.Add(firstResource);
        }
        foreach (var p in players)
        {
            PlayerCards.Add(p, new());
            PlayerResources.Add(p, new());
            for (var i = 0; i < 3; i++)
            {
                var firstResource = DrawStack.First();
                DrawStack.Remove(firstResource);
                PlayerResources[p].Add(firstResource);
            }
            var villageFirst = VillageAvailable.First();
            PlayerCards[p].Add(villageFirst);
            VillageAvailable.Remove(villageFirst);
            var streetFirst = StreetAvailable.First();
            PlayerCards[p].Add(streetFirst);
            StreetAvailable.Remove(streetFirst);
            PlayerOrder.Add(p);
        }
        PlayerOrder.Sort(new BdayOrder());
        CurrentPlayer = PlayerOrder.First();
    }

    public int GetPoints(Player player) => PlayerCards[player].Sum(x => x.Points);
}

public class BdayOrder : IComparer<Player>
{
    public int Compare(Player? x, Player? y)
    {
        if (x?.BirthDay == y?.BirthDay) return 0;
        return x?.BirthDay < y?.BirthDay ? -1 : 1;
    }
}