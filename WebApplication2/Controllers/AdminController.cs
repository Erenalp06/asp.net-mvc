using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
	[Authorize(Roles ="admin")]
	//[Authorize(Roles = "admin, manager")]
	public class AdminController : Controller
	{
		//[Authorize]
		public IActionResult Index()
		{
			return View();
		}
	}
}
