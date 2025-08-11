using Venta.Domain.Models;
using Venta.Infrastructure.External.Interfaces;
using Venta.ui.Services.Interfaces;

namespace Venta.ui.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<Cliente?> ConsultarAsync(int idCliente, string accessToken)
        {
            return await _clienteRepository.ConsultarAsync(idCliente, accessToken);
        }
    }
}
