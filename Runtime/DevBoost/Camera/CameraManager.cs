/* ---------------------------------------------------------------------
 * Created on Mon Jan 11 2021 3:35:30 PM
 * Author : Benjamin Park
 * Description : Camera manager class 
--------------------------------------------------------------------- */
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace DevBoost
{
    using DevBoost.Data;
    using DevBoost.Utilities;

#if UNITY_PIPELINE_URP
    using UnityEngine.Rendering.Universal;
#endif

    /// <summary>
    /// Camera manager to handle cameras in the scenes
    /// </summary>
    public class CameraManager : SingletonMono<CameraManager>
    {
        public class StartedEvent
        {
        }
        public class CreatedEvent { public GameObject obj; public CreatedEvent(GameObject obj) { this.obj = obj; } }
        public class DestroyedEvent { public GameObject obj; public DestroyedEvent(GameObject obj) { this.obj = obj; } }

        [SerializeField] private RenderMode mode = RenderMode.ScreenSpaceCamera;
        [SerializeField] public List<CameraHandler> defaultCameras;
        [SerializeField] private bool autoSwitch = false;
        [SerializeField] private bool ModeSwitch = false;

        public CameraHandler UICamera => defaultCameras.Find(va=>va.Type == CameraType.UICamera);

        // Get current camera instance
        [ShowNativeProperty]
        public static Camera Current {
            get {
                if (Instance == null)
                    return null;
                if (Instance.UICamera?.Camera != null)
                    return Instance.UICamera.Camera;
                return Instance.m_lstCamera.Count > 0 ? 
                        Instance.m_lstCamera[ Instance.m_lstCamera.Count - 1].Camera : 
                        Camera.main;
            }
        }

        private List<CanvasHandler> m_lstCanvas = new List<CanvasHandler>();
        private List<CameraHandler> m_lstCamera = new List<CameraHandler>();

        [ReadOnly, BoxGroup, SerializeField]
        public List<CameraHandler> CameraList;
        [ReadOnly, BoxGroup, SerializeField]
        public List<CanvasHandler> CanvasList;

        public List<CameraHandler> CameraObjects = new List<CameraHandler>();

#if UNITY_PIPELINE_URP
        public CameraHandler BaseCamera => FindCamera(CameraRenderType.Base, CameraType.Default);
#endif
        private new void Awake()
        {
            base.Awake();

            CameraList = m_lstCamera;
            CanvasList = m_lstCanvas;
        }
        /// <summary>
        /// Update Canvas scaler
        /// </summary>
        private void Start()
        {
            if (autoSwitch)
            {
                UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, mode) =>
                {
                    Logger.Trace($"[ CameraManager ] Loaded Scene : {scene.name}");
                    SwitchUICamera(GameObject.FindObjectsOfType<CanvasHandler>());
                };
            }
            Add(UICamera);

            SyncDataDeliver.Publish(new StartedEvent());

            SyncDataDeliver.Receiver<CreatedEvent>(v => CameraObjects.Add(v.obj.GetComponent<CameraHandler>()))
                            .Assign(OnDestroyListener);
            SyncDataDeliver.Receiver<DestroyedEvent>(v => CameraObjects.Remove(v.obj.GetComponent<CameraHandler>()))
                            .Assign(OnDestroyListener);
        }

#if UNITY_PIPELINE_URP
         public CameraHandler FindCamera(CameraRenderType type, CameraType camType)
        {
            return m_lstCamera.Find(va => va.Data.renderType == type && va.Type == camType);
        }
#endif
        /// <summary>
        /// Get a camera handler by camera
        /// </summary>
        /// <param name="cam"></param>
        /// <returns></returns>
        public CameraHandler GetHandler(Camera cam)
        {
            return m_lstCamera.Find(va => va.Camera == cam);
        }

        public void SwitchUICamera(CanvasHandler[] canvasList = null, bool updateMode = false)
        {
            if (m_lstCamera.Count + (canvasList?.Length ?? 0)== 0 || Current == null)
                return;

            var uiCanvasList = new List<CanvasHandler>(m_lstCanvas);
            if (canvasList != null)
                uiCanvasList.AddRange(canvasList);

            Logger.Trace("[ CameraManager ] SwitchUICamera - Current : {0}", Current.name);

            // update canvas with UI Camera
            UpdateCanvasWithCamera(uiCanvasList, Current , updateMode);

            // update tutorial canvas and tutorial camera
            var tutorialCanvas = uiCanvasList.Find(va => va.Type == CanvasType.Tutorial);
            var tutorialCamera = m_lstCamera.Find(va => va.Type == CameraType.Tutorial);
            if (tutorialCanvas != null && tutorialCamera != null)
            {
                tutorialCanvas.Canvas.worldCamera = tutorialCamera.Camera;
            }

        }


        private void UpdateCanvasWithCamera(List<CanvasHandler> list, Camera cam, bool force = false)
        {
            foreach (var item in list)
            {
                if (null == item || item.Type == CanvasType.Custom) continue;
                if (force || ModeSwitch)
                    item.Canvas.renderMode = mode;
                item.Canvas.worldCamera = cam;
            }
        }



        /// <summary>
        /// Add camera object into the list
        /// </summary>
        /// <param name="cam"></param>
        public void Add(CameraHandler cam)
        {
            if (cam == null || m_lstCamera.Contains(cam))
                return;
            Logger.Trace($"[TRACE] Camera Added: {cam.Type} | {cam.name}");

            Debug.Assert(cam.Camera != null,"Camera is null : " + cam.name);
#if UNITY_PIPELINE_URP

            //if (cam.Type != CameraType.UICamera && cam.Data != null && cam.Data.renderType == CameraRenderType.Overlay)
            //    cam.Type = CameraType.Overlay;
            // stacking UI Camera into new camera
            if (cam.Type == CameraType.Default)
            {
                // add overlay cameras
                foreach (var overlayCam in CameraObjects.Where(v => v.isActiveAndEnabled && (v.Type != CameraType.Default && v.Type != CameraType.Independant)))
                    cam.AddToStack(overlayCam);
                // additional check on UI camera
                if (UICamera != null && !cam.Data.cameraStack.Any(v => v == UICamera.Camera))
                    cam.AddToStack(UICamera);
            }
            else
            {
                if (cam.Type == CameraType.Overlay || cam.Type == CameraType.Tutorial || cam.Type == CameraType.UICamera)
                {
                    BaseCamera?.AddToStack(cam);
                }
                if (cam.Type == CameraType.Tutorial)
                {
                    cam.transform.localPosition = UICamera.transform.localPosition;
                    cam.Camera.orthographicSize = UICamera.Camera.orthographicSize;
                }
            }

            // reorder clear depth
            BaseCamera?.UpdateClearDepth();
#endif
            m_lstCamera.Add(cam);
            SwitchUICamera();
        }

        /// <summary>
        /// Remove camera object in the list
        /// </summary>
        /// <param name="cam"></param>
        public void Remove(CameraHandler cam)
        {
            if (cam == null)
                return;

            Logger.Trace($"[TRACE] Camera Removed :{cam.Type} | {cam.name}");
            m_lstCamera.Remove(cam);

#if UNITY_PIPELINE_URP
            var baseCamera = this.BaseCamera;

            if (baseCamera == null)
            {
                Logger.Trace("---- No Base Camera in the app for now -----------------------");
            }
            if (cam.Type == CameraType.Overlay || cam.Type == CameraType.Tutorial)
            {
                baseCamera?.RemoveToStack(cam);
            }

            baseCamera?.UpdateClearDepth();
#endif
            SwitchUICamera();
        }

        public void Add(CanvasHandler canvas)
        {
            m_lstCanvas.Add(canvas);

            SwitchUICamera();
        }
        public void Remove(CanvasHandler canvas)
        {
            m_lstCanvas.Remove(canvas);
        }

        /// <summary>
        /// Convert Rect
        /// </summary>
        /// <param name="targetCanvas"></param>
        /// <returns></returns>
        public Rect ConvertToScreen(RectTransform target, Camera camera, Canvas targetCanvas, Vector2? pivot = null)
        {
            // get target canvas
            var rect = target.GetScreenCoordinatesOfCorners();
            var canvasRect = targetCanvas.GetComponent<RectTransform>();

            var min = camera.WorldToScreenPoint(rect.min);
            var max = camera.WorldToScreenPoint(rect.max);

            var world_rect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);//, pivot);

            return world_rect;
        }


        public Rect UIConvertToScreen(RectTransform target,Vector2? pivot = null)
        {
            return ConvertToScreen(target, UICamera.Camera, target.GetComponentInParent<Canvas>(),pivot);

        }
    }


}

