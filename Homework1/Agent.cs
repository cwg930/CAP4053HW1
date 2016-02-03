using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework1
{
	public abstract class Agent
	{
		#region Properties
		public float Heading { get; set; }
		public Texture2D AgentTexture { get; set; }
		public Vector2 Position { get; set; }
		public abstract Rectangle BoundingBox {
			get; 
		}
		public int Width {
			get { return AgentTexture.Width; }
		}
		public int Height {
			get { return AgentTexture.Height; }
		}
		#endregion

		#region Methods
		public bool DetectCollision(Agent target){
			if (this.BoundingBox.Intersects (target.BoundingBox)) {
				return true;
			}
			return false;
		}
		#endregion

	}
}

