using System.Diagnostics;
using UnityEngine;


namespace DevBoost
{
    public static class DebugDraw
    {
        [Conditional("DEBUG_DRAW")]
        public static void DrawMarker(Vector3 position, float size, Color color, float duration = 0, bool depthTest = false)
        {
            Vector3 line1PosA = position + Vector3.up * size * 0.5f;
            Vector3 line1PosB = position - Vector3.up * size * 0.5f;

            Vector3 line2PosA = position + Vector3.right * size * 0.5f;
            Vector3 line2PosB = position - Vector3.right * size * 0.5f;

            Vector3 line3PosA = position + Vector3.forward * size * 0.5f;
            Vector3 line3PosB = position - Vector3.forward * size * 0.5f;

            UnityEngine.Debug.DrawLine(line1PosA, line1PosB, color, duration, depthTest);
            UnityEngine.Debug.DrawLine(line2PosA, line2PosB, color, duration, depthTest);
            UnityEngine.Debug.DrawLine(line3PosA, line3PosB, color, duration, depthTest);
        }

        // draw circle on debug
        [Conditional("DEBUG_DRAW")]
        public static void DrawCircle(Vector3 centerPos, float Radius, Color color, float thetafact = 0.2f)
        {
            float theta = 0;
            float x = Radius * Mathf.Cos(theta);
            float y = Radius * Mathf.Sin(theta);
            var pos = centerPos + new Vector3(x, 0, y);
            var newPos = pos;
            var lastPos = pos;
            for (theta = 0.1f; theta < Mathf.PI * 2; theta += thetafact)
            {
                x = Radius * Mathf.Cos(theta);
                y = Radius * Mathf.Sin(theta);
                newPos = centerPos + new Vector3(x, 0, y);
                UnityEngine.Debug.DrawLine(pos, newPos, color);
                pos = newPos;
            }
            UnityEngine.Debug.DrawLine(pos, lastPos, color);
        }

        // draw circle on gizmos
        [Conditional("DEBUG_DRAW")]
        public static void DrawCircleGizmos(Vector3 centerPos, float Radius, float thetafact = 0.2f)
        {
            float theta = 0;
            float x = Radius * Mathf.Cos(theta);
            float y = Radius * Mathf.Sin(theta);
            var pos = centerPos + new Vector3(x, 0, y);
            var newPos = pos;
            var lastPos = pos;
            for (theta = 0.1f; theta < Mathf.PI * 2; theta += thetafact)
            {
                x = Radius * Mathf.Cos(theta);
                y = Radius * Mathf.Sin(theta);
                newPos = centerPos + new Vector3(x, 0, y);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }
            Gizmos.DrawLine(pos, lastPos);
        }

        [Conditional("DEBUG_DRAW")]
        public static void DrawBox(Bounds box)
        {
            Gizmos.DrawWireCube(box.center, box.size);

        }

    }

}