using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Venta.Domain.Models;
using Venta.ui.Services.Interfaces;

namespace Venta.ui.Pages
{
    [Authorize(Policy = "AdministratorOrUser")]
    public class ClientesModel : PageModel
    {
        private readonly IClienteService _clienteService;

        public ClientesModel(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [BindProperty]
        public int IdClienteConsulta { get; set; }
        public string? _accessToken { get; set; }
        public Cliente ClienteConsultado { get; set; } = new Cliente();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["nonce"] = HttpContext.Items["nonce"] ?? string.Empty;
            // This method is intentionally left empty.
            // It can be used to initialize any data needed for the page.
            var authenticateResult = await HttpContext.AuthenticateAsync();
            _accessToken = authenticateResult.Properties.GetTokenValue("access_token");
            return Page();

        }


        public async Task<IActionResult> OnPostConsultarAsync()
        {
            if (IdClienteConsulta <= 0)
            {
                ModelState.AddModelError("", "Ingrese un ID válido.");
                return Page();
            }

            ClienteConsultado = await _clienteService.ConsultarAsync(IdClienteConsulta, _accessToken);

            if (ClienteConsultado == null)
            {
                ModelState.AddModelError("", "Cliente no encontrado.");
            }

            return Page();
        }
    }
}
