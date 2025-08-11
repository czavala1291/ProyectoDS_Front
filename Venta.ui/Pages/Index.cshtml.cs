using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Net;
using Microsoft.Graph;
using Microsoft.AspNetCore.Authorization;

namespace Venta.ui.Pages
{
    [Authorize(Policy = "AdministratorOrUser")]
    public class IndexModel : PageModel
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, GraphServiceClient graphServiceClient)
        {
            _logger = logger;
            _graphServiceClient = graphServiceClient;;
        }

        public async Task OnGet()
        {
            var user = await _graphServiceClient.Me.GetAsync();
            ViewData["GraphApiResult"] = user.DisplayName;;

        }
    }
}
