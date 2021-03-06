﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent_SimpleMaze : Agent
{
    
    public GameObject goal;             // The goal we are trying to reach.
    public GameObject ground;           // The floor of the Maze.
    public List<GameObject> spawnPoints;    // The point to spawn our capsule at.

    Academy_SimpleMaze academy;         // The academy (we need this for some of the variables it holds)
    CharacterController agentCC;        // Will attempt to Use this in place of the rigidbody. 
    Material groundMaterial;            // The default material assigned to the ground.
    Renderer groundRenderer;            // The renderer of the ground so we can change its colour on success/fail.
    List<Camera> agentCameras;                 // The camera the agent will use for seeing things.
    GameObject MazeArea;                // The maze area that this agent belongs to. Useful for allowing multiple maze areas in the same scene.

    void Awake()
    {
        // Get the brain and academy items that we require for this agent. Theres only ever going to be one of these in
        // this environment so we can find it by type.
        brain = FindObjectOfType<Brain>();
        academy = FindObjectOfType<Academy_SimpleMaze>();
        MazeArea = transform.parent.gameObject;

        // Iterate through the children of this maze and capture the spawn points.
        foreach(Transform child in MazeArea.GetComponentsInChildren<Transform>(false))
        {
            if (child.tag == "SpawnPoint")
                spawnPoints.Add(child.gameObject);
        }
        
    }

    public override void InitializeAgent()
    {
        // We need to extend the initial InitializeAgent script to support our other variables. This acts sort of like how the Start()
        // function works in standard projects. 
        base.InitializeAgent();

        // Get the rigidbody, ground material and ground renderer and cache em.
        agentCC = GetComponent<CharacterController>();
        groundRenderer = ground.GetComponent<Renderer>();
        groundMaterial = groundRenderer.material;

        goal.GetComponent<GoalReached>().agent = this;

        // Clear any other cameras on the agent then add the ones present.
        // agentParameters.agentCameras.Clear();
        // foreach (Camera cam in this.GetComponents<Camera>())
        // {
            // Iterate through all cameras on the agent and send them to the brain.
        //    agentParameters.agentCameras.Add(cam);
        // }
    }

    public void ReachedTheGoal()
    { 
        // The agent reached the goal, give it a reward.
        AddReward(1f);

        // Set the Done state, this will trigger a reset.
        Done();

        // Swap the material on the ground to indicate a score.
        StartCoroutine(GoalReachedSwapGroundMaterial(academy.successMaterial, 0.25f));
    }

    IEnumerator GoalReachedSwapGroundMaterial(Material mat, float time)
    {
        groundRenderer.material = mat;
        yield return new WaitForSeconds(time); // Wait for 2 sec
        groundRenderer.material = groundMaterial;
    }

    public void MoveAgent(float[] act)
    {
        // The input from the brain that tells us which button its pressed. These are values 0 to 3 and correspond to
        // W (forward), S (back), A (rotate left), D (rotate right).

        // Zero off any current movement or rotation
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;

        // Round the float off to an integer. 
        int action = Mathf.FloorToInt(act[0]);

        switch(action)
        {
            case 0:
                // Forward
                dirToGo = transform.forward * 1f;
                break;
            case 1:
                // Backward
                dirToGo = transform.forward * -1f;
                break;
            case 2:
                // strafe right
                rotateDir = transform.up * -1f;
                break;
            case 3:
                // strafe left
                rotateDir = transform.up * 1f;
                break;
        }
        // Apply any rotation.
        transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
        agentCC.Move(dirToGo * academy.agentRunSpeed);
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        // The action the agent needs to take. This is called every step of the engine. 
        // Move the agent
        MoveAgent(vectorAction);

        // Apply a time penalty to encourage the agent to finish as quickly as possible.
        AddReward(-1f / agentParameters.maxStep);
    }

    public override void AgentReset()
    {
        // Get a random spawn point out of the array:
        int spawnPointIndex = Random.Range(0, spawnPoints.Count);
        // Zero out any existing movement and return the agent to a new point.
        transform.position = spawnPoints[spawnPointIndex].transform.position;
        transform.rotation = Quaternion.identity;

        // Move the goal as well, just make sure its not the same as the above spawn point position.
        int goalIndex = Random.Range(0, spawnPoints.Count);

        do
        {
            // Spawn point and goal are the same, re-roll it.
            goalIndex = Random.Range(0, spawnPoints.Count);
        } while (goalIndex == spawnPointIndex);
        
        goal.transform.position = spawnPoints[goalIndex].transform.position;
        
    }

}
