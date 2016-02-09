using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Homework1
{
	public class Player : Agent
	{
		#region Fields
		private Vector2 center;
//		private float moveSpeed;
//		private float turnSpeed;
		#endregion

		#region Properties
		public override Rectangle BoundingBox{
			get { return new Rectangle ((int)Position.X - AgentTexture.Width/2, (int)Position.Y - AgentTexture.Height/2, Width, Height); }
		}

		public AdjacentAgentSensor AASensor { get; set;}
		public Rangefinder FrontRangefinder{ get; set; }
		public Rangefinder LeftRangefinder{ get; set; }
		public Rangefinder RightRangefinder{ get; set; }
		#endregion

		#region Methods
		public void Initialize(Texture2D texture, Vector2 position, float heading)
		{
			AgentTexture = texture;
			Position = position;
			Heading = heading;
			center.X = AgentTexture.Width / 2;
			center.Y = AgentTexture.Height / 2;

			FrontRangefinder = new Rangefinder (this, 100, MathHelper.ToRadians(-90));
			LeftRangefinder = new Rangefinder (this, 100, MathHelper.ToRadians (-135));
			RightRangefinder = new Rangefinder (this, 100, MathHelper.ToRadians (-45));
			AASensor = new AdjacentAgentSensor (this, 100.0f);
		}

		/*	More appropriate player movement method
		 * 	Not currently working (doesn't get keypresses)
		 * 
		 */
/*
		public void Update(GameTime gameTime, KeyboardState currentKeyboardState, Wall[] walls, Viewport viewport)
		{
			float velX = (float)(Math.Cos (Heading - MathHelper.PiOver2) * moveSpeed);
			float velY = (float)(Math.Sin (Heading - MathHelper.PiOver2) * moveSpeed);
			if (currentKeyboardState.IsKeyDown (Keys.W)) {
				Position += new Vector2 (velX, velY);
			}
			if (currentKeyboardState.IsKeyDown (Keys.S)) {
				Position -= new Vector2 (velX, velY);
			}
			if (currentKeyboardState.IsKeyDown (Keys.A)) {
				Heading -= turnSpeed;
			}
			if (currentKeyboardState.IsKeyDown (Keys.D)) {
				Heading += turnSpeed;
			}

			// Force heading to wrap around to prevent ambiguous orientation values.
			// For example if we don't wrap heading, if we rotate from 30 degrees one direction
			//   60 degrees and through the 0 angle, we would consider our heading (incorrectly)
			//   to be 30 degrees again.
			if (Heading < 0)
			{
				Heading += MathHelper.ToRadians (360);
			}
			// Clamp player heading between 0 and 360 degrees.
			Heading = (Heading % MathHelper.ToRadians(360));

			foreach (Wall w in walls) {
				if (DetectCollision (w)) {
					if (currentKeyboardState.IsKeyDown(Keys.W)) {
						Position -= new Vector2 (velX, velY);
					} else {
						Position += new Vector2 (velX, velY);
					}
				}
			}

			float clampedX = MathHelper.Clamp (Position.X, Width / 2, viewport.Width - Width / 2);
			float clampedY = MathHelper.Clamp (Position.Y, Height / 2, viewport.Height - Height / 2);
			Position = new Vector2 (clampedX, clampedY);
		}
*/
		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw (AgentTexture, Position, null, Color.White, Heading, center, 1.0f, SpriteEffects.None, 0f);
		}
		#endregion
	}
}

