using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.Domain.Aggregates.Commands
{
    public class CreateVacancyApplicationTemplate : CommandBase, IRequest<Guid>
    {
        public CreateVacancyApplicationTemplate() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public VacancyApplicationDataModel Data { get; set; }
    }
    public class UpdateVacancyApplicationTemplate : CommandBase, IRequest
    {
        public UpdateVacancyApplicationTemplate() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }

        public VacancyApplicationDataModel Data { get; set; }
    }
    public class RemoveVacancyApplicationTemplate : CommandBase, IRequest
    {
        public RemoveVacancyApplicationTemplate() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }
    }
    public class SendVacancyApplication : CommandBase, IRequest
    {
        public SendVacancyApplication() : base() { }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }
    }
}
