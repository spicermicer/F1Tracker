using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1TournamentTracker.Misc
{
    static class LinqHelpers
    {
        public static int IndexOf<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {

            var index = 0;
            foreach (var item in source)
            {
                if (predicate.Invoke(item))
                {
                    return index;
                }
                index++;
            }

            return -1;
        }
    }
}
