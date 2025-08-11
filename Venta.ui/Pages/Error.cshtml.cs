using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Venta.ui.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // Propiedad para el código de error HTTP
        public int? ErrorCode { get; set; }

        // Propiedad para el cuerpo del error (opcional, útil para excepciones personalizadas)
        public string? Body { get; set; }

        private readonly ILogger<ErrorModel> _logger;

        public ErrorModel(ILogger<ErrorModel> logger)
        {
            _logger = logger;
        }

        public void OnGet(int? errorCode = null)
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ErrorCode = errorCode;
        }
    }

}
