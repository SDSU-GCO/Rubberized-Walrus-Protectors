using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITabController : MonoBehaviour
{
    [HideInInspector, Required]
    public PanelController panelController = null;
    [Required]
    public UIContentFrameController uIContentFrameControllerToActivateOnClick = null;

    private void OnValidate()
    {
        Transform transformRef = transform;
        bool panelNotFount = true;
        while (panelNotFount == true && transform.parent != null)
        {
            transformRef = transformRef.parent;
            PanelController panelController = transformRef.GetComponent<PanelController>();
            if (panelController != null)
            {
                panelNotFount = false;
                panelController.OnValidate();
            }
        }
    }

    private void Awake()
    {
        if (panelController == null)
        {
            Debug.LogError("Orphaned UI element 'UITabController': " + this);
        }
    }

    public void ActivateFrame() => panelController.SetActiveFrame(uIContentFrameControllerToActivateOnClick);
}