using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Venta.Domain.Models;
using Venta.Infrastructure.External.Contracts.Request.Productos;
using Venta.Infrastructure.External.Contracts.Response.Productos;
using Venta.Infrastructure.External.CustomExceptions;
using Venta.Infrastructure.External.Interfaces;
using Venta.Infrastructure.Settings;

namespace Venta.Infrastructure.External.Repositories
{
    public class ProductosApiRepository : IProductosApiRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductosApiRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Producto>> ConsultarProductosAsync(string accessToken, string filtro)
        {
            var client = _httpClientFactory.CreateClient();
            var path = $"{client.BaseAddress}{ApiEndpoints.ConsultarProductos}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var bodyRequest = new ConsultarProductosRequest();

            if (filtro != null)
                bodyRequest.FiltroPorNombre = filtro;

            var jsonBody = JsonSerializer.Serialize(bodyRequest);
            var body = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, path) { Content = body };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var errorBody = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new HttpClientCustomException((int)httpResponseMessage.StatusCode, errorBody);
            }

            var consultarResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ConsultarProductosResponse>();
            return consultarResponse?.Resultado?
                .Select(r => new Producto
                {
                    IdProducto = r.IdProducto,
                    Nombre = r.Nombre,
                    Stock = r.Stock,
                    StockMinimo = r.StockMinimo,
                    PrecioUnitario = r.PrecioUnitario,
                    IdCategoria = r.IdCategoria
                }).ToList() ?? new List<Producto>();
        }

        public async Task<bool> AgregarProductoAsync(Producto producto, string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            var path = $"{client.BaseAddress}{ApiEndpoints.AgregarProducto}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var bodyRequest = new RegistrarProductosRequest
            {
                Nombre = producto.Nombre,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo,
                PrecioUnitario = producto.PrecioUnitario,
                IdCategoria = producto.IdCategoria
            };
            var jsonBody = JsonSerializer.Serialize(bodyRequest);
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

        public async Task<bool> ActualizarProductoAsync(Producto producto, string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            var path = $"{client.BaseAddress}{ApiEndpoints.ActualizarProducto}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var bodyRequest = new ActualizarProductosRequest
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo,
                PrecioUnitario = producto.PrecioUnitario,
                IdCategoria = producto.IdCategoria
            };
            var jsonBody = JsonSerializer.Serialize(bodyRequest);
            var body = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, path) { Content = body };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);


            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var errorBody = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new HttpClientCustomException((int)httpResponseMessage.StatusCode, errorBody);
            }

            return true;
        }

        public async Task<bool> ModificarStockAsync(int idProducto, int cantidad, string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            var path = $"{client.BaseAddress}{ApiEndpoints.ModificarStockProducto}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var bodyRequest = new ActualizarStockProductosRequest
            {
                IdProducto = idProducto,
                Stock = cantidad
            };
            var jsonBody = JsonSerializer.Serialize(bodyRequest);
            var body = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Put, path) { Content = body };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);


            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var errorBody = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new HttpClientCustomException((int)httpResponseMessage.StatusCode, errorBody);
            }

            return true;
        }

        public async Task<bool> EliminarProductoAsync(int idProducto, string accessToken)
        {
            var client = _httpClientFactory.CreateClient();
            var path = $"{client.BaseAddress}{ApiEndpoints.EliminarProducto}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var eliminarRequest = new EliminarProductosRequest
            {
                IdProducto = idProducto
            };
            var jsonBody = JsonSerializer.Serialize(eliminarRequest);
            var body = new StringContent(jsonBody, Encoding.UTF8, MediaTypeNames.Application.Json);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, path) { Content = body };
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);


            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                var errorBody = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new HttpClientCustomException((int)httpResponseMessage.StatusCode, errorBody);
            }

            return true;
        }
    }
}
