using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Venta.Domain.Models;

namespace Venta.ui.Pages
{
    [Authorize(Policy = "Administrator")]
    public class VentasModel : PageModel
    {
        public List<Producto> Productos { get; set; } = new List<Producto>();
        public string? _accessToken;
        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["nonce"] = HttpContext.Items["nonce"] ?? string.Empty;

            var authenticateResult = await HttpContext.AuthenticateAsync();
            _accessToken = authenticateResult.Properties.GetTokenValue("access_token");

            return Page();
        }

    }
}
