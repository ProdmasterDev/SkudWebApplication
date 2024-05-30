using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.Card;
using DB = ControllerDomain.Entities;

namespace SkudWebApplication.Handlers.Card
{
    public class DeleteCardHandler : IRequestHandler<DeleteCardRequest>
    {
        private readonly IMapper _mapper;
        private readonly WebAppContext _dbContext;
        public DeleteCardHandler(WebAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task Handle(DeleteCardRequest request, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Set<DB.Card>().Include(x => x.Worker).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (entity is null)
            {
                return;
            }

            entity.Arch = true;
            entity.Worker = null;

            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
