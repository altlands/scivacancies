using System;
using AutoMapper;
using NPoco;
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
            Mapper.CreateMap<OrganizationCreated, Organization>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.name, o => o.MapFrom(s => s.Data.Name))
                .ForMember(d => d.shortname, o => o.MapFrom(s => s.Data.ShortName))
                .ForMember(d => d.address, o => o.MapFrom(s => s.Data.Address))
                .ForMember(d => d.email, o => o.MapFrom(s => s.Data.Email))
                .ForMember(d => d.inn, o => o.MapFrom(s => s.Data.INN))
                .ForMember(d => d.ogrn, o => o.MapFrom(s => s.Data.OGRN))
                .ForMember(d => d.head_firstname, o => o.MapFrom(s => s.Data.HeadFirstName))
                .ForMember(d => d.head_secondname, o => o.MapFrom(s => s.Data.HeadSecondName))
                .ForMember(d => d.head_patronymic, o => o.MapFrom(s => s.Data.HeadPatronymic))
                .ForMember(d => d.foiv_id, o => o.MapFrom(s => s.Data.FoivId))
                .ForMember(d => d.orgform_id, o => o.MapFrom(s => s.Data.OrgFormId))
                .ForMember(d => d.creation_date, o => o.MapFrom(s => s.TimeStamp));
            //Обновление организации
            Mapper.CreateMap<Organization, OrganizationDataModel>();
            //информация об организации
            Mapper.CreateMap<Organization, OrganizationDetailsViewModel>();
            //индексация организации в поисковике
            Mapper.CreateMap<SciVacancies.ReadModel.ElasticSearchModel.Model.Organization, OrganizationDataModel>();

            /*Researcher*/

            //Создание исследователя
            Mapper.CreateMap<ResearcherCreated, Researcher>()
                .ForMember(d => d.firstname, o => o.MapFrom(s => s))
                .ForMember(d => d.firstname_eng, o => o.MapFrom(s => s))
                .ForMember(d => d.secondname, o => o.MapFrom(s => s))
                .ForMember(d => d.secondname_eng, o => o.MapFrom(s => s))
                .ForMember(d => d.patronymic, o => o.MapFrom(s => s))
                .ForMember(d => d.patronymic_eng, o => o.MapFrom(s => s))
                .ForMember(d => d.birthdate, o => o.MapFrom(s => s))
                .ForMember(d => d.email, o => o.MapFrom(s => s))
                .ForMember(d => d.extraemail, o => o.MapFrom(s => s))
                .ForMember(d => d.phone, o => o.MapFrom(s => s))
                .ForMember(d => d.extraphone, o => o.MapFrom(s => s))
                .ForMember(d => d.nationality, o => o.MapFrom(s => s))
                .ForMember(d => d.research_activity, o => o.MapFrom(s => s))
                .ForMember(d => d.teaching_activity, o => o.MapFrom(s => s))
                .ForMember(d => d.other_activity, o => o.MapFrom(s => s))
                .ForMember(d => d.science_degree, o => o.MapFrom(s => s))
                .ForMember(d => d.science_rank, o => o.MapFrom(s => s))
                .ForMember(d => d.rewards, o => o.MapFrom(s => s))
                .ForMember(d => d.memberships, o => o.MapFrom(s => s))
                .ForMember(d => d.conferences, o => o.MapFrom(s => s))
                .ForMember(d => d.creation_date, o => o.MapFrom(s => s));
            //Обновление исследователя
            Mapper.CreateMap<Researcher, ResearcherDataModel>();
            //Информация об исследователе
            Mapper.CreateMap<Researcher, ResearcherDetailsViewModel>();

            #region SearchSubscription

            Mapper.CreateMap<SearchSubscriptionCreated, SearchSubscription>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.SearchSubscriptionGuid))
                .ForMember(d => d.researcher_guid, o => o.MapFrom(s => s.ResearcherGuid))
                .ForMember(d => d.title, o => o.MapFrom(s => s.Data.Title))
                .ForMember(d => d.query, o => o.MapFrom(s => s.Data.Query))
                .ForMember(d => d.creation_date, o => o.MapFrom(s => s.TimeStamp));

            #endregion
            /*Vacancy*/

            //position
            //Mapper.CreateMap<PositionDataModel, VacancyDataModel>();
            //Mapper.CreateMap<Position, VacancyDataModel>();
            //Mapper.CreateMap<PositionCreateViewModel, PositionDataModel>();
            //Mapper.CreateMap<Position, PositionEditViewModel>();
            //Mapper.CreateMap<PositionEditViewModel, Position>();

            //vacancy
            Mapper.CreateMap<VacancyCreated, Vacancy>();

            Mapper.CreateMap<Vacancy, VacancyDetailsViewModel>();
            //индексация только что созданной вакансии в поисковике
            Mapper.CreateMap<SciVacancies.ReadModel.ElasticSearchModel.Model.Vacancy, VacancyPublished>();
            /*VacancyApplication*/

            //create 
            Mapper.CreateMap<VacancyApplicationCreateViewModel, VacancyApplicationDataModel>();
            Mapper.CreateMap<VacancyApplication, VacancyApplicationDetailsViewModel>();
            Mapper.CreateMap<Page<VacancyApplication>, Page<VacancyApplicationDetailsViewModel>>();
            Mapper.CreateMap<VacancyApplication, VacancyApplicationSetWinnerViewModel>();

            /*VacancyApplications*/

            Mapper.CreateMap<VacancyApplicationCreated, VacancyApplication>();

            /*Account*/

            //researcher
            Mapper.CreateMap<AccountResearcherRegisterViewModel, ResearcherDataModel>()
                .ForMember(dest => dest.BirthDate, src => src.MapFrom(c => new DateTime(c.BirthYear, 1, 1)));
            //organization
            Mapper.CreateMap<AccountOrganizationRegisterViewModel, OrganizationDataModel>();
        }
    }
}
