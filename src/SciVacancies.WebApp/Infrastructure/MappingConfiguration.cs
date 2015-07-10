using System;
using AutoMapper;
using NPoco;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.ViewModels;
using SciVacancies.WebApp.Models.OAuth;

namespace SciVacancies.WebApp.Infrastructure
{
    public static class MappingConfiguration
    {
        public static void Initialize()
        {
            /*pagers*/
            Mapper.CreateMap<Organization, Organization>().IncludePagedResultMapping();
            Mapper.CreateMap<Researcher, Researcher>().IncludePagedResultMapping();
            Mapper.CreateMap<Vacancy, Vacancy>().IncludePagedResultMapping();
            Mapper.CreateMap<ReadModel.ElasticSearchModel.Model.Vacancy, ReadModel.ElasticSearchModel.Model.Vacancy>().IncludePagedResultMapping();
            Mapper.CreateMap<Notification, Notification>().IncludePagedResultMapping();
            Mapper.CreateMap<SearchSubscription, SearchSubscription>().IncludePagedResultMapping();


            /*Account*/

            //researcher
            Mapper.CreateMap<AccountResearcherRegisterViewModel, ResearcherDataModel>()
                .ForMember(dest => dest.BirthDate, src => src.MapFrom(c => new DateTime(c.BirthYear, 1, 1)));
            //organization
            Mapper.CreateMap<AccountOrganizationRegisterViewModel, OrganizationDataModel>();
            Mapper.CreateMap<OAuthOrgInformation, AccountOrganizationRegisterViewModel>();

            /*Organization*/

            //Создание организации
            Mapper.CreateMap<Organization, OrganizationCreated>();
            //Обновление организации
            Mapper.CreateMap<Organization, OrganizationDataModel>();
            //информация об организации
            Mapper.CreateMap<Organization, OrganizationDetailsViewModel>();
            //индексация организации в поисковике
            Mapper.CreateMap<ReadModel.ElasticSearchModel.Model.Organization, OrganizationDataModel>();

            /*Researcher*/

            Mapper.CreateMap<Researcher, ResearcherCreated>();
            Mapper.CreateMap<Researcher, ResearcherDataModel>();
            Mapper.CreateMap<ResearcherEditViewModel, ResearcherDataModel>();
            Mapper.CreateMap<Researcher, ResearcherDetailsViewModel>();
            Mapper.CreateMap<Researcher, ResearcherEditViewModel>();

            //education

            Mapper.CreateMap<Education, EducationEditViewModel>()
                .ForMember(dest => dest.GraduationYear, src => src.MapFrom(c => c.GraduationYear.HasValue ? c.GraduationYear.Value.Year : 0));
            Mapper.CreateMap<EducationEditViewModel, SciVacancies.Domain.Core.Education>()
                .ForMember(dest => dest.GraduationYear, src => src.MapFrom(c => (c.GraduationYear.HasValue && c.GraduationYear.Value != 0) ? new DateTime(c.GraduationYear.Value, 1, 1) : default(DateTime)));

            //piblication
            Mapper.CreateMap<Publication, PublicationEditViewModel>();
            Mapper.CreateMap<PublicationEditViewModel, SciVacancies.Domain.Core.Publication>();


            /*Vacancy*/

            //vacancy
            Mapper.CreateMap<Vacancy, VacancyCreateViewModel>();
            Mapper.CreateMap<Vacancy, VacancyDetailsViewModel>();
            Mapper.CreateMap<Vacancy, VacancyDataModel>();
            Mapper.CreateMap<VacancyCreateViewModel, Vacancy>();
            Mapper.CreateMap<VacancyCreateViewModel, VacancyDataModel>();
            //индексация только что созданной вакансии в поисковике
            Mapper.CreateMap<ReadModel.ElasticSearchModel.Model.Vacancy, VacancyPublished>();


            /*VacancyApplication*/

            //create 
            Mapper.CreateMap<VacancyApplicationCreateViewModel, VacancyApplicationDataModel>();
            Mapper.CreateMap<VacancyApplication, VacancyApplicationDetailsViewModel>().IncludePagedResultMapping();
            Mapper.CreateMap<Page<VacancyApplication>, Page<VacancyApplicationDetailsViewModel>>();
            Mapper.CreateMap<VacancyApplication, VacancyApplicationSetWinnerViewModel>();


            /*dictionaties*/
            Mapper.CreateMap<Foiv, FoivViewModel>();
        }
    }
}
