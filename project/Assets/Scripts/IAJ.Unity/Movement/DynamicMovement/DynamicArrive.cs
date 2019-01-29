using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.DynamicMovement
{

	public class DynamicArrive : DynamicVelocityMatch
	{
		public float MaxSpeed;
		public float StopRadius;
		public float SlowRadius;
		public KinematicData DestinationTarget;

		public GameObject DebugTarget { get; set; }

		public DynamicArrive()
		{
			this.Target = new KinematicData();
			this.MaxSpeed = 20f;
			this.StopRadius = 1f;
			this.SlowRadius = 30f;
		}

		public override string Name
		{
			get { return "Arrive"; }
		}


		public override MovementOutput GetMovement()
		{
			Vector3 direction = this.DestinationTarget.Position - this.Character.Position;
			float distance = direction.magnitude;
			float desiredSpeed;

			if (distance < StopRadius)
				desiredSpeed = 0;
			else if (distance > SlowRadius)
				desiredSpeed = MaxSpeed;
			else
				desiredSpeed = MaxSpeed * (distance / SlowRadius);

			base.Target.velocity = direction.normalized * desiredSpeed;
				
			return base.GetMovement();
		}
	}
}
