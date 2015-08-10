using SciVacancies.Domain.Aggregates;

using System;

using CommonDomain.Persistence;
using MediatR;
using SciVacancies.WebApp.Engine.SmtpNotificators;

namespace SciVacancies.WebApp.Commands
{
    public class CreateVacancyApplicationTemplateCommandHandler : IRequestHandler<CreateVacancyApplicationTemplateCommand, Guid>
    {
        private readonly IRepository _repository;

        public CreateVacancyApplicationTemplateCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Guid Handle(CreateVacancyApplicationTemplateCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            Guid vacancyApplicationGuid = researcher.CreateVacancyApplicationTemplate(msg.VacancyGuid, msg.Data);
            _repository.Save(researcher, Guid.NewGuid(), null);

            //TODO: вынести в отдельный VacancyCreatedEventHandler
            ////send email notification
            //var vacancyStatusChangedSmtpNotificator = new VacancyStatusChangedSmtpNotificator();
            //vacancyStatusChangedSmtpNotificator.Send(msg.Data.FullName, message.Data.UserName, message.Data.Password, message.Data.Email);

            return vacancyApplicationGuid;
        }
    }
    public class CreateAndApplyVacancyApplicationCommandHandler : IRequestHandler<CreateAndApplyVacancyApplicationCommand, Guid>
    {
        private readonly IRepository _repository;

        public CreateAndApplyVacancyApplicationCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Guid Handle(CreateAndApplyVacancyApplicationCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            Guid vacancyApplicationGuid = researcher.CreateVacancyApplicationTemplate(msg.VacancyGuid, msg.Data);
            researcher.ApplyToVacancy(vacancyApplicationGuid);
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

        protected override void HandleCore(UpdateVacancyApplicationTemplateCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.UpdateVacancyApplicationTemplate(msg.VacancyApplicationGuid, msg.Data);
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

        protected override void HandleCore(RemoveVacancyApplicationTemplateCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.RemoveVacancyApplicationTemplate(msg.VacancyApplicationGuid);
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

        protected override void HandleCore(ApplyVacancyApplicationCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.ApplyToVacancy(msg.VacancyApplicationGuid);
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

        protected override void HandleCore(CancelVacancyApplicationCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.CancelVacancyApplication(msg.VacancyApplicationGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
    public class MakeVacancyApplicationWinnerCommandHandler : RequestHandler<MakeVacancyApplicationWinnerCommand>
    {
        private readonly IRepository _repository;

        public MakeVacancyApplicationWinnerCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(MakeVacancyApplicationWinnerCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.MakeVacancyApplicationWinner(msg.VacancyApplicationGuid, msg.Reason);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
    public class MakeVacancyApplicationPretenderCommandHandler : RequestHandler<MakeVacancyApplicationPretenderCommand>
    {
        private readonly IRepository _repository;

        public MakeVacancyApplicationPretenderCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(MakeVacancyApplicationPretenderCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.MakeVacancyApplicationPretender(msg.VacancyApplicationGuid, msg.Reason);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
    public class MakeVacancyApplicationLooserCommandHandler : RequestHandler<MakeVacancyApplicationLooserCommand>
    {
        private readonly IRepository _repository;

        public MakeVacancyApplicationLooserCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(MakeVacancyApplicationLooserCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            researcher.MakeVacancyApplicationLooser(msg.VacancyApplicationGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);
        }
    }
}