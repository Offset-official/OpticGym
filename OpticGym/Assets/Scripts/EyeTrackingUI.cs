using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Text = UnityEngine.UI.Text;

[RequireComponent(typeof(Text))]
public class EyeTrackingUI : MonoBehaviour
{
    [SerializeField]
    ARFaceManager m_Manager;

    void OnEnable()
    {
        if (m_Manager == null)
        {
            m_Manager = FindObjectOfType<ARFaceManager>();
        }
        if (m_Manager != null && m_Manager.subsystem != null && m_Manager.descriptor.supportsEyeTracking)
        {
            var infoGO = GetComponent<Text>();
            infoGO.text = "This device supports eye tracking.";
        }
        else
        {
            var infoGO = GetComponent<Text>();
            infoGO.text = "No support :(";
        }
    }
}