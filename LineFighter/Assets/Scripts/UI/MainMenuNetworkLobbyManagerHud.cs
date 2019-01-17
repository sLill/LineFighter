using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[AddComponentMenu("Network/MainMenuNetworkLobbyManagerHUD")]
[RequireComponent(typeof(NetworkLobbyManager))]
[EditorBrowsable(EditorBrowsableState.Never)]
public class MainMenuNetworkLobbyManagerHud : MonoBehaviour
{
    #region Member Variables..
    // Runtime variable
    bool m_ShowServer;
    #endregion Member Variables..

    #region Properties..
    public NetworkLobbyManager Manager;
    [SerializeField] public bool ShowGUI = false;
    [SerializeField] public int OffsetX;
    [SerializeField] public int FffsetY; 
    #endregion

    #region Events..
    void Awake()
    {
        Manager = GetComponent<NetworkLobbyManager>();
    }

    void Update()
    {
        if (!ShowGUI)
            return;

        if (!Manager.IsClientConnected() && !NetworkServer.active && Manager.matchMaker == null)
        {
            if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    Manager.StartServer();
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    Manager.StartHost();
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                Manager.StartClient();
            }
        }
        if (NetworkServer.active && Manager.IsClientConnected())
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Manager.StopHost();
            }
        }
    }

    void OnGUI()
    {
        if (!ShowGUI)
            return;

        int xpos = 10 + OffsetX;
        int ypos = 40 + FffsetY;
        const int spacing = 24;

        bool noConnection = (Manager.client == null || Manager.client.connection == null ||
                             Manager.client.connection.connectionId == -1);

        if (!Manager.IsClientConnected() && !NetworkServer.active && Manager.matchMaker == null)
        {
            if (noConnection)
            {
                if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
                    {
                        Manager.StartHost();
                    }
                    ypos += spacing;
                }

                if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
                {
                    Manager.StartClient();
                }

                Manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), Manager.networkAddress);
                ypos += spacing;

                if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUI.Box(new Rect(xpos, ypos, 200, 25), "(  WebGL cannot be server  )");
                    ypos += spacing;
                }
                else
                {
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Server Only(S)"))
                    {
                        Manager.StartServer();
                    }
                    ypos += spacing;
                }
            }
            else
            {
                GUI.Label(new Rect(xpos, ypos, 200, 20), "Connecting to " + Manager.networkAddress + ":" + Manager.networkPort + "..");
                ypos += spacing;


                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Cancel Connection Attempt"))
                {
                    Manager.StopClient();
                }
            }
        }
        else
        {
            if (NetworkServer.active)
            {
                string serverMsg = "Server: port=" + Manager.networkPort;
                if (Manager.useWebSockets)
                {
                    serverMsg += " (Using WebSockets)";
                }
                GUI.Label(new Rect(xpos, ypos, 300, 20), serverMsg);
                ypos += spacing;
            }
            if (Manager.IsClientConnected())
            {
                GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + Manager.networkAddress + " port=" + Manager.networkPort);
                ypos += spacing;
            }
        }

        if (Manager.IsClientConnected() && !ClientScene.ready)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
            {
                ClientScene.Ready(Manager.client.connection);

                if (ClientScene.localPlayers.Count == 0)
                {
                    ClientScene.AddPlayer(0);
                }
            }
            ypos += spacing;
        }

        if (NetworkServer.active || Manager.IsClientConnected())
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
            {
                Manager.StopHost();
            }
            ypos += spacing;
        }

        if (!NetworkServer.active && !Manager.IsClientConnected() && noConnection)
        {
            ypos += 10;

            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                GUI.Box(new Rect(xpos - 5, ypos, 220, 25), "(WebGL cannot use Match Maker)");
                return;
            }

            if (Manager.matchMaker == null)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)"))
                {
                    Manager.StartMatchMaker();
                }
                ypos += spacing;
            }
            else
            {
                if (Manager.matchInfo == null)
                {
                    if (Manager.matches == null)
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
                        {
                            Manager.matchMaker.CreateMatch(Manager.matchName, Manager.matchSize, true, "", "", "", 0, 0, Manager.OnMatchCreate);
                        }
                        ypos += spacing;

                        GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
                        Manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), Manager.matchName);
                        ypos += spacing;

                        ypos += 10;

                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
                        {
                            Manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, Manager.OnMatchList);
                        }
                        ypos += spacing;
                    }
                    else
                    {
                        for (int i = 0; i < Manager.matches.Count; i++)
                        {
                            var match = Manager.matches[i];
                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
                            {
                                Manager.matchName = match.name;
                                Manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, Manager.OnMatchJoined);
                            }
                            ypos += spacing;
                        }

                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Back to Match Menu"))
                        {
                            Manager.matches = null;
                        }
                        ypos += spacing;
                    }
                }

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Change MM server"))
                {
                    m_ShowServer = !m_ShowServer;
                }
                if (m_ShowServer)
                {
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Local"))
                    {
                        Manager.SetMatchHost("localhost", 1337, false);
                        m_ShowServer = false;
                    }
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
                    {
                        Manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                        m_ShowServer = false;
                    }
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
                    {
                        Manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                        m_ShowServer = false;
                    }
                }

                ypos += spacing;

                GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + Manager.matchMaker.baseUri);
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
                {
                    Manager.StopMatchMaker();
                }
                ypos += spacing;
            }
        }
    }
    #endregion Events..
}