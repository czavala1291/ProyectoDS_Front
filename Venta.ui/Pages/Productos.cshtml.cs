using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Venta.Domain.Models;
using Venta.ui.Services.Interfaces;

namespace Venta.ui.Pages
{
    public class ProductosModel : PageModel
    {
        private readonly IProductoService _productoService;
        private string? _accessToken;

        public ProductosModel(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public List<Producto> listaProductos { get; set; } = new List<Producto>();

        [BindProperty]
        public Producto NuevoProducto { get; set; } = new Producto();

        [BindProperty]
        public Producto ProductoEditado { get; set; } = new Producto();

        [BindProperty]
        public int IdProductoEliminar { get; set; }

        public async Task OnGetAsync()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync();
            _accessToken = authenticateResult.Properties.GetTokenValue("access_token");
            listaProductos = await _productoService.ConsultarProductosAsync(string.Empty, _accessToken);
        }

        public async Task<IActionResult> OnPostAgregarAsync()
        {
            var exito = await _productoService.AgregarProductoAsync(NuevoProducto, _accessToken);
            if (exito)
                return RedirectToPage();
            ModelState.AddModelError("", "Error al agregar producto");
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostActualizarAsync()
        {
            var exito = await _productoService.ActualizarProductoAsync(ProductoEditado, _accessToken);
            if (exito)
                return RedirectToPage();
            ModelState.AddModelError("", "Error al actualizar producto");
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync()
        {
            var exito = await _productoService.EliminarProductoAsync(IdProductoEliminar, _accessToken);
            if (exito)
                return RedirectToPage();
            ModelState.AddModelError("", "Error al eliminar producto");
            await OnGetAsync();
            return Page();
        }
    }
}
