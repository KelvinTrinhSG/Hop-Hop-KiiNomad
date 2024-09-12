using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag(GameTag.Player.ToString()))
        {
            Destroy(col.gameObject);
            if (GameManager.Ins)
            {
                GameManager.Ins.state = GameState.Gameover;
            }
            if (GUIManager.Ins && GUIManager.Ins.gameOverDialog)
            {
                GUIManager.Ins.gameOverDialog.Show(true);
            }

            if (AudioController.Ins)
            {
                AudioController.Ins.PlaySound(AudioController.Ins.gameover);
            }

            Debug.Log("Gameover!!!");
        }

        if (col.CompareTag(GameTag.Platform.ToString()))
        {
            Destroy(col.gameObject);
        }

    }
}
