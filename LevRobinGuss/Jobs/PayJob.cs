
using LevRobinGuss.Core.Entities;
using LevRobinGuss.Service;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Quartz;
using System.Numerics;
using System.Text;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.RPC.TransactionTypes;
using Nethereum.Util;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;

namespace LevRobinGuss.Jobs
{
    [DisallowConcurrentExecution]
    public class PayJob : IJob
    {
        private readonly ILogger<PayJob> _logger;
        private readonly FootballDataAPi _footballDataApi;

        public PayJob(
          ILogger<PayJob> logger, FootballDataAPi footballDataApi)
        {
            _logger = logger;
            _footballDataApi = footballDataApi;
        }
        [Event("BetInfo")]
        public class BetInfoEventDTO : IEventDTO
        {
            [Parameter("address", "sender", 1, true)]
            public string Sender { get; set; }

            [Parameter("uint256", "value", 2, false)]
            public BigInteger Value { get; set; }

            [Parameter("uint256", "bettingPercentage", 3, false)]
            public BigInteger BettingPercentage { get; set; }

            [Parameter("bytes32", "betOnHouse", 4, false)]
            public byte[] BetOnHouse { get; set; }

            [Parameter("int256", "gameId", 5, false)]
            public BigInteger GameId { get; set; }

        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("PayJob started");

            try
            {

                var privateKey = "53f1716172320f33a9254da404fa8bbe2f233e9af1acfa41c1734b4bb47ceda5";
                var account = new Account(privateKey); // or load it from your keystore file as you are doing.

                var web3 = new Web3(account, "https://goerli.infura.io/v3/ec686ca032bc4f0da49034e99646a762");

                var contractAddress = "0xd6636468f2a74f44159f0f02eebacf6edfcd40f2";
                string abi = @"[
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""spender"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""value"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""approve"",
    ""outputs"": [
      {
        ""internalType"": ""bool"",
        ""name"": """",
        ""type"": ""bool""
      }
    ],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": ""amount"",
        ""type"": ""uint256""
      },
      {
        ""internalType"": ""address"",
        ""name"": ""tokenContractAddress"",
        ""type"": ""address""
      }
    ],
    ""name"": ""approveTokenTransfer"",
    ""outputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": ""value"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""burn"",
    ""outputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""account"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""value"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""burnFrom"",
    ""outputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": ""amount"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""buy"",
    ""outputs"": [],
    ""stateMutability"": ""payable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""spender"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""requestedDecrease"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""decreaseAllowance"",
    ""outputs"": [
      {
        ""internalType"": ""bool"",
        ""name"": """",
        ""type"": ""bool""
      }
    ],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""constructor""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""spender"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""currentAllowance"",
        ""type"": ""uint256""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""requestedDecrease"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""ERC20FailedDecreaseAllowance"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""spender"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""allowance"",
        ""type"": ""uint256""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""needed"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""ERC20InsufficientAllowance"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""sender"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""balance"",
        ""type"": ""uint256""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""needed"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""ERC20InsufficientBalance"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""approver"",
        ""type"": ""address""
      }
    ],
    ""name"": ""ERC20InvalidApprover"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""receiver"",
        ""type"": ""address""
      }
    ],
    ""name"": ""ERC20InvalidReceiver"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""sender"",
        ""type"": ""address""
      }
    ],
    ""name"": ""ERC20InvalidSender"",
    ""type"": ""error""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""spender"",
        ""type"": ""address""
      }
    ],
    ""name"": ""ERC20InvalidSpender"",
    ""type"": ""error""
  },
  {
    ""anonymous"": false,
    ""inputs"": [
      {
        ""indexed"": true,
        ""internalType"": ""address"",
        ""name"": ""owner"",
        ""type"": ""address""
      },
      {
        ""indexed"": true,
        ""internalType"": ""address"",
        ""name"": ""spender"",
        ""type"": ""address""
      },
      {
        ""indexed"": false,
        ""internalType"": ""uint256"",
        ""name"": ""value"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""Approval"",
    ""type"": ""event""
  },
  {
    ""anonymous"": false,
    ""inputs"": [
      {
        ""indexed"": false,
        ""internalType"": ""address"",
        ""name"": ""sender"",
        ""type"": ""address""
      },
      {
        ""indexed"": false,
        ""internalType"": ""uint256"",
        ""name"": ""value"",
        ""type"": ""uint256""
      },
      {
        ""indexed"": false,
        ""internalType"": ""uint256"",
        ""name"": ""bettingPercentage"",
        ""type"": ""uint256""
      },
      {
        ""indexed"": false,
        ""internalType"": ""bytes32"",
        ""name"": ""betOnHouse"",
        ""type"": ""bytes32""
      },
      {
        ""indexed"": false,
        ""internalType"": ""int256"",
        ""name"": ""gameId"",
        ""type"": ""int256""
      }
    ],
    ""name"": ""BetInfo"",
    ""type"": ""event""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""spender"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""addedValue"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""increaseAllowance"",
    ""outputs"": [
      {
        ""internalType"": ""bool"",
        ""name"": """",
        ""type"": ""bool""
      }
    ],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": ""value"",
        ""type"": ""uint256""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""bettingPercentage"",
        ""type"": ""uint256""
      },
      {
        ""internalType"": ""bytes32"",
        ""name"": ""betOnHouse"",
        ""type"": ""bytes32""
      },
      {
        ""internalType"": ""int256"",
        ""name"": ""gameId"",
        ""type"": ""int256""
      }
    ],
    ""name"": ""SendBit"",
    ""outputs"": [],
    ""stateMutability"": ""payable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""to"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""value"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""transfer"",
    ""outputs"": [
      {
        ""internalType"": ""bool"",
        ""name"": """",
        ""type"": ""bool""
      }
    ],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""anonymous"": false,
    ""inputs"": [
      {
        ""indexed"": true,
        ""internalType"": ""address"",
        ""name"": ""from"",
        ""type"": ""address""
      },
      {
        ""indexed"": true,
        ""internalType"": ""address"",
        ""name"": ""to"",
        ""type"": ""address""
      },
      {
        ""indexed"": false,
        ""internalType"": ""uint256"",
        ""name"": ""value"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""Transfer"",
    ""type"": ""event""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""from"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""address"",
        ""name"": ""to"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""uint256"",
        ""name"": ""value"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""transferFrom"",
    ""outputs"": [
      {
        ""internalType"": ""bool"",
        ""name"": """",
        ""type"": ""bool""
      }
    ],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": ""amount"",
        ""type"": ""uint256""
      }
    ],
    ""name"": ""withdrawEther"",
    ""outputs"": [],
    ""stateMutability"": ""nonpayable"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""name"": ""adminAddress"",
    ""outputs"": [
      {
        ""internalType"": ""address payable"",
        ""name"": """",
        ""type"": ""address""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""owner"",
        ""type"": ""address""
      },
      {
        ""internalType"": ""address"",
        ""name"": ""spender"",
        ""type"": ""address""
      }
    ],
    ""name"": ""allowance"",
    ""outputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": """",
        ""type"": ""uint256""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""account"",
        ""type"": ""address""
      }
    ],
    ""name"": ""balanceOf"",
    ""outputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": """",
        ""type"": ""uint256""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": """",
        ""type"": ""address""
      }
    ],
    ""name"": ""balances"",
    ""outputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": """",
        ""type"": ""uint256""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""name"": ""decimals"",
    ""outputs"": [
      {
        ""internalType"": ""uint8"",
        ""name"": """",
        ""type"": ""uint8""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""name"": ""getAdminBalance"",
    ""outputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": """",
        ""type"": ""uint256""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [
      {
        ""internalType"": ""address"",
        ""name"": ""account"",
        ""type"": ""address""
      }
    ],
    ""name"": ""getBalance"",
    ""outputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": """",
        ""type"": ""uint256""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""name"": ""getSenderBalance"",
    ""outputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": """",
        ""type"": ""uint256""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""name"": ""name"",
    ""outputs"": [
      {
        ""internalType"": ""string"",
        ""name"": """",
        ""type"": ""string""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""name"": ""symbol"",
    ""outputs"": [
      {
        ""internalType"": ""string"",
        ""name"": """",
        ""type"": ""string""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  },
  {
    ""inputs"": [],
    ""name"": ""totalSupply"",
    ""outputs"": [
      {
        ""internalType"": ""uint256"",
        ""name"": """",
        ""type"": ""uint256""
      }
    ],
    ""stateMutability"": ""view"",
    ""type"": ""function""
  }
]";

                var contractABI = System.IO.File.ReadAllText("wwwroot/ContractABI/ContractABI.json");
                var contract = web3.Eth.GetContract(abi, contractAddress);
                var betInfoEvent = contract.GetEvent<BetInfoEventDTO>();

                // Create a filter to get all BetInfo events
                var filterInput = betInfoEvent.CreateFilterInput();

                // Get the filter changes to obtain all matching events
                var filterLogs = await web3.Eth.Filters.GetLogs.SendRequestAsync(filterInput);


                var matchingTransactions = new List<Transaction>();
                var function = contract.GetFunction("SendBit");

                var datatimenow = DateTime.Now.AddDays(-1);
                var from = datatimenow.ToString("yyyy-MM-dd");
                var to = datatimenow.AddDays(10).ToString("yyyy-MM-dd");
                var footballData = await _footballDataApi.MatchesList(to, from);

                foreach (var football in footballData.Matches)
                {
                    if (football.Status == "FINISHED")
                    {
                        foreach (var log in filterLogs)
                        {
                            int senderLength = 64;
                            int valueLength = 64;
                            int bettingPercentageLength = 64;
                            int betOnHouseLength = 64;
                            int gameIdLength = 64;

                            // Extract substrings for each parameter
                            int currentIndex = 2; // Start after the leading "0x"
                            var sender = "0x" + log.Data.Substring(currentIndex, senderLength);
                            var senderWithoutLeadingZeros = RemoveLeadingZerosFromAddress(sender);

                            currentIndex += senderLength;
                            var valueHex = log.Data.Substring(currentIndex, valueLength);
                            currentIndex += valueLength;
                            var bettingPercentageHex = log.Data.Substring(currentIndex, bettingPercentageLength);
                            currentIndex += bettingPercentageLength;
                            var betOnHouseHex = log.Data.Substring(currentIndex, betOnHouseLength);
                            currentIndex += betOnHouseLength;
                            var gameIdHex = log.Data.Substring(currentIndex, gameIdLength);

                            // Convert hex strings to actual values
                            var value = BigInteger.Parse(valueHex, System.Globalization.NumberStyles.HexNumber);
                            var bettingPercentage = BigInteger.Parse(bettingPercentageHex, System.Globalization.NumberStyles.HexNumber);
                            var betOnHouse = Encoding.UTF8.GetString(HexToByteArray(betOnHouseHex));
                            var gameId = BigInteger.Parse(gameIdHex, System.Globalization.NumberStyles.HexNumber);
                            decimal newValue = 1;
                            if (gameId == football.Id)
                            {
                                decimal etherValuebettingPercentage = (decimal)bettingPercentage / (decimal)Math.Pow(10, 18);
                                decimal etherValue = (decimal)value / (decimal)Math.Pow(10, 18);

                                string homeScoreString = football.Score.FullTime.Home?.ToString();
                                string awayScoreString = football.Score.FullTime.Away?.ToString();
                                int homeScore;
                                int awayScore;
                                string winornot = "";
                                string whatHapped = "";
                                if (int.TryParse(homeScoreString, out homeScore) && int.TryParse(awayScoreString, out awayScore))
                                {
                                    if (homeScore > awayScore)
                                    {
                                        whatHapped = "home win ";
                                        if (betOnHouse.Contains(WinOrNot.Win.ToString()))
                                        {
                                            winornot = "win";
                                            newValue = etherValue * etherValuebettingPercentage;
                                        }
                                    }
                                    else if (homeScore < awayScore)
                                    {
                                        whatHapped = "home lost ";
                                        if (betOnHouse.Contains(WinOrNot.Lose.ToString()))
                                        {
                                            winornot = "Lost";
                                            newValue = etherValue * etherValuebettingPercentage;
                                        }
                                    }
                                    else
                                    {
                                        whatHapped = "game in draw ";
                                        if (betOnHouse.Contains(WinOrNot.Draw.ToString()))
                                        {
                                            winornot = "Draw";
                                            newValue = etherValue * etherValuebettingPercentage;

                                        }
                                    }
                                }
                                if (newValue != 1)
                                {
                                    // Convert to Wei using Nethereum's UnitConversion utility
                                    BigInteger amountInWei = UnitConversion.Convert.ToWei(newValue);
                                    var nonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync("0x26f250197A8356bDEFC923838d9c5F17476ce55E");
                                    //var transferFunction = contract.GetFunction("SendBit");
                                    byte[] bytes = Encoding.UTF8.GetBytes("user bet " + winornot +whatHapped + "user paid");
                                    var transferFunction = contract.GetFunction("transfer");

                                    var transferEncodedData = transferFunction.GetData(senderWithoutLeadingZeros, amountInWei);

                                    //// Estimate the gas needed for the transaction
                                    //var gas = await transferFunction.EstimateGasAsync("0x26f250197A8356bDEFC923838d9c5F17476ce55E", null, null, amountInWei);

                                    //var encodedDataString = transferFunction.GetData(amountInWei, bettingPercentage, bytes, gameId);


                                    
                                    var transactionManager = web3.Eth.TransactionManager;
                                    
                                    var txParams = new TransactionInput
                                    {
                                        To = contractAddress,
                                        From = "0x26f250197A8356bDEFC923838d9c5F17476ce55E",
                                        Gas = new HexBigInteger(200000), // You can set an appropriate gas value here
                                        GasPrice = new HexBigInteger(20000000000), // You can set an appropriate gas price here
                                        Nonce = nonce,
                                        Data = transferEncodedData // Specify the data of your transaction if applicable
                                    };
                                    var signedTx = await transactionManager.SignTransactionAsync(txParams);

                                    string transactionHash = await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(signedTx);

                                    // Wait for the transaction to be mined and retrieve the receipt
                                    TransactionReceipt receipt = null;
                                    while (receipt == null)
                                    {
                                        receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
                                        await Task.Delay(1000); // Wait for a few seconds before checking the receipt again
                                    }

                                    // Check the status in the receipt to see if the transaction was successful
                                    if (receipt.Status.Value == 1)
                                    {
                                        // Transaction was successful
                                        Console.WriteLine("Transaction successful. Transaction hash: " + transactionHash);
                                        // You can do additional actions here after the transaction is successful.
                                    }
                                    else
                                    {
                                        // Transaction failed
                                        Console.WriteLine("Transaction failed.");
                                        // You can handle the failure scenario here.
                                    }
                                    // Wait for the transaction to be mined and retrieve the receipt

                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            _logger.LogInformation("PayJob end");

        }
        public byte[] HexToByteArray(string hexString)
        {
            int NumberChars = hexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static string RemoveLeadingZerosFromAddress(string address)
        {
            if (address.StartsWith("0x"))
            {
                int startIndex = 2; // Skip the "0x" prefix
                while (startIndex < address.Length && address[startIndex] == '0')
                {
                    startIndex++; // Move past the leading zeros
                }
                return "0x" + address.Substring(startIndex); // Add the "0x" prefix back after removing zeros
            }
            return address; // Return as is if it doesn't start with "0x"
        }

    }
}
