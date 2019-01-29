namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicFlee : DynamicSeek
    {
        public override string Name
        {
            get { return "Flee"; }
        }

        public DynamicFlee()
        {
            this.Output = new MovementOutput();
        }

        public override MovementOutput GetMovement()
        {

            this.Output.linear = this.Character.Position - this.Target.Position;

            if (this.Output.linear.sqrMagnitude > 0)
            {
                this.Output.linear.Normalize();
                this.Output.linear *= this.MaxAcceleration;
            }

            return this.Output;
        }
    }
}
