using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public HomeController(ICategoryService categoryService, IArticleService articleService, ICommentService commentService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _articleService = articleService;
            _commentService = commentService;
            _userManager = userManager;
        }
        [Authorize(Roles = "SuperAdmin,AdminArea.Home.Read")]
        public async Task<IActionResult> Index()
        {
            var categoriesCountResult = await _categoryService.CountByNonDeletedAsync();
            var articleCountResult = await _articleService.CountByNonDeletedAsync();
            var commentCountResult= await _commentService.CountByNonDeletedAsync();
            var userCount = await _userManager.Users.CountAsync();
            var articlesResult = await _articleService.GetAllAsync();
            if (categoriesCountResult.ResultStatus==Shared.Utilities.Results.ComplexTypes.ResultStatus.Success&&articleCountResult.ResultStatus==Shared.Utilities.Results.ComplexTypes.ResultStatus.Success&&commentCountResult.ResultStatus==Shared.Utilities.Results.ComplexTypes.ResultStatus.Success&&userCount>-1&&articlesResult.ResultStatus==Shared.Utilities.Results.ComplexTypes.ResultStatus.Success)
            {
                return View(new DashboardViewModel
                {
                    CategoriesCount = categoriesCountResult.Data,
                    ArticlesCount = articleCountResult.Data,
                    CommentsCount = commentCountResult.Data,
                    UsersCount=userCount,
                    Articles=articlesResult.Data
                });
            }
            return NotFound ();
        }
    }
}
