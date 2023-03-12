/* ---------------------------------------------------------------------
 * Created on Mon Jan 11 2021 3:55:29 PM
 * Author : Benjamin Park
 * Description : Ui Extension functionalites
--------------------------------------------------------------------- */

using UnityEngine;

namespace DevBoost
{
    /// <summary>
    /// Extention class for Canvas UI
    /// </summary>
    public static class CanvasExtensions
    {
        /// <summary>
        /// Convert world view location to the Canvas location
        /// </summary>
        /// <param name="canvasRect"></param>
        /// <param name="ViewportPosition"></param>
        /// <returns></returns>
        public static Vector2 CalcViewToCanvas(RectTransform canvasRect, Vector2 ViewportPosition, Vector2? pivot = null)
        {
            //then you calculate the position of the UI element
            //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.
            Vector2 ui_anchoredPos = new Vector2(ViewportPosition.x * canvasRect.sizeDelta.x, ViewportPosition.y * canvasRect.sizeDelta.y);
            if (pivot == null)
                pivot = new Vector2(-0.5f, -0.5f);

            ui_anchoredPos.x += canvasRect.sizeDelta.x * pivot.Value.x;
            ui_anchoredPos.y += canvasRect.sizeDelta.y * pivot.Value.y;

            return ui_anchoredPos;
        }

        /// <summary>
        /// Convert world view location to the Canvas location
        /// </summary>
        /// <param name="canvasRect"></param>
        /// <param name="ViewportPosition"></param>
        /// <returns></returns>
        public static Rect CalcViewToCanvas(RectTransform canvasRect, Rect ViewportRect, Vector2? pivot = null)
        {
            //then you calculate the position of the UI element
            //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

            ViewportRect.xMin *= canvasRect.sizeDelta.x;
            ViewportRect.yMin *= canvasRect.sizeDelta.y;
            ViewportRect.xMax *= canvasRect.sizeDelta.x;
            ViewportRect.yMax *= canvasRect.sizeDelta.y;

            if (pivot == null)
                pivot = new Vector2(-0.5f, -0.5f);

            ViewportRect.center += new Vector2( canvasRect.sizeDelta.x * pivot.Value.x, canvasRect.sizeDelta.y * pivot.Value.y);

            return ViewportRect;
        }

        /// <summary>
        /// Get offset vector2 from the Canvas
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static Vector3 GetOffSetVector3(this Canvas canvas,Vector2 origin)
        {
            var canvasRect = canvas.GetComponent<RectTransform>();
            return (canvasRect.sizeDelta - origin) / 2;  
        }

        public static Vector2 GetOffSetVector2(this Canvas canvas, Vector2 origin)
        {
            var canvasRect = canvas.GetComponent<RectTransform>();
            return (canvasRect.sizeDelta - origin) / 2;
        }

        /// <summary>
        /// Get the anchored position vector from the world position via camera
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="world_position"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Vector2 WorldToCanvas(this Canvas canvas, Vector3 world_position, RectTransform canvasRect = null, Camera camera = null)
        {
            if (camera == null)
                camera = CameraManager.Current;

            //first you need the RectTransform component of your canvas
            if( canvasRect == null)
                canvasRect = canvas.GetComponent<RectTransform>();                
            
            //now you can set the position of the ui element
            return CalcViewToCanvas(canvasRect, camera.WorldToViewportPoint(world_position));

        }

