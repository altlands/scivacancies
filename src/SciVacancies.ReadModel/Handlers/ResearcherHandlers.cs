using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
    public class ResearcherCreatedHandler : EventBaseHandler<ResearcherCreated>
    {
        public ResearcherCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(ResearcherCreated msg)
        {
            Researcher researcher = new Researcher()
            {
                Guid = msg.ResearcherGuid,

                Login = msg.Data.Login,

                FirstName = msg.Data.FirstName,
                SecondName = msg.Data.SecondName,
                Patronymic = msg.Data.Patronymic,

                FirstNameEng = msg.Data.FirstNameEng,
                SecondNameEng = msg.Data.SecondNameEng,
                PatronymicEng = msg.Data.PatronymicEng,

                PreviousSecondName = msg.Data.PreviousSecondName,

                BirthDate=msg.Data.BirthDate,

                Email=msg.Data.Email,
                ExtraEmail=msg.Data.ExtraEmail,

                Phone=msg.Data.Phone,
                ExtraPhone=msg.Data.ExtraPhone,

                Nationality=msg.Data.Nationality,

                CreationDate = msg.TimeStamp
            };

            _db.Insert(researcher);
        }
    }
    public class ResearcherUpdateHandler:EventBaseHandler<ResearcherUpdated>
    {
        public ResearcherUpdateHandler(IDatabase db) : base(db) { }
        public override void Handle(ResearcherUpdated msg)
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

            researcher.UpdateDate = msg.TimeStamp;

            _db.Update(researcher);
        }
    }
    public class ResearcherRemovedHandler : EventBaseHandler<ResearcherRemoved>
    {
        public ResearcherRemovedHandler(IDatabase db) : base(db) { }
        public override void Handle(ResearcherRemoved msg)
        {
            _db.Delete<Researcher>(msg.ResearcherGuid);
        }
    }
}
