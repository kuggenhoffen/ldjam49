using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitButton : IActivatable
{
    public GameLoop gameLoop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void PerformAction() {
        gameLoop.SubmitWork();
    }
}