        /// <summary>
        /// Screen pos to Canvas
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="screen_pos"></param>
        /// <param name="pivot"></param>
        /// <returns></returns>
        public static Vector2 ScreenToCanvas(this Canvas canvas, Vector2 screen_pos, Vector2? pivot = null)
        {
            if (null == pivot) pivot = new Vector2(0.5f, 0.5f);

            RectTransform rectTransform = canvas.GetComponent<RectTransform>();
            Vector2 anchorPos;
            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay && canvas.worldCamera != null)
            {
                //Canvas is in Camera mode
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screen_pos, canvas.worldCamera, out anchorPos);
            }
            else
            {
                //Canvas is in Overlay mode
                anchorPos = screen_pos - new Vector2(rectTransform.position.x, rectTransform.position.y);
                anchorPos = new Vector2(anchorPos.x / rectTransform.lossyScale.x, anchorPos.y / rectTransform.lossyScale.y);
            }
            // pivot
            anchorPos.x += rectTransform.sizeDelta.x * pivot.Value.x;
            anchorPos.y += rectTransform.sizeDelta.y * pivot.Value.y;
            return anchorPos;
        }

        public static Rect ScreenToCanvas(this Canvas canvas, Rect screenRect, Vector2? pivot = null)
        {
            if (null == pivot) pivot = new Vector2(0.5f, 0.5f);

            RectTransform rectTransform = canvas.GetComponent<RectTransform>();
            Vector2 min, max;
            if (canvas.renderMode != RenderMode.ScreenSpaceOverlay && canvas.worldCamera != null)
            {
                //Canvas is in Camera mode
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenRect.min, canvas.worldCamera, out min);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenRect.max, canvas.worldCamera, out max);
            }
            else
            {
                //Canvas is in Overlay mode
                min = screenRect.min - new Vector2(rectTransform.position.x, rectTransform.position.y);
                min = new Vector2(min.x / rectTransform.lossyScale.x, min.y / rectTransform.lossyScale.y);
                max = screenRect.max - new Vector2(rectTransform.position.x, rectTransform.position.y);
                max = new Vector2(max.x / rectTransform.lossyScale.x, max.y / rectTransform.lossyScale.y);
            }
            Rect anchorRect = Rect.MinMaxRect(min.x,min.y,max.x, max.y);
            // pivot
            anchorRect.center += new Vector2(rectTransform.sizeDelta.x * pivot.Value.x, rectTransform.sizeDelta.y * pivot.Value.y);
            return anchorRect;
        }

        /// <summary>
        /// Overwrap ui element positon with world position
        /// </summary>
        /// <param name="rectTrans"></param>
        /// <param name="world_pos"></param>
        /// <param name="canvasRect"></param>
        /// <param name="canvas"></param>
        /// <param name="camera"></param>
        public static void OverlayFromWorld(this RectTransform rectTrans, Vector3 world_pos, Vector2? pivot = null,RectTransform canvasRect = null, Canvas canvas = null, Camera camera = null)
        {
            if (camera == null)
                camera = CameraManager.Current;

            if (canvas == null)
                canvas = rectTrans.GetComponentInParent<Canvas>();

            if (canvas != null)
            {
                if (canvasRect == null)
                    canvasRect = canvas.GetComponent<RectTransform>();

                rectTrans.anchoredPosition = CalcViewToCanvas(canvasRect, camera.WorldToViewportPoint(world_pos), pivot);
            }
        }

        /// <summary>
        /// Overlay Rect
        /// </summary>
        /// <param name="rectTrans"></param>
        /// <param name="world_rect"></param>
        /// <param name="pivot"></param>
        /// <param name="canvasRect"></param>
        /// <param name="canvas"></param>
        /// <param name="camera"></param>
        public static void OverlayFromWorld(this RectTransform rectTrans, Rect world_rect, Vector2? pivot = null, RectTransform canvasRect = null, Canvas canvas = null, Camera camera = null)
        {
            if (camera == null)
                camera = CameraManager.Current;

            if (canvas == null)
                canvas = rectTrans.GetComponentInParent<Canvas>();

            if (canvas != null)
            {
                if (canvasRect == null)
                    canvasRect = canvas.GetComponent<RectTransform>();
                var min = camera.WorldToViewportPoint(world_rect.min);
                var max = camera.WorldToViewportPoint(world_rect.max);

                world_rect = CalcViewToCanvas(canvasRect, Rect.MinMaxRect(min.x, min.y, max.x, max.y), pivot);
                rectTrans.anchoredPosition = world_rect.center;
                rectTrans.sizeDelta = world_rect.size;
                // offset
                rectTrans.anchoredPosition += rectTrans.sizeDelta * new Vector2(-0.5f, 0.5f);
            }
        }

        /// <summary>
        /// Update position refer to world object position
        /// </summary>
        /// <param name="rectTrans"></param>
        /// <param name="world_pos"></param>
        /// <param name="ui_canvas"></param>
        /// <param name="camera"></param>
        public static void OverlayLocalFromWorld(this RectTransform rectTrans, Vector3 world_pos, RectTransform parentRect = null, Canvas canvas = null, Camera camera = null)
        {
            if (canvas == null)
                canvas = rectTrans.GetComponentInParent<Canvas>();

            if (canvas != null)
            {
                if (camera == null)
                    camera = CameraManager.Current;

                if (parentRect == null)
                    parentRect = canvas.GetComponent<RectTransform>();

                // get screen pos
                Vector3 screenPos = camera.WorldToScreenPoint(world_pos);
                Vector2 screenPos2D = new Vector2(screenPos.x, screenPos.y);
                Vector2 anchoredPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPos2D, Camera.main, out anchoredPos);
                rectTrans.anchoredPosition = anchoredPos;
            }
        }


        public static Rect GetScreenCoordinatesOfCorners(this RectTransform uiElement)
        {
            var worldCorners = new Vector3[4];
            uiElement.GetWorldCorners(worldCorners);
            var result = new Rect(
                          worldCorners[0].x,
                          worldCorners[0].y,
                          worldCorners[2].x - worldCorners[0].x,
                          worldCorners[2].y - worldCorners[0].y);
            return result;
        }

        public static Vector2 GetPixelPositionOfRect(this RectTransform uiElement)
        {
            Rect screenRect = GetScreenCoordinatesOfCorners(uiElement);

            return new Vector2(screenRect.center.x, screenRect.center.y);
        }

    }
}