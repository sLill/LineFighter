using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerProfile : MonoBehaviour, IPlayerProfile
{
    #region Properties..
    public bool IsLocalProfile { get; set; }

    public NetworkConnection NetworkConnection { get; set; }

    public List<short> PlayerControllerIds { get; set; }
    #endregion Properties..

    #region Events..
    void Start()
    {
        PlayerControllerIds = new List<short>();
    }

    void Update()
    {

    } 
    #endregion
}
