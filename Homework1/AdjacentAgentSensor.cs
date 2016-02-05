using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Homework1
{
	public class AdjacentAgentSensor : Sensor
	{
		#region Properties
		public Dictionary<Agent, Tuple<float, float>> AgentsInRange { get; }
		#endregion

		#region Constructors
		public AdjacentAgentSensor(Agent owner, float range) : base (owner, range){
			AgentsInRange = new Dictionary<Agent, Tuple<float, float>>();
		}
		#endregion

		#region Methods
		//Finds all agents in range of owner then calculates distance and relative heading
		public override void Update (List<Agent> agents)
		{
			AgentsInRange.Clear ();
			foreach (Agent a in agents){
				if (Vector2.Distance (owner.Position, a.Position) <= range) {
					//dot product of 2 unit vectors = cosine of angle between them
					float relativeHeading = (float)Math.Acos(Vector2.Dot(Vector2.Normalize(owner.HeadingVector), Vector2.Normalize(a.HeadingVector)));
					relativeHeading = MathHelper.ToDegrees (relativeHeading);
					AgentsInRange.Add (a, new Tuple<float, float>(Vector2.Distance(owner.Position, a.Position), relativeHeading));
				}
			}
		}
		#endregion
	}
}

