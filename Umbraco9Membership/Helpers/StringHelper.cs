using System;
using System.Collections.Generic;
using System.Linq;

namespace Umbraco9Membership.Helpers
{
    public static class StringHelper
    {
        public static List<string> SortListOfStrings(string sortOrder, List<string> itemsToSort)
        {
            if (!itemsToSort.Any() || string.IsNullOrWhiteSpace(sortOrder)) return null;

            var sortOrderArray =
                sortOrder.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x)).ToArray();

            return SortListOfStrings(sortOrderArray, itemsToSort);
        }

        public static List<string> SortListOfStrings(int[] sortOrderArray, List<string> itemsToSort)
        {
            if (sortOrderArray == null || !sortOrderArray.Any() || itemsToSort == null || !itemsToSort.Any()) return null;

            var sortedArray = new List<string>();

            var numberOfItems = itemsToSort.Count;
            foreach (var index in sortOrderArray)
            {
                if (index < numberOfItems)
                {
                    sortedArray.Add(itemsToSort[index]);
                }
            }

            return sortedArray;
        }
    }
}
