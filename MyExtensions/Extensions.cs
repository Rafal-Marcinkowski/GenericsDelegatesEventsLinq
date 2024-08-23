using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GenericsDelegatesEventsLinq.MyExtensions
{
    public static class Extensions
    {
        /// <summary>
        /// Method CheckAllElementsForPredicate works same as LINQ All Method, or almost same, but just for List<T>
        /// </summary>
        public static bool CheckAllElementsForPredicate<T>(this List<T> senderList, Predicate<T> predicate)
        {
            if (senderList == null || predicate==null)
            {
                throw new Exception();
            }
            foreach(var item in senderList)
            {
                if(!predicate(item))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Method CheckAllElementsForPredicate works same as LINQ Any Method, or almost same.
        /// </summary>
        public static bool CheckAnyElementForPredicate<TSource>(this IEnumerable<TSource> senderList, Predicate<TSource> predicate)
        {
            if (senderList == null || predicate == null)
            {
                throw new Exception();
            }
            foreach (var item in senderList)
            {
                if(predicate(item))
                {
                    return true;
                }
            }
            return false;
        }    
    }
}
