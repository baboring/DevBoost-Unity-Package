/* ---------------------------------------------------------------------
 * Created on Mon Jan 11 2021 3:34:03 PM
 * Author : Benjamin Park
 * Description : Camera Information class to handle each camera
--------------------------------------------------------------------- */
using DevBoost.Data;
using System;
using System.Reflection;
using UnityEngine;
#if UNITY_PIPELINE_URP
using UnityEngine.Rendering.Universal;
#endif

namespace DevBoost
{

    public enum CameraType
    {
        Independant,
        Primary,
        Overlay,
        UICamera,
        Tutorial,
    }
    /// <summary>
    /// Camera manager to handle cameras in the scenes
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField]
        public CameraType Type;

        // Get Camera
        public Camera Camera { get; private set; }

#if UNITY_PIPELINE_URP
        public UniversalAdditionalCameraData Data { get; private set; }
#endif

        private void Awake()
        {
            Camera = this.GetComponent<Camera>();
#if UNITY_PIPELINE_URP
            Data = Camera.GetUniversalAdditionalCameraData();
#endif
        }

        private void Start()
        {
            SyncDataDeliver.Publish(new CameraManager.CreatedEvent(gameObject));
        }

        private void OnDestroy()
        {
            SyncDataDeliver.Publish(new CameraManager.DestroyedEvent(gameObject));
        }


        private void OnEnable()
        {
            CameraManager.Instance?.Add(this);
        }

        private void OnDisable() 
        {
            CameraManager.Instance?.Remove(this);
        }
#if UNITY_PIPELINE_URP

        public void AddToStack(CameraHandler cam)
        {
            var camData = Data ?? GetComponent<Camera>()?.GetUniversalAdditionalCameraData();
            Debug.Assert(camData != null, "Universal Additional Camera Data is null");
            if (camData != null && cam != null )
            {
                Debug.Assert(camData.renderType == CameraRenderType.Base,"trying to add the camera to Overlay type camera : " + this.name);
                if (camData.cameraStack != null && !Data.cameraStack.Contains(cam.Camera))
                    camData.cameraStack.Add(cam.Camera);
            }
        }
        public void RemoveToStack(CameraHandler cam)
        {
            var camData = Data ?? GetComponent<Camera>()?.GetUniversalAdditionalCameraData();
            Debug.Assert(camData != null, "Universal Additional Camera Data is null");
            if (camData != null && cam != null)
            {
                Debug.Assert(camData.renderType == CameraRenderType.Base, "trying to remove the camera to Overlay type camera" + this.name);
                if (camData.cameraStack != null && camData.cameraStack.Contains(cam.Camera))
                    camData.cameraStack.Remove(cam.Camera);
            }
        }

        /// <summary>
        /// Update ClearDepth
        /// </summary>
        public void UpdateClearDepth()
        {
            var camData = Data ?? GetComponent<Camera>()?.GetUniversalAdditionalCameraData();
            Debug.Assert(camData != null, "Universal Additional Camera Data is null");
            if (camData != null)
            {
                Debug.Assert(camData.renderType == CameraRenderType.Base, "trying to remove the camera to Overlay type camera" + this.name);
                if (camData.cameraStack != null)
                {
                    Type type = camData.GetType();
                    var field = type.GetField("m_ClearDepth", BindingFlags.NonPublic | BindingFlags.Instance);
                    for (int i = 0; i < camData.cameraStack.Count; ++i)
                    {
                        if (camData.cameraStack[i] == null)
                            continue;
                        try
                        {
                            var handler = CameraManager.Instance.GetHandler(camData.cameraStack[i]);
                            field.SetValue(handler?.Data ?? camData.cameraStack[i]?.GetUniversalAdditionalCameraData(), i == 0);
                        }
                        catch (Exception exp)
                        {
                            Debug.LogException(exp);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Change camera render order to the top
        /// </summary>
        /// <param name="cam"></param>
        public void BringTopCamera(Camera cam)
        {
            if (Data != null && Data.cameraStack.Contains(cam))
            {
                Data.cameraStack.Remove(cam);

                // if found top position under the tutorials
                for (int i = 0; i < Data.cameraStack.Count; ++i)
                {
                    var handler = CameraManager.Instance.GetHandler(Data.cameraStack[i]);
                    if (handler != null && handler.Type == module.camera.CameraType.Tutorial)
                    {
                        Data.cameraStack.Insert(i, cam);
                        return;
                    }
                }

                Data.cameraStack.Add(cam);
                CameraManager.Instance?.BaseCamera?.UpdateClearDepth();
            }
        }

        /// <summary>
        /// Change camera render order to the bottom
        /// </summary>
        /// <param name="cam"></param>
        public void PushToBottom(Camera cam)
        {
            if (Data != null && Data.cameraStack.Contains(cam))
            {
                Data.cameraStack.Remove(cam);
                Data.cameraStack.Insert(0, cam);
                CameraManager.Instance?.BaseCamera?.UpdateClearDepth();
            }
        }
#endif
    }
}

