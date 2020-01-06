using System;
using System.Collections.Generic;
using System.Linq;
using Models.Interfaces;

namespace Models.Extensions
{
    public static class ListExtension
    {
        /// <summary>
        ///     ID Aware update entities
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="idSelector"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        // ReSharper disable once ParameterTypeCanBeEnumerable.Global
        public static List<T> IdAwareUpdate<T, TProperty>(this List<T> source, List<T> destination,
            Func<T, TProperty> idSelector)
            where T : class, IEntityUpdatable<T>
            where TProperty : IComparable<TProperty>
        {
            source ??= new List<T>();
            destination ??= new List<T>();

            // Apply addition
            foreach (var dtoPropValListItem in destination.Where(dtoPropValListItem =>
                !source.Any(entityPropValListItem =>
                    Equals(idSelector(entityPropValListItem), idSelector(dtoPropValListItem)))).ToList())
            {
                source.Add(dtoPropValListItem);
            }

            // Apply deletion
            foreach (var entityPropValListItem in source.Where(entityPropValListItem =>
                !destination.Any(dtoPropValListItem =>
                    Equals(idSelector(entityPropValListItem), idSelector(dtoPropValListItem)))).ToList())
            {
                source.Remove(entityPropValListItem);
            }

            // Update each value one-by-one
            source.Sort((x, y) => idSelector(x).CompareTo(idSelector(y)));
            destination.Sort((x, y) => idSelector(x).CompareTo(idSelector(y)));

            for (var index = 0; index < source.Count; index++)
            {
                var entity = source[index];
                var match = destination.FirstOrDefault(x => Equals(idSelector(x), idSelector(entity)));

                if (match != null)
                {
                    source[index] = entity switch
                    {
                        IEntityUpdatable<T> updatable => updatable.Update(match),
                        _ => match
                    };
                }
            }

            return source;
        }
    }
}