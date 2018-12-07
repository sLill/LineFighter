using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIController : MonoBehaviour
{
    private EnemyState _state;

    public enum EnemyState
    {
        Idle,
        Alert
    }

	// Use this for initialization
	void Start ()
    {
        _state = EnemyState.Idle;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
