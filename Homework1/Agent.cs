using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework1
{
	public abstract class Agent
	{
		public Texture2D AgentTexture;
		public Vector2 Position;
		public abstract Rectangle BoundingBox {
			get; 
		}
		public int Width {
			get { return AgentTexture.Width; }
		}
		public int Height {
			get { return AgentTexture.Height; }
		}

		public bool DetectCollision(Agent target){
			if (this.BoundingBox.Intersects (target.BoundingBox)) {
				return true;
			}
			return false;
		}

	}
}

