using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Homework1
{
	/// <summary>
	/// Concrete impelementation of the Rangefinder sensor.  Returns the range to the nearest point
	/// lying on an Agent's bounding box found along a single ray cast by itself.
	/// </summary>
	public class Rangefinder : Sensor
	{
		#region Fields
		private float headingOffset;
		#endregion

		#region Properties
		public float Reading { get; private set; }
		#endregion

		#region Constructors
		public Rangefinder (Agent owner, int range, float headingOffset) : base(owner, range)
		{
			this.headingOffset = headingOffset;
		}
		#endregion

		#region Methods
		public override void Update(List<Agent> agents)
		{
			float currentReading = findMinDistance (agents);
			if (currentReading < range)
			{
				Reading = currentReading;
			} 
			else
			{
				Reading = range;
			}
		}

		private float findMinDistance(List<Agent> agents)
		{
			float heading = (owner.Heading + headingOffset) % 360;
			foreach (Agent agent in agents)
			{
				// Determine intersection and compute distance
			}

			// Placeholder for building.
			return 10.0f;
		}
		#endregion
	}
}

