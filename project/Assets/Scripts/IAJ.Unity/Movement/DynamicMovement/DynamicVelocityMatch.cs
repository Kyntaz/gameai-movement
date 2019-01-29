namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicVelocityMatch : DynamicMovement
    {
        public override string Name
        {
            get { return "VelocityMatch"; }
        }

        public float TimeToDesiredSpeed { get; set; }

        public DynamicVelocityMatch()
        {
            this.TimeToDesiredSpeed = 0.5f;
            this.Output = new MovementOutput();
        }
        public override MovementOutput GetMovement()
        {
           
            this.Output.linear = (this.Target.velocity - this.Character.velocity)/this.TimeToDesiredSpeed;

            if (this.Output.linear.sqrMagnitude > this.MaxAcceleration*this.MaxAcceleration)
            {
                this.Output.linear = this.Output.linear.normalized*this.MaxAcceleration;
            }
            this.Output.angular = 0;
            return this.Output;
        }
    }
}
