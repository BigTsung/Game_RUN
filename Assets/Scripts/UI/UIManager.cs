using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager> {

    [Header("UI")]
    public UserInfoUIManager userInfoUIManager;
    public GameObject GameStatusUI;

    void Start ()
    {
        SetActiveGameStausUI(false);
    }

    // ===========================
    // Private Function
    // ===========================

    private void SetActiveGameStausUI(bool status)
    {
        if (GameStatusUI != null)
        {
            GameStatusUI.SetActive(status);
        }
    }

    // ===========================
    // Public Function
    // ===========================

    public void SetGameStatus(GameStatusUIManager.STATUS gameStatus)
    {
        SetActiveGameStausUI(true);

        if (GameStatusUI != null)
        {
            GameStatusUIManager gameStatusUIManager = GameStatusUI.GetComponent<GameStatusUIManager>();
            if (gameStatusUIManager != null)
            {
                gameStatusUIManager.SetGameStatus(gameStatus);
            }
        }
    }

    public void InitHealthBar(int val)
    {
        if (userInfoUIManager != null)
        {
            userInfoUIManager.InitHealthBar(val);
        }
    }

    public void RefreshHealthBar(int val)
    {
        if (userInfoUIManager != null)
        {
            userInfoUIManager.RefreshHealthBar(val);
        }
    }

    public void RefreshOverheadBar(float val)
    {
        if (userInfoUIManager != null)
        {
            userInfoUIManager.RefreshOverheadBar(val);
        }
    }
}
