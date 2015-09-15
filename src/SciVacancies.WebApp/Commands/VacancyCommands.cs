using SciVacancies.Domain.DataModels;

using System;
using System.Collections.Generic;
using MediatR;
using SciVacancies.Domain.Core;

namespace SciVacancies.WebApp.Commands
{
    public class CreateVacancyCommand : CommandBase, IRequest<Guid>
    {
        public Guid OrganizationGuid { get; set; }

        public VacancyDataModel Data { get; set; }
    }
    public class UpdateVacancyCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }

        public VacancyDataModel Data { get; set; }
    }
    public class RemoveVacancyCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }
    }

    public class PublishVacancyCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }
    }
    public class SwitchVacancyInCommitteeCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }
    }
    public class SetVacancyWinnerCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }

        public string Reason { get; set; }
        public List<VacancyAttachment> Attachments { get; set; } = new List<VacancyAttachment>();
    }
    public class SetVacancyPretenderCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }

        public Guid ResearcherGuid { get; set; }
        public Guid VacancyApplicationGuid { get; set; }

        public string Reason { get; set; }
    }
    public class SetVacancyToResponseAwaitingFromWinnerCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }
    }
    public class SetWinnerAcceptOfferCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }
    }
    public class SetWinnerRejectOfferCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }
    }
    public class SetVacancyToResponseAwaitingFromPretenderCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }
    }
    public class SetPretenderAcceptOfferCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }
    }
    public class SetPretenderRejectOfferCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }
    }

    public class CloseVacancyCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }
    }
    public class CancelVacancyCommand : CommandBase, IRequest
    {
        public Guid VacancyGuid { get; set; }

        public string Reason { get; set; }
    }

    public class AddVacancyToFavoritesCommand : CommandBase, IRequest<int>
    {
        public Guid ResearcherGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
    public class RemoveVacancyFromFavoritesCommand : CommandBase, IRequest<int>
    {
        public Guid ResearcherGuid { get; set; }
        public Guid VacancyGuid { get; set; }
    }
}