using System;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement
{
    public class MovementOutput
    {
        public Vector3 linear;
        public float angular;

        public void Clear()
        {
            this.linear = Vector3.zero;
            this.angular = 0;
        }

        public float SquareMagnitude()
        {
            return this.linear.sqrMagnitude + this.angular * this.angular;
        }


        public float Magnitude()
        {
            return (float)Math.Sqrt(this.SquareMagnitude());
        }
    }
}

