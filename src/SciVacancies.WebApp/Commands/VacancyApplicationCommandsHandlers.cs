using SciVacancies.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using CommonDomain.Persistence;
using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateVacancyApplicationTemplateCommandHandler : IRequestHandler<CreateVacancyApplicationTemplateCommand, Guid>
    {
        private readonly IRepository _repository;

        public CreateVacancyApplicationTemplateCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Guid Handle(CreateVacancyApplicationTemplateCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");
            if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {message.VacancyGuid}");

            var rdm = message.Data;

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            Guid vacancyApplicationGuid = researcher.CreateVacancyApplicationTemplate(message.VacancyGuid, rdm);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return vacancyApplicationGuid;
        }
    }
    public class UpdateVacancyApplicationTemplateCommandHandler : RequestHandler<UpdateVacancyApplicationTemplateCommand>
    {
        private readonly IRepository _repository;

        public UpdateVacancyApplicationTemplateCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(UpdateVacancyApplicationTemplateCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");
            if (message.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {message.VacancyApplicationGuid}");

            var rdm = message.Data;

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            researcher.UpdateVacancyApplicationTemplate(message.VacancyApplicationGuid, rdm);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
    public class RemoveVacancyApplicationTemplateCommandHandler : RequestHandler<RemoveVacancyApplicationTemplateCommand>
    {
        private readonly IRepository _repository;

        public RemoveVacancyApplicationTemplateCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(RemoveVacancyApplicationTemplateCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");
            if (message.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {message.VacancyApplicationGuid}");

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            researcher.RemoveVacancyApplicationTemplate(message.VacancyApplicationGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
    public class ApplyVacancyApplicationCommandHandler : RequestHandler<ApplyVacancyApplicationCommand>
    {
        private readonly IRepository _repository;

        public ApplyVacancyApplicationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(ApplyVacancyApplicationCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");
            if (message.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {message.VacancyApplicationGuid}");

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            researcher.ApplyToVacancy(message.VacancyApplicationGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
    public class CancelVacancyApplicationCommandHandler : RequestHandler<CancelVacancyApplicationCommand>
    {
        private readonly IRepository _repository;

        public CancelVacancyApplicationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(CancelVacancyApplicationCommand message)
        {
            if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");
            if (message.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {message.VacancyApplicationGuid}");

            Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
            researcher.CancelVacancyApplication(message.VacancyApplicationGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
    ////TODO - нужен тут:?
    //public class MakeVacancyApplicationWinnerCommandHandler : RequestHandler<MakeVacancyApplicationWinnerCommand>
    //{
    //    private readonly IRepository _repository;

    //    public MakeVacancyApplicationWinnerCommandHandler(IRepository repository)
    //    {
    //        _repository = repository;
    //    }

    //    protected override void HandleCore(MakeVacancyApplicationWinnerCommand message)
    //    {
    //        if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");
    //        if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {message.VacancyGuid}");

    //        if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");
    //        if (message.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {message.VacancyApplicationGuid}");

            

    //        Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
    //        researcher.MakeVacancyApplicationWinner(message.VacancyApplicationGuid)
    //        //_repository.Save(researcher, Guid.NewGuid(), null);
    //    }
    //}
    ////TODO - нужен тут:?
    //public class MakeVacancyApplicationPretenderCommandHandler : RequestHandler<MakeVacancyApplicationPretenderCommand>
    //{
    //    private readonly IRepository _repository;

    //    public MakeVacancyApplicationPretenderCommandHandler(IRepository repository)
    //    {
    //        _repository = repository;
    //    }

    //    protected override void HandleCore(MakeVacancyApplicationPretenderCommand message)
    //    {
    //        if (message.OrganizationGuid == Guid.Empty) throw new ArgumentNullException($"OrganizationGuid is empty: {message.OrganizationGuid}");
    //        if (message.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {message.VacancyGuid}");

    //        if (message.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {message.ResearcherGuid}");
    //        if (message.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {message.VacancyApplicationGuid}");

    //        //Researcher researcher = _repository.GetById<Researcher>(message.ResearcherGuid);
    //        //researcher.CancelVacancyApplication(message.VacancyApplicationGuid);
    //        //_repository.Save(researcher, Guid.NewGuid(), null);
    //    }
    //}
}