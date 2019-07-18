using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win_Screen_Check : MonoBehaviour
{
    public Canvas winScreenCanvas;
    public bool treesCured;
    public bool enemiesCured;

    private void Update()
    {
        //so right now this doesn't work, I'm too tired to think lol, it needs to check if the enemies and trees are cured then enable itself.
        if (treesCured == true && enemiesCured == true)
        {
            winScreenCanvas.enabled = true;
        }
    }
        
}
