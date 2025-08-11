using Venta.Domain.Models;

namespace Venta.ui.Services.Interfaces
{
    public interface IVentaService
    {
        Task<bool> RegistrarAsync(Domain.Models.Venta venta, string accessToken);
        Task<IEnumerable<Pago>> GetPagosAsync(string accessToken);
    }
}
