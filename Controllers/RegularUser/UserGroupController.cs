using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System.Threading.Tasks;

namespace dotnet_facebook.Controllers.RegularUser
{
    [Route("UserGroup")]
    public class UserGroupController : Controller
    {
        private readonly TestContext _context;
        private readonly GroupService _groupService;

        public UserGroupController(TestContext context, GroupService groupService)
        {
            _context = context;
            _groupService = groupService;
        }

        // GET: UserGroup
        [HttpGet("{id?}")]
        [OutputCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("GroupNotFound");
            }

            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null)
            {
                return RedirectToAction("GroupNotFound");
            }

            return View(group);
        }

        [HttpGet("GroupNotFound")]
        public IActionResult GroupNotFound()
        {
            return View();
        }
    } 
}
