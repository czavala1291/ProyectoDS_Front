using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Venta.Domain.Models;
using Venta.Infrastructure.External.CustomExceptions;
using Venta.Infrastructure.Settings;

namespace Venta.Infrastructure.External.Repositories
{
    public class VentasApiRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public VentasApiRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> Registrar(Venta.Domain.Models.Venta venta, string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            var path = $"{client.BaseAddress}{ApiEndpoints.RegistrarVenta}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var jsonBody = JsonSerializer.Serialize(venta);
            var body = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, path) { Content = body };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var errorBody = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new HttpClientCustomException((int)httpResponseMessage.StatusCode, errorBody);
            }

            return true;
        }

        public async Task<IEnumerable<Pago>> Get(string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            var path = $"{client.BaseAddress}{ApiEndpoints.ConsultarPagos}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, path);
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var errorBody = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new HttpClientCustomException((int)httpResponseMessage.StatusCode, errorBody);
            }

            var pagos = await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<Pago>>();
            return pagos ?? new List<Pago>();
        }
    }
}
