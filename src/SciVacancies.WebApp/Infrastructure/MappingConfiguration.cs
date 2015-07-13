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
            Mapper.CreateMap<OrganizationNotification, OrganizationNotification>().IncludePagedResultMapping();
            Mapper.CreateMap<ResearcherNotification, ResearcherNotification>().IncludePagedResultMapping();
            Mapper.CreateMap<SearchSubscription, SearchSubscription>().IncludePagedResultMapping();


            /*Account*/

            //researcher
            Mapper.CreateMap<AccountResearcherRegisterViewModel, ResearcherDataModel>()
                .ForMember(dest => dest.BirthDate, src => src.MapFrom(c => new DateTime(c.BirthYear, 1, 1)));
            //organization
            Mapper.CreateMap<AccountOrganizationRegisterViewModel, OrganizationDataModel>();
            Mapper.CreateMap<OAuthOrgInformation, AccountOrganizationRegisterViewModel>();

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
                .ForMember(d => d.researchdirections, o => o.MapFrom(s => s.Data.ResearchDirections))
                .ForMember(d => d.creation_date, o => o.MapFrom(s => s.TimeStamp));
            //Обновление организации
            Mapper.CreateMap<OrganizationUpdated, Organization>()
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
                .ForMember(d => d.researchdirections, o => o.MapFrom(s => s.Data.ResearchDirections))
                .ForMember(d => d.update_date, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<Domain.Core.ResearchDirection, ReadModel.Core.ResearchDirection>()
                .ForMember(d => d.id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.parent_id, o => o.MapFrom(s => s.ParentId))
                .ForMember(d => d.title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.title_eng, o => o.MapFrom(s => s.TitleEng))
                .ForMember(d => d.lft, o => o.MapFrom(s => s.Lft))
                .ForMember(d => d.rgt, o => o.MapFrom(s => s.Rgt))
                .ForMember(d => d.lvl, o => o.MapFrom(s => s.Lvl))
                .ForMember(d => d.oecd_code, o => o.MapFrom(s => s.OecdCode))
                .ForMember(d => d.wos_code, o => o.MapFrom(s => s.WosCode))
                .ForMember(d => d.root_id, o => o.MapFrom(s => s.RootId));

            Mapper.CreateMap<Organization, OrganizationDataModel>();
            //информация об организации
            Mapper.CreateMap<Organization, OrganizationDetailsViewModel>();
            //индексация организации в поисковике
            Mapper.CreateMap<ReadModel.ElasticSearchModel.Model.Organization, OrganizationDataModel>();

            #endregion

            #region Elastic

            Mapper.CreateMap<OrganizationCreated, ReadModel.ElasticSearchModel.Model.Organization>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Data.Name))
                .ForMember(d => d.ShortName, o => o.MapFrom(s => s.Data.ShortName))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Data.Address))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Data.Email))
                .ForMember(d => d.INN, o => o.MapFrom(s => s.Data.INN))
                .ForMember(d => d.OGRN, o => o.MapFrom(s => s.Data.OGRN))
                .ForMember(d => d.HeadFirstName, o => o.MapFrom(s => s.Data.HeadFirstName))
                .ForMember(d => d.HeadSecondName, o => o.MapFrom(s => s.Data.HeadSecondName))
                .ForMember(d => d.HeadPatronymic, o => o.MapFrom(s => s.Data.HeadPatronymic))
                .ForMember(d => d.Foiv, o => o.MapFrom(s => s.Data.Foiv))
                .ForMember(d => d.FoivId, o => o.MapFrom(s => s.Data.FoivId))
                .ForMember(d => d.OrgForm, o => o.MapFrom(s => s.Data.OrgForm))
                .ForMember(d => d.OrgFormId, o => o.MapFrom(s => s.Data.OrgFormId))
                .ForMember(d => d.ResearchDirections, o => o.MapFrom(s => s.Data.ResearchDirections))
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<OrganizationUpdated, ReadModel.ElasticSearchModel.Model.Organization>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Data.Name))
                .ForMember(d => d.ShortName, o => o.MapFrom(s => s.Data.ShortName))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Data.Address))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Data.Email))
                .ForMember(d => d.INN, o => o.MapFrom(s => s.Data.INN))
                .ForMember(d => d.OGRN, o => o.MapFrom(s => s.Data.OGRN))
                .ForMember(d => d.HeadFirstName, o => o.MapFrom(s => s.Data.HeadFirstName))
                .ForMember(d => d.HeadSecondName, o => o.MapFrom(s => s.Data.HeadSecondName))
                .ForMember(d => d.HeadPatronymic, o => o.MapFrom(s => s.Data.HeadPatronymic))
                .ForMember(d => d.Foiv, o => o.MapFrom(s => s.Data.Foiv))
                .ForMember(d => d.FoivId, o => o.MapFrom(s => s.Data.FoivId))
                .ForMember(d => d.OrgForm, o => o.MapFrom(s => s.Data.OrgForm))
                .ForMember(d => d.OrgFormId, o => o.MapFrom(s => s.Data.OrgFormId))
                .ForMember(d => d.ResearchDirections, o => o.MapFrom(s => s.Data.ResearchDirections))
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<Domain.Core.ResearchDirection, ReadModel.ElasticSearchModel.Model.ResearchDirection>();

            Mapper.CreateMap<VacancyCreated, ReadModel.ElasticSearchModel.Model.Vacancy>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.VacancyGuid))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Data.Name))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Data.FullName))
                .ForMember(d => d.Tasks, o => o.MapFrom(s => s.Data.Tasks))
                .ForMember(d => d.ResearchTheme, o => o.MapFrom(s => s.Data.ResearchTheme))
                .ForMember(d => d.CityName, o => o.MapFrom(s => s.Data.CityName))
                .ForMember(d => d.Details, o => o.MapFrom(s => s.Data.Details))
                .ForMember(d => d.ContactName, o => o.MapFrom(s => s.Data.ContactName))
                .ForMember(d => d.ContactEmail, o => o.MapFrom(s => s.Data.ContactEmail))
                .ForMember(d => d.ContactPhone, o => o.MapFrom(s => s.Data.ContactPhone))
                .ForMember(d => d.ContactDetails, o => o.MapFrom(s => s.Data.ContactDetails))
                .ForMember(d => d.SalaryFrom, o => o.MapFrom(s => s.Data.SalaryFrom))
                .ForMember(d => d.SalaryTo, o => o.MapFrom(s => s.Data.SalaryTo))
                .ForMember(d => d.Bonuses, o => o.MapFrom(s => s.Data.Bonuses))
                .ForMember(d => d.ContractType, o => o.MapFrom(s => s.Data.ContractType))
                .ForMember(d => d.ContractTime, o => o.MapFrom(s => s.Data.ContractTime))
                .ForMember(d => d.EmploymentType, o => o.MapFrom(s => s.Data.EmploymentType))
                .ForMember(d => d.OperatingScheduleType, o => o.MapFrom(s => s.Data.OperatingScheduleType))
                .ForMember(d => d.SocialPackage, o => o.MapFrom(s => s.Data.SocialPackage))
                .ForMember(d => d.Rent, o => o.MapFrom(s => s.Data.Rent))
                .ForMember(d => d.OfficeAccomodation, o => o.MapFrom(s => s.Data.OfficeAccomodation))
                .ForMember(d => d.TransportCompensation, o => o.MapFrom(s => s.Data.TransportCompensation))
                .ForMember(d => d.PositionType, o => o.MapFrom(s => s.Data.PositionType))
                .ForMember(d => d.PositionTypeId, o => o.MapFrom(s => s.Data.PositionTypeId))
                .ForMember(d => d.Region, o => o.MapFrom(s => s.Data.Region))
                .ForMember(d => d.RegionId, o => o.MapFrom(s => s.Data.RegionId))
                .ForMember(d => d.ResearchDirection, o => o.MapFrom(s => s.Data.ResearchDirection))
                .ForMember(d => d.ResearchDirectionId, o => o.MapFrom(s => s.Data.ResearchDirectionId))
                .ForMember(d => d.Criterias, o => o.MapFrom(s => s.Data.Criterias))
                .ForMember(d => d.OrganizationGuid, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<VacancyUpdated, ReadModel.ElasticSearchModel.Model.Vacancy>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.VacancyGuid))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Data.Name))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.Data.FullName))
                .ForMember(d => d.Tasks, o => o.MapFrom(s => s.Data.Tasks))
                .ForMember(d => d.ResearchTheme, o => o.MapFrom(s => s.Data.ResearchTheme))
                .ForMember(d => d.CityName, o => o.MapFrom(s => s.Data.CityName))
                .ForMember(d => d.Details, o => o.MapFrom(s => s.Data.Details))
                .ForMember(d => d.ContactName, o => o.MapFrom(s => s.Data.ContactName))
                .ForMember(d => d.ContactEmail, o => o.MapFrom(s => s.Data.ContactEmail))
                .ForMember(d => d.ContactPhone, o => o.MapFrom(s => s.Data.ContactPhone))
                .ForMember(d => d.ContactDetails, o => o.MapFrom(s => s.Data.ContactDetails))
                .ForMember(d => d.SalaryFrom, o => o.MapFrom(s => s.Data.SalaryFrom))
                .ForMember(d => d.SalaryTo, o => o.MapFrom(s => s.Data.SalaryTo))
                .ForMember(d => d.Bonuses, o => o.MapFrom(s => s.Data.Bonuses))
                .ForMember(d => d.ContractType, o => o.MapFrom(s => s.Data.ContractType))
                .ForMember(d => d.ContractTime, o => o.MapFrom(s => s.Data.ContractTime))
                .ForMember(d => d.EmploymentType, o => o.MapFrom(s => s.Data.EmploymentType))
                .ForMember(d => d.OperatingScheduleType, o => o.MapFrom(s => s.Data.OperatingScheduleType))
                .ForMember(d => d.SocialPackage, o => o.MapFrom(s => s.Data.SocialPackage))
                .ForMember(d => d.Rent, o => o.MapFrom(s => s.Data.Rent))
                .ForMember(d => d.OfficeAccomodation, o => o.MapFrom(s => s.Data.OfficeAccomodation))
                .ForMember(d => d.TransportCompensation, o => o.MapFrom(s => s.Data.TransportCompensation))
                .ForMember(d => d.PositionType, o => o.MapFrom(s => s.Data.PositionType))
                .ForMember(d => d.PositionTypeId, o => o.MapFrom(s => s.Data.PositionTypeId))
                .ForMember(d => d.Region, o => o.MapFrom(s => s.Data.Region))
                .ForMember(d => d.RegionId, o => o.MapFrom(s => s.Data.RegionId))
                .ForMember(d => d.ResearchDirection, o => o.MapFrom(s => s.Data.ResearchDirection))
                .ForMember(d => d.ResearchDirectionId, o => o.MapFrom(s => s.Data.ResearchDirectionId))
                .ForMember(d => d.Criterias, o => o.MapFrom(s => s.Data.Criterias))
                .ForMember(d => d.OrganizationGuid, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.UpdateDate, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<Domain.Core.VacancyCriteria, ReadModel.ElasticSearchModel.Model.VacancyCriteria>();

            #endregion


            #region Researcher

            //Создание исследователя
            Mapper.CreateMap<ResearcherCreated, Researcher>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.ResearcherGuid))
                .ForMember(d => d.firstname, o => o.MapFrom(s => s.Data.FirstName))
                .ForMember(d => d.firstname_eng, o => o.MapFrom(s => s.Data.FirstNameEng))
                .ForMember(d => d.secondname, o => o.MapFrom(s => s.Data.SecondName))
                .ForMember(d => d.secondname_eng, o => o.MapFrom(s => s.Data.SecondNameEng))
                .ForMember(d => d.patronymic, o => o.MapFrom(s => s.Data.Patronymic))
                .ForMember(d => d.patronymic_eng, o => o.MapFrom(s => s.Data.PatronymicEng))
                .ForMember(d => d.birthdate, o => o.MapFrom(s => s.Data.BirthDate))
                .ForMember(d => d.email, o => o.MapFrom(s => s.Data.Email))
                .ForMember(d => d.extraemail, o => o.MapFrom(s => s.Data.ExtraEmail))
                .ForMember(d => d.phone, o => o.MapFrom(s => s.Data.Phone))
                .ForMember(d => d.extraphone, o => o.MapFrom(s => s.Data.ExtraPhone))
                .ForMember(d => d.nationality, o => o.MapFrom(s => s.Data.Nationality))
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
                .ForMember(d => d.creation_date, o => o.MapFrom(s => s.TimeStamp));
            //Обновление исследователя
            Mapper.CreateMap<ResearcherUpdated, Researcher>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.ResearcherGuid))
                .ForMember(d => d.firstname, o => o.MapFrom(s => s.Data.FirstName))
                .ForMember(d => d.firstname_eng, o => o.MapFrom(s => s.Data.FirstNameEng))
                .ForMember(d => d.secondname, o => o.MapFrom(s => s.Data.SecondName))
                .ForMember(d => d.secondname_eng, o => o.MapFrom(s => s.Data.SecondNameEng))
                .ForMember(d => d.patronymic, o => o.MapFrom(s => s.Data.Patronymic))
                .ForMember(d => d.patronymic_eng, o => o.MapFrom(s => s.Data.PatronymicEng))
                .ForMember(d => d.birthdate, o => o.MapFrom(s => s.Data.BirthDate))
                .ForMember(d => d.email, o => o.MapFrom(s => s.Data.Email))
                .ForMember(d => d.extraemail, o => o.MapFrom(s => s.Data.ExtraEmail))
                .ForMember(d => d.phone, o => o.MapFrom(s => s.Data.Phone))
                .ForMember(d => d.extraphone, o => o.MapFrom(s => s.Data.ExtraPhone))
                .ForMember(d => d.nationality, o => o.MapFrom(s => s.Data.Nationality))
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
                .ForMember(d => d.update_date, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<Domain.Core.Education, ReadModel.Core.Education>()
                .ForMember(d => d.city, o => o.MapFrom(s => s.City))
                .ForMember(d => d.university_shortname, o => o.MapFrom(s => s.UniversityShortName))
                .ForMember(d => d.faculty_shortname, o => o.MapFrom(s => s.FacultyShortName))
                .ForMember(d => d.graduation_date, o => o.MapFrom(s => s.GraduationYear))
                .ForMember(d => d.degree, o => o.MapFrom(s => s.Degree));
            Mapper.CreateMap<Domain.Core.Publication, ReadModel.Core.Publication>()
                .ForMember(d => d.title, o => o.MapFrom(s => s.Title));

            Mapper.CreateMap<Researcher, ResearcherDataModel>();
            Mapper.CreateMap<ResearcherEditViewModel, ResearcherDataModel>();
            Mapper.CreateMap<Researcher, ResearcherDetailsViewModel>();
            Mapper.CreateMap<Researcher, ResearcherEditViewModel>();

            //education

            Mapper.CreateMap<Education, EducationEditViewModel>()
                .ForMember(dest => dest.GraduationYear, src => src.MapFrom(c => c.graduation_date.HasValue ? c.graduation_date.Value.Year : 0));
            Mapper.CreateMap<EducationEditViewModel, SciVacancies.Domain.Core.Education>()
                .ForMember(dest => dest.GraduationYear, src => src.MapFrom(c => (c.GraduationYear.HasValue && c.GraduationYear.Value != 0) ? new DateTime(c.GraduationYear.Value, 1, 1) : default(DateTime)));

            //piblication
            Mapper.CreateMap<Publication, PublicationEditViewModel>();
            Mapper.CreateMap<PublicationEditViewModel, SciVacancies.Domain.Core.Publication>();

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

            Mapper.CreateMap<VacancyUpdated, Vacancy>()
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
                .ForMember(d => d.update_date, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<Domain.Core.VacancyCriteria, ReadModel.Core.VacancyCriteria>()
                .ForMember(d => d.criteria_id, o => o.MapFrom(s => s.CriteriaId))
                .ForMember(d => d.from, o => o.MapFrom(s => s.From))
                .ForMember(d => d.to, o => o.MapFrom(s => s.To));

            //vacancy
            Mapper.CreateMap<Vacancy, VacancyCreateViewModel>();
            Mapper.CreateMap<Vacancy, VacancyDetailsViewModel>();
            Mapper.CreateMap<Vacancy, VacancyDataModel>();
            Mapper.CreateMap<VacancyCreateViewModel, Vacancy>();
            Mapper.CreateMap<VacancyCreateViewModel, VacancyDataModel>();
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

            Mapper.CreateMap<VacancyApplicationUpdated, VacancyApplication>()
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
                .ForMember(d => d.update_date, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<Domain.Core.Attachment, ReadModel.Core.Attachment>()
                .ForMember(d => d.name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.size, o => o.MapFrom(s => s.Size))
                .ForMember(d => d.extension, o => o.MapFrom(s => s.Extension))
                .ForMember(d => d.url, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.upload_date, o => o.MapFrom(s => s.UploadDate));

            Mapper.CreateMap<ReadModel.ElasticSearchModel.Model.Vacancy, VacancyPublished>();


            /*VacancyApplication*/

            //create 
            Mapper.CreateMap<VacancyApplicationCreateViewModel, VacancyApplicationDataModel>();
            Mapper.CreateMap<VacancyApplication, VacancyApplicationDetailsViewModel>().IncludePagedResultMapping();
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
            /*dictionaties*/
            Mapper.CreateMap<Foiv, FoivViewModel>();
            Mapper.CreateMap<ResearchDirection, ResearchDirectionViewModel>();
        }
    }
}
