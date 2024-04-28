using UnityEngine;
using UnityEngine.XR.ARSubsystems;


namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARFace))]
    public class FixationPoint2DCoords : MonoBehaviour
    {


        Canvas m_Canvas;
        ARFace m_Face;

        private Vector3 fixationCoords;

        public bool IsFixationAt(CustomEyeData.EyePositionStates eyeState)
        {
            var diseredRange = CustomEyeData.EyePositions[eyeState];

            var myX = fixationCoords[0];
            var myY = fixationCoords[1];

            var lowerX = diseredRange[0];
            var lowerY = diseredRange[1];
            var upperX = diseredRange[2];
            var upperY = diseredRange[3];

            if(lowerX <= myX && myX <= upperX)
            {
                if(lowerY <= myY && myY <= upperY)
                {
                    return true;
                }
            }

            return false;
        }

        void Awake()
        {
            m_Face = GetComponent<ARFace>();
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
            UpdateScreenReticle();
        }

        void UpdateScreenReticle()
        {
            var mainCamera = Camera.main;
            var fixationInViewSpace = mainCamera.WorldToViewportPoint(m_Face.fixationPoint.position);
            // The camera texture is mirrored so x and y must be changed to match where the fixation point is in relation to the face.
            var mirrorFixationInView = new Vector3(1 - fixationInViewSpace.x, 1 - fixationInViewSpace.y, fixationInViewSpace.z);

            Debug.Log("Fixation Point coords:");
            Debug.Log(mirrorFixationInView);

            fixationCoords = mirrorFixationInView;

            var eyeStateString = "Null";
            foreach (var state in CustomEyeData.EyePositions.Keys)
            {
                
                var isCorrect = IsFixationAt(state);
                if (isCorrect)
                {
                    Debug.Log(state.ToString() + ": " + isCorrect);
                    eyeStateString = state.ToString();
                }
            }
            GameObject.Find("Eye State Info").GetComponent<UI.Text>().text = eyeStateString;
        }
    }
}
