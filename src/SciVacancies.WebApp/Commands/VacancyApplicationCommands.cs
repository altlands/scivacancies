using SciVacancies.Domain.DataModels;

using System;

using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateVacancyApplicationTemplateCommand : CommandBase, IRequest<Guid>
    {
        public Guid ResearcherGuid { get; set; }
        public Guid VacancyGuid { get; set; }

        public VacancyApplicationDataModel Data { get; set; }
    }
    public class UpdateVacancyApplicationTemplateCommand : CommandBase, IRequest
    {
        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }

        public VacancyApplicationDataModel Data { get; set; }
    }
    public class RemoveVacancyApplicationTemplateCommand : CommandBase, IRequest
    {
        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }
    }
    public class ApplyVacancyApplicationCommand : CommandBase, IRequest
    {
        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }
    }
    public class CancelVacancyApplicationCommand : CommandBase, IRequest
    {
        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }
    }
    //public class MakeVacancyApplicationWinnerCommand : CommandBase, IRequest
    //{

    //}
    //public class MakeVacancyApplicationPretenderCommand : CommandBase, IRequest
    //{

    //}
}
