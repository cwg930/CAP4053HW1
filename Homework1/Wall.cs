using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework1
{
	public class Wall : Agent
	{

		public void Initialize(Texture2D texture, Vector2 position)
		{
			AgentTexture = texture;
			Position = position;
			Bounds = new Rectangle (Position.X, Position.Y, Width, Height);
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw (AgentTexture, Position, Color.White);
		}

	}
}

