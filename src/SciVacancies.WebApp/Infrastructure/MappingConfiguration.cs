using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using SciVacancies.WebApp.ViewModels;

namespace SciVacancies.WebApp.Infrastructure
{
    public static class MappingConfiguration
    {
        public static void Initialize()
        {
            /*Organization*/

            //Создание организации
            Mapper.CreateMap<Organization, OrganizationCreated>();
            //Обновление организации
            Mapper.CreateMap<Organization, OrganizationDataModel>();
            

            /*Researcher*/
            
            //Создание исследователя
            Mapper.CreateMap<Researcher, ResearcherCreated>();
            //Обновление исследователя
            Mapper.CreateMap<Researcher, ResearcherDataModel>();


            /*Vacancy*/

            //create position
            Mapper.CreateMap<PositionCreateViewModel, PositionDataModel>();
        }
    }
}
