using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework1
{
	public abstract class Agent
	{
		public Texture2D AgentTexture;
		public Vector2 Position;
		public Rectangle Bounds;

		public int Width {
			get { return AgentTexture.Width; }
		}
		public int Height {
			get { return AgentTexture.Height; }
		}

	}
}

