using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Homework1
{
	public class Wall
	{
		public Texture2D WallTexture;
		public Vector2 Position;

		public void Initialize(Texture2D texture, Vector2 position)
		{
			WallTexture = texture;
			Position = position;
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw (WallTexture, Position, Color.White);
		}

	}
}

