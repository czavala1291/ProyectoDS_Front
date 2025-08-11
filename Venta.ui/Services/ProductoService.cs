using Azure.Core;
using System.Net.Http.Json;
using Venta.Domain.Models;
using Venta.Infrastructure.External.Contracts.Response.Productos;
using Venta.Infrastructure.External.Interfaces;
using Venta.ui.Services.Interfaces;


namespace Venta.ui.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductosApiRepository _productosApiRepository;

        public ProductoService(IProductosApiRepository productosApiRepository)
        {
            _productosApiRepository = productosApiRepository;
        }

        public async Task<List<Producto>> ConsultarProductosAsync(string filtro, string accessToken)
        {
            return await _productosApiRepository.ConsultarProductosAsync(filtro, accessToken);
        }

        public async Task<bool> AgregarProductoAsync(Producto producto, string accessToken)
        {
            return await _productosApiRepository.AgregarProductoAsync(producto, accessToken);
        }

        public async Task<bool> ActualizarProductoAsync(Producto producto, string accessToken)
        {
            return await _productosApiRepository.ActualizarProductoAsync(producto, accessToken);
        }

        public async Task<bool> EliminarProductoAsync(int idProducto, string accessToken)
        {
            return await _productosApiRepository.EliminarProductoAsync(idProducto, accessToken);
        }

        public async Task<bool> ModificarStockAsync(int idProducto, int cantidad, string accessToken)
        {
            return await _productosApiRepository.ModificarStockAsync(idProducto, cantidad, accessToken);
        }


    }
}
