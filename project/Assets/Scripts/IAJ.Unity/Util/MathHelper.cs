using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Util
{
    public static class MathHelper
    {
        public static Vector3 ConvertOrientationToVector(float orientation)
        {
            return new Vector3((float)Math.Sin(orientation), 0, (float)Math.Cos(orientation));
        }

        public static float ConvertVectorToOrientation(Vector3 vector)
        {
            return Mathf.Atan2(vector.x, vector.z);
        }

        public static Vector3 PerpendicularVector2D(Vector3 vector)
        {
            Vector3 perpendicularVector = new Vector3(vector.z, vector.y, -vector.x);

            perpendicularVector.Normalize();

            return perpendicularVector;
        }

        public static Vector3 Rotate2D(Vector3 vector, float angle)
        {
            var sin = (float)Math.Sin(angle);
            var cos = (float)Math.Cos(angle);

            var x = vector.x*cos - vector.z*sin;
            var z = vector.x*sin + vector.z*cos;
            return new Vector3(x,vector.y,z);
        }

        //method adapted from https://www.scratchapixel.com/lessons/3d-basic-rendering/minimal-ray-tracer-rendering-simple-shapes/ray-sphere-intersection
        //returns a negative value if there is no collision, and the time to the first collision if there is a collision detected
        public static float TimeToCollisionBetweenRayAndCircle(Vector3 position, Vector3 direction, Vector3 circleCenter, float radius)
        {
            // geometric solution
            float sqrRadius = radius * radius;
            Vector3 centerVector = circleCenter - position;
            float tca = Vector3.Dot(centerVector, direction);

            //no collision if the projection is negative, the current ray is moving away from the circle
            if (tca < 0) return -1;

            float sqrLineDistanceToCenter = Vector3.Dot(centerVector, centerVector) - tca * tca;

            //no collision if the distance to center is bigger than the radius of the circle
            if (sqrLineDistanceToCenter > sqrRadius) return -1;

            float thc = Mathf.Sqrt(sqrRadius - sqrLineDistanceToCenter);

            float t0 = tca - thc;
            float t1 = tca + thc;

            //if t0 is bigger, swap with t1
            if (t0 > t1)
            {
                var temp = t0;
                t0 = t1;
                t1 = temp;
            }

            if (t0 < 0)
            {
                t0 = t1; //if t0 is negative, use t1 instead
            }

            return t0;
        }

    }
}
