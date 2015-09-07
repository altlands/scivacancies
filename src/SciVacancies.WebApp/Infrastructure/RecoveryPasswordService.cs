using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace SciVacancies.WebApp.Infrastructure
{
    public class RecoveryPasswordService: IRecoveryPasswordService
    {
        private IMediator _mediator;
        public RecoveryPasswordService(IMediator mediator)
        {
            _mediator = mediator;
        }



    }

    public interface IRecoveryPasswordService
    {
        
    }
}
