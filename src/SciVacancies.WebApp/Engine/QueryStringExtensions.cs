using System;
using System.Collections.Generic;
using Microsoft.AspNet.Http;
using System.Linq;
using SciVacancies.WebApp.Engine;
using Microsoft.AspNet.WebUtilities;

namespace SciVacancies.WebApp
{
    /// <summary>
    /// расширение позволяет модифицировать строку запроса
    /// </summary>
    public static class QueryStringExtensions
    {
        /// <summary>
        /// получить список параметров в виде словаря
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IDictionary<string, string[]> ToQueryStringDictionary(this HttpRequest source)
        {
            return QueryHelpers.ParseQuery(source.QueryString.Value).ToDictionary(s => s.Key, s => s.Value.ToArray());
        }

        /// <summary>
        /// заменить (добавить) параметр в строке запроса
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parameterName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ToStringWithResetParameter(this HttpRequest source, string parameterName, string newValue)
        {
            return source.ToStringWithResetParameter(new Dictionary<string, string[]> { { parameterName, new[] { newValue } } });
        }

        /// <summary>
        /// заменить (добавить) параметр в строке запроса
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parameterName"></param>
        /// <param name="newValues"></param>
        /// <returns></returns>
        public static string ToStringWithResetParameter(this HttpRequest source, string parameterName, string[] newValues)
        {
            return source.ToStringWithResetParameter(new Dictionary<string, string[]> { { parameterName, newValues } });
        }

        /// <summary>
        /// заменить (добавить) параметр в строке запроса
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parametersSet"></param>
        /// <returns></returns>
        public static string ToStringWithResetParameter(this HttpRequest source, IDictionary<string, string> parametersSet)
        {
            return source.ToStringWithResetParameter(parametersSet.ToDictionary(parameter => parameter.Key, parameter => new[] { parameter.Value }));
        }

        /// <summary>
        /// заменить (добавить) параметр в строке запроса
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parametersSet"></param>
        /// <returns></returns>
        public static string ToStringWithResetParameter(this HttpRequest source, IDictionary<string, string[]> parametersSet)
        {
            var queryDictionary = source.ToQueryStringDictionary();

            foreach (var item in parametersSet)
            {
                if (!queryDictionary.Keys.Select(c => c.ToUpper()).Contains(item.Key.ToUpper()))
                    queryDictionary.Add(item.Key, item.Value);
            }

            return CreateString(queryDictionary);
        }

        /// <summary>
        /// собрать все параметры в Строку Запроса
        /// </summary>
        /// <param name="queryDictionary"></param>
        /// <returns></returns>
        public static string CreateString(this IDictionary<string, string[]> queryDictionary)
        {
            return
                queryDictionary.Aggregate(string.Empty,
                    (current1, item) =>
                        current1 + item.Value.Aggregate(string.Empty, (current, value) => current + $"&{item.Key}={value}"))
                    .Trim('&')
                    .Trim();
        }

        /// <summary>
        /// Задать значение сортировки по-умолчанию, либо изменить его. Обработка первого клика по Сортировке.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sortField"></param>
        /// <returns></returns>
        public static IDictionary<string, string[]> SetDefaultOrInvertOrderParameter(this IDictionary<string, string[]> source, string sortField)
        {
            //поле, по которому сорировать:

            //если параметра в списке нет
            if (source.Keys.Select(c => c.ToUpper()).Contains(ConstTerms.OrderFieldName.ToUpper()))
            {
                source.Remove(ConstTerms.OrderFieldName);
            }
            //добавить значение по-умолчанию
            source.Add(ConstTerms.OrderFieldName, new[] { sortField });


            //направление сортировки:

            const string parameter = ConstTerms.OrderDirectionFieldName;
            //если параметра в списке нет
            if (!source.Keys.Select(c => c.ToUpper()).Contains(parameter.ToUpper()))
            {
                //добавить значение по-умолчанию
                source.Add(parameter, new[] { ConstTerms.OrderByAscending });
            }
            else
            {
                //изменить значение на противоположное
                var value = source[parameter].First();
                source.Remove(parameter);
                if (value == ConstTerms.OrderByAscending)
                    source.Add(parameter, new[] { ConstTerms.OrderByDescending });
                else
                    source.Add(parameter, new[] { ConstTerms.OrderByAscending });

            }
            return source;
        }


        /// <summary>
        /// получить путь к иконке для сортировки по заданному полю. остальные игнорируются
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sortField">поле для которого нужно показывать (или не показывать) иконку</param>
        /// <returns></returns>
        public static string GetOrderIcon(this HttpRequest source, string sortField)
        {
            var queryDictionary = source.ToQueryStringDictionary();
            //если параметра в списке нет
            if (queryDictionary.ContainsKey(ConstTerms.OrderDirectionFieldName)
                && queryDictionary.ContainsKey(ConstTerms.OrderFieldName)
                && queryDictionary[ConstTerms.OrderFieldName].Contains(sortField))
            {
                if (queryDictionary[ConstTerms.OrderDirectionFieldName].Contains(ConstTerms.OrderByAscending))
                {
                    return "/images/icons/arrow-top-tab.png";
                }
                {
                    return "/images/icons/arrow-bottom-tab.png";
                }
            }
            return "/images/icons/arrow-vers-tab.png";
        }
        /// <summary>
        /// получить путь к иконке для сортировки
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string GetOrderIcon(this HttpRequest source)
        {
            var queryDictionary = source.ToQueryStringDictionary();
            //если параметра в списке нет
            if (queryDictionary.ContainsKey(ConstTerms.OrderDirectionFieldName)
                && queryDictionary[ConstTerms.OrderDirectionFieldName].Contains(ConstTerms.OrderByAscending))
            {
                return "/images/icons/arrow-top-tab.png";
            }
            return "/images/icons/arrow-bottom-tab.png";
        }
    }
}
