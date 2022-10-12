
/* *************************************************
*  Author:   Benjamin
*  Purpose:  [save image of the render camera]
****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace DevBoost.ActionBehaviour
{
    public class CaptureRenderCamera : ActionNode
    {

        [SerializeField] private Camera _camera;
        [SerializeField] private string savefile = Application.dataPath + "/filname";

        protected override ActionState OnUpdate()
        {

            // parent update
            ActionState result = base.OnUpdate();
            if (result != ActionState.Success)
                return result;

            CamCapture();

            return ActionState.Success;
        }


        void CamCapture()
        {
            Camera Cam = _camera ?? Camera.current;

            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = Cam.targetTexture;

            Cam.Render();

            Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
            Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
            Image.Apply();
            RenderTexture.active = currentRT;

            var Bytes = Image.EncodeToPNG();
            Destroy(Image);

            File.WriteAllBytes(savefile + ".png", Bytes);
        }

    }
}