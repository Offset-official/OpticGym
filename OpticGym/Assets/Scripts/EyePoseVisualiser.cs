using UnityEngine.XR.ARSubsystems;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Visualizes the eye poses for an <see cref="ARFace"/>.
    /// </summary>
    /// <remarks>
    /// Face space is the space where the origin is the transform of an <see cref="ARFace"/>.
    /// </remarks>
    [RequireComponent(typeof(ARFace))]
    public class EyePoseVisualizer : MonoBehaviour
    {
        [SerializeField]
        GameObject m_EyePrefab;

        private bool showShader = true;

        public GameObject eyePrefab
        {
            get => m_EyePrefab;
            set => m_EyePrefab = value;
        }

        GameObject m_LeftEyeGameObject;
        GameObject m_RightEyeGameObject;
        ARFace m_Face;

        void Awake()
        {
            m_Face = GetComponent<ARFace>();
        }
        void Update()
        {

            // if (m_RightEyeGameObject)
            // {
            //     Debug.Log("Right eye rotation: " + m_RightEyeGameObject.transform.rotation.eulerAngles);
            // }

        }

        void CreateEyeGameObjectsIfNecessary()
        {
            if (m_Face.leftEye != null && m_LeftEyeGameObject == null)
            {
                m_LeftEyeGameObject = Instantiate(m_EyePrefab, m_Face.leftEye);
                // Debug.Log("Left eye created");
                // Debug.Log(m_Face.leftEye.rotation.eulerAngles);
                m_LeftEyeGameObject.SetActive(false);
            }
            if (m_Face.rightEye != null && m_RightEyeGameObject == null)
            {
                m_RightEyeGameObject = Instantiate(m_EyePrefab, m_Face.rightEye);
                // Debug.Log("Right eye created");
                // Debug.Log(m_Face.rightEye.rotation.eulerAngles);
                m_RightEyeGameObject.SetActive(false);
            }
        }

        void SetVisible(bool visible)
        {
            if (m_LeftEyeGameObject != null && m_RightEyeGameObject != null)
            {
                m_LeftEyeGameObject.SetActive(visible);
                m_RightEyeGameObject.SetActive(visible);
            }
        }


        void OnEnable()
        {
            var faceManager = FindObjectOfType<ARFaceManager>();
            if (faceManager != null && faceManager.subsystem != null && faceManager.descriptor.supportsEyeTracking)
            {
                SetVisible((m_Face.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready));
                m_Face.updated += OnUpdated;
            }
            else
            {
                enabled = false;
            }
        }

        void OnDisable()
        {
            m_Face.updated -= OnUpdated;
            SetVisible(false);
        }

        void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
        {
            if (m_LeftEyeGameObject)
            {
                Debug.Log("Left eye rotation: " + m_LeftEyeGameObject.transform.parent.rotation);
            }
            CreateEyeGameObjectsIfNecessary();
            SetVisible((m_Face.trackingState == TrackingState.Tracking) && (ARSession.state > ARSessionState.Ready));
        }

        public void ToggleLaserShader()
        {
            showShader = !showShader;
            var laserCylinders = GameObject.FindGameObjectsWithTag("ARDebug");
            foreach (var cylinder in laserCylinders)
            {
                cylinder.GetComponent<MeshRenderer>().enabled = showShader;
            }
        }
    }
}
