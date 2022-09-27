/* ---------------------------------------------------------------------
 * Created on Mon Jan 11 2021 3:34:03 PM
 * Author : Benjamin Park
 * Description : Camera Information class to handle each camera
--------------------------------------------------------------------- */

using UnityEngine;
using NaughtyAttributes;
using System.Collections;

namespace DevBoost
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraMatchWidth : MonoBehaviour
    {

        // Set this to the in-world distance between the left & right edges of your scene.
        public float sceneWidth { get; set; }

        [SerializeField]
        private Chunk ScreenMin = new Chunk() { Screen = new Vector2(9f, 16f), size = 9.2f };
        [SerializeField]
        private Chunk ScreenMax = new Chunk() { Screen = new Vector2(3f, 4f), size = 13f };

        public float ScreenRatio { get { return (float)Screen.width / (float)Screen.height; } }

        [System.Serializable]
        public class Chunk
        {
            public Vector2 Screen;
            public float size;
            public float ratio => Screen.y == 0 ? 0 : Screen.x / Screen.y;
        }

        private Camera _camera;

        void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private IEnumerable Start()
        {
            while(true)
            {
                FitScreen();
                yield return new WaitForSeconds(0.05f);
            }
        }

        // Adjust the camera's height so the desired scene width fits in view
        // even if the screen/window size changes dynamically.
        public float FitScreen()
        {
            var len = ScreenMax.ratio - ScreenMin.ratio;
            var scrRatio = Mathf.Clamp(ScreenRatio, ScreenMin.ratio, ScreenMax.ratio);
            // calc range
            var fact = len != 0 ? (scrRatio - ScreenMin.ratio) / len : 1;

            var lenTarget = ScreenMax.size - ScreenMin.size;
            sceneWidth = ScreenMin.size + fact * lenTarget;

            float unitsPerPixel = sceneWidth / Screen.width;
            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            _camera.orthographicSize = desiredHalfHeight;

            return desiredHalfHeight;
        }

    }
}

