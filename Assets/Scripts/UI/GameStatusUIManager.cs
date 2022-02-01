using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatusUIManager : MonoBehaviour {

    public enum STATUS
    {
        COMPLETE,
        GAMEOVER
    }

    public GameObject imageComplete;
    public GameObject imageGameOver;

    // Use this for initialization
	void Start ()
    {
    }

    // ===========================
    // Private Function
    // ===========================

    // ===========================
    // Public Function
    // ===========================

    public void SetGameStatus(STATUS gamestatus)
    {
        if (imageComplete == null || imageGameOver == null)
        {
            Debug.LogWarning("Check the image of complete and gameover");
            return; 
        }

        if (gamestatus == STATUS.COMPLETE)
        {
            imageComplete.SetActive(true);
            imageGameOver.SetActive(false);
        }
        else if (gamestatus == STATUS.GAMEOVER)
        {
            imageGameOver.SetActive(true);
            imageComplete.SetActive(false);
        }


    }
}
