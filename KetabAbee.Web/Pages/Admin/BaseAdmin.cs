using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KetabAbee.Web.Pages.Admin
{
    [Authorize]
    public class BaseAdmin : PageModel
    {

    }
}
