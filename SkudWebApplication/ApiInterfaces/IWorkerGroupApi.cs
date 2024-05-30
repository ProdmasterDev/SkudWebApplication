using Microsoft.AspNetCore.Mvc;
using RestEase;
using SkudWebApplication.Requests.WorkerGroup;

namespace SkudWebApplication.ApiInterfaces
{
    public interface IWorkerGroupApi
    {
        [Post("WorkerGroupApi")]
        Task<HttpResponseMessage> Add([Body] AddWorkerGroupRequest request);
        [Put("WorkerGroupApi")]
        Task<HttpResponseMessage> Edit([Body] EditWorkerGroupRequest request);
    }
}
