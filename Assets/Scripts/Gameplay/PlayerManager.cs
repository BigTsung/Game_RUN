using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager> {

    private List<Transform> playerList = new List<Transform>();
    
    public List<Transform> PlayerList
    {
        get { return playerList; }
    }

    public bool ExistPlayer
    {
        get { return playerList.Count > 0 ? true:false; }
    }

    public void AddToPlayerList(Transform player)
    {
        if (!playerList.Contains(player))
            playerList.Add(player);
    }

    public void RemoveFromPlayerList(Transform player)
    {
        if (playerList.Contains(player))
            playerList.Remove(player);
    }

    public Transform GetClosedPlayer(Transform other)
    {
        Transform targetTrans = null;
        float minDis = float.MaxValue;
        for(int i = 0; i < playerList.Count; i++)
        {
            float dis = Vector3.Distance(other.position, playerList[i].transform.position);
            if(dis < minDis)
            {
                minDis = dis;
                targetTrans = playerList[i];
            }
        }
        return targetTrans;
    }
}
