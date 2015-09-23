using System;
using AutoMapper;
using Newtonsoft.Json;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.ViewModels;
using SciVacancies.WebApp.Models.OAuth;
using System.Linq;
using SciVacancies.WebApp.Models.DataModels;

namespace SciVacancies.WebApp.Infrastructure
{
    public static class MappingConfiguration
    {
        public static void Initialize()
        {
            InitializePagers();
            InitializeAccount();
            MappingConfigurationOrganization.InitializeOrganization();
            InitializeElastic();
            MappingConfigurationResearcher.InitializeResearcher();
            MappingConfigurationResearcher.InitializeSearchSubscription();
            MappingConfigurationOrganization.InitializeVacancy();
            MappingConfigurationResearcher.InitializeVacancyApplication();
            InitializeDictionaries();
        }

        public static void InitializePagers()
        {
            Mapper.CreateMap<Organization, Organization>().IncludePagedResultMapping();
            Mapper.CreateMap<Researcher, Researcher>().IncludePagedResultMapping();
            Mapper.CreateMap<Vacancy, Vacancy>().IncludePagedResultMapping();
            Mapper.CreateMap<ReadModel.ElasticSearchModel.Model.Vacancy, ReadModel.ElasticSearchModel.Model.Vacancy>().IncludePagedResultMapping();
            Mapper.CreateMap<OrganizationNotification, OrganizationNotification>().IncludePagedResultMapping();
            Mapper.CreateMap<ResearcherNotification, ResearcherNotification>().IncludePagedResultMapping();
            Mapper.CreateMap<SearchSubscription, SearchSubscription>().IncludePagedResultMapping();
        }
        public static void InitializeAccount()
        {
            //researcher


            //TODO - дописать поля в модели
            Mapper.CreateMap<AccountResearcherRegisterViewModel, ResearcherRegisterDataModel>()
                ;
            Mapper.CreateMap<ResearcherUpdateDataModel, ResearcherDataModel>()
                .Include<ResearcherRegisterDataModel, ResearcherDataModel>()
                .ForMember(dest => dest.BirthDate, src => src.MapFrom(c => c.BirthDate ?? new DateTime(c.BirthYear, 1, 1)))
                ;
            Mapper.CreateMap<ResearcherRegisterDataModel, ResearcherDataModel>()
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
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.ImageUrl ?? string.Empty))
                .ForMember(d => d.Foiv, o => o.MapFrom(s => s.Foiv))
                .ForMember(d => d.FoivId, o => o.MapFrom(s => s.FoivId))
                .ForMember(d => d.OrgForm, o => o.MapFrom(s => s.OrgForm))
                .ForMember(d => d.OrgFormId, o => o.MapFrom(s => s.OrgFormId))
                .ForMember(d => d.ResearchDirections, o => o.Ignore());

