using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalletConnect : MonoBehaviour
{
    public GameObject GoBtn;
    public GameObject MintNFTERC721Btn;

    // Start is called before the first frame update
    void Start()
    {
        GoBtn.SetActive(false);
        MintNFTERC721Btn.SetActive(false);
    }
}
