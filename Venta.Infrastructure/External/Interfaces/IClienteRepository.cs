using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Venta.Domain.Models;

namespace Venta.Infrastructure.External.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente?> ConsultarAsync(int idCliente, string accessToken);

    }
}
