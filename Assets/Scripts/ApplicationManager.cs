using UnityEngine;
using System.Collections.Generic;

public class ApplicationManager : MonoBehaviour {

  public AppNetworkManager networkManager;
  List<System.Action> todo = new List<System.Action>();
  
  public List<Player> connected = new List<Player>();
  
  GameObject playerList;
  GameObject playerListEntryPrefab;

  void Start () {
    playerList = GameObject.Find("PlayerList");
    playerListEntryPrefab = Resources.Load("PlayerUIEntry") as GameObject;
  }
  
  void Update () {
    int count = 0;
    foreach (var action in todo) {
      action.Invoke();
      count++;
    }
    todo.RemoveRange(0, count);
  }
  
  public void AddTodo (System.Action action) {
    todo.Add(action);
  }
  
  public void UpdatePlayers (List<Player> players) {
    connected = players;
    for (int i=0; i<playerList.transform.childCount; i++) {
      GameObject.Destroy(playerList.transform.GetChild(i));
    }
    foreach (Player player in players) {
      GameObject playerListEntry = GameObject.Instantiate(playerListEntryPrefab);
      playerListEntry.GetComponent<UnityEngine.UI.Text>().text = player.id + " (ConnID: " + player.connection.connectionId + ")";
      playerListEntry.transform.SetParent(playerList.transform, false);
    }
  }
  
  public void UpdatePlayer (Player player, string action) {
    switch (action) {
      case "connect":
        connected.Add(player);
        UpdatePlayers(connected);
        break;
      case "disconnect":
        connected.Remove(player);
        UpdatePlayers(connected);
        break;
      case "update":
        Player playerToUpdate = connected.Find((p) => p.connection == player.connection);
        if (playerToUpdate != null) connected.Remove(playerToUpdate);
        connected.Add(player);
        break;
      default:
        break;
    }
    
  }

}
