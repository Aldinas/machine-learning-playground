using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Academy_SimpleMaze : Academy
{
    // We need some global variables for our testing, so we can set them here.
    public float agentRunSpeed = 2;             // The speed the agent moves at.
    public Material successMaterial;            // The colour to change the floor to on success.
    public Material failMaterial;               // The colour to change the floor to on failure.

    /// The reason we change the floor color is that during training you may have one (or more) instances
    /// of the training area visible on screen, and the color change will allow you to see at a glance 
    /// the general performance of your agents. You could skip this, but if you looked in on your game during training
    /// you wouldnt really be able to tell much of what is happening. 

    // Lastly, we want to borrow the gravity settings from PushBlockAcademy, we will use a multiplier of around 3 to make things
    // less floaty.
    public float gravityMultiplier;

    void State()
    {
        Physics.gravity *= gravityMultiplier;
    }

}
