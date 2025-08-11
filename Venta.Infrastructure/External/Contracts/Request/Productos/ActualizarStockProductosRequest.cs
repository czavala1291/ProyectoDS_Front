using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta.Infrastructure.External.Contracts.Request.Productos
{
    public class ActualizarStockProductosRequest
    {
        public int IdProducto { get; set; }
        public int Stock { get; set; }
    }
}
