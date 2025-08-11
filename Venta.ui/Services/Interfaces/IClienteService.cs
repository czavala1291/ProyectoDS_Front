using Venta.Domain.Models;

namespace Venta.ui.Services.Interfaces
{
    public interface IClienteService
    {
        Task<Cliente?> ConsultarAsync(int idCliente, string accessToken);

    }
}
