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
            #region Organization

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

            #endregion

            #region Researcher

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

            #endregion

            #region SearchSubscription

            Mapper.CreateMap<SearchSubscriptionCreated, SearchSubscription>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.SearchSubscriptionGuid))
                .ForMember(d => d.researcher_guid, o => o.MapFrom(s => s.ResearcherGuid))
                .ForMember(d => d.title, o => o.MapFrom(s => s.Data.Title))
                .ForMember(d => d.query, o => o.MapFrom(s => s.Data.Query))
                .ForMember(d => d.creation_date, o => o.MapFrom(s => s.TimeStamp));

            #endregion

            #region Vacancy

            Mapper.CreateMap<VacancyCreated, Vacancy>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.VacancyGuid))
                .ForMember(d => d.name, o => o.MapFrom(s => s.Data.Name))
                .ForMember(d => d.fullname, o => o.MapFrom(s => s.Data.FullName))
                .ForMember(d => d.tasks, o => o.MapFrom(s => s.Data.Tasks))
                .ForMember(d => d.researchtheme, o => o.MapFrom(s => s.Data.ResearchTheme))
                .ForMember(d => d.cityname, o => o.MapFrom(s => s.Data.CityName))
                .ForMember(d => d.details, o => o.MapFrom(s => s.Data.Details))
                .ForMember(d => d.contact_name, o => o.MapFrom(s => s.Data.ContactName))
                .ForMember(d => d.contact_email, o => o.MapFrom(s => s.Data.ContactEmail))
                .ForMember(d => d.contact_phone, o => o.MapFrom(s => s.Data.ContactPhone))
                .ForMember(d => d.contact_details, o => o.MapFrom(s => s.Data.ContactDetails))
                .ForMember(d => d.salary_from, o => o.MapFrom(s => s.Data.SalaryFrom))
                .ForMember(d => d.salary_to, o => o.MapFrom(s => s.Data.SalaryTo))
                .ForMember(d => d.contract_type, o => o.MapFrom(s => s.Data.ContractType))
                .ForMember(d => d.contract_time, o => o.MapFrom(s => s.Data.ContractTime))
                .ForMember(d => d.employment_type, o => o.MapFrom(s => s.Data.EmploymentType))
                .ForMember(d => d.operatingschedule_type, o => o.MapFrom(s => s.Data.OperatingScheduleType))
                .ForMember(d => d.socialpackage, o => o.MapFrom(s => s.Data.SocialPackage))
                .ForMember(d => d.rent, o => o.MapFrom(s => s.Data.Rent))
                .ForMember(d => d.officeaccomodation, o => o.MapFrom(s => s.Data.OfficeAccomodation))
                .ForMember(d => d.transportcompensation, o => o.MapFrom(s => s.Data.TransportCompensation))
                .ForMember(d => d.positiontype_id, o => o.MapFrom(s => s.Data.PositionTypeId))
                .ForMember(d => d.region_id, o => o.MapFrom(s => s.Data.RegionId))
                .ForMember(d => d.researchdirection_id, o => o.MapFrom(s => s.Data.ResearchDirectionId))
                .ForMember(d => d.criterias, o => o.MapFrom(s => s.Data.Criterias))
                .ForMember(d => d.organization_guid, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.creation_date, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<Vacancy, VacancyDetailsViewModel>();
            //индексация только что созданной вакансии в поисковике
            Mapper.CreateMap<SciVacancies.ReadModel.ElasticSearchModel.Model.Vacancy, VacancyPublished>();

            #endregion

            #region VacancyApplication

            Mapper.CreateMap<VacancyApplicationCreated, VacancyApplication>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.VacancyApplicationGuid))
                .ForMember(d => d.researcher_fullname, o => o.MapFrom(s => s.Data.ResearcherFullName))
                .ForMember(d => d.position_name, o => o.MapFrom(s => s.Data.PositionName))
                .ForMember(d => d.email, o => o.MapFrom(s => s.Data.Email))
                .ForMember(d => d.extraemail, o => o.MapFrom(s => s.Data.ExtraEmail))
                .ForMember(d => d.phone, o => o.MapFrom(s => s.Data.Phone))
                .ForMember(d => d.extraphone, o => o.MapFrom(s => s.Data.ExtraPhone))
                .ForMember(d => d.research_activity, o => o.MapFrom(s => s.Data.ResearchActivity))
                .ForMember(d => d.teaching_activity, o => o.MapFrom(s => s.Data.TeachingActivity))
                .ForMember(d => d.other_activity, o => o.MapFrom(s => s.Data.OtherActivity))
                .ForMember(d => d.science_degree, o => o.MapFrom(s => s.Data.ScienceDegree))
                .ForMember(d => d.science_rank, o => o.MapFrom(s => s.Data.ScienceRank))
                .ForMember(d => d.rewards, o => o.MapFrom(s => s.Data.Rewards))
                .ForMember(d => d.memberships, o => o.MapFrom(s => s.Data.Memberships))
                .ForMember(d => d.conferences, o => o.MapFrom(s => s.Data.Conferences))
                .ForMember(d => d.educations, o => o.MapFrom(s => s.Data.Educations))
                .ForMember(d => d.publications, o => o.MapFrom(s => s.Data.Publications))
                .ForMember(d => d.vacancy_guid, o => o.MapFrom(s => s.VacancyGuid))
                .ForMember(d => d.researcher_guid, o => o.MapFrom(s => s.ResearcherGuid))
                .ForMember(d => d.attachments, o => o.MapFrom(s => s.Data.Attachments))
                .ForMember(d => d.creation_date, o => o.MapFrom(s => s.TimeStamp));

            //create 
            Mapper.CreateMap<VacancyApplicationCreateViewModel, VacancyApplicationDataModel>();
            Mapper.CreateMap<VacancyApplication, VacancyApplicationDetailsViewModel>();
            Mapper.CreateMap<Page<VacancyApplication>, Page<VacancyApplicationDetailsViewModel>>();
            Mapper.CreateMap<VacancyApplication, VacancyApplicationSetWinnerViewModel>();

            #endregion

            #region Account

            //researcher
            Mapper.CreateMap<AccountResearcherRegisterViewModel, ResearcherDataModel>()
                .ForMember(dest => dest.BirthDate, src => src.MapFrom(c => new DateTime(c.BirthYear, 1, 1)));
            //organization
            Mapper.CreateMap<AccountOrganizationRegisterViewModel, OrganizationDataModel>();

            #endregion
        }
    }
}
