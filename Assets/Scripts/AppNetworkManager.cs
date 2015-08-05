using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Threading;

public class AppNetworkManager : NetworkManager {

  public int DISCOVERY_PORT = 7777;
  public int SERVER_PORT = 7778;

  bool findingServer = true;
  bool serverFound = false;
  
  int serverConnection;
  public bool server = false;
  
  string serverAddress;
  int serverPort;
  
  NetworkDiscovery discovery;
  ApplicationManager application;

  void Start () {
    application = gameObject.GetComponent<ApplicationManager>();
    discovery = gameObject.GetComponent<NetworkDiscovery>();
    startDiscovery();
  }

  void Update () {
    if (!findingServer) {
      if (!serverFound) {
        startServer();
      }
    }
  }
  
  void startDiscovery () {
    Debug.Log("Starting discovery");
    discovery.Initialize();
    discovery.StartAsClient();
    System.Timers.Timer timer = new System.Timers.Timer(5000);
    timer.Elapsed += (sender, e) => {
      if (findingServer != false) { 
        findingServer = false;
        Debug.Log("Server not found.");
      }
    };
    timer.Enabled = true;
  }
  
  public void OnReceivedBroadcast (string fromAddress, string data) {
    if (!serverFound) {
      Debug.Log("Server Found");
      findingServer = false;
      serverFound = true;
      discovery.StopBroadcast();
      GameObject.Find("StatusText").GetComponent<UnityEngine.UI.Text>().text = "Server Found!";
      networkAddress = fromAddress;
      networkPort = System.Int32.Parse(data);
      Debug.Log("Starting Client");
      StartClient();
    }
  }
  
  // SERVER METHODS
  
  void startServer () {
    discovery.StopBroadcast();
    Debug.Log("Starting own server");
    networkPort = SERVER_PORT;
    server = true;
    serverFound = true;
    StartHost();
  }
  
  public override void OnStartServer () {
    base.OnStartServer();
    discovery.Initialize();
    discovery.StartAsServer();
    Debug.Log("Hosting server");
    GameObject.Find("StatusText").GetComponent<UnityEngine.UI.Text>().text = "Hosting Server";
  }
  
  public override void OnServerReady (NetworkConnection conn)
  {
    base.OnServerReady(conn);
    Debug.Log("New player has connected and is ready.");
    Player player = new Player("Player " + conn.connectionId, conn);
    application.UpdatePlayer(player, "connect");
  }
  
  // CLIENT METHODS
  
  public override void OnClientConnect (NetworkConnection conn) {
    base.OnClientConnect(conn);
    Debug.Log("Connected to Server");
  }

}