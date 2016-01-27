using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework1
{
	public class Player : Agent
	{
		public float Heading;

		private Vector2 Center;

		public void Initialize(Texture2D texture, Vector2 position, float heading)
		{
			AgentTexture = texture;
			Position = position;
			Heading = heading;
			Center.X = AgentTexture.Width / 2;
			Center.Y = AgentTexture.Height / 2;
			Bounds = new Rectangle (Position.X, Position.Y, Width, Height);
		}

		public void Update()
		{
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (AgentTexture, Position, null, Color.White, Heading, Center, 1.0f, SpriteEffects.None, 0f);
		}
			
	}
}

