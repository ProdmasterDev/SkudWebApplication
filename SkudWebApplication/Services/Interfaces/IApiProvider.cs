namespace SkudWebApplication.Services.Interfaces
{
    public interface IApiProvider
    {
        //string ApiDomain { get; }
        Task<TResponse> SendGetRequestAsync<TRequest, TResponse>(string apiMethod, TRequest request) where TResponse : class, new();
        Task SendAddRequestAsync<TRequest>(string apiMethod, TRequest request) where TRequest: class;
        Task SendEditRequestAsync<TRequest>(string apiMethod, TRequest request) where TRequest : class;
        Task SendDeleteRequestAsync<TRequest>(string apiMethod, TRequest request) where TRequest : class;
    }
}
