using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
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
    public class EyeBlinkCouunt : MonoBehaviour
    {

        ARFace m_Face;
        UI.Text m_BlinkCountText;
        ARKit.ARKitFaceSubsystem m_FaceSubsystem;

        private float blinkState = 0;

        void Awake()
        {
            m_Face = GetComponent<ARFace>();

            m_FaceSubsystem = (ARKit.ARKitFaceSubsystem)FindObjectOfType<ARFaceManager>().subsystem;

            m_BlinkCountText = GameObject.Find("Blink Count").GetComponent<UI.Text>();
            m_BlinkCountText.text = "Ready to blink";

        }



        void OnEnable()
        {
            var faceManager = FindObjectOfType<ARFaceManager>();
            if (faceManager != null && faceManager.subsystem != null && faceManager.descriptor.supportsEyeTracking)
            {
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
        }

        void OnUpdated(ARFaceUpdatedEventArgs eventArgs)
        {

            var blendShapes = m_FaceSubsystem.GetBlendShapeCoefficients(m_Face.trackableId, Unity.Collections.Allocator.Temp);
            foreach(var blendShape in blendShapes)
            {
                if (blendShape.blendShapeLocation == ARKit.ARKitBlendShapeLocation.EyeBlinkLeft)
                {
                    blinkState += blendShape.coefficient;

                    m_BlinkCountText.text = blinkState.ToString();
                }
            }

        }

    }
}
