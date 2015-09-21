using SciVacancies.Domain.Aggregates;

using System;

using CommonDomain.Persistence;
using MediatR;

namespace SciVacancies.WebApp.Commands
{
    public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand, Guid>
    {
        private readonly IRepository _repository;

        public CreateVacancyCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public Guid Handle(CreateVacancyCommand msg)
        {
            Vacancy vacancy = new Vacancy(Guid.NewGuid(), msg.OrganizationGuid, msg.Data);
            _repository.Save(vacancy, Guid.NewGuid(), null);

            return vacancy.Id;
        }
    }
    public class UpdateVacancyCommandHandler : RequestHandler<UpdateVacancyCommand>
    {
        private readonly IRepository _repository;

        public UpdateVacancyCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(UpdateVacancyCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.Update(msg.Data);
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class RemoveVacancyCommandHandler : RequestHandler<RemoveVacancyCommand>
    {
        private readonly IRepository _repository;

        public RemoveVacancyCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(RemoveVacancyCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.Remove();
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }

    public class PublishVacancyCommandHandler : RequestHandler<PublishVacancyCommand>
    {
        private readonly IRepository _repository;

        public PublishVacancyCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(PublishVacancyCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.Publish(msg.InCommitteeStartDate, msg.InCommitteeEndDate);
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class SwitchVacancyInCommitteeCommandHandler : RequestHandler<SwitchVacancyInCommitteeCommand>
    {
        private readonly IRepository _repository;

        public SwitchVacancyInCommitteeCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(SwitchVacancyInCommitteeCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.VacancyToCommittee();
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class ProlongVacancyInCommitteeCommandHandler : RequestHandler<ProlongVacancyInCommitteeCommand>
    {
        private readonly IRepository _repository;

        public ProlongVacancyInCommitteeCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(ProlongVacancyInCommitteeCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.ProlongInCommittee(msg.Reason, msg.InCommitteeEndDate);
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class SetVacancyCommitteeResolutionCommandHandler : RequestHandler<SetVacancyCommitteeResolutionCommand>
    {
        private readonly IRepository _repository;

        public SetVacancyCommitteeResolutionCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(SetVacancyCommitteeResolutionCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.SetCommitteeResolution(msg.Resolution, msg.Attachments);
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class SetVacancyWinnerCommandHandler : RequestHandler<SetVacancyWinnerCommand>
    {
        private readonly IRepository _repository;

        public SetVacancyWinnerCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(SetVacancyWinnerCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.SetWinner(msg.ResearcherGuid, msg.VacancyApplicationGuid);
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class SetVacancyPretenderCommandHandler : RequestHandler<SetVacancyPretenderCommand>
    {
        private readonly IRepository _repository;

        public SetVacancyPretenderCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(SetVacancyPretenderCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.SetPretender(msg.ResearcherGuid, msg.VacancyApplicationGuid);
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class SetVacancyToResponseAwaitingFromWinnerCommandHandler : RequestHandler<SetVacancyToResponseAwaitingFromWinnerCommand>
    {
        private readonly IRepository _repository;

        public SetVacancyToResponseAwaitingFromWinnerCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(SetVacancyToResponseAwaitingFromWinnerCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);

            if (vacancy.IsWinnerAccept.HasValue && vacancy.IsPretenderAccept.HasValue) throw new InvalidOperationException("IsWinnerAccept and IsPretenderAccept already have values");

            vacancy.VacancyToResponseAwaitingFromWinner();
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class SetWinnerAcceptOfferCommandHandler : RequestHandler<SetWinnerAcceptOfferCommand>
    {
        private readonly IRepository _repository;

        public SetWinnerAcceptOfferCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(SetWinnerAcceptOfferCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.WinnerAcceptOffer();
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class SetWinnerRejectOfferCommandHandler : RequestHandler<SetWinnerRejectOfferCommand>
    {
        private readonly IRepository _repository;

        public SetWinnerRejectOfferCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(SetWinnerRejectOfferCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.WinnerRejectOffer();
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class SetVacancyToResponseAwaitingFromPretenderCommandHandler : RequestHandler<SetVacancyToResponseAwaitingFromPretenderCommand>
    {
        private readonly IRepository _repository;

        public SetVacancyToResponseAwaitingFromPretenderCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(SetVacancyToResponseAwaitingFromPretenderCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);

            if (vacancy.IsWinnerAccept.HasValue && vacancy.IsPretenderAccept.HasValue) throw new InvalidOperationException("IsWinnerAccept and IsPretenderAccept already have values");

            vacancy.VacancyToResponseAwaitingFromPretender();
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class SetPretenderAcceptOfferCommandHandler : RequestHandler<SetPretenderAcceptOfferCommand>
    {
        private readonly IRepository _repository;

        public SetPretenderAcceptOfferCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(SetPretenderAcceptOfferCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.PretenderAcceptOffer();
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class SetPretenderRejectOfferCommandHandler : RequestHandler<SetPretenderRejectOfferCommand>
    {
        private readonly IRepository _repository;

        public SetPretenderRejectOfferCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(SetPretenderRejectOfferCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.PretenderRejectOffer();
            vacancy.Cancel("Победитель и претендент отказались от контракта");
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }

    public class CloseVacancyCommandHandler : RequestHandler<CloseVacancyCommand>
    {
        private readonly IRepository _repository;

        public CloseVacancyCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(CloseVacancyCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.Close();
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }
    public class CancelVacancyCommandHandler : RequestHandler<CancelVacancyCommand>
    {
        private readonly IRepository _repository;

        public CancelVacancyCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        protected override void HandleCore(CancelVacancyCommand msg)
        {
            if (msg.VacancyGuid == Guid.Empty) throw new ArgumentNullException($"VacancyGuid is empty: {msg.VacancyGuid}");

            Vacancy vacancy = _repository.GetById<Vacancy>(msg.VacancyGuid);
            vacancy.Cancel(msg.Reason);
            _repository.Save(vacancy, Guid.NewGuid(), null);
        }
    }


    public class AddVacancyToFavoritesCommandHandler : IRequestHandler<AddVacancyToFavoritesCommand, int>
    {
        private readonly IRepository _repository;

        public AddVacancyToFavoritesCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public int Handle(AddVacancyToFavoritesCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            int favoritesCount = researcher.AddVacancyToFavorites(msg.VacancyGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return favoritesCount;
        }
    }
    public class RemoveVacancyFromFavoritesCommandHandler : IRequestHandler<RemoveVacancyFromFavoritesCommand, int>
    {
        private readonly IRepository _repository;

        public RemoveVacancyFromFavoritesCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public int Handle(RemoveVacancyFromFavoritesCommand msg)
        {
            if (msg.ResearcherGuid == Guid.Empty) throw new ArgumentNullException($"ResearcherGuid is empty: {msg.ResearcherGuid}");

            Researcher researcher = _repository.GetById<Researcher>(msg.ResearcherGuid);
            int favoritesCount = researcher.RemoveVacancyFromFavorites(msg.VacancyGuid);
            _repository.Save(researcher, Guid.NewGuid(), null);

            return favoritesCount;
        }
    }
}