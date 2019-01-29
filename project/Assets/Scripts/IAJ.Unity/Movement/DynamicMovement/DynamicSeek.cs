namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
    public class DynamicSeek : DynamicMovement
    {
        public override string Name
        {
            get { return "Seek"; }
        }

        public DynamicSeek()
        {
            this.Output = new MovementOutput();
        }

        public override MovementOutput GetMovement()
        {
            this.Output.linear = this.Target.Position - this.Character.Position;

            if (this.Output.linear.sqrMagnitude > 0)
            {
                this.Output.linear.Normalize();
                this.Output.linear *= this.MaxAcceleration;
            }

            return this.Output;
        }
    }
}
