using Venta.Domain.Models;
using Venta.Infrastructure.External.Interfaces;
using Venta.ui.Services.Interfaces;

namespace Venta.ui.Services
{
    public class VentaService : IVentaService
    {
        private readonly IVentasApiRepository _ventasApiRepository;

        public VentaService(IVentasApiRepository ventasApiRepository)
        {
            _ventasApiRepository = ventasApiRepository;
        }

        public async Task<bool> RegistrarAsync(Domain.Models.Venta venta, string accessToken)
        {
            return await _ventasApiRepository.Registrar(venta, accessToken);
        }

        public async Task<IEnumerable<Pago>> GetPagosAsync(string accessToken)
        {
            return await _ventasApiRepository.Get(accessToken);
        }
    }
}
