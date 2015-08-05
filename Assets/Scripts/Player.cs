using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player {

  public string id; // Name of player
  public NetworkConnection connection; // Connection id from the POV of the server
    
  public Player (string name, NetworkConnection connection) {
    this.id = name;
    this.connection = connection;
  }
  
  public override bool Equals (object o) {
    if (!(o is Player)) return false;
    else {
      if ((o as Player).connection.connectionId == this.connection.connectionId) return true;
      return false;
    }
  }
  
  public override int GetHashCode ()
  {
    return connection.connectionId;
  }

}
