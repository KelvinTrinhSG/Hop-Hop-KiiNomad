using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MintNFTERC721 : MonoBehaviour
{
    private ThirdwebSDK sdk;
    public Button mintNFTERC721Btn;
    public Text mintNFTERC721BtnTxt;
    public Button goBtn;

    public Button onChainBtn;
    public Text onChainBtnText;
    public Text scoreTxt;
    public Button replayBtn;

    private string nftContractAddress = "0x05DE6aF60b49bFd552aa7f2fE3011E63f38a9f2a";
    private string tokenContractAddress = "0x53e713bb63Eb2fb84812e3041ba85aB0AC1A01B4";

    void Start()
    {
        sdk = ThirdwebManager.Instance.SDK;
        mintNFTERC721Btn.gameObject.SetActive(false);
        goBtn.gameObject.SetActive(false);
    }

    public void OnConnected() {
        GetNFTBalance();
        GetTokenBalance();
    }

    public async void GetNFTBalance() {
        string address = await sdk.Wallet.GetAddress();
        Contract contract = sdk.GetContract(nftContractAddress);
        List<NFT> nftList = await contract.ERC721.GetOwned(address);
        if (nftList.Count > 0)
        {
            goBtn.gameObject.SetActive(true);
            mintNFTERC721Btn.gameObject.SetActive(false);
        }
        else {
            goBtn.gameObject.SetActive(false);
            mintNFTERC721Btn.gameObject.SetActive(true);
        }
    }

    public async void ClaimNFT() {
        mintNFTERC721Btn.interactable = false;
        mintNFTERC721BtnTxt.text = "Claiming!";
        Contract contract = sdk.GetContract(nftContractAddress);
        var data = await contract.ERC721.Claim(1);
        GetNFTBalance();
    }

    public static int ConvertStringToRoundedInt(string numberStr)
    {
        // Convert the string to a double
        double number = double.Parse(numberStr);

        // Round the number
        double roundedNumber = Math.Round(number);

        // Convert to int and return
        return (int)roundedNumber;
    }

    public async void GetTokenBalance()
    {
        sdk = ThirdwebManager.Instance.SDK;
        string address = await sdk.Wallet.GetAddress();
        Contract contract = sdk.GetContract(tokenContractAddress);
        var data = await contract.ERC20.BalanceOf(address);
        scoreTxt.text = data.displayValue;
    }

    public async void ClaimERC20Token()
    {
        onChainBtn.interactable = false;
        onChainBtnText.text = "Uploading!";

        sdk = ThirdwebManager.Instance.SDK;
        string address = await sdk.Wallet.GetAddress();
        Contract contract = sdk.GetContract(tokenContractAddress);
        var tokenBlance = await contract.ERC20.BalanceOf(address);
        int ownedERC20 = ConvertStringToRoundedInt(tokenBlance.displayValue);
        int currentScore = GameManager.Ins.Score;
        if (currentScore > ownedERC20) {
            int tokenToBeAdded = currentScore - ownedERC20;
            await contract.ERC20.Claim(tokenToBeAdded.ToString());
            onChainBtnText.text = "Replay";
            GetTokenBalance();
        }
        onChainBtn.interactable = true;
        onChainBtnText.text = "On Chain";
        onChainBtn.gameObject.SetActive(false);
        replayBtn.gameObject.SetActive(true);
    }

}