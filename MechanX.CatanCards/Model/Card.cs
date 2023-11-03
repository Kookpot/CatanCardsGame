namespace MechanX.CatanCards.Model;

public abstract class Card
{
    public bool BackSide { get; internal set; }
    public abstract int Points { get; }
    public abstract CardType CardType { get; }
    public bool Tapped { get; internal set; }
}