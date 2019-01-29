using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{
	public class DynamicAvoidCharacter : DynamicMovement
	{
		public float AvoidMargin;
		public float MaxTimeLookAhead;

		public override string Name
		{
			get { return "Avoid Character"; }
		}

		public DynamicAvoidCharacter(KinematicData target)
		{
			this.Target = target;
		}

		public override MovementOutput GetMovement()
		{
			Vector3 deltaPos = this.Target.Position - this.Character.Position;
			Vector3 deltaVel = this.Target.velocity - this.Character.velocity;
			float deltaSqrSpeed = deltaVel.sqrMagnitude;
			MovementOutput output = new MovementOutput();
			output.Clear();

			if (deltaSqrSpeed == 0) return output;

			float timeToClosest = Mathf.Abs(- Vector3.Dot(deltaPos, deltaVel) / deltaSqrSpeed);
			if (timeToClosest > this.MaxTimeLookAhead) return output;

			Vector3 futureDeltaPos = deltaPos + deltaVel * timeToClosest;
			float futureDistance = futureDeltaPos.magnitude;

			if (futureDistance > 2 * AvoidMargin) return output;

			if (futureDistance <= 0 || deltaPos.magnitude < 2 * AvoidMargin)
				output.linear = Character.Position - Target.Position;
			else
				output.linear = -futureDeltaPos;

			output.linear = output.linear.normalized * this.MaxAcceleration;
			return output;
		}
	}
}
