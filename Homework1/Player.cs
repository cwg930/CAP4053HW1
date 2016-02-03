using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework1
{
	public class Player : Agent
	{
		#region Fields
		private Vector2 center;
		#endregion

		#region Properties
		public override Rectangle BoundingBox{
			get { return new Rectangle ((int)Position.X - AgentTexture.Width/2, (int)Position.Y - AgentTexture.Height/2, Width, Height); }
		}
		#endregion

		#region Methods
		public void Initialize(Texture2D texture, Vector2 position, float heading)
		{
			AgentTexture = texture;
			Position = position;
			Heading = heading;
			center.X = AgentTexture.Width / 2;
			center.Y = AgentTexture.Height / 2;
		}

		public void Update()
		{
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (AgentTexture, Position, null, Color.White, Heading, Center, 1.0f, SpriteEffects.None, 0f);
		}
		#endregion
	}
}

