using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Aris.ServerTest.Filters;

namespace Aris.ServerTest.Controllers
{
    
    [ReturnUrlFromRequest]
    public class HomeController : BaseController
    {
        private readonly Services.IKoreApiGameService _gameService;

        public HomeController(Services.IKoreApiGameService gameService) 
        {
            _gameService = gameService;
        }

        public async Task<IActionResult> Index(string catFilter, string returnUrl)
        {
            var viewModel = new ViewModels.GamesListViewModel();
            var games = await _gameService.GetGamesAsync(GetAuthToken(), returnUrl);

            //Task 4. Ordered the list to 'Category', then by 'Platform', then by 'Name'
            viewModel.Games = games.OrderBy(g => g.Category)
                .ThenBy(g => g.Platform)
                .ThenBy(g => g.Name);

            return View(viewModel);
        }

        public async Task<IActionResult> Details(string game, string returnUrl)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(game);
            var gameUrl = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            var gameJson = await _gameService.GetGameAsync(GetAuthToken(), gameUrl, returnUrl);

            return Json(gameJson);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
