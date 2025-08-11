using Venta.Domain.Models;

namespace Venta.Infrastructure.External.Interfaces
{
    public interface IVentasApiRepository
    {
        Task<bool> Registrar(Domain.Models.Venta venta, string accessToken);
        Task<IEnumerable<Pago>> Get(string accessToken);
    }
}
