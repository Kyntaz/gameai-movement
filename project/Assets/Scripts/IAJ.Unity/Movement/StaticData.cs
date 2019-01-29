using System;
using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement
{
    public class StaticData
    {
        protected Transform transform;

        public Vector3 Position {
            get
            {
                return this.transform.position;
            }
            set
            {
                this.transform.position = value;
            }
        }

        public float Orientation { get; set; }

        public StaticData()
        {
            var gameObject = new GameObject();
            this.transform = gameObject.transform;
            this.Clear();
        }

        public StaticData(StaticData data)
        {
            this.transform = data.transform;
            this.Orientation = data.Orientation;
        }

        public StaticData(Transform transform)
        {
            this.transform = transform;
            this.Orientation = 0;
        }

        public virtual void Clear()
        {
            this.transform.position = Vector3.zero;
            this.Orientation = 0;
        }

        public static bool operator ==(StaticData l1, StaticData l2)
        {
            return l1.Position == l2.Position && l1.Orientation == l2.Orientation;
        }

        public static bool operator !=(StaticData l1, StaticData l2)
        {
            return l1.Position != l2.Position || l1.Orientation != l2.Orientation;
        }

        public virtual void Integrate(MovementOutput movement, float duration)
        {
            this.transform.position += movement.linear * duration;
            //this.transform.position.x +=  movement.linear.x*duration;
            //this.position.y += movement.linear.y*duration;
            //this.position.z += movement.linear.z*duration;
            this.Orientation += movement.angular*duration;
            this.Orientation = this.Orientation%MathConstants.MATH_2PI;

            this.transform.rotation = Quaternion.AngleAxis(this.Orientation * MathConstants.MATH_180_PI, Vector3.up);
        }

        public void ApplyWorldLimit(float xWorldSize, float zWorldSize)
        {
            Vector3 position = this.transform.position; //creates a new copy of the position (because position is a Property of a Value Type)

            if (position.x < -xWorldSize)
            {
                position.x = xWorldSize;
            }
            else if (position.x > xWorldSize)
            {
                position.x = -xWorldSize;
            }
            if (position.z < -zWorldSize)
            {
                position.z = zWorldSize;
            }
            else if (position.z > zWorldSize)
            {
                position.z = -zWorldSize;
            }

            this.transform.position = position; 
        }
       

        /**
         * Sets the orientation of this position so it points along
         * the given velocity vector.
         */

        public void SetOrientationFromVelocity(Vector3 velocity)
        {
            // If we haven't got any velocity, then we can do nothing.
            if (velocity.sqrMagnitude > 0)
            {
                this.Orientation = MathHelper.ConvertVectorToOrientation(velocity);
                this.transform.rotation = Quaternion.AngleAxis(this.Orientation * MathConstants.MATH_180_PI, Vector3.up);
            }
        }

        /**
         * Returns a unit vector in the direction of the current
         * orientation.
         */

        public Vector3 GetOrientationAsVector()
        {
            return MathHelper.ConvertOrientationToVector(this.Orientation);
        }
    }
}
