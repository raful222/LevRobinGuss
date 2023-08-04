async function connectWallet() {
    accounts = await window.ethereum.request({ method: "eth_requestAccounts" }).catch((err) => {
        console.log(err.code)
    })

    console.log(accounts);
}




////// Connect to MetaMask

////const loadContractABI = async () => {
////    const response = await fetch('ContractABI.json');
////    const contractABI = await response.json();
////    return contractABI;
////};
////// index.js

////// Load Contract ABI from ContractABI.json
////const loadContractABI = async () => {
////    const response = await fetch('ContractABI.json');
////    const contractABI = await response.json();
////    return contractABI;
////};

////// Connect to MetaMask and Transfer Funds
////document.addEventListener('DOMContentLoaded', async () => {
////    const connectButton = document.getElementById('connectButton');
////    const transferButton = document.getElementById('transferButton');

////    connectButton.addEventListener('click', async () => {
////        if (typeof window.ethereum !== 'undefined') {
////            try {
////                // Connect to MetaMask
////                await window.ethereum.enable();
////                connectButton.disabled = true;
////                transferButton.disabled = false;
////                console.log('Connected to MetaMask');
////            } catch (error) {
////                console.error('Error connecting to MetaMask:', error);
////            }
////        } else {
////            console.error('MetaMask not found');
////        }
////    });

////    transferButton.addEventListener('click', async () => {
////        const web3 = new Web3('https://mainnet.infura.io/v3/YOUR_INFURA_PROJECT_ID'); // Replace with your Infura project ID
////        const accounts = await web3.eth.getAccounts();
////        const userAccount = accounts[0]; // Assuming the first account is used

////        const contractAddress = '0x123456789ABCDEF'; // Replace with your smart contract address
////        const contractABI = await loadContractABI();
////        const contract = new web3.eth.Contract(contractABI, contractAddress);

////        const recipientAddress = '0x987654321CBADEF'; // Replace with the recipient's address
////        const amount = web3.utils.toWei('1', 'ether'); // Amount in wei (1 ETH in this example)

////        try {
////            // Transfer funds code...
////        } catch (error) {
////            // Error handling code...
////        }
////    });
////});
