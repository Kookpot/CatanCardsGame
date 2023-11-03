namespace MechanX.CatanCards.Model;

public class Village : Card
{
    public bool IsHijack { get; internal set; }
    public override int Points => BackSide ? 2 : 1;
    public override CardType CardType => BackSide ? CardType.City : CardType.Village;
}