using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;
using MediatR;

namespace SciVacancies.ReadModel.EventHandlers
{
    public class ResearcherCreatedHandler : INotificationHandler<ResearcherCreated>
    {
        private readonly IDatabase _db;

        public ResearcherCreatedHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(ResearcherCreated msg)
        {
            Researcher researcher = new Researcher()
            {
                Guid = msg.ResearcherGuid,

                Login = msg.Data.UserId,

                FirstName = msg.Data.FirstName,
                SecondName = msg.Data.SecondName,
                Patronymic = msg.Data.Patronymic,

                FirstNameEng = msg.Data.FirstNameEng,
                SecondNameEng = msg.Data.SecondNameEng,
                PatronymicEng = msg.Data.PatronymicEng,

                PreviousSecondName = msg.Data.PreviousSecondName,

                BirthDate = msg.Data.BirthDate,

                Email = msg.Data.Email,
                ExtraEmail = msg.Data.ExtraEmail,

                Phone = msg.Data.Phone,
                ExtraPhone = msg.Data.ExtraPhone,

                Nationality = msg.Data.Nationality,

                ResearchActivity = msg.Data.ResearchActivity,
                TeachingActivity = msg.Data.TeachingActivity,
                OtherActivity = msg.Data.OtherActivity,

                ScienceDegree = msg.Data.ScienceDegree,
                AcademicStatus = msg.Data.AcademicStatus,
                Rewards = msg.Data.Rewards,
                Memberships = msg.Data.Memberships,
                Conferences = msg.Data.Conferences,

                CreationDate = msg.TimeStamp
            };

            _db.Insert(researcher);
        }
    }
    public class ResearcherUpdateHandler : INotificationHandler<ResearcherUpdated>
    {
        private readonly IDatabase _db;

        public ResearcherUpdateHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(ResearcherUpdated msg)
        {
            Researcher researcher = _db.SingleById<Researcher>(msg.ResearcherGuid);

            researcher.FirstName = msg.Data.FirstName;
            researcher.SecondName = msg.Data.SecondName;
            researcher.Patronymic = msg.Data.Patronymic;

            researcher.FirstNameEng = msg.Data.FirstNameEng;
            researcher.SecondNameEng = msg.Data.SecondNameEng;
            researcher.PatronymicEng = msg.Data.PatronymicEng;

            researcher.PreviousSecondName = msg.Data.PreviousSecondName;

            researcher.BirthDate = msg.Data.BirthDate;

            researcher.ExtraEmail = msg.Data.ExtraEmail;

            researcher.ExtraPhone = msg.Data.ExtraPhone;

            researcher.Nationality = msg.Data.Nationality;

            researcher.ResearchActivity = msg.Data.ResearchActivity;
            researcher.TeachingActivity = msg.Data.TeachingActivity;
            researcher.OtherActivity = msg.Data.OtherActivity;

            researcher.ScienceDegree = msg.Data.ScienceDegree;
            researcher.AcademicStatus = msg.Data.AcademicStatus;
            researcher.Rewards = msg.Data.Rewards;
            researcher.Memberships = msg.Data.Memberships;
            researcher.Conferences = msg.Data.Conferences;

            researcher.UpdateDate = msg.TimeStamp;

            _db.Update(researcher);
        }
    }
    //TODO - Удалять совсем или помечать удалённым
    public class ResearcherRemovedHandler : INotificationHandler<ResearcherRemoved>
    {
        private readonly IDatabase _db;

        public ResearcherRemovedHandler(IDatabase db)
        {
            _db = db;
        }
        public void Handle(ResearcherRemoved msg)
        {
            _db.Delete<Researcher>(msg.ResearcherGuid);
        }
    }
}
