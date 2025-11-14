namespace ScrabbleScorer.Core.Utilities;

public static class CombinationUtils
{
    // Get all subsets (the power set)
    public static IEnumerable<List<T>> GetAllCombinations<T>(IList<T> items)
    {
        var count = items.Count;
        var combinationCount = 1 << count; // 2^count

        for (var i = 0; i < combinationCount; i++)
        {
            var combination = new List<T>();
            for (var j = 0; j < count; j++)
                if ((i & (1 << j)) != 0)
                    combination.Add(items[j]);
            yield return combination;
        }
    }

    // Get all permutations of a given list
    public static IEnumerable<List<T>> GetPermutations<T>(IList<T> items)
    {
        if (items.Count <= 1)
        {
            yield return new List<T>(items);
            yield break;
        }

        for (var i = 0; i < items.Count; i++)
        {
            var current = items[i];
            var remaining = items.Where((_, idx) => idx != i).ToList();

            foreach (var perm in GetPermutations(remaining))
            {
                perm.Insert(0, current);
                yield return perm;
            }
        }
    }

    // Combine both: permutations of all subsets
    public static IEnumerable<List<T>> GetAllPermutationsOfAllSubsets<T>(IList<T> items)
    {
        foreach (var subset in GetAllCombinations(items))
        // include empty set (optional, can skip if undesired)
        foreach (var perm in GetPermutations(subset))
            yield return perm;
    }
}