            //TODO - часть этого маппинга лежит в accountController!!!
            Mapper.CreateMap<OAuthOrgInformation, AccountOrganizationRegisterViewModel>()
                //.ForMember(d => d.UserName, o => o.MapFrom(s => s.inn))
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
            Mapper.CreateMap<OAuthResProfile, ResearcherUpdateDataModel>()
                .Include<OAuthResProfile, ResearcherRegisterDataModel>()
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.firstName))
                .ForMember(d => d.ExtNumber, o => o.MapFrom(s => s.identityNumberSc != null ? s.identityNumberSc.scimap : 0))
                .ForMember(d => d.FirstNameEng, o => o.MapFrom(s => s.firstNameEn))
                .ForMember(d => d.SecondName, o => o.MapFrom(s => s.lastName))
                .ForMember(d => d.SecondNameEng, o => o.MapFrom(s => s.lastNameEn))
                .ForMember(d => d.Patronymic, o => o.MapFrom(s => s.middleName))
                .ForMember(d => d.PatronymicEng, o => o.MapFrom(s => s.middleNameEn))
                .ForMember(d => d.PreviousSecondName, o => o.MapFrom(s => s.lastNameOld))
                .ForMember(d => d.PreviousSecondNameEng, o => o.MapFrom(s => s.lastNameEnOld))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.phone))
                .ForMember(d => d.ExtraPhone, o => o.Ignore())
                //.ForMember(d => d, o => o.MapFrom(s => s.nationality))
                .ForMember(d => d.OtherActivity, o => o.MapFrom(s => s.researchers != null && s.researchers.Any(c => c.type.Equals("X")) 
                    ? JsonConvert.SerializeObject(s.researchers.Where(c => c.type.Equals("X") /* X - Прочая деятельность*/ ).Select(c=>new OAuthResActivity
                        {
                            title = c.title,
                            organization = c.organization,
                            type = "Прочая деятельность",
                            position = c.position,
                            yearFrom = c.yearFrom
                        }
                        )) 
                    : string.Empty
                    ))
                .ForMember(d => d.TeachingActivity, o => o.MapFrom(s => s.researchers != null && s.researchers.Any(c => c.type.Equals("L"))
                    ? JsonConvert.SerializeObject(s.researchers.Where(c => c.type.Equals("L") /* L - Преподавательская деятельность*/ ).Select(c => new OAuthResActivity
                        {
                            title = c.title,
                            organization = c.organization,
                            type = "Преподавательская деятельность",
                            position = c.position,
                            yearFrom = c.yearFrom
                        }
                        ))
                    : string.Empty
                    ))
                .ForMember(d => d.ResearchActivity, o => o.MapFrom(s => s.researchers != null && s.researchers.Any(c => !c.type.Equals("X") && !c.type.Equals("l"))
                    ? JsonConvert.SerializeObject(s.researchers.Where(c => !c.type.Equals("X") && !c.type.Equals("l") /* E,S,M,C,W,I,A,O - Исследовательская деятельность*/ ).Select(c => new OAuthResActivity
                    {
                        title = c.title,
                        organization = c.organization,
                        type = "Исследовательская деятельность",
                        position = c.position,
                        yearFrom = c.yearFrom
                    }
                        ))
                    : string.Empty
                    ))
                //.ForMember(d => d, o => o.MapFrom(s => s.degrees))
                //.ForMember(d => d, o => o.MapFrom(s => s.ranks))
                .ForMember(d => d.Rewards, o => o.MapFrom(s => s.honors != null && s.honors.Any() ? JsonConvert.SerializeObject(s.honors.Where(d => d.deleted != "1").ToList()) : string.Empty))
                .ForMember(d => d.Memberships, o => o.MapFrom(s => s.members != null && s.members.Any() ? JsonConvert.SerializeObject(s.members) : string.Empty))
                .ForMember(d => d.Conferences, o => o.MapFrom(s => s.abstracts != null && s.abstracts.Any() ? JsonConvert.SerializeObject(s.abstracts.Select(
                    c =>
                        new OAuthResAbstract
                        {
                            title = c.title,
                            year = c.year,
                            updated = c.updated,
                            conference = c.conference,
                            categoryType = c.categoryType.Equals("R") ? "Всероссийсикая" : "Международная" /* R - Всероссиская; W - Международная */
                        }
                    )) : string.Empty))
                .ForMember(d => d.Publications, o => o.MapFrom(s => s.entities))
                .ForMember(d => d.Interests, o => o.MapFrom(s => s.interests != null && s.interests.Any() ? JsonConvert.SerializeObject(s.interests) : string.Empty))
                //.ForMember(d => d, o => o.MapFrom(s => s.photo))
                .ForMember(d => d.Education, o => o.MapFrom(s => s.education != null && s.education.Any() ? JsonConvert.SerializeObject(s.education) : string.Empty))
                .ForMember(d => d.BirthDate, o => o.MapFrom(s => DateTime.Parse(s.birthday)));
            Mapper.CreateMap<ResearcherUpdateDataModel, ResearcherRegisterDataModel>()
                ;
            Mapper.CreateMap<OAuthResProfile, ResearcherRegisterDataModel>()
                //TODO: какой номер (или какое значение) использовать для заполнения НомерПользователяКартыНаук
                .ForMember(d => d.SciMapNumber, o => o.MapFrom(s => s.id))
                //.ForMember(d => d, o => o.MapFrom(s => s.login))
                .ForMember(d => d.Password, o => o.Ignore())
                .ForMember(d => d.ConfirmPassword, o => o.Ignore())
                .ForMember(d => d.Claims, o => o.Ignore())
                ;
            
            Mapper.CreateMap<OAuthResEntity, Domain.Core.Publication>()
                .ForMember(d => d.Authors, opt => opt.MapFrom(src => src.authors ?? string.Empty))
                .ForMember(d => d.ExtId, opt => opt.MapFrom(src => src.ext_id ?? string.Empty))
                .ForMember(d => d.Name, opt => opt.MapFrom(src => src.name ?? string.Empty))
                .ForMember(d => d.Type, opt => opt.MapFrom(src => src.type ?? string.Empty))
                .ForMember(d => d.Updated, opt => opt.MapFrom(src => src.updated))
                ;
            Mapper.CreateMap<OAuthResEducation, Domain.Core.Education>()
                .ForMember(d => d.FacultyShortName, opt => opt.MapFrom(src => src.faculty ?? string.Empty))
                .ForMember(d => d.UniversityShortName, opt => opt.MapFrom(src => src.org ?? string.Empty))
                .ForMember(d => d.City, opt => opt.MapFrom(src => src.town ?? string.Empty))
                .ForMember(d => d.GraduationYear, opt => opt.MapFrom(src => src.year))
                .ForMember(d => d.Degree, opt => opt.MapFrom(src => string.Empty))
                ;
        }
        public static void InitializeElastic()
        {
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
        }
        public static void InitializeDictionaries()
        {
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
        }
    }
}
