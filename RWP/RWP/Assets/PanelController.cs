using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject contentFrames = null;
    [SerializeField]
    private GameObject tabs = null;

    [SerializeField]
    private List<UITabController> uITabControllers = new List<UITabController>();
    [SerializeField]
    private List<UIContentFrameController> uIContentFrameControllers = new List<UIContentFrameController>();

    public void OnValidate()
    {
        //get content frames
        uIContentFrameControllers.Clear();
        if (contentFrames != null)
        {
            int childCount = contentFrames.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                UIContentFrameController uIContentFrameController = contentFrames.transform.GetChild(i).GetComponent<UIContentFrameController>();
                if (uIContentFrameController != null)
                {
                    uIContentFrameControllers.Add(uIContentFrameController);
                }
            }

            foreach (UIContentFrameController uIContentFrameController in uIContentFrameControllers)
            {
                uIContentFrameController.panelController = this;
            }
        }
        else
        {
            Debug.LogError("Content Frames is null in " + this);
        }

        //get tabs
        uITabControllers.Clear();
        if (tabs != null)
        {
            int childCount = tabs.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                UITabController uITabController = tabs.transform.GetChild(i).GetComponent<UITabController>();
                if (uITabController != null)
                {
                    uITabControllers.Add(uITabController);
                }
            }

            foreach (UITabController uITabController in uITabControllers)
            {
                uITabController.panelController = this;
            }
        }
        else
        {
            Debug.LogError("Tabs is null in " + this);
        }
    }

    public void SetActiveFrame(UIContentFrameController frameToActivate)
    {
        if (uIContentFrameControllers.Contains(frameToActivate) != true)
        {
            Debug.LogError("The frame: " + frameToActivate + " does not appear to listed in this panels Content Frames!  Please ensure the heirarchy grouping is correct!");
        }
        else
        {
            foreach (UIContentFrameController uIContentFrameController in uIContentFrameControllers)
            {
                uIContentFrameController.gameObject.SetActive(false);
            }
            frameToActivate.gameObject.SetActive(true);
        }
    }
}