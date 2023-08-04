using LevRobinGuss.Models;
using LevRobinGuss.Service;
using Microsoft.AspNetCore.Mvc;
using Nethereum.Web3;
using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;
using System.Net;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using System.Globalization;
using LevRobinGuss.Models.SmartContract;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using BalanceOfFunction = Nethereum.Contracts.Standards.ERC20.ContractDefinition.BalanceOfFunction;
using Nethereum.Util;
using LevRobinGuss.VIewModels;
using LevRobinGuss.Core.Data;
using LevRobinGuss.Core.Entities;

namespace LevRobinGuss.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FootballDataAPi _footballDataApi;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger
            , FootballDataAPi footballDataAPi,
            ApplicationDbContext context)
        {
            _logger = logger;
            _footballDataApi = footballDataAPi;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            
            var datatimenow = DateTime.Now.AddDays(-1);
            var from = datatimenow.ToString("yyyy-MM-dd");
            var to = datatimenow.AddDays(10).ToString("yyyy-MM-dd");
            var footballData = await _footballDataApi.MatchesList(to, from);

            var homeViewModel = new HomeViewModel()
            {
                GameMatches = footballData,
                GameViewModel = new GameViewModel()
            };
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }


      

      
    }
}
