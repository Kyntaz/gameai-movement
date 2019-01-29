using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.KinematicMovement
{
    public class KinematicArrive : KinematicMovement
    {
        public override string Name
        {
            get { return "Arrive"; }
        }
        public float TimeToTarget { get; set; }
        public float Radius { get; set; }

        public KinematicArrive() 
        {
            this.TimeToTarget = 2.0f;
            this.Radius = 1.0f;
            this.Output = new MovementOutput();
        }
        
        public override MovementOutput GetMovement()
        {
            this.Output.linear = this.Target.Position - this.Character.Position;

            if (this.Output.linear.sqrMagnitude < this.Radius*this.Radius)
            {
                this.Output.linear = Vector3.zero;
            }
            else
            {
                // We'd like to arrive in timeToTarget seconds
                this.Output.linear *= (1.0f/this.TimeToTarget);

                // If that is too fast, then clip the speed
                if (this.Output.linear.sqrMagnitude > this.MaxSpeed*this.MaxSpeed)
                {
                    this.Output.linear.Normalize();
                    this.Output.linear *= this.MaxSpeed;
                }
            }

            return this.Output;
        }
    }
}
