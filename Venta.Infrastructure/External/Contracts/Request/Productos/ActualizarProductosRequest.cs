namespace Venta.Infrastructure.External.Contracts.Request.Productos
{
    public class ActualizarProductosRequest
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public decimal PrecioUnitario { get; set; }

        public int IdCategoria { get; set; }
    }
}
