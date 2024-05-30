using FluentValidation;
using Newtonsoft.Json;
using RestEase;
using System.Net;

namespace SkudWebApplication.Extensions
{
    public static class EnsureSuccessStatusCode
    {
        public async static Task<HttpResponseMessage> EnsureAndThrow(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode.Equals(HttpStatusCode.BadRequest))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ex = JsonConvert.DeserializeObject<ValidationException>(content);
                    throw ex ?? new ValidationException("Неизвестная ошибка валидации!");
                }

                if (response.StatusCode.Equals(HttpStatusCode.InternalServerError))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ex = JsonConvert.DeserializeObject<Exception>(content);
                    throw ex ?? new Exception("Неизвестная ошибка!");
                }

                throw new Exception("Непонятная ошибка");
            }
            return response;
        }
    }
}
