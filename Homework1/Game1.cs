#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Homework1
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		SpriteFont font;

		Player player;
		Wall wall1;
		Wall wall2;
		KeyboardState currentKeyboardState;
		KeyboardState previousKeyboardState;

		float playerMoveSpeed;
		float playerTurnSpeed;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = true;		
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			player = new Player();
			playerMoveSpeed = 8.0f;
			playerTurnSpeed = MathHelper.ToRadians (5.0f); 
			wall1 = new Wall ();
			wall2 = new Wall ();
			base.Initialize ();
				
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);
			//TODO: use this.Content to load your game content here 
			Vector2 playerPosition = new Vector2 (GraphicsDevice.Viewport.TitleSafeArea.X, 
				GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
	
			player.Initialize (Content.Load<Texture2D> ("Graphics/HW1Player"), playerPosition, 0.0f);
			Random r = new Random ();
			Vector2 wallPosition = new Vector2 (r.Next (0, GraphicsDevice.Viewport.TitleSafeArea.Width), 
				                       r.Next (0, GraphicsDevice.Viewport.TitleSafeArea.Height)); 

			wall1.Initialize (Content.Load<Texture2D> ("Graphics/HW1WallHorizontal"), wallPosition);

			wallPosition = new Vector2 (r.Next (0, GraphicsDevice.Viewport.TitleSafeArea.Width), 
				r.Next (0, GraphicsDevice.Viewport.TitleSafeArea.Height)); 

			wall2.Initialize (Content.Load<Texture2D> ("Graphics/HW1WallVertical"), wallPosition);

			font = Content.Load<SpriteFont> ("Fonts/DebugText");

		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			#endif
			// TODO: Add your update logic here		
			previousKeyboardState = currentKeyboardState;
			currentKeyboardState = Keyboard.GetState ();
			UpdatePlayer (gameTime);

			base.Update (gameTime);
		}

		private void UpdatePlayer(GameTime gameTime)
		{
			if (currentKeyboardState.IsKeyDown (Keys.W)) {
				player.Position.X += (float)(Math.Cos (player.Heading - MathHelper.PiOver2) * playerMoveSpeed);
				player.Position.Y += (float)(Math.Sin (player.Heading - MathHelper.PiOver2) * playerMoveSpeed);
			}
			if (currentKeyboardState.IsKeyDown (Keys.S)) {
				player.Position.X -= (float)(Math.Cos (player.Heading - MathHelper.PiOver2) * playerMoveSpeed);
				player.Position.Y -= (float)(Math.Sin (player.Heading - MathHelper.PiOver2) * playerMoveSpeed);
			}
			if (currentKeyboardState.IsKeyDown (Keys.A)) {
				player.Heading -= playerTurnSpeed;
			}
			if (currentKeyboardState.IsKeyDown (Keys.D)) {
				player.Heading += playerTurnSpeed;
			}
				
			player.Position.X = MathHelper.Clamp (player.Position.X, player.Width / 2, GraphicsDevice.Viewport.Width - player.Width / 2);
			player.Position.Y = MathHelper.Clamp (player.Position.Y, player.Height / 2, GraphicsDevice.Viewport.Height - player.Height / 2);

		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			//TODO: Add your drawing code here
			spriteBatch.Begin();
			player.Draw (spriteBatch);
			wall1.Draw (spriteBatch);
			wall2.Draw (spriteBatch);
			spriteBatch.DrawString (font, "Heading (deg): " + (MathHelper.ToDegrees (player.Heading) % 360), new Vector2 (0, 0), Color.Black);
			spriteBatch.End ();

			base.Draw (gameTime);
		}
			
	}
}

