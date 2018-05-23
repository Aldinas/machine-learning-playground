﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalReached : MonoBehaviour {

    public Agent_SimpleMaze agent;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            Debug.Log("Goal!");
            agent.ReachedTheGoal();
        }
    }
}
