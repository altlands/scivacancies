using SciVacancies.Domain.Enums;
using SciVacancies.Domain.Events;
using SciVacancies.ReadModel.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NPoco;

namespace SciVacancies.ReadModel.Handlers
{
    public class PositionCreatedHandler : EventBaseHandler<PositionCreated>
    {
        public PositionCreatedHandler(IDatabase db) : base(db) { }
        public override void Handle(PositionCreated msg)
        {
            Position position = new Position()
            {
                Guid = msg.PositionGuid,
                OrganizationGuid = msg.OrganizationGuid,
                PositionTypeGuid = msg.Data.PositionTypeGuid,

                Name = msg.Data.Name,
                FullName = msg.Data.FullName,

                Tasks = msg.Data.Tasks,

                SalaryFrom = msg.Data.SalaryFrom,
                SalaryTo = msg.Data.SalaryTo,

                Bonuses = msg.Data.Bonuses,

                ContractType = msg.Data.ContractType,
                ContractTime = msg.Data.ContractTime,
                SocialPackage = msg.Data.SocialPackage,
                Rent = msg.Data.Rent,
                OfficeAccomodation = msg.Data.OfficeAccomodation,
                TransportCompensation = msg.Data.TransportCompensation,
                RegionId = msg.Data.RegionId,
                CityName = msg.Data.CityName,
                Details = msg.Data.Details,

                ContactName = msg.Data.ContactName,
                ContactEmail = msg.Data.ContactEmail,
                ContactPhone = msg.Data.ContactPhone,
                ContactDetails = msg.Data.ContactDetails,

                Status = msg.Data.Status,

                CreationDate = msg.TimeStamp
            };

            _db.Insert(position);
        }
    }
    public class PositionUpdatedHandler : EventBaseHandler<PositionUpdated>
    {
        public PositionUpdatedHandler(IDatabase db) : base(db) { }
        public override void Handle(PositionUpdated msg)
        {
            Position position = _db.SingleById<Position>(msg.PositionGuid);

            position.PositionTypeGuid = msg.Data.PositionTypeGuid;

            position.Name = msg.Data.Name;
            position.FullName = msg.Data.FullName;

            position.Tasks = msg.Data.Tasks;

            position.SalaryFrom = msg.Data.SalaryFrom;
            position.SalaryTo = msg.Data.SalaryTo;

            position.Bonuses = msg.Data.Bonuses;

            position.ContractType = msg.Data.ContractType;
            position.ContractTime = msg.Data.ContractTime;
            position.SocialPackage = msg.Data.SocialPackage;
            position.Rent = msg.Data.Rent;
            position.OfficeAccomodation = msg.Data.OfficeAccomodation;
            position.TransportCompensation = msg.Data.TransportCompensation;
            position.RegionId = msg.Data.RegionId;
            position.CityName = msg.Data.CityName;
            position.Details = msg.Data.Details;

            position.ContactName = msg.Data.ContactName;
            position.ContactEmail = msg.Data.ContactEmail;
            position.ContactPhone = msg.Data.ContactPhone;
            position.ContactDetails = msg.Data.ContactDetails;

            position.Status = msg.Data.Status;

            position.UpdateDate = msg.TimeStamp;

            _db.Update(position);
        }
    }
    public class PositionRemovedHandler : EventBaseHandler<PositionRemoved>
    {
        public PositionRemovedHandler(IDatabase db) : base(db) { }
        public override void Handle(PositionRemoved msg)
        {
            _db.Delete<Position>(msg.PositionGuid);
        }
    }
}