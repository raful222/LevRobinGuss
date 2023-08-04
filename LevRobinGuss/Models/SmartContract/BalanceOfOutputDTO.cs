using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System.Numerics;


namespace LevRobinGuss.Models.SmartContract
{
	[FunctionOutput]
	public class BalanceOfOutputDTO : IFunctionOutputDTO
	{
		[Parameter("uint256", "balance", 1)]
		public BigInteger Balance { get; set; }
	}

}
