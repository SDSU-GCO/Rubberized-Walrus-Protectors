using UnityEngine;

internal struct MBDOInitializationHelper
{
    private GameObject cardinalSubsystem;
    private MBDataObjectReferences mbDatabaseObjectReferences;
    private MonoBehaviour caller;
    private bool isSetup;

    public MBDOInitializationHelper(MonoBehaviour callerAkaThis)
    {
        isSetup = false;
        cardinalSubsystem = null;
        mbDatabaseObjectReferences = null;
        caller = callerAkaThis;

        Setup(callerAkaThis);
    }

    public void Setup(MonoBehaviour callerAkaThis)
    {
        isSetup = true;
        cardinalSubsystem = null;
        mbDatabaseObjectReferences = null;
        caller = callerAkaThis;

        cardinalSubsystem = GameObject.Find("Cardinal Subsystem");
        if (cardinalSubsystem != null)
        {
            mbDatabaseObjectReferences = cardinalSubsystem.GetComponent<MBDataObjectReferences>();
            if (mbDatabaseObjectReferences == null)
            {
                //Debug.Log("mbDatabaseObjectReferences not found in " + cardinalSubsystem);
            }
        }
        else
        {
            //Debug.Log("Cardinal Subsystem not found in " + this);
        }
    }

    public void SetupMBDO<T>(ref T mbdo) where T : MBDataObject
    {
        if (isSetup == false)
        {
            //Debug.LogWarning("MBDOInitializationHelper: " + mbdo + "is not set up in::: " + caller);
        }
        else if (cardinalSubsystem != null && mbDatabaseObjectReferences != null)
        {
            if (mbdo == null && cardinalSubsystem.scene == caller.gameObject.scene && cardinalSubsystem.scene != new UnityEngine.SceneManagement.Scene())
            {
                mbDatabaseObjectReferences.TryPopulate(out mbdo);
            }
        }
    }
}