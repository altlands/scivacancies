using System;
using AutoMapper;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
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
            //информация об организации
            Mapper.CreateMap<Organization, OrganizationDetailsViewModel>();

            /*Researcher*/

            //Создание исследователя
            Mapper.CreateMap<Researcher, ResearcherCreated>();
            //Обновление исследователя
            Mapper.CreateMap<Researcher, ResearcherDataModel>();
            //Информация об исследователе
            Mapper.CreateMap<Researcher, ResearcherDetailsViewModel>();


            /*Vacancy*/

            //create position
            Mapper.CreateMap<PositionCreateViewModel, PositionDataModel>();
            Mapper.CreateMap<PositionDataModel, VacancyDataModel>();
            
            
            /*VacancyApplication*/

            //create 
            Mapper.CreateMap<VacancyApplicationCreateViewModel, VacancyApplicationDataModel>();


            /*Account*/
            
            //researcher
            Mapper.CreateMap<AccountResearcherRegisterViewModel, ResearcherDataModel>()
                .ForMember(dest => dest.BirthDate, src => src.MapFrom(c=>new DateTime(c.BirthYear, 1,1)));
            //organization
            Mapper.CreateMap<AccountOrganizationRegisterViewModel, OrganizationDataModel>();
        }
    }
}
