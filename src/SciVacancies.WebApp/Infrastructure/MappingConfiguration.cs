using System;
using AutoMapper;
using NPoco;
using Newtonsoft.Json;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.ViewModels;
using SciVacancies.WebApp.Models.OAuth;
using System.Linq;
using System.Collections.Generic;

namespace SciVacancies.WebApp.Infrastructure
{
    public static class MappingConfiguration
    {
        public static void Initialize()
        {
            #region Pagers

            Mapper.CreateMap<Organization, Organization>().IncludePagedResultMapping();
            Mapper.CreateMap<Researcher, Researcher>().IncludePagedResultMapping();
            Mapper.CreateMap<Vacancy, Vacancy>().IncludePagedResultMapping();
            Mapper.CreateMap<ReadModel.ElasticSearchModel.Model.Vacancy, ReadModel.ElasticSearchModel.Model.Vacancy>().IncludePagedResultMapping();
            Mapper.CreateMap<OrganizationNotification, OrganizationNotification>().IncludePagedResultMapping();
            Mapper.CreateMap<ResearcherNotification, ResearcherNotification>().IncludePagedResultMapping();
            Mapper.CreateMap<SearchSubscription, SearchSubscription>().IncludePagedResultMapping();

            #endregion

            #region Account

            //researcher
            Mapper.CreateMap<AccountResearcherRegisterViewModel, ResearcherDataModel>()
                .ForMember(dest => dest.BirthDate, src => src.MapFrom(c => new DateTime(c.BirthYear, 1, 1)))
                .ForMember(dest => dest.Patronymic, src => src.MapFrom(c => c.Patronymic))
                ;
            //organization
            Mapper.CreateMap<AccountOrganizationRegisterViewModel, OrganizationDataModel>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ShortName, o => o.MapFrom(s => s.ShortName))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.Address))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.INN, o => o.MapFrom(s => s.INN))
                .ForMember(d => d.OGRN, o => o.MapFrom(s => s.OGRN))
                .ForMember(d => d.HeadFirstName, o => o.MapFrom(s => s.HeadFirstName))
                .ForMember(d => d.HeadSecondName, o => o.MapFrom(s => s.HeadLastName))
                .ForMember(d => d.HeadPatronymic, o => o.MapFrom(s => s.HeadPatronymic))
                .ForMember(d => d.ImageName, o => o.MapFrom(s => s.ImageName))
                .ForMember(d => d.ImageSize, o => o.MapFrom(s => s.ImageSize))
                .ForMember(d => d.ImageExtension, o => o.MapFrom(s => s.ImageExtension))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.ImageUrl))
                .ForMember(d => d.Foiv, o => o.MapFrom(s => s.Foiv))
                .ForMember(d => d.FoivId, o => o.MapFrom(s => s.FoivId))
                .ForMember(d => d.OrgForm, o => o.MapFrom(s => s.OrgForm))
                .ForMember(d => d.OrgFormId, o => o.MapFrom(s => s.OrgFormId))
                .ForMember(d => d.ResearchDirections, o => o.Ignore());

            Mapper.CreateMap<OAuthOrgInformation, AccountOrganizationRegisterViewModel>()
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.inn))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.Foiv, o => o.MapFrom(s => s.foiv.title))
                .ForMember(d => d.FoivId, o => o.MapFrom(s => s.foiv.id))
                .ForMember(d => d.HeadFirstName, o => o.MapFrom(s => s.headFirstName))
                .ForMember(d => d.HeadLastName, o => o.MapFrom(s => s.headLastName))
                .ForMember(d => d.HeadPatronymic, o => o.MapFrom(s => s.headPatronymic))
                .ForMember(d => d.INN, o => o.MapFrom(s => s.inn))
                .ForMember(d => d.OGRN, o => o.MapFrom(s => s.ogrn))
                .ForMember(d => d.OrgForm, o => o.MapFrom(s => s.opf.title))
                .ForMember(d => d.OrgFormId, o => o.MapFrom(s => s.opf.id))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.postAddress))
                .ForMember(d => d.ResearchDirections, o => o.MapFrom(s => s.researchDirections.Select(ss => Int32.Parse(ss.id))))
                .ForMember(d => d.ShortName, o => o.MapFrom(s => s.shortTitle))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.title));
            //TODO - добавить нужные маппинги и допилить структуру моделей
            //Mapper.CreateMap<OAuthResProfile, AccountResearcherRegisterViewModel>()
            //            .ForMember(d => d, o => o.MapFrom(s => s.id))
            //            .ForMember(d => d, o => o.MapFrom(s => s.login))

            //            .ForMember(d => d.FirstName, o => o.MapFrom(s => s.firstName))
            //            .ForMember(d => d.FirstNameEng, o => o.MapFrom(s => s.firstNameEn))

            //            .ForMember(d => d, o => o.MapFrom(s => s.middleName))
            //            .ForMember(d => d, o => o.MapFrom(s => s.middleNameEn))

            //            .ForMember(d => d.SecondName, o => o.MapFrom(s => s.lastName))
            //            .ForMember(d => d.SecondNameEng, o => o.MapFrom(s => s.lastNameEn))

            //            //.ForMember(d => d.Patronymic, o => o.MapFrom(s => s.))
            //            //.ForMember(d => d.PatronymicEng, o => o.MapFrom(s => s.))

            //            .ForMember(d => d, o => o.MapFrom(s => s.lastNameOld))
            //            .ForMember(d => d, o => o.MapFrom(s => s.lastNameEnOld))

            //            .ForMember(d => d.Email, o => o.MapFrom(s => s.email))
            //            //.ForMember(d => d.ExtraEmail, o => o.MapFrom(s => s.))

            //            .ForMember(d => d.Phone, o => o.MapFrom(s => s.phone))
            //            //.ForMember(d => d.ExtraPhone, o => o.MapFrom(s => s.))

            //            .ForMember(d => d, o => o.MapFrom(s => s.nationality))

            //            //.ForMember(d => d.ResearchActivity, o => o.MapFrom(s => s.))
            //            //.ForMember(d => d.TeachingActivity, o => o.MapFrom(s => s.))
            //            //.ForMember(d => d.OtherActivity, o => o.MapFrom(s => s.))

            //            .ForMember(d => d, o => o.MapFrom(s => s.degrees))
            //            .ForMember(d => d, o => o.MapFrom(s => s.ranks))
            //            .ForMember(d => d, o => o.MapFrom(s => s.honors))
            //            .ForMember(d => d, o => o.MapFrom(s => s.members))
            //            //.ForMember(d => d.Conferences, o => o.MapFrom(s => s.))

            //            .ForMember(d => d, o => o.MapFrom(s => s.entities))
            //            .ForMember(d => d, o => o.MapFrom(s => s.interests))
            //            .ForMember(d => d, o => o.MapFrom(s => s.photo))
            //            .ForMember(d => d, o => o.MapFrom(s => s.sAbstracts))

            //            .ForMember(d => d, o => o.MapFrom(s => s.education))
            //            //.ForMember(d => d.Publications, o => o.MapFrom(s => s.))
            //            .ForMember(d => d.BirthYear, o => o.MapFrom(s => s.birthday));

            #endregion


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
                    .ForMember(d => d.image_name, o => o.MapFrom(s => s.Data.ImageName))
                    .ForMember(d => d.image_size, o => o.MapFrom(s => s.Data.ImageSize))
                    .ForMember(d => d.image_extension, o => o.MapFrom(s => s.Data.ImageExtension))
                    .ForMember(d => d.image_url, o => o.MapFrom(s => s.Data.ImageUrl))
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
                .ForMember(d => d.image_name, o => o.MapFrom(s => s.Data.ImageName))
                .ForMember(d => d.image_size, o => o.MapFrom(s => s.Data.ImageSize))
                .ForMember(d => d.image_extension, o => o.MapFrom(s => s.Data.ImageExtension))
                .ForMember(d => d.image_url, o => o.MapFrom(s => s.Data.ImageUrl))
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

            //информация об организации во View
            Mapper.CreateMap<Organization, OrganizationDetailsViewModel>().IncludePagedResultMapping()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.name))
                .ForMember(d => d.ShortName, o => o.MapFrom(s => s.shortname))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.address))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.INN, o => o.MapFrom(s => s.inn))
                .ForMember(d => d.OGRN, o => o.MapFrom(s => s.ogrn))
                .ForMember(d => d.HeadFirstName, o => o.MapFrom(s => s.head_firstname))
                .ForMember(d => d.HeadSecondName, o => o.MapFrom(s => s.head_secondname))
                .ForMember(d => d.HeadPatronymic, o => o.MapFrom(s => s.head_patronymic))
                .ForMember(d => d.FoivId, o => o.MapFrom(s => s.foiv_id))
                .ForMember(d => d.OrgFormId, o => o.MapFrom(s => s.orgform_id))
                .ForMember(d => d.ImageName, o => o.MapFrom(s => s.image_name))
                .ForMember(d => d.ImageSize, o => o.MapFrom(s => s.image_size))
                .ForMember(d => d.ImageExtension, o => o.MapFrom(s => s.image_extension))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.image_url))
                //.ForMember(d => d.ResearchDirectionIds, o => o.MapFrom(s => s.researchdirections))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.status));
            //Mapper.CreateMap<ResearchDirection, int>().ForMember(d => d, o => o.MapFrom(s => s.id));

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
                .ForMember(d => d.OrganizationFoiv, o => o.MapFrom(s => s.Data.OrganizationFoiv))
                .ForMember(d => d.OrganizationFoivId, o => o.MapFrom(s => s.Data.OrganizationFoivId))
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
                .ForMember(d => d.OrganizationFoiv, o => o.MapFrom(s => s.Data.OrganizationFoiv))
                .ForMember(d => d.OrganizationFoivId, o => o.MapFrom(s => s.Data.OrganizationFoivId))
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
                .ForMember(d => d.image_name, o => o.MapFrom(s => s.Data.ImageName))
                .ForMember(d => d.image_size, o => o.MapFrom(s => s.Data.ImageSize))
                .ForMember(d => d.image_extension, o => o.MapFrom(s => s.Data.ImageExtension))
                .ForMember(d => d.image_url, o => o.MapFrom(s => s.Data.ImageUrl))
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
                .ForMember(d => d.image_name, o => o.MapFrom(s => s.Data.ImageName))
                .ForMember(d => d.image_size, o => o.MapFrom(s => s.Data.ImageSize))
                .ForMember(d => d.image_extension, o => o.MapFrom(s => s.Data.ImageExtension))
                .ForMember(d => d.image_url, o => o.MapFrom(s => s.Data.ImageUrl))
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

            Mapper.CreateMap<ResearcherEditViewModel, ResearcherDataModel>();
            Mapper.CreateMap<Researcher, ResearcherDetailsViewModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.firstname))
                .ForMember(d => d.FirstNameEng, o => o.MapFrom(s => s.firstname_eng))
                .ForMember(d => d.SecondName, o => o.MapFrom(s => s.secondname))
                .ForMember(d => d.SecondNameEng, o => o.MapFrom(s => s.secondname_eng))
                .ForMember(d => d.Patronymic, o => o.MapFrom(s => s.patronymic))
                .ForMember(d => d.PatronymicEng, o => o.MapFrom(s => s.patronymic_eng))
                .ForMember(d => d.PreviousSecondName, o => o.MapFrom(s => s.previous_secondname))
                .ForMember(d => d.PreviousSecondNameEng, o => o.MapFrom(s => s.previous_secondname_eng))
                .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.birthdate))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.ExtraEmail, o => o.MapFrom(s => s.extraemail))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.phone))
                .ForMember(d => d.ExtraPhone, o => o.MapFrom(s => s.extraphone))
                .ForMember(d => d.Nationality, o => o.MapFrom(s => s.nationality))
                .ForMember(d => d.ResearchActivity, o => o.MapFrom(s => s.research_activity))
                .ForMember(d => d.TeachingActivity, o => o.MapFrom(s => s.teaching_activity))
                .ForMember(d => d.OtherActivity, o => o.MapFrom(s => s.other_activity))
                .ForMember(d => d.ScienceDegree, o => o.MapFrom(s => s.science_degree))
                .ForMember(d => d.ScienceRank, o => o.MapFrom(s => s.science_rank))
                .ForMember(d => d.Rewards, o => o.MapFrom(s => s.rewards))
                .ForMember(d => d.Memberships, o => o.MapFrom(s => s.memberships))
                .ForMember(d => d.Conferences, o => o.MapFrom(s => s.conferences))
                .ForMember(d => d.ImageName, o => o.MapFrom(s => s.image_name))
                .ForMember(d => d.ImageSize, o => o.MapFrom(s => s.image_size))
                .ForMember(d => d.ImageExtension, o => o.MapFrom(s => s.image_extension))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.image_url))
                .ForMember(d => d.Educations, o => o.MapFrom(s => s.educations))
                .ForMember(d => d.Publications, o => o.MapFrom(s => s.publications))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.status))
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.creation_date))
                .ForMember(d => d.UpdateDate, o => o.MapFrom(s => s.update_date));
            Mapper.CreateMap<Researcher, ResearcherEditViewModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.firstname))
                .ForMember(d => d.FirstNameEng, o => o.MapFrom(s => s.firstname_eng))
                .ForMember(d => d.SecondName, o => o.MapFrom(s => s.secondname))
                .ForMember(d => d.SecondNameEng, o => o.MapFrom(s => s.secondname_eng))
                .ForMember(d => d.Patronymic, o => o.MapFrom(s => s.patronymic))
                .ForMember(d => d.PatronymicEng, o => o.MapFrom(s => s.patronymic_eng))
                .ForMember(d => d.PreviousSecondName, o => o.MapFrom(s => s.previous_secondname))
                .ForMember(d => d.PreviousSecondNameEng, o => o.MapFrom(s => s.previous_secondname_eng))
                .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.birthdate))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.ExtraEmail, o => o.MapFrom(s => s.extraemail))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.phone))
                .ForMember(d => d.ExtraPhone, o => o.MapFrom(s => s.extraphone))
                .ForMember(d => d.Nationality, o => o.MapFrom(s => s.nationality))
                .ForMember(d => d.ResearchActivity, o => o.MapFrom(s => s.research_activity))
                .ForMember(d => d.TeachingActivity, o => o.MapFrom(s => s.teaching_activity))
                .ForMember(d => d.OtherActivity, o => o.MapFrom(s => s.other_activity))
                .ForMember(d => d.ScienceDegree, o => o.MapFrom(s => s.science_degree))
                .ForMember(d => d.ScienceRank, o => o.MapFrom(s => s.science_rank))
                .ForMember(d => d.Rewards, o => o.MapFrom(s => s.rewards))
                .ForMember(d => d.Memberships, o => o.MapFrom(s => s.memberships))
                .ForMember(d => d.Conferences, o => o.MapFrom(s => s.conferences))
                .ForMember(d => d.ImageName, o => o.MapFrom(s => s.image_name))
                .ForMember(d => d.ImageSize, o => o.MapFrom(s => s.image_size))
                .ForMember(d => d.ImageExtension, o => o.MapFrom(s => s.image_extension))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.image_url))
                .ForMember(d => d.Educations, o => o.MapFrom(s => s.educations))
                .ForMember(d => d.Publications, o => o.MapFrom(s => s.publications));

            //education

            Mapper.CreateMap<Education, SciVacancies.Domain.Core.Education>();
            Mapper.CreateMap<Education, EducationEditViewModel>()
                .ForMember(d => d.ResearcherGuid, o => o.MapFrom(s => s.researcher_guid))
                .ForMember(d => d.City, o => o.MapFrom(s => s.city))
                .ForMember(d => d.UniversityShortName, o => o.MapFrom(s => s.university_shortname))
                .ForMember(d => d.FacultyShortName, o => o.MapFrom(s => s.faculty_shortname))
                .ForMember(dest => dest.GraduationYear, src => src.MapFrom(c => c.graduation_date.HasValue ? c.graduation_date.Value.Year : 0))
                .ForMember(d => d.Degree, o => o.MapFrom(s => s.degree));
            Mapper.CreateMap<EducationEditViewModel, SciVacancies.Domain.Core.Education>()
                .ForMember(dest => dest.GraduationYear, src => src.MapFrom(c => (c.GraduationYear.HasValue && c.GraduationYear.Value != 0) ? new DateTime(c.GraduationYear.Value, 1, 1) : default(DateTime)));

            //piblication
            Mapper.CreateMap<Publication, SciVacancies.Domain.Core.Publication>();
            Mapper.CreateMap<Publication, PublicationEditViewModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.ResearcherGuid, o => o.MapFrom(s => s.researcher_guid))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.title));
            Mapper.CreateMap<PublicationEditViewModel, SciVacancies.Domain.Core.Publication>();

            #endregion


            #region SearchSubscription

            Mapper.CreateMap<SearchSubscriptionCreated, SearchSubscription>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.SearchSubscriptionGuid))
                .ForMember(d => d.researcher_guid, o => o.MapFrom(s => s.ResearcherGuid))
                .ForMember(d => d.title, o => o.MapFrom(s => s.Data.Title))
                .ForMember(d => d.query, o => o.MapFrom(s => s.Data.Query))
                .ForMember(d => d.orderby, o => o.MapFrom(s => s.Data.OrderBy))
                .ForMember(d => d.foiv_ids, o => o.MapFrom(s => JsonConvert.SerializeObject(s.Data.FoivIds)))
                .ForMember(d => d.positiontype_ids, o => o.MapFrom(s => JsonConvert.SerializeObject(s.Data.PositionTypeIds)))
                .ForMember(d => d.region_ids, o => o.MapFrom(s => JsonConvert.SerializeObject(s.Data.RegionIds)))
                .ForMember(d => d.researchdirection_ids, o => o.MapFrom(s => JsonConvert.SerializeObject(s.Data.ResearchDirectionIds)))
                .ForMember(d => d.salary_from, o => o.MapFrom(s => s.Data.SalaryFrom))
                .ForMember(d => d.salary_to, o => o.MapFrom(s => s.Data.SalaryTo))
                .ForMember(d => d.vacancy_statuses, o => o.MapFrom(s => JsonConvert.SerializeObject(s.Data.VacancyStatuses)))
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
                .ForMember(d => d.attachments, o => o.MapFrom(s => s.Data.Attachments))
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
                .ForMember(d => d.attachments, o => o.MapFrom(s => s.Data.Attachments))
                .ForMember(d => d.organization_guid, o => o.MapFrom(s => s.OrganizationGuid))
                .ForMember(d => d.update_date, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<Domain.Core.VacancyCriteria, ReadModel.Core.VacancyCriteria>()
                .ForMember(d => d.criteria_id, o => o.MapFrom(s => s.CriteriaId))
                .ForMember(d => d.count, o => o.MapFrom(s => s.Count));
            Mapper.CreateMap<CriteriaItemViewModel, Domain.Core.VacancyCriteria>()
                .ForMember(d => d.CriteriaId, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CriteriaParentId, o => o.MapFrom(s => s.ParentId))
                .ForMember(d => d.Count, o => o.MapFrom(s => s.Count))
                .ForMember(d => d.CriteriaTitle, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.CriteriaCode, o => o.MapFrom(s => s.Code))
                ;
            Mapper.CreateMap<Domain.Core.VacancyAttachment, ReadModel.Core.VacancyAttachment>()
                .ForMember(d => d.name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.size, o => o.MapFrom(s => s.Size))
                .ForMember(d => d.extension, o => o.MapFrom(s => s.Extension))
                .ForMember(d => d.url, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.upload_date, o => o.MapFrom(s => s.UploadDate));

            //vacancy
            Mapper.CreateMap<Vacancy, VacancyCreateViewModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.name))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.fullname))
                .ForMember(d => d.Tasks, o => o.MapFrom(s => s.tasks))
                .ForMember(d => d.ResearchTheme, o => o.MapFrom(s => s.researchtheme))
                .ForMember(d => d.CityName, o => o.MapFrom(s => s.cityname))
                .ForMember(d => d.Details, o => o.MapFrom(s => s.details))
                .ForMember(d => d.ContactName, o => o.MapFrom(s => s.contact_name))
                .ForMember(d => d.ContactEmail, o => o.MapFrom(s => s.contact_email))
                .ForMember(d => d.ContactPhone, o => o.MapFrom(s => s.contact_phone))
                .ForMember(d => d.ContactDetails, o => o.MapFrom(s => s.contact_details))
                .ForMember(d => d.SalaryFrom, o => o.MapFrom(s => s.salary_from))
                .ForMember(d => d.SalaryTo, o => o.MapFrom(s => s.salary_to))
                .ForMember(d => d.Bonuses, o => o.MapFrom(s => s.bonuses))
                .ForMember(d => d.ContractType, o => o.MapFrom(s => s.contract_type))
                .ForMember(d => d.ContractTime, o => o.MapFrom(s => s.contract_time))
                .ForMember(d => d.EmploymentType, o => o.MapFrom(s => s.employment_type))
                .ForMember(d => d.OperatingScheduleType, o => o.MapFrom(s => s.operatingschedule_type))
                .ForMember(d => d.SocialPackage, o => o.MapFrom(s => s.socialpackage))
                .ForMember(d => d.Rent, o => o.MapFrom(s => s.rent))
                .ForMember(d => d.OfficeAccomodation, o => o.MapFrom(s => s.officeaccomodation))
                .ForMember(d => d.TransportCompensation, o => o.MapFrom(s => s.transportcompensation))
                .ForMember(d => d.PositionTypeId, o => o.MapFrom(s => s.positiontype_id))
                .ForMember(d => d.RegionId, o => o.MapFrom(s => s.region_id))
                .ForMember(d => d.ResearchDirectionId, o => o.MapFrom(s => s.researchdirection_id))
                .ForMember(d => d.Criterias, o => o.MapFrom(s => s.criterias))
                //.ForMember(d => d, o => o.MapFrom(s => s.attachments))
                .ForMember(d => d.OrganizationGuid, o => o.MapFrom(s => s.organization_guid));
            Mapper.CreateMap<Vacancy, VacancyDetailsViewModel>().IncludePagedResultMapping()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.name))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.fullname))
                .ForMember(d => d.Tasks, o => o.MapFrom(s => s.tasks))
                .ForMember(d => d.ResearchTheme, o => o.MapFrom(s => s.researchtheme))
                .ForMember(d => d.CityName, o => o.MapFrom(s => s.cityname))
                .ForMember(d => d.Details, o => o.MapFrom(s => s.details))
                .ForMember(d => d.ContactName, o => o.MapFrom(s => s.contact_name))
                .ForMember(d => d.ContactEmail, o => o.MapFrom(s => s.contact_email))
                .ForMember(d => d.ContactPhone, o => o.MapFrom(s => s.contact_phone))
                .ForMember(d => d.ContactDetails, o => o.MapFrom(s => s.contact_details))
                .ForMember(d => d.SalaryFrom, o => o.MapFrom(s => s.salary_from))
                .ForMember(d => d.SalaryTo, o => o.MapFrom(s => s.salary_to))
                .ForMember(d => d.Bonuses, o => o.MapFrom(s => s.bonuses))
                .ForMember(d => d.ContractType, o => o.MapFrom(s => s.contract_type))
                .ForMember(d => d.ContractTime, o => o.MapFrom(s => s.contract_time))
                .ForMember(d => d.EmploymentType, o => o.MapFrom(s => s.employment_type))
                .ForMember(d => d.OperatingScheduleType, o => o.MapFrom(s => s.operatingschedule_type))
                .ForMember(d => d.SocialPackage, o => o.MapFrom(s => s.socialpackage))
                .ForMember(d => d.Rent, o => o.MapFrom(s => s.rent))
                .ForMember(d => d.OfficeAccomodation, o => o.MapFrom(s => s.officeaccomodation))
                .ForMember(d => d.TransportCompensation, o => o.MapFrom(s => s.transportcompensation))
                .ForMember(d => d.PositionTypeId, o => o.MapFrom(s => s.positiontype_id))
                .ForMember(d => d.RegionId, o => o.MapFrom(s => s.region_id))
                .ForMember(d => d.ResearchDirectionId, o => o.MapFrom(s => s.researchdirection_id))
                //.ForMember(d => d., o => o.MapFrom(s => s.criterias))
                .ForMember(d => d.OrganizationGuid, o => o.MapFrom(s => s.organization_guid))
                .ForMember(d => d.IsWinnerAccept, o => o.MapFrom(s => s.is_winner_accept))
                .ForMember(d => d.WinnerResearcherGuid, o => o.MapFrom(s => s.winner_researcher_guid))
                .ForMember(d => d.WinnerRequestDate, o => o.MapFrom(s => s.winner_request_date))
                .ForMember(d => d.WinnerResponseDate, o => o.MapFrom(s => s.winner_response_date))
                .ForMember(d => d.WinnerVacancyApplicationGuid, o => o.MapFrom(s => s.winner_vacancyapplication_guid))
                .ForMember(d => d.IsPretenderAccept, o => o.MapFrom(s => s.is_pretender_accept))
                .ForMember(d => d.PretenderResearcherGuid, o => o.MapFrom(s => s.pretender_researcher_guid))
                .ForMember(d => d.PretenderRequestDate, o => o.MapFrom(s => s.pretender_request_date))
                .ForMember(d => d.PretenderResponseDate, o => o.MapFrom(s => s.pretender_response_date))
                .ForMember(d => d.PretenderVacancyApplicationGuid, o => o.MapFrom(s => s.pretender_vacancyapplication_guid))
                ;
            Mapper.CreateMap<VacancyCreateViewModel, VacancyDataModel>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.FullName))
                .ForMember(d => d.Tasks, o => o.MapFrom(s => s.Tasks))
                .ForMember(d => d.ResearchTheme, o => o.MapFrom(s => s.ResearchTheme))
                .ForMember(d => d.CityName, o => o.MapFrom(s => s.CityName))
                .ForMember(d => d.Details, o => o.MapFrom(s => s.Details))
                .ForMember(d => d.ContactName, o => o.MapFrom(s => s.ContactName))
                .ForMember(d => d.ContactEmail, o => o.MapFrom(s => s.ContactEmail))
                .ForMember(d => d.ContactPhone, o => o.MapFrom(s => s.ContactPhone))
                .ForMember(d => d.ContactDetails, o => o.MapFrom(s => s.ContactDetails))
                .ForMember(d => d.SalaryFrom, o => o.MapFrom(s => s.SalaryFrom))
                .ForMember(d => d.SalaryTo, o => o.MapFrom(s => s.SalaryTo))
                .ForMember(d => d.Bonuses, o => o.MapFrom(s => s.Bonuses))
                .ForMember(d => d.ContractType, o => o.MapFrom(s => s.ContractType))
                .ForMember(d => d.ContractTime, o => o.MapFrom(s => s.ContractTime))
                .ForMember(d => d.EmploymentType, o => o.MapFrom(s => s.EmploymentType))
                .ForMember(d => d.OperatingScheduleType, o => o.MapFrom(s => s.OperatingScheduleType))
                .ForMember(d => d.SocialPackage, o => o.MapFrom(s => s.SocialPackage))
                .ForMember(d => d.Rent, o => o.MapFrom(s => s.Rent))
                .ForMember(d => d.OfficeAccomodation, o => o.MapFrom(s => s.OfficeAccomodation))
                .ForMember(d => d.TransportCompensation, o => o.MapFrom(s => s.TransportCompensation))
                .ForMember(d => d.PositionType, o => o.MapFrom(s => s.PositionType))
                .ForMember(d => d.PositionTypeId, o => o.MapFrom(s => s.PositionTypeId))
                .ForMember(d => d.Region, o => o.MapFrom(s => s.Region))
                .ForMember(d => d.RegionId, o => o.MapFrom(s => s.RegionId))
                .ForMember(d => d.ResearchDirection, o => o.MapFrom(s => s.ResearchDirection))
                .ForMember(d => d.ResearchDirectionId, o => o.MapFrom(s => s.ResearchDirectionId))
                .ForMember(d => d.Criterias, o => o.MapFrom(s => s.CriteriasHierarchy.SelectMany(c => c.Items.Where(d => d.Count > 0))))
                //.ForMember(d => d.OrganizationFoiv, o => o.MapFrom(s => s))
                //.ForMember(d => d.OrganizationFoivId, o => o.MapFrom(s => s))
                ;
            Mapper.CreateMap<ReadModel.ElasticSearchModel.Model.Vacancy, VacancyElasticResult>().IncludePagedResultMapping();
            ;

            //TODO - VACANCY EDIT VIEW MODEL MAPPINGS

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
                .ForMember(d => d.covering_letter, o => o.MapFrom(s => s.Data.CoveringLetter))
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
                .ForMember(d => d.covering_letter, o => o.MapFrom(s => s.Data.CoveringLetter))
                .ForMember(d => d.educations, o => o.MapFrom(s => s.Data.Educations))
                .ForMember(d => d.publications, o => o.MapFrom(s => s.Data.Publications))
                .ForMember(d => d.vacancy_guid, o => o.MapFrom(s => s.VacancyGuid))
                .ForMember(d => d.researcher_guid, o => o.MapFrom(s => s.ResearcherGuid))
                .ForMember(d => d.attachments, o => o.MapFrom(s => s.Data.Attachments))
                .ForMember(d => d.update_date, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<Domain.Core.VacancyApplicationAttachment, ReadModel.Core.VacancyApplicationAttachment>()
                .ForMember(d => d.name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.size, o => o.MapFrom(s => s.Size))
                .ForMember(d => d.extension, o => o.MapFrom(s => s.Extension))
                .ForMember(d => d.url, o => o.MapFrom(s => s.Url))
                .ForMember(d => d.upload_date, o => o.MapFrom(s => s.UploadDate));


            /*VacancyApplication*/

            //create 
            Mapper.CreateMap<VacancyApplicationCreateViewModel, VacancyApplicationDataModel>()
                .ForSourceMember(c => c.Attachments, o => o.Ignore())
                .ForMember(c => c.Attachments, o => o.Ignore())
                ;
            Mapper.CreateMap<VacancyApplication, VacancyApplicationDetailsViewModel>().IncludePagedResultMapping()
                .ForMember(d => d.SentDate, o => o.MapFrom(s => s.apply_date))
                .ForMember(d => d.AcademicStatus, o => o.MapFrom(s => s.science_rank))
                .ForMember(d => d.Conferences, o => o.MapFrom(s => s.conferences))
                .ForMember(d => d.CoveringLetter, o => o.MapFrom(s => s.covering_letter))
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.creation_date))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.ExtraEmail, o => o.MapFrom(s => s.extraemail))
                .ForMember(d => d.ExtraPhone, o => o.MapFrom(s => s.extraphone))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.researcher_fullname))
                .ForMember(d => d.Memberships, o => o.MapFrom(s => s.memberships))
                .ForMember(d => d.OtherActivity, o => o.MapFrom(s => s.other_activity))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.phone))
                .ForMember(d => d.PositionTypeName, o => o.MapFrom(s => s.position_name))
                .ForMember(d => d.ResearchActivity, o => o.MapFrom(s => s.research_activity))
                .ForMember(d => d.ResearcherGuid, o => o.MapFrom(s => s.researcher_guid))
                .ForMember(d => d.Rewards, o => o.MapFrom(s => s.rewards))
                .ForMember(d => d.ScienceDegree, o => o.MapFrom(s => s.science_degree))
                .ForMember(d => d.TeachingActivity, o => o.MapFrom(s => s.teaching_activity))
                .ForMember(d => d.UpdateDate, o => o.MapFrom(s => s.update_date))
                .ForMember(d => d.VacancyGuid, o => o.MapFrom(s => s.vacancy_guid))
                ;
            Mapper.CreateMap<VacancyApplication, VacancyApplicationSetWinnerViewModel>()
                .ForMember(d => d.AcademicStatus, o => o.MapFrom(s => s.science_rank))
                .ForMember(d => d.Conferences, o => o.MapFrom(s => s.conferences))
                .ForMember(d => d.CoveringLetter, o => o.MapFrom(s => s.covering_letter))
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.creation_date))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.ExtraEmail, o => o.MapFrom(s => s.extraemail))
                .ForMember(d => d.ExtraPhone, o => o.MapFrom(s => s.extraphone))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.researcher_fullname))
                .ForMember(d => d.Memberships, o => o.MapFrom(s => s.memberships))
                .ForMember(d => d.OtherActivity, o => o.MapFrom(s => s.other_activity))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.phone))
                .ForMember(d => d.PositionTypeName, o => o.MapFrom(s => s.position_name))
                .ForMember(d => d.ResearchActivity, o => o.MapFrom(s => s.research_activity))
                .ForMember(d => d.ResearcherGuid, o => o.MapFrom(s => s.researcher_guid))
                .ForMember(d => d.Rewards, o => o.MapFrom(s => s.rewards))
                .ForMember(d => d.ScienceDegree, o => o.MapFrom(s => s.science_degree))
                .ForMember(d => d.TeachingActivity, o => o.MapFrom(s => s.teaching_activity))
                .ForMember(d => d.UpdateDate, o => o.MapFrom(s => s.update_date))
                .ForMember(d => d.VacancyGuid, o => o.MapFrom(s => s.vacancy_guid))
                ;

            #endregion


            #region Dictionaries

            Mapper.CreateMap<Foiv, FoivViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.ParentId, o => o.MapFrom(s => s.parent_id))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.title))
                .ForMember(d => d.ShortTitle, o => o.MapFrom(s => s.shorttitle));
            Mapper.CreateMap<ResearchDirection, ResearchDirectionViewModel>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.id))
                .ForMember(d => d.ParentId, o => o.MapFrom(s => s.parent_id))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.title))
                .ForMember(d => d.TitleEng, o => o.MapFrom(s => s.title_eng))
                .ForMember(d => d.Lft, o => o.MapFrom(s => s.lft))
                .ForMember(d => d.Rgt, o => o.MapFrom(s => s.rgt))
                .ForMember(d => d.Lvl, o => o.MapFrom(s => s.lvl))
                .ForMember(d => d.OecdCode, o => o.MapFrom(s => s.oecd_code))
                .ForMember(d => d.WosCode, o => o.MapFrom(s => s.wos_code))
                .ForMember(d => d.Root, o => o.MapFrom(s => s.root_id));

            #endregion

        }
    }
}
