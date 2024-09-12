using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverDialog : Dialog
{
    public Text totalScoreTxt;
    public Text bestScoreTxt;

    public GameObject gameoverDialog;
    public GameObject replayBtn;
    public GameObject onChainBtn;

    public GameObject mainMenu;
    public GameObject gameplay;

    public override void Show(bool isShow)
    {
        base.Show(isShow);
        if (totalScoreTxt && GameManager.Ins)
            totalScoreTxt.text = GameManager.Ins.Score.ToString();         
    }

    public void Replay()
    {
        mainMenu.SetActive(true);
        gameplay.SetActive(false);

        gameoverDialog.SetActive(false);
        onChainBtn.SetActive(true);
        replayBtn.SetActive(false);
        //SceneManager.sceneLoaded += OnSceneLoadedEvent;
        SceneController.Ins.LoadCurrentScene();
    }

    private void OnSceneLoadedEvent(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.Ins)
            GameManager.Ins.PlayGame();
        SceneManager.sceneLoaded -= OnSceneLoadedEvent;
    }
}
