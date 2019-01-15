using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[AddComponentMenu("Network/MainMenuNetworkManagerHUD")]
[RequireComponent(typeof(NetworkManager))]
[EditorBrowsable(EditorBrowsableState.Never)]
public class MainMenuNetworkManagerHud : MonoBehaviour
{
    #region Member Variables..

    #endregion Member Variables..

    public NetworkManager _manager;
    [SerializeField] public bool showGUI = false;
    [SerializeField] public int offsetX;
    [SerializeField] public int offsetY;

    // Runtime variable
    bool m_ShowServer;

    void Awake()
    {
        _manager = GetComponent<NetworkManager>();
    }

    void Update()
    {
        if (!showGUI)
            return;

        if (!_manager.IsClientConnected() && !NetworkServer.active && _manager.matchMaker == null)
        {
            if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    _manager.StartServer();
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    _manager.StartHost();
                }
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                _manager.StartClient();
            }
        }
        if (NetworkServer.active && _manager.IsClientConnected())
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                _manager.StopHost();
            }
        }
    }
    

     void OnGUI()
    {
        if (!showGUI)
            return;

        int xpos = 10 + offsetX;
        int ypos = 40 + offsetY;
        const int spacing = 24;

        bool noConnection = (_manager.client == null || _manager.client.connection == null ||
                             _manager.client.connection.connectionId == -1);

        if (!_manager.IsClientConnected() && !NetworkServer.active && _manager.matchMaker == null)
        {
            if (noConnection)
            {
                if (UnityEngine.Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUI.Button(new Rect(xpos, ypos, 200, 20), "LAN Host(H)"))
                    {
                        _manager.StartHost();
                    }
                    ypos += spacing;
                }

                if (GUI.Button(new Rect(xpos, ypos, 105, 20), "LAN Client(C)"))
                {
                    _manager.StartClient();
                }

                _manager.networkAddress = GUI.TextField(new Rect(xpos + 100, ypos, 95, 20), _manager.networkAddress);
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
                        _manager.StartServer();
                    }
                    ypos += spacing;
                }
            }
            else
            {
                GUI.Label(new Rect(xpos, ypos, 200, 20), "Connecting to " + _manager.networkAddress + ":" + _manager.networkPort + "..");
                ypos += spacing;


                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Cancel Connection Attempt"))
                {
                    _manager.StopClient();
                }
            }
        }
        else
        {
            if (NetworkServer.active)
            {
                string serverMsg = "Server: port=" + _manager.networkPort;
                if (_manager.useWebSockets)
                {
                    serverMsg += " (Using WebSockets)";
                }
                GUI.Label(new Rect(xpos, ypos, 300, 20), serverMsg);
                ypos += spacing;
            }
            if (_manager.IsClientConnected())
            {
                GUI.Label(new Rect(xpos, ypos, 300, 20), "Client: address=" + _manager.networkAddress + " port=" + _manager.networkPort);
                ypos += spacing;
            }
        }

        if (_manager.IsClientConnected() && !ClientScene.ready)
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Client Ready"))
            {
                ClientScene.Ready(_manager.client.connection);

                if (ClientScene.localPlayers.Count == 0)
                {
                    ClientScene.AddPlayer(0);
                }
            }
            ypos += spacing;
        }

        if (NetworkServer.active || _manager.IsClientConnected())
        {
            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop (X)"))
            {
                _manager.StopHost();
            }
            ypos += spacing;
        }

        if (!NetworkServer.active && !_manager.IsClientConnected() && noConnection)
        {
            ypos += 10;

            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                GUI.Box(new Rect(xpos - 5, ypos, 220, 25), "(WebGL cannot use Match Maker)");
                return;
            }

            if (_manager.matchMaker == null)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Enable Match Maker (M)"))
                {
                    _manager.StartMatchMaker();
                }
                ypos += spacing;
            }
            else
            {
                if (_manager.matchInfo == null)
                {
                    if (_manager.matches == null)
                    {
                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Create Internet Match"))
                        {
                            _manager.matchMaker.CreateMatch(_manager.matchName, _manager.matchSize, true, "", "", "", 0, 0, _manager.OnMatchCreate);
                        }
                        ypos += spacing;

                        GUI.Label(new Rect(xpos, ypos, 100, 20), "Room Name:");
                        _manager.matchName = GUI.TextField(new Rect(xpos + 100, ypos, 100, 20), _manager.matchName);
                        ypos += spacing;

                        ypos += 10;

                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Find Internet Match"))
                        {
                            _manager.matchMaker.ListMatches(0, 20, "", false, 0, 0, _manager.OnMatchList);
                        }
                        ypos += spacing;
                    }
                    else
                    {
                        for (int i = 0; i < _manager.matches.Count; i++)
                        {
                            var match = _manager.matches[i];
                            if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Join Match:" + match.name))
                            {
                                _manager.matchName = match.name;
                                _manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, _manager.OnMatchJoined);
                            }
                            ypos += spacing;
                        }

                        if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Back to Match Menu"))
                        {
                            _manager.matches = null;
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
                        _manager.SetMatchHost("localhost", 1337, false);
                        m_ShowServer = false;
                    }
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Internet"))
                    {
                        _manager.SetMatchHost("mm.unet.unity3d.com", 443, true);
                        m_ShowServer = false;
                    }
                    ypos += spacing;
                    if (GUI.Button(new Rect(xpos, ypos, 100, 20), "Staging"))
                    {
                        _manager.SetMatchHost("staging-mm.unet.unity3d.com", 443, true);
                        m_ShowServer = false;
                    }
                }

                ypos += spacing;

                GUI.Label(new Rect(xpos, ypos, 300, 20), "MM Uri: " + _manager.matchMaker.baseUri);
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Disable Match Maker"))
                {
                    _manager.StopMatchMaker();
                }
                ypos += spacing;
            }
        }
    }
}