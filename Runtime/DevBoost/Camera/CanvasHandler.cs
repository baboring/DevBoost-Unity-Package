/* ---------------------------------------------------------------------
 * Created on Mon Jan 11 2021 3:34:03 PM
 * Author : Benjamin Park
 * Description : CanvasHandle Information class to handle each canvas
--------------------------------------------------------------------- */

using System.Linq;
using UniRx;

namespace DevBoost
{
    using UnityEngine;

    public enum CanvasType
    {
        Default,
        Custom,
        Tutorial,
    }

    /// <summary>
    /// Canvas handler for fixing specific ui camera
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class CanvasHandler : MonoBehaviour
    {
        /// <summary>
        /// Get highest number or the sorting order of canvases
        /// </summary>
        public static int HighestSortOrder
        {
            get
            {
                var all = FindObjectsOfType<Canvas>();
                var found = all.OrderByDescending(v => v.sortingOrder).FirstOrDefault();
                return found?.sortingOrder ?? 0;
            }
        }
        [SerializeField]
        public CanvasType Type;

        public Canvas Canvas { get; private set; }

        [SerializeField]
        private float planeDistance = 10;
        [SerializeField]
        private int sortOrder = -1;

        private bool subscribed;

        private void OnEnable()
        {
            Canvas = this.GetComponent<Canvas>();

            CameraManager.Instance?.Add(this);

            if (this.Canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                this.Canvas.planeDistance = planeDistance;
                if (this.sortOrder > -1)
                    this.Canvas.sortingOrder = this.sortOrder;
            }

            if (!subscribed)
            {
                MessageBroker.Default.Receive<CameraManager.CameraManagerStartedEvent>().Subscribe( result =>
                {
                    UpdateMode(this.Canvas.renderMode);    
                }).AddTo(this);
                subscribed = true;
            }
        }

        private void OnDisable() {

            CameraManager.Instance?.Remove(this);

        }

        public void UpdateMode(RenderMode mode, Camera cam = null)
        {
            this.Canvas.worldCamera = cam;
            UpdateMode(mode);

        }

        private void UpdateMode(RenderMode mode)
        {
            this.Canvas.renderMode = mode;
            if (this.Canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                this.Canvas.planeDistance = planeDistance;
            }
        }
    }
    
}

