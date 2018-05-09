using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalReached : MonoBehaviour {

    public Agent_SimpleMaze agent;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.CompareTag("goal"))
        {
            agent.ReachedTheGoal();
        }
    }
}
