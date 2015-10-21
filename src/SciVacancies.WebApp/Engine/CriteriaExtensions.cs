using System;
using System.Collections.Generic;
using System.Linq;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Engine
{
    public static class CriteriaExtensions
    {
        /// <summary>
        /// создать иерархический список критериев
        /// </summary>
        /// <param name="source">список критериев</param>
        /// <param name="usedCriterias">критерии, использованные в Вакансии</param>
        /// <returns></returns>
        public static List<CriteriaItemViewModel> ToHierarchyCriteriaViewModelList(this List<Criteria> source, List<VacancyCriteria> usedCriterias)
        {
            var result = new List<CriteriaItemViewModel>();

            result.AddRange(source.Where(c => !c.parent_id.HasValue || c.parent_id.Value == 0).Select(c => new CriteriaItemViewModel
            {
                Id = c.id,
                Title = c.title
            }));

            result.ForEach(c =>
            {
                c.Items =
                    source.Where(d => d.parent_id.HasValue && d.parent_id.Value == c.Id)
                        .Select(d => new CriteriaItemViewModel
                        {
                            Id = d.id,
                            ParentId = d.parent_id,
                            Code = d.code,
                            Title = d.title
                        })
                        .ToList();
                c.Items.ForEach(d =>
                {
                    if (usedCriterias != null && usedCriterias.Any(e => e.criteria_id == d.Id))
                    {
                        d.Count = usedCriterias.First(e => e.criteria_id == d.Id).count ?? 0;
                    }
                });
            }
            );

            return result;
        }
    }
}
