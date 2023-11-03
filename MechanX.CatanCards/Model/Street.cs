namespace MechanX.CatanCards.Model;

public class Street : Card
{
    public override int Points => BackSide ? 1 : 0;
    public override CardType CardType => CardType.Street;
}