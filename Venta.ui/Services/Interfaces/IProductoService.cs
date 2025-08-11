using Venta.Domain.Models;

namespace Venta.ui.Services.Interfaces
{
    public interface IProductoService
    {
        Task<List<Producto>> ConsultarProductosAsync(string filtro, string accessToken);
        Task<bool> AgregarProductoAsync(Producto producto, string accessToken);
        Task<bool> ActualizarProductoAsync(Producto producto, string accessToken);
        Task<bool> EliminarProductoAsync(int idProducto, string accessToken);
        Task<bool> ModificarStockAsync(int idProducto, int cantidad, string accessToken);

    }
}
