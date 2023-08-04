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
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            //var web3 = new Web3("https://goerli.infura.io/v3/ec686ca032bc4f0da49034e99646a762");
            //var accounts = await web3.Eth.Accounts.SendRequestAsync();
            //var a = web3.Eth.Sign;
            //    string fromAddress = "0x26f250197A8356bDEFC923838d9c5F17476ce55E";
            //string privateKey = "53f1716172320f33a9254da404fa8bbe2f233e9af1acfa41c1734b4bb47ceda5";

            //var balance = await web3.Eth.GetBalance.SendRequestAsync("0x26f250197A8356bDEFC923838d9c5F17476ce55E");
            ////var balance1 = await web3.Eth.GetBalance.SendRequestAsync("0xaadd4e08c64d9f299945f5649c1848f35f3f68bc");
            //var contractAddress = "0xaadd4e08c64d9f299945f5649c1848f35f3f68bc";
            //var contractABI = System.IO.File.ReadAllText("wwwroot/ContractABI/ContractABI.json");


            //var account = accounts.FirstOrDefault();
            ////await metaMaskInterop.ConnectToMetaMask();

            //var contract = web3.Eth.GetContract(contractABI, contractAddress);


            //var senderAccount = new Account(senderPrivateKey);
            //var gasPrice = new HexBigInteger(20000000000); // Example gas price
            //var gasLimit = new HexBigInteger(90000); // Example gas limit

            //var transferFunction = new TransferFunction();
            //var transactionInput = transferFunction.CreateTransactionInput(senderAccount.Address, new Address(recipientAddress), new HexBigInteger(amount));

            //var gas = await buyFunction.EstimateGasAsync(account);

            //var transactionInput = buyFunction.CreateTransactionInput(account, gas, null, null, null);

            //var transactionHash = await buyFunction.SendTransactionAsync(transactionInput);

            // Wait for the transaction to be mined and get the receipt
            //var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);




            //const result = await contract.methods.balanceOf(tokenHolder).call(); // 29803630997051883414242659
            //var function = contract.GetFunction("balanceOf");
            //var address = "0xaadd4e08c64d9f299945f5649c1848f35f3f68bc";

            //var balanceOfFunctionMessage = new BalanceOfFunction()
            //{
            //    Owner = address.Address,
            //};

            //var balanceHandler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();

            //var balance = await balanceHandler.QueryAsync<BigInteger>(contractAddress, balanceOfFunctionMessage);
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


        public IActionResult MakeBet(HomeViewModel model)
        {
            var name = User.Identity.Name;
            var userid = _context.Users.Where(x => x.Email == name).FirstOrDefault().Id;
            var gamebling = new Gambling()
            {
                AmountBet = (double)model.GameViewModel.AmountBet,
                BetOnHouse = model.GameViewModel.BetOnHouse,
                GameId = model.GameViewModel.GameId,
                BetPercentage = (double)model.GameViewModel.BetPercentage,
                UserId = userid,
                WinOrNot = WinOrNot.NotYet
            };
            _context.Add(gamebling);
            _context.SaveChanges();
            return RedirectToAction("index");

        }


        public async Task Test()
        {
            var contractAddress = "0xaadd4e08c64d9f299945f5649c1848f35f3f68bc";
            var senderAddress = "0x26f250197A8356bDEFC923838d9c5F17476ce55E";
            var receiverAddress = "0xaadd4e08c64d9f299945f5649c1848f35f3f68bc";
            var web3 = new Web3("https://goerli.infura.io/v3/ec686ca032bc4f0da49034e99646a762");


            //current block number
            var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var code = await web3.Eth.GetCode.SendRequestAsync(contractAddress); // runtime code;


            //Creating a balance function to simulate in the evm using the state of mainnet
            var balanceOfFunction = new BalanceOfFunction();
            balanceOfFunction.Owner = senderAddress;
            var callInput = balanceOfFunction.CreateCallInput(contractAddress);
            callInput.From = senderAddress;


            //Now we are going to make a transfer to the receiver
            var transferFunction = new Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferFunction();
            transferFunction.FromAddress = senderAddress; ;
            transferFunction.To = receiverAddress;
            transferFunction.Value = Web3.Convert.ToWei(9000);

            callInput = transferFunction.CreateCallInput(contractAddress);
        }
    }
}