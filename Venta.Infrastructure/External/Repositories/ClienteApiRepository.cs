using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Models;
using Venta.Infrastructure.External.CustomExceptions;
using Venta.Infrastructure.Settings;

namespace Venta.Infrastructure.External.Repositories
{
    public class ClienteApiRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ClienteApiRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Cliente?> ConsultarAsync(int idCliente, string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            var path = $"{client.BaseAddress}{ApiEndpoints.ConsultarCliente}/{idCliente}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, path);
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var errorBody = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new HttpClientCustomException((int)httpResponseMessage.StatusCode, errorBody);
            }

            return await httpResponseMessage.Content.ReadFromJsonAsync<Cliente>();
        }
    }
}
