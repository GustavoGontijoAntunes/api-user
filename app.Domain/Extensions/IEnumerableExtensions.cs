using app.Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace app.Domain.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> enumerable, string message = "EmptySearch")
        {
            if (enumerable is null || !enumerable.Any())
            {
                throw new DomainException(message);
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void ThrowIfInvalid<T>(
            this List<T> enumerable,
            AbstractValidator<T> validator)
        {
            var validationResult = new List<string>();

            foreach (var item in enumerable)
            {
                ValidationResult results = validator.Validate(item);

                if (!results.IsValid)
                {
                    foreach (var failure in results.Errors)
                    {
                        validationResult.Add(failure.ErrorMessage);
                    }
                }
            }

            if (validationResult.Any())
            {
                throw new DomainException(validationResult.Distinct().ToList());
            }
        }
    }
}