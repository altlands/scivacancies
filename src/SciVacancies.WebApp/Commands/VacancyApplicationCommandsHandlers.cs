using SciVacancies.Domain.Aggregates;

using System;

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

        public Guid Handle(CreateVacancyApplicationTemplateCommand msg)
        {
            VacancyApplication vacancyApplication = new VacancyApplication(Guid.NewGuid(), msg.ResearcherGuid, msg.VacancyGuid, msg.Data);
            _repository.Save(vacancyApplication, Guid.NewGuid(), null);

            return vacancyApplication.Id;
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
            VacancyApplication vacancyApplication = new VacancyApplication(Guid.NewGuid(), msg.ResearcherGuid, msg.VacancyGuid, msg.Data);
            vacancyApplication.ApplyToVacancy();
            _repository.Save(vacancyApplication, Guid.NewGuid(), null);

            return vacancyApplication.Id;
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
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            VacancyApplication vacancyApplication = _repository.GetById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.Update(msg.Data);
            _repository.Save(vacancyApplication, Guid.NewGuid(), null);
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
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            VacancyApplication vacancyApplication = _repository.GetById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.Remove();
            _repository.Save(vacancyApplication, Guid.NewGuid(), null);
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
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            VacancyApplication vacancyApplication = _repository.GetById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.ApplyToVacancy();
            _repository.Save(vacancyApplication, Guid.NewGuid(), null);
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
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            VacancyApplication vacancyApplication = _repository.GetById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.Cancel();
            _repository.Save(vacancyApplication, Guid.NewGuid(), null);
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
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            VacancyApplication vacancyApplication = _repository.GetById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.MakeVacancyApplicationWinner(msg.Reason);
            _repository.Save(vacancyApplication, Guid.NewGuid(), null);
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
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            VacancyApplication vacancyApplication = _repository.GetById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.MakeVacancyApplicationPretender(msg.Reason);
            _repository.Save(vacancyApplication, Guid.NewGuid(), null);
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
            if (msg.VacancyApplicationGuid == Guid.Empty) throw new ArgumentNullException($"VacancyApplicationGuid is empty: {msg.VacancyApplicationGuid}");

            VacancyApplication vacancyApplication = _repository.GetById<VacancyApplication>(msg.VacancyApplicationGuid);
            vacancyApplication.MakeVacancyApplicationLooser();
            _repository.Save(vacancyApplication, Guid.NewGuid(), null);
        }
    }
}