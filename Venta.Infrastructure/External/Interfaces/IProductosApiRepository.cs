using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Models;

namespace Venta.Infrastructure.External.Interfaces
{
    public interface IProductosApiRepository
    {
        Task<List<Producto>> ConsultarProductosAsync(string filtro,string accessToken);
        Task<bool> AgregarProductoAsync(Producto producto, string accessToken);
        Task<bool> ActualizarProductoAsync(Producto producto, string accessToken);
        Task<bool> EliminarProductoAsync(int idProducto, string accessToken);
        Task<bool> ModificarStockAsync(int idProducto, int cantidad, string accessToken);

    }
}
