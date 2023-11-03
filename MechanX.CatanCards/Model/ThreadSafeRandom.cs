namespace MechanX.CatanCards.Model;

internal static class ThreadSafeRandom
{
    [ThreadStatic] private static Random? Local;

    internal static Random ThisThreadsRandom
    {
        get { return Local ??= new Random(unchecked(Environment.TickCount * 31 + Environment.CurrentManagedThreadId)); }
    }
}