using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Worker;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Worker
{
    public class RefreshAccessGroupWorkerHandler : IRequestHandler<RefreshAccessGroupWorkerRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public RefreshAccessGroupWorkerHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(RefreshAccessGroupWorkerRequest request, CancellationToken cancellationToken)
        {
            
        }
       
    }
}
