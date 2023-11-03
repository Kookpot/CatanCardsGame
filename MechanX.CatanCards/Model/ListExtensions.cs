namespace MechanX.CatanCards.Model;

internal static class ListExtensions
{
    internal static void Shuffle<T>(this IList<T> list)
    {
        var n = list.Count;
        while (n > 1)
        {
            n--;
            var k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}