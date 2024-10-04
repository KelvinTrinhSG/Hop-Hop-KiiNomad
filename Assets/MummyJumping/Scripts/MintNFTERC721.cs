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
    public Button goBtn;

    public Button onChainBtn;
    public Text onChainBtnText;
    public Text scoreTxt;
    public Button replayBtn;

    private string tokenContractAddress = "0x7B46fE9C380228d707fdEf59366dC5006656B251";

    void Start()
    {
        sdk = ThirdwebManager.Instance.SDK;
        goBtn.gameObject.SetActive(false);
    }

    public void OnConnected() {
        goBtn.gameObject.SetActive(true);
        GetTokenBalance();
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
        try
        {
            var tokenBlance = await contract.ERC20.BalanceOf(address);
            int ownedERC20 = ConvertStringToRoundedInt(tokenBlance.displayValue);
            int currentScore = GameManager.Ins.Score;
            if (currentScore > ownedERC20)
            {
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
        catch (Exception ex)
        {
            Debug.LogError($"Error occurred: {ex.Message}");
            // Xử lý lỗi, ví dụ hiển thị thông báo cho người dùng hoặc reset lại giao diện
            onChainBtn.interactable = true;
            onChainBtnText.text = "On Chain";
        }
       
    }

}