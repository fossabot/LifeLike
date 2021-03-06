﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LifeLike.Data.Models;
using LifeLike.Data.Models.Enums;
using LifeLike.Repositories;
using LifeLike.Web.Extensions;
using LifeLike.Web.Utils;
using LifeLike.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LifeLike.Web.Controllers
{
    [Authorize]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IConfigRepository _config;
        private readonly IEventLogRepository _logger;
        private readonly IMapper _mapper;

        public HomeController(IConfigRepository config, IEventLogRepository logger, IMapper mapper,
        SignInManager<User> signInManager,
        ILinkRepository link)
        {
            _mapper = mapper;
            _logger = logger;
            _config = config;
        }
        // [HttpGet]
        // [AllowAnonymous]
        // public IActionResult Index()
        // {
        //     return View();
        // }
        [HttpGet("Api/Menu")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMenuLinks()
        {
            var   isLogged = User.Identity.IsAuthenticated;

            await _logger.AddStat("Menu", "Index", "Home");
            var list =   MenuList(isLogged);

           // var list = await _links.List(LinkCategory.Menu);
            return Json(list.Select(LinkViewModel.Get));
        }

      

        private static List<Link> MenuList(bool isLogged)
        {
            var context = new List<Link>
            {
                new Link
                {
                    Id = 1,
                    Action = "",
                    Controller = "Posts",
                    Name = "News",
                    IconName = "newspaper",

                    Category = LinkCategory.Menu
                },
                new Link
                {
                    Id = 2,
                    Action = "",
                    Controller = "Albums",
                    Name = "Albums",
                    IconName = "camera-retro",
                    Category = LinkCategory.Menu
                },
                new Link
                {
                    Id = 3,
                    Action = "",
                    Controller = "Videos",
                    Name = "VIDEOS",
                    IconName = "film",
                    Category = LinkCategory.Menu
                },
                new Link
                {
                    Id = 4,
                    Action = "",
                    Controller = "Pages",
                    Name = "PROJECTS",
                    IconName = "code",
                    Category = LinkCategory.Menu
                }
            };

            if (isLogged)
            context.Add(new Link
            {
                Id=6,
                Action = "",
                Controller = "Logs",
                Name = "Logs",
                IconName = "book",
                Category = LinkCategory.Menu
            });
            context.Add(new Link
            {
                Id=7,
                Action = "Contact",
                Controller = "Page",
                Name = "CONTACT",
                IconName = "at",
                Category = LinkCategory.Menu
            });
           
            return context;
        }

        [HttpGet("Api/Config")]
        public async Task<IActionResult> GetList()
        {
            await _logger.AddStat("Configs","Index", "Home");
            var configs =await _config.List();
            
            Debug.WriteLine(configs.ToJSON());
            return Ok(configs.Select(_mapper.Map<ConfigViewModel>));
        }
    }
}