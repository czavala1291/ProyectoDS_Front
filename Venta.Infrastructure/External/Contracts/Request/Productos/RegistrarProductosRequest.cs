using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Venta.Infrastructure.External.Contracts.Request.Productos
{
    public class RegistrarProductosRequest
    {
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public decimal PrecioUnitario { get; set; }

        public int IdCategoria { get; set; }
    }
}
