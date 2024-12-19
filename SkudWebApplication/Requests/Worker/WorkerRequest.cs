using ControllerDomain.Entities;
using MediatR;
using SkudWebApplication.Db;
using SkudWebApplication.Requests.WorkerGroup;
using SkudWebApplication.Services.Interfaces;

namespace SkudWebApplication.Requests.Worker
{
    public abstract class WorkerRequest : IRequest
    {
        public int? Id { get; set; }
        public int DisanId { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public DateTime? DateBlock {  get; set; }
        public IEnumerable<AccessRequest> Accesses { get; set; } = new List<AccessRequest>();
        public IEnumerable<WorkerCard> Cards { get; set; } = new List<WorkerCard>();
        public IEnumerable<AccessGroupWorker> WorkerAccessGroup { get; set; } = new List<AccessGroupWorker>();
        public WorkerAccessMethod? AccessMethod { get; set; }
        public WorkerGroup? Group { get; set; }
        protected string _apiMethod = $"{nameof(WorkerRequest).Replace("Request", string.Empty)}Api";
        public abstract Task SendToApiAsync(IApiProvider apiProvider);
        public abstract Task ValidateAndThrow(WebAppContext dbContext);
    }
    public class WorkerCard
    {
        public int Id { get; set; }
        public string CardNumb16 { get; set; } = string.Empty;
        public string CardNumb { get; set; } = string.Empty;
        public bool IsNew { get; set; } = false;
    }
    public class WorkerGroup
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
    public class WorkerAccessMethod
    {
        public int Id { get; set; }
        public string Name { set; get; } = string.Empty;
    }

}
