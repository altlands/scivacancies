﻿using System;
using AutoMapper;
using Newtonsoft.Json;
using SciVacancies.Domain.DataModels;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;
using SciVacancies.WebApp.ViewModels;
using System.Linq;
using System.Collections.Generic;
using SciVacancies.WebApp.Models;

namespace SciVacancies.WebApp.Infrastructure
{
    public static class MappingConfigurationResearcher
    {
        public static void InitializeResearcher()
        {
            //Создание исследователя
            Mapper.CreateMap<ResearcherCreated, Researcher>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.ResearcherGuid))
                .ForMember(d => d.ext_number, o => o.MapFrom(s => s.Data.ExtNumber))
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
                .ForMember(d => d.interests, o => o.MapFrom(s => s.Data.Interests))
                .ForMember(d => d.publications, o => o.MapFrom(s => s.Data.Publications))
                .ForMember(d => d.creation_date, o => o.MapFrom(s => s.TimeStamp));
            //Обновление исследователя
            Mapper.CreateMap<ResearcherUpdated, Researcher>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.ResearcherGuid))
                .ForMember(d => d.ext_number, o => o.MapFrom(s => s.Data.ExtNumber))
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
                .ForMember(d => d.interests, o => o.MapFrom(s => s.Data.Interests))
                .ForMember(d => d.update_date, o => o.MapFrom(s => s.TimeStamp));

            Mapper.CreateMap<Domain.Core.Education, ReadModel.Core.Education>()
                .ForMember(d => d.city, o => o.MapFrom(s => s.City))
                .ForMember(d => d.university_shortname, o => o.MapFrom(s => s.UniversityShortName))
                .ForMember(d => d.faculty_shortname, o => o.MapFrom(s => s.FacultyShortName))
                .ForMember(d => d.graduation_date, o => o.MapFrom(s => s.GraduationYear))
                .ForMember(d => d.degree, o => o.MapFrom(s => s.Degree));
            Mapper.CreateMap<Domain.Core.Publication, ReadModel.Core.Publication>()
                .ForMember(d => d.name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.guid, o => o.MapFrom(s => s.Guid))
                .ForMember(d => d.authors, o => o.MapFrom(s => s.Authors))
                .ForMember(d => d.ext_id, o => o.MapFrom(s => s.ExtId))
                .ForMember(d => d.name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.type, o => o.MapFrom(s => s.Type))
                .ForMember(d => d.updated, o => o.MapFrom(s => s.Updated))
                ;

            Mapper.CreateMap<ResearcherEditViewModel, ResearcherDataModel>()
                .ForSourceMember(src=> src.ExtNumber, o=>o.Ignore()) // Индивидуальный номер учёного обрабатывается по отдельной логике (в контроллере)
                .ForMember(dest => dest.BirthDate, src => src.MapFrom(c => new DateTime(c.BirthYear, 1, 1)))
                .ForMember(dest => dest.Rewards, src => src.MapFrom(c => JsonConvert.SerializeObject(c.Rewards)))
                .ForMember(dest => dest.Memberships, src => src.MapFrom(c => JsonConvert.SerializeObject(c.Memberships)))
                .ForMember(dest => dest.Interests, src => src.MapFrom(c => JsonConvert.SerializeObject(c.Interests)))
                ;
            Mapper.CreateMap<ResearcherEditPhotoViewModel, ResearcherDataModel>()
                ;

            Mapper.CreateMap<Researcher, ResearcherDataModel>()
                .ForMember(d => d.ExtNumber, o => o.MapFrom(s => s.ext_number))
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
                .ForMember(d => d.Interests, o => o.MapFrom(s => s.interests))
                .ForMember(d => d.Publications, o => o.MapFrom(s => s.publications))
            ;

            Mapper.CreateMap<Researcher, ResearcherDetailsViewModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.ExtNumber, o => o.MapFrom(s => s.ext_number))
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
                .ForMember(d => d.ExtNumber, o => o.MapFrom(s => s.ext_number))
                .ForMember(d => d.ScienceDegree, o => o.MapFrom(s => s.science_degree))
                .ForMember(d => d.ScienceRank, o => o.MapFrom(s => s.science_rank))
                .ForMember(d => d.Rewards, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.rewards) ? JsonConvert.DeserializeObject<List<RewardDetailsViewModel>>(s.rewards) : new List<RewardDetailsViewModel>()))
                .ForMember(d => d.Memberships, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.memberships) ? JsonConvert.DeserializeObject<List<MembershipDetailsViewModel>>(s.memberships) : new List<MembershipDetailsViewModel>()))
                .ForMember(d => d.Conferences, o => o.MapFrom(s => s.conferences))
                .ForMember(d => d.ImageName, o => o.MapFrom(s => s.image_name))
                .ForMember(d => d.ImageSize, o => o.MapFrom(s => s.image_size))
                .ForMember(d => d.ImageExtension, o => o.MapFrom(s => s.image_extension))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.image_url))
                .ForMember(d => d.Educations, o => o.MapFrom(s => s.educations))
                .ForMember(d => d.Interests, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.interests) ? JsonConvert.DeserializeObject<List<InterestDetailsViewModel>>(s.interests) : new List<InterestDetailsViewModel>()))
                .ForMember(d => d.Publications, o => o.MapFrom(s => s.publications))
                .ForMember(d => d.Status, o => o.MapFrom(s => s.status))
                .ForMember(d => d.CreationDate, o => o.MapFrom(s => s.creation_date))
                .ForMember(d => d.UpdateDate, o => o.MapFrom(s => s.update_date));
            Mapper.CreateMap<Researcher, ResearcherEditViewModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.ExtNumber, o => o.MapFrom(s => s.ext_number))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.firstname))
                .ForMember(d => d.FirstNameEng, o => o.MapFrom(s => s.firstname_eng))
                .ForMember(d => d.SecondName, o => o.MapFrom(s => s.secondname))
                .ForMember(d => d.SecondNameEng, o => o.MapFrom(s => s.secondname_eng))
                .ForMember(d => d.Patronymic, o => o.MapFrom(s => s.patronymic))
                .ForMember(d => d.PatronymicEng, o => o.MapFrom(s => s.patronymic_eng))
                .ForMember(d => d.PreviousSecondName, o => o.MapFrom(s => s.previous_secondname))
                .ForMember(d => d.PreviousSecondNameEng, o => o.MapFrom(s => s.previous_secondname_eng))
                //.ForMember(d => d.BirthDate, o => o.MapFrom(s => s.birthdate))
                .ForMember(d => d.BirthYear, o => o.MapFrom(s => s.birthdate.Year))
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
                .ForMember(d => d.Rewards, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.rewards) ? JsonConvert.DeserializeObject<List<RewardEditViewModel>>(s.rewards) : new List<RewardEditViewModel>()))
                .ForMember(d => d.Memberships, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.memberships) ? JsonConvert.DeserializeObject<List<MembershipEditViewModel>>(s.memberships) : new List<MembershipEditViewModel>()))
                .ForMember(d => d.Interests, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.interests) ? JsonConvert.DeserializeObject<List<InterestEditViewModel>>(s.interests) : new List<InterestEditViewModel>()))
                .ForMember(d => d.Conferences, o => o.MapFrom(s => s.conferences))
                .ForMember(d => d.Educations, o => o.MapFrom(s => s.educations))
                .ForMember(d => d.Publications, o => o.MapFrom(s => s.publications));
            Mapper.CreateMap<Researcher, ResearcherEditPhotoViewModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.ImageName, o => o.MapFrom(s => s.image_name))
                .ForMember(d => d.ImageSize, o => o.MapFrom(s => s.image_size))
                .ForMember(d => d.ImageExtension, o => o.MapFrom(s => s.image_extension))
                .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.image_url))
                ;


            Mapper.CreateMap<ResearcherDataModel, ResearcherProfileCompareModelItem>()
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName))
                .ForMember(d => d.MiddleName, o => o.MapFrom(s => s.Patronymic))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.SecondName))
                .ForMember(d => d.PreviousLastName, o => o.MapFrom(s => s.PreviousSecondName))
                .ForMember(d => d.FirstNameEng, o => o.MapFrom(s => s.FirstNameEng))
                .ForMember(d => d.MiddleNameEng, o => o.MapFrom(s => s.PatronymicEng))
                .ForMember(d => d.LastNameEng, o => o.MapFrom(s => s.SecondNameEng))
                .ForMember(d => d.PreviousLastNameEng, o => o.MapFrom(s => s.PreviousSecondNameEng))
                .ForMember(d => d.Educations, o => o.MapFrom(s => 
                    s.Educations.Select(c => new CheckableListItem<EducationEditViewModel> { This = Mapper.Map<EducationEditViewModel>(c) })))
                .ForMember(d => d.Publications, o => o.MapFrom(s => s.Publications.Select(c => new CheckableListItem<PublicationEditViewModel> { This = Mapper.Map<PublicationEditViewModel>(c) })))
                .ForMember(d => d.Rewards, o => o.MapFrom(s =>
                    !string.IsNullOrWhiteSpace(s.Rewards)
                    ? JsonConvert.DeserializeObject<List<RewardEditViewModel>>(s.Rewards).Select(c => new CheckableListItem<RewardEditViewModel> { This = c })
                    : new CheckableItemsList<RewardEditViewModel>()
                    ))
                .ForMember(d => d.Memberships, o => o.MapFrom(s =>
                    !string.IsNullOrWhiteSpace(s.Memberships)
                    ? JsonConvert.DeserializeObject<List<MembershipEditViewModel>>(s.Memberships).Select(c => new CheckableListItem<MembershipEditViewModel> { This = c })
                    : new CheckableItemsList<MembershipEditViewModel>()
                    ))
                .ForMember(d => d.Interests, o => o.MapFrom(s =>
                    !string.IsNullOrWhiteSpace(s.Interests)
                    ? JsonConvert.DeserializeObject<List<InterestEditViewModel>>(s.Interests).Select(c => new CheckableListItem<InterestEditViewModel> { This = c })
                    : new CheckableItemsList<InterestEditViewModel>()
                    ))
                ;

            //education

            Mapper.CreateMap<Education, SciVacancies.Domain.Core.Education>();
            Mapper.CreateMap<Education, EducationEditViewModel>()
                .ForMember(d => d.ResearcherGuid, o => o.MapFrom(s => s.researcher_guid))
                .ForMember(d => d.City, o => o.MapFrom(s => s.city))
                .ForMember(d => d.UniversityShortName, o => o.MapFrom(s => s.university_shortname))
                .ForMember(d => d.FacultyShortName, o => o.MapFrom(s => s.faculty_shortname))
                .ForMember(dest => dest.GraduationYear, src => src.MapFrom(c => c.graduation_date.HasValue ? c.graduation_date.Value.Year : 0))
                .ForMember(d => d.Degree, o => o.MapFrom(s => s.degree));
            Mapper.CreateMap<SciVacancies.Domain.Core.Education, EducationEditViewModel>()
                .ForMember(d => d.City, o => o.MapFrom(s => s.City))
                .ForMember(d => d.UniversityShortName, o => o.MapFrom(s => s.UniversityShortName))
                .ForMember(d => d.FacultyShortName, o => o.MapFrom(s => s.FacultyShortName))
                .ForMember(dest => dest.GraduationYear, src => src.MapFrom(c => c.GraduationYear.HasValue ? c.GraduationYear.Value.Year : 0))
                .ForMember(d => d.Degree, o => o.MapFrom(s => s.Degree));
            Mapper.CreateMap<EducationEditViewModel, SciVacancies.Domain.Core.Education>()
                .ForMember(dest => dest.GraduationYear, src => src.MapFrom(c => (c.GraduationYear.HasValue && c.GraduationYear.Value != 0) ? new DateTime(c.GraduationYear.Value, 1, 1) : default(DateTime)));

            //piblication
            Mapper.CreateMap<Publication, SciVacancies.Domain.Core.Publication>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.Authors, o => o.MapFrom(s => s.authors))
                .ForMember(d => d.ExtId, o => o.MapFrom(s => s.ext_id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.name))
                .ForMember(d => d.Type, o => o.MapFrom(s => s.type))
                .ForMember(d => d.Updated, o => o.MapFrom(s => s.updated))
                ;
            Mapper.CreateMap<Publication, PublicationEditViewModel>()
                .ForMember(d => d.Guid, o => o.MapFrom(s => s.guid))
                .ForMember(d => d.ResearcherGuid, o => o.MapFrom(s => s.researcher_guid))
                .ForMember(d => d.Authors, o => o.MapFrom(s => s.authors))
                .ForMember(d => d.ExtId, o => o.MapFrom(s => s.ext_id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.name))
                .ForMember(d => d.Type, o => o.MapFrom(s => s.type))
                .ForMember(d => d.Updated, o => o.MapFrom(s => s.updated))
                ;
            Mapper.CreateMap<PublicationEditViewModel, SciVacancies.Domain.Core.Publication>()
                ;
            Mapper.CreateMap<SciVacancies.Domain.Core.Publication, PublicationEditViewModel>()
                ;
        }
        public static void InitializeSearchSubscription()
        {
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
        }
        public static void InitializeVacancyApplication()
        {
            Mapper.CreateMap<VacancyApplicationCreated, VacancyApplication>()
                .ForMember(d => d.guid, o => o.MapFrom(s => s.VacancyApplicationGuid))
                .ForMember(d => d.researcher_fullname, o => o.MapFrom(s => s.Data.ResearcherFullName))
                .ForMember(d => d.position_name, o => o.MapFrom(s => s.Data.PositionName))
                .ForMember(d => d.email, o => o.MapFrom(s => s.Data.Email))
                .ForMember(d => d.extraemail, o => o.MapFrom(s => s.Data.ExtraEmail))
                .ForMember(d => d.extraphone, o => o.MapFrom(s => s.Data.ExtraPhone))
                .ForMember(d => d.research_activity, o => o.MapFrom(s => s.Data.ResearchActivity))
                .ForMember(d => d.phone, o => o.MapFrom(s => s.Data.Phone))
                .ForMember(d => d.teaching_activity, o => o.MapFrom(s => s.Data.TeachingActivity))
                .ForMember(d => d.other_activity, o => o.MapFrom(s => s.Data.OtherActivity))
                .ForMember(d => d.science_degree, o => o.MapFrom(s => s.Data.ScienceDegree))
                .ForMember(d => d.science_rank, o => o.MapFrom(s => s.Data.ScienceRank))
                .ForMember(d => d.rewards, o => o.MapFrom(s => s.Data.Rewards))
                .ForMember(d => d.memberships, o => o.MapFrom(s => s.Data.Memberships))
                .ForMember(d => d.conferences, o => o.MapFrom(s => s.Data.Conferences))
                .ForMember(d => d.covering_letter, o => o.MapFrom(s => s.Data.CoveringLetter))
                .ForMember(d => d.educations, o => o.MapFrom(s => s.Data.Educations.Any() ? JsonConvert.SerializeObject(s.Data.Educations) : string.Empty))
                .ForMember(d => d.publications, o => o.MapFrom(s => s.Data.Publications.Any() ? JsonConvert.SerializeObject(s.Data.Publications) : string.Empty))
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
                .ForMember(d => d.educations, o => o.MapFrom(s => s.Data.Educations.Any() ? JsonConvert.SerializeObject(s.Data.Educations) : string.Empty))
                .ForMember(d => d.publications, o => o.MapFrom(s => s.Data.Publications.Any() ? JsonConvert.SerializeObject(s.Data.Publications) : string.Empty))
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
                .ForMember(d => d.Educations, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.educations) ? JsonConvert.DeserializeObject<List<Education>>(s.educations) : new List<Education>()))
                .ForMember(d => d.Publications, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.publications) ? JsonConvert.DeserializeObject<List<Publication>>(s.publications) : new List<Publication>()))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.email))
                .ForMember(d => d.ExtraEmail, o => o.MapFrom(s => s.extraemail))
                .ForMember(d => d.ExtraPhone, o => o.MapFrom(s => s.extraphone))
                .ForMember(d => d.FullName, o => o.MapFrom(s => s.researcher_fullname))
                .ForMember(d => d.Memberships, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.memberships) ? JsonConvert.DeserializeObject<List<MembershipDetailsViewModel>>(s.memberships) : new List<MembershipDetailsViewModel>()))
                .ForMember(d => d.OtherActivity, o => o.MapFrom(s => s.other_activity))
                .ForMember(d => d.Phone, o => o.MapFrom(s => s.phone))
                .ForMember(d => d.PositionTypeName, o => o.MapFrom(s => s.position_name))
                .ForMember(d => d.ResearchActivity, o => o.MapFrom(s => s.research_activity))
                .ForMember(d => d.ResearcherGuid, o => o.MapFrom(s => s.researcher_guid))
                .ForMember(d => d.Rewards, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.rewards) ? JsonConvert.DeserializeObject<List<RewardDetailsViewModel>>(s.rewards) : new List<RewardDetailsViewModel>()))
                .ForMember(d => d.ScienceDegree, o => o.MapFrom(s => s.science_degree))
                .ForMember(d => d.TeachingActivity, o => o.MapFrom(s => s.teaching_activity))
                .ForMember(d => d.UpdateDate, o => o.MapFrom(s => s.update_date))
                .ForMember(d => d.VacancyGuid, o => o.MapFrom(s => s.vacancy_guid))
                ;
            Mapper.CreateMap<VacancyApplication, VacancyApplicationSetWinnerViewModel>()
                .ForMember(d => d.Educations, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.educations) ? JsonConvert.DeserializeObject<List<Education>>(s.educations) : new List<Education>()))
                .ForMember(d => d.Publications, o => o.MapFrom(s => !string.IsNullOrWhiteSpace(s.publications) ? JsonConvert.DeserializeObject<List<Publication>>(s.publications) : new List<Publication>()))
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
        }
    }
}
