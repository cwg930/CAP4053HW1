using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework1
{
	public class Player
	{
		public Texture2D PlayerTexture;
		public Vector2 Position;
		public float Heading;
		public int Width 
		{
			get { return PlayerTexture.Width; }
		}

		public int Height
		{
			get { return PlayerTexture.Height; }
		}

		private Vector2 Center;

		public void Initialize(Texture2D texture, Vector2 position, float heading)
		{
			PlayerTexture = texture;
			Position = position;
			Heading = heading;
			Center.X = PlayerTexture.Width / 2;
			Center.Y = PlayerTexture.Height / 2;
		}

		public void Update()
		{
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (PlayerTexture, Position, null, Color.White, Heading, Center, 1.0f, SpriteEffects.None, 0f);
		}
	}
}

