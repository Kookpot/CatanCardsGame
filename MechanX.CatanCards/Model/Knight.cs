namespace MechanX.CatanCards.Model;

public class Knight : Card
{
    public bool SideB { get; set; }
    public override int Points => SideB ? 1 : 0;
    public override CardType CardType => CardType.Knight;
}