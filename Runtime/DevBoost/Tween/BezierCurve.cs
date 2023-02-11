using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace DevBoost.Effects
{
    /*    
                private BezierCurve bc = new BezierCurve();
                private List<Vector3> movepath = new List<Vector3>();
                // how many points do you need on the curve?
                const int POINTS_ON_CURVE = 1000;

                double[] ptind = new double[ptList.Count];
                double[] p = new double[POINTS_ON_CURVE];
                ptList.CopyTo(ptind, 0);

                bc.Bezier2D(ptind, (POINTS_ON_CURVE) / 2, p);
                // draw points
                for (int i = 1; i != POINTS_ON_CURVE - 1; i += 2)
                    movepath.Add(new Vector3((float)p[i + 1], (float)p[i], currentPos.z));
    */
    public class BezierCurve
    {
        private static float[] FactorialLookup = new float[] {
            1.0f,
            1.0f,
            2.0f,
            6.0f,
            24.0f,
            120.0f,
            720.0f,
            5040.0f,
            40320.0f,
            362880.0f,
            3628800.0f,
            39916800.0f,
            479001600.0f,
            6227020800.0f,
            87178291200.0f,
            1307674368000.0f,
            20922789888000.0f,
            355687428096000.0f,
            6402373705728000.0f,
        };

        public BezierCurve()
        {
            //CreateFactorialTable();
        }

        // create lookup table for fast factorial calculation
        private static double[] CreateFactorialTable()
        {
            // fill untill n=32. The rest is too high to represent
            double[] a = new double[33];
            a[0] = 1.0;
            a[1] = 1.0;
            a[2] = 2.0;
            a[3] = 6.0;
            a[4] = 24.0;
            a[5] = 120.0;
            a[6] = 720.0;
            a[7] = 5040.0;
            a[8] = 40320.0;
            a[9] = 362880.0;
            a[10] = 3628800.0;
            a[11] = 39916800.0;
            a[12] = 479001600.0;
            a[13] = 6227020800.0;
            a[14] = 87178291200.0;
            a[15] = 1307674368000.0;
            a[16] = 20922789888000.0;
            a[17] = 355687428096000.0;
            a[18] = 6402373705728000.0;
            a[19] = 121645100408832000.0;
            a[20] = 2432902008176640000.0;
            a[21] = 51090942171709440000.0;
            a[22] = 1124000727777607680000.0;
            a[23] = 25852016738884976640000.0;
            a[24] = 620448401733239439360000.0;
            a[25] = 15511210043330985984000000.0;
            a[26] = 403291461126605635584000000.0;
            a[27] = 10888869450418352160768000000.0;
            a[28] = 304888344611713860501504000000.0;
            a[29] = 8841761993739701954543616000000.0;
            a[30] = 265252859812191058636308480000000.0;
            a[31] = 8222838654177922817725562880000000.0;
            a[32] = 263130836933693530167218012160000000.0;

            return a;
        }

        // just check if n is appropriate, then return the result
        private static float factorial(int n)
        {
            if (n < 0) { throw new System.Exception("n is less than 0"); }
            if (n > FactorialLookup.Length - 1) { throw new System.Exception("n is greater than " + (FactorialLookup.Length - 1)); }

            return FactorialLookup[n]; /* returns the value n! as a SUMORealing point number */
        }


        private static float Binomial(int n, int i)
        {
            float ni;
            float a1 = factorial(n);
            float a2 = factorial(i);
            float a3 = factorial(n - i);
            ni = a1 / (a2 * a3);
            return ni;
        }

        // Calculate Bernstein basis
        private static float Bernstein(int n, int i, float t)
        {
            float basis;
            float ti; /* t^i */
            float tni; /* (1 - t)^i */

            /* Prevent problems with pow */

            if (t == 0.0 && i == 0)
                ti = 1.0f;
            else
                ti = Mathf.Pow(t, i);

            if (n == i && t == 1.0)
                tni = 1.0f;
            else
                tni = Mathf.Pow((1 - t), (n - i));

            //Bernstein basis
            basis = Binomial(n, i) * ti * tni;
            return basis;
        }

        public void Bezier2D(float[] b, int cpts, float[] p)
        {
            int npts = (b.Length) / 2;
            int icount, jcount;
            float step, t;

            // Calculate points on curve

            icount = 0;
            t = 0;
            step = (float)1.0 / (cpts - 1);

            for (int i1 = 0; i1 != cpts; i1++)
            {
                if ((1.0 - t) < 5e-6)
                    t = 1.0f;

                jcount = 0;
                p[icount] = 0.0f;
                p[icount + 1] = 0.0f;
                for (int i = 0; i != npts; i++)
                {
                    float basis = Bernstein(npts - 1, i, t);
                    p[icount] += basis * b[jcount];
                    p[icount + 1] += basis * b[jcount + 1];
                    jcount = jcount + 2;
                }

                icount += 2;
                t += step;
            }
        }


        // Get an interpolated point where t must be between 0 and 1.
        public static Vector3 Point3(float t, List<Vector3> controlPoints)
        {
            int N = controlPoints.Count - 1;
            if (N > 18)
            {
                Debug.Log("You have used more than 18 control points. The maximum control points allowed is 18.");
                controlPoints.RemoveRange(18, controlPoints.Count - 18);
            }
            if (t <= 0) return controlPoints[0];
            if (t >= 1) return controlPoints[controlPoints.Count - 1];

            Vector3 p = new Vector3();

            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Vector3 bn = Bernstein(N, i, t) * controlPoints[i];
                p += bn;
            }

            return p;
        }

        public static Vector2 Point2(float t, List<Vector2> controlPoints)
        {
            int N = controlPoints.Count - 1;
            if (N > 18)
            {
                Debug.Log("You have used more than 18 control points. The maximum control points allowed is 18.");
                controlPoints.RemoveRange(18, controlPoints.Count - 18);
            }

            if (t <= 0) return controlPoints[0];
            if (t >= 1) return controlPoints[controlPoints.Count - 1];

            Vector2 p = new Vector2();

            for (int i = 0; i < controlPoints.Count; ++i)
            {
                Vector2 bn = Bernstein(N, i, t) * controlPoints[i];
                p += bn;
            }

            return p;
        }
        public static List<Vector2> PointList2(List<Vector2> controlPoints, float interval = 0.01f)
        {
            int N = controlPoints.Count - 1;
            if (N > 18)
            {
                Debug.Log("You have used more than 18 control points. The maximum control points allowed is 18.");
                controlPoints.RemoveRange(18, controlPoints.Count - 18);
            }

            List<Vector2> points = new List<Vector2>();
            for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
            {
                Vector2 p = new Vector2();
                for (int i = 0; i < controlPoints.Count; ++i)
                {
                    Vector2 bn = Bernstein(N, i, t) * controlPoints[i];
                    p += bn;
                }
                points.Add(p);
            }

            return points;
        }

        public static List<Vector3> PointList3(List<Vector3> controlPoints, float interval = 0.01f)
        {
            int N = controlPoints.Count - 1;
            if (N > 18)
            {
                Debug.Log("You have used more than 18 control points. The maximum control points allowed is 18.");
                controlPoints.RemoveRange(18, controlPoints.Count - 18);
            }

            List<Vector3> points = new List<Vector3>();
            for (float t = 0.0f; t <= 1.0f + interval - 0.0001f; t += interval)
            {
                Vector3 p = new Vector3();
                for (int i = 0; i < controlPoints.Count; ++i)
                {
                    Vector3 bn = Bernstein(N, i, t) * controlPoints[i];
                    p += bn;
                }
                points.Add(p);
            }

            return points;
        }
    }
}