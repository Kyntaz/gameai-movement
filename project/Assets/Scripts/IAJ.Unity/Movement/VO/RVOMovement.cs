//class adapted from the HRVO library http://gamma.cs.unc.edu/HRVO/
//adapted to IAJ classes by João Dias

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.IAJ.Unity.Movement.DynamicMovement;
using Assets.Scripts.IAJ.Unity.Util;
using UnityEngine;

namespace Assets.Scripts.IAJ.Unity.Movement.VO
{
    public class RVOMovement : DynamicMovement.DynamicVelocityMatch
    {
        public override string Name
        {
            get { return "RVO"; }
        }

        protected List<KinematicData> Characters { get; set; }
        protected List<StaticData> Obstacles { get; set; }
        public float CharacterSize { get; set; }
        public float IgnoreDistance { get; set; }
        public float MaxSpeed { get; set; }
        //create additional properties if necessary
		public int NumSamples;
		public float Weight;
		public float ObstacleWeight;
		public Vector3 LastSample;
		public float ObstacleSize;
		public float GoodEnoughPenalty;

        protected DynamicMovement.DynamicMovement DesiredMovement { get; set; }

        public RVOMovement(DynamicMovement.DynamicMovement goalMovement, List<KinematicData> movingCharacters, List<StaticData> obstacles)
        {
            this.DesiredMovement = goalMovement;
            this.Characters = movingCharacters;
            this.Obstacles = obstacles;
            base.Target = new KinematicData();

            //initialize other properties if you think is relevant
			this.Weight = 6.0f;
			this.ObstacleWeight = 7.0f;
			this.CharacterSize = 0.5f;
			this.ObstacleSize = 1.6f;
			this.GoodEnoughPenalty = 0.0f;

			this.LastSample = Vector3.zero;
        }

        public override MovementOutput GetMovement()
        {
			// 1) calculate desired velocity
			MovementOutput desiredOutput = this.DesiredMovement.GetMovement();
			Vector3 desiredVelocity = this.Character.velocity + desiredOutput.linear;
			if (desiredVelocity.magnitude > this.MaxSpeed) {
				desiredVelocity.Normalize();
				desiredVelocity *= this.MaxSpeed;
			}

			// 2) generate samples
			List<Vector3> samples = new List<Vector3>();
			samples.Add(desiredVelocity);
			samples.Add(this.LastSample);
			for (int i = 0; i < NumSamples; i++) {
				float angle = Random.Range(0, Util.MathConstants.MATH_2PI);
				float magnitude = Random.Range(MaxSpeed / 2, MaxSpeed);
				Vector3 velocitySample = Util.MathHelper.ConvertOrientationToVector(angle) * magnitude;
				samples.Add(velocitySample);
			}

			// 3) evaluate and get best sample
			base.Target.velocity = getBestSample(samples);
			//Debug.Log(base.Target.velocity);

			return base.GetMovement();
        }

		private Vector3 getBestSample(List<Vector3> samples) {
			Vector3 bestSample = Vector3.zero;
			Vector3 desiredVelocity = samples[0];
			float minimumPenalty = float.MaxValue;

			foreach (Vector3 sample in samples) {
				float distancePenalty = (desiredVelocity - sample).magnitude;
				float maximumTimePenalty = 0;

				foreach (KinematicData b in this.Characters) {
					if (b == this.Character) continue;

					Vector3 deltaP = b.Position - this.Character.Position;
					if (deltaP.magnitude > IgnoreDistance) continue;

					Vector3 rayVector = 2 * sample - this.Character.velocity - b.velocity;
					float tc = Util.MathHelper.TimeToCollisionBetweenRayAndCircle(this.Character.Position, rayVector, b.Position, this.CharacterSize * 2);
					float timePenalty;

					if (tc > 0)
						timePenalty = this.Weight / tc;
					else if (tc == 0)
						timePenalty = float.MaxValue;
					else
						timePenalty = 0f;

					if (timePenalty > maximumTimePenalty) maximumTimePenalty = timePenalty; // TODO: Optimize?
				}

				foreach (StaticData b in this.Obstacles) {
					if (b == this.Character) continue;

					Vector3 deltaP = b.Position - this.Character.Position;
					//if (deltaP.magnitude > IgnoreDistance) continue;

					Vector3 rayVector = sample - this.Character.velocity;
					float tc = Util.MathHelper.TimeToCollisionBetweenRayAndCircle(this.Character.Position, rayVector, b.Position, this.ObstacleSize * 2);
					float timePenalty;

					if (tc > 0)
						timePenalty = this.ObstacleWeight / tc;
					else if (tc == 0) {
						maximumTimePenalty = float.MaxValue;
						break;
					}
					else
						timePenalty = 0f;

					if (timePenalty > maximumTimePenalty) maximumTimePenalty = timePenalty; // TODO: Optimize?
				}

				float penalty = distancePenalty + maximumTimePenalty;

				if (penalty < minimumPenalty) {
					minimumPenalty = penalty;
					bestSample = sample;
					if (minimumPenalty <= this.GoodEnoughPenalty) break;
				} // TODO: Optimize?
			}

			this.LastSample = bestSample;
			return bestSample;
		}
    }
}
