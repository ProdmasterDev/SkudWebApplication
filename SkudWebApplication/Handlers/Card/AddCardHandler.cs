using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Card;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Card
{
    public class AddCardHandler : IRequestHandler<AddCardRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public AddCardHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(AddCardRequest request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<DB.Card>(request);
            if (entity.Worker != null)
                entity.Worker = await _dbContext.Set<DB.Worker>().FirstAsync(x => x.Id == entity.Worker.Id);
            await _dbContext.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
