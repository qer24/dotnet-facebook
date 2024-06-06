using dotnet_facebook.Controllers.Services;
using dotnet_facebook.Models.Contexts;
using dotnet_facebook.Models.DatabaseObjects.Groups;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace dotnet_facebook.Controllers.RegularUser
{
    [Route("UserGroup")]
    public class UserGroupController : Controller
    {
        private readonly TestContext _context;
        private readonly GroupService _groupService;
        private readonly UserService _userservice;

        public UserGroupController(TestContext context, GroupService groupService, UserService userservice)
        {
            _context = context;
            _groupService = groupService;
            _userservice = userservice;
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
            var userRole = group.Users.FirstOrDefault(u => u.User.Nickname == User.Identity.Name)?.GroupRole;


            ViewBag.IsAdmin = userRole == GroupRole.Admin;

            return View(group);
        }
        [HttpGet("GroupNotFound")]
        public IActionResult GroupNotFound()
        {
            return View();
        }
    } 
}
