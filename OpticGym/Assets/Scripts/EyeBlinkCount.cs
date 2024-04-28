using UnityEngine.UIElements;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif


namespace UnityEngine.XR.ARFoundation.Samples
{
    enum EyeState
    {
        Closed,
        Open
    }

    [RequireComponent(typeof(ARFace))]
    public class EyeBlinkCouunt : MonoBehaviour
    {


        [SerializeField]
        const int totalBlinkCount = 10;

        ARFace m_Face;
        UIElements.Label m_BlinkCountText;
        UIElements.Label m_FaceStatusText;
        ARKit.ARKitFaceSubsystem m_FaceSubsystem;

        private EyeState eyeState = EyeState.Open;
        private int blinkCount = 0;

        void Awake()
        {
            m_Face = GetComponent<ARFace>();

            m_FaceSubsystem = (ARKit.ARKitFaceSubsystem)FindObjectOfType<ARFaceManager>().subsystem;


            var root = FindObjectOfType<UIDocument>().rootVisualElement;

            m_BlinkCountText = root.Q<UIElements.Label>("blinkCountInfo");
            m_FaceStatusText = root.Q<UIElements.Label>("status");
            m_BlinkCountText.text = blinkCount.ToString() + "/" + totalBlinkCount.ToString();
            m_FaceStatusText.text = "waiting for face";

        }



        void OnEnable()
        {
            var faceManager = FindObjectOfType<ARFaceManager>();
            if (faceManager != null && faceManager.subsystem != null && faceManager.descriptor.supportsEyeTracking)
            {
                m_FaceStatusText.text = "face detected";
                m_Face.updated += OnUpdated;
            }
            else
            {
                enabled = false;
                m_FaceStatusText.text = "waiting for face";
            }
        }

        void OnDisable()
        {
            m_Face.updated -= OnUpdated;
            m_FaceStatusText.text = "waiting for face";
        }


        void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
        {

            var blendShapes = m_FaceSubsystem.GetBlendShapeCoefficients(m_Face.trackableId, Unity.Collections.Allocator.Temp);
            foreach(var blendShape in blendShapes)
            {
                if (blendShape.blendShapeLocation == ARKit.ARKitBlendShapeLocation.EyeBlinkLeft)
                {

                    if (blendShape.coefficient >= 0.8)
                    {
                        eyeState = EyeState.Closed;
                    }
                    else
                    {
                        if(eyeState == EyeState.Closed)
                        {
                            blinkCount += 1;
                            UpdateBlinkCountText();
                        }

                        eyeState = EyeState.Open;
                    }


                    break; // early exit only care about this
                }
            }

        }

        private void UpdateBlinkCountText()
        {
            if (blinkCount >= totalBlinkCount)
            {
                m_BlinkCountText.text = totalBlinkCount.ToString() + "/" + totalBlinkCount.ToString();
                m_FaceStatusText.text = "Completed!";
                return;
            }
            Debug.Log(blinkCount);
            m_BlinkCountText.text = blinkCount.ToString() + "/" + totalBlinkCount.ToString();
        }

    }

}
