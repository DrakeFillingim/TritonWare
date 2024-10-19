using UnityEngine;

public class GeneralSettings : MonoBehaviour
{
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
    }
}
