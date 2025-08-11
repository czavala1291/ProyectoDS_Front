using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta.Infrastructure.Settings
{
    public static class ApiEndpoints
    {
        public const string ConsultarCliente = "/api/clientes/consultar";
        public const string ConsultarProductos = "/api/productos/consultar";
        public const string AgregarProducto = "/api/productos/agregar";
        public const string ActualizarProducto = "/api/productos/actualizar";
        public const string ModificarStockProducto = "/api/productos/actualizar-stock";
        public const string EliminarProducto = "/api/productos/eliminar";
        public const string RegistrarVenta = "/api/ventas/registrar";
        public const string ConsultarPagos = "/api/ventas/pagos";
    }
}
