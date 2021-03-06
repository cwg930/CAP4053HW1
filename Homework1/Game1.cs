﻿#region Using Statements
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using System.Text;

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
		Wall[] walls;
		List<Agent> wallList;
		List<Agent> agents;
		LinkedList<String> lines;
		int numWalls = 2;
		KeyboardState currentKeyboardState;
		KeyboardState previousKeyboardState;
		float playerMoveSpeed;
		float playerTurnSpeed;

		//Debug stuff
		Texture2D debugTex;

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
			walls = new Wall[numWalls];
			for(int i = 0; i < numWalls; i++)
				walls[i] = new Wall();
			wallList = new List<Agent> (walls);
			agents = new List<Agent> ();
			StreamReader sr = new StreamReader ("agents.txt");
			lines = new LinkedList<String>();
			while (!sr.EndOfStream) {
				lines.AddLast(sr.ReadLine ());
			}
			for (int i = 0; i < lines.Count; i++) {
				agents.Add (new Player ());
			}
		
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
			for (int i = 0; i < numWalls; i++) {				
				Vector2 wallPosition = new Vector2 (r.Next (0, GraphicsDevice.Viewport.TitleSafeArea.Width), 
					                      r.Next (0, GraphicsDevice.Viewport.TitleSafeArea.Height));
				if (r.Next (1, 10) <= 5)
					walls [i].Initialize (Content.Load<Texture2D>("Graphics/HW1WallHorizontal"), wallPosition);
				else
					walls [i].Initialize (Content.Load<Texture2D>("Graphics/HW1WallVertical"), wallPosition);
			}
			font = Content.Load<SpriteFont> ("Fonts/DebugText");
			//extremely hacky agent adding code, will break easily if agents.txt isn't formatted correctly
			foreach (Player agent in agents) {
				String[] tokens = lines.First.Value.Split (',');
				lines.RemoveFirst ();
				Vector2 agentPosition = new Vector2 (Convert.ToInt32(tokens[0]), Convert.ToInt32(tokens[1]));
				agent.Initialize (Content.Load<Texture2D> ("Graphics/HW1Agent2"), agentPosition, 
					MathHelper.ToRadians ((float)Convert.ToInt32(tokens[2])));
			}

			//debug texture for drawing collison rectangles
			debugTex = new Texture2D(GraphicsDevice, 1, 1);
			debugTex.SetData (new Color[] { Color.White });
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

			//player.Update (gameTime, currentKeyboardState, walls, GraphicsDevice.Viewport);
			UpdatePlayer(gameTime);
			player.AASensor.Update (agents);

			// Update rangefinders
			foreach (Rangefinder r in player.Rangefinders)
				r.Update (wallList);
			// Update pie slice sensors
			foreach (PieSliceSensor p in player.PieSliceSensors)
				p.Update (agents);
			
			base.Update (gameTime);
		}

		/*	Player movement method
		 *  to be replaced by player.Update()
		 * */

		private void UpdatePlayer(GameTime gameTime)
		{
			float velX = (float)(Math.Cos (player.Heading) * playerMoveSpeed);
			float velY = (float)(Math.Sin (player.Heading) * playerMoveSpeed);
			if (currentKeyboardState.IsKeyDown (Keys.W)) {
				player.Position += new Vector2 (velX, velY);
			}
			if (currentKeyboardState.IsKeyDown (Keys.S)) {
				player.Position -= new Vector2 (velX, velY);
			}
			if (currentKeyboardState.IsKeyDown (Keys.A)) {
				player.Heading -= playerTurnSpeed;
			}
			if (currentKeyboardState.IsKeyDown (Keys.D)) {
				player.Heading += playerTurnSpeed;
			}

			// Force heading to wrap around to prevent ambiguous orientation values.
			// For example if we don't wrap heading, if we rotate from 30 degrees one direction
			//   60 degrees and through the 0 angle, we would consider our heading (incorrectly)
			//   to be 30 degrees again.
			if (player.Heading < 0)
			{
				player.Heading += MathHelper.ToRadians (360);
			}
			// Clamp player heading between 0 and 360 degrees.
			player.Heading = (player.Heading % MathHelper.ToRadians(360));
			//simple collision detection for walls only
			foreach (Wall w in walls) {
				if (player.DetectCollision (w)) {
					if (currentKeyboardState.IsKeyDown(Keys.W)) {
						player.Position -= new Vector2 (velX, velY);
					} else {
						player.Position += new Vector2 (velX, velY);
					}
				}
			}

			float clampedX = MathHelper.Clamp (player.Position.X, player.Width / 2, GraphicsDevice.Viewport.Width - player.Width / 2);
			float clampedY = MathHelper.Clamp (player.Position.Y, player.Height / 2, GraphicsDevice.Viewport.Height - player.Height / 2);
			player.Position = new Vector2 (clampedX, clampedY);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Prsovides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			//TODO: Add your drawing code here
			spriteBatch.Begin();
			player.Draw (spriteBatch);
			for (int i = 0; i < numWalls; i++) {
				walls [i].Draw (spriteBatch);
				//debug stuff
				spriteBatch.DrawString (font, "Wall " + i + " loc (X,Y): " + walls [i].Position.ToString ()
					+ "Bounding box: " + walls[i].BoundingBox.ToString(), 
					new Vector2 (0, GraphicsDevice.Viewport.Height - (font.LineSpacing*(walls.Length-i))), Color.Black);
				//spriteBatch.Draw (debugTex, walls [i].BoundingBox, Color.White);
			}
			foreach (Player p in agents) {
				p.Draw (spriteBatch);
			}
			spriteBatch.DrawString (font, "Heading (deg): " + (MathHelper.ToDegrees (player.Heading) % 360) 
				+ "\nPosition (x,y): "+ player.Position.ToString(), new Vector2 (0, 0), Color.Black);
			int j = 2;
			foreach (KeyValuePair<Agent, Tuple<float, float>> agent in player.AASensor.AgentsInRange) {
				spriteBatch.DrawString (font, "Agent: " + agent.Key.ToString() + " Distance: " + agent.Value.Item1
				+ " Rel. Heading: " + agent.Value.Item2, new Vector2 (0, font.LineSpacing * j), Color.Black);
				j++;
			}

			// Draw strings indicating Pie slice activation levels
			spriteBatch.DrawString (font, "Activation Levels: ", new Vector2 (0, font.LineSpacing * j), Color.Black);
			j++;

			int pieSliceNumber = 1;
			string pieSliceString = "";
			foreach (PieSliceSensor p in player.PieSliceSensors)
			{
				pieSliceString += "" + pieSliceNumber + ": " + p.ActivationLevel + "\n";
				pieSliceNumber++;
			}
			spriteBatch.DrawString (font, pieSliceString , new Vector2 (0, font.LineSpacing * j), Color.Black);
			j++;

			// Draw for middle, left, right rangefinders.
			// Really hacky and stupid but it's fine for this.
			spriteBatch.DrawString (font, "Left Rangefinder: " + player.Rangefinders[1].Reading , new Vector2 (0, font.LineSpacing * (j+player.PieSliceSensors.Count)), Color.Black);
			j++;
			spriteBatch.DrawString (font, "Center Rangefinder: " + player.Rangefinders[0].Reading , new Vector2 (0, font.LineSpacing * (j+player.PieSliceSensors.Count)), Color.Black);
			j++;
			spriteBatch.DrawString (font, "Right Rangefinder: " + player.Rangefinders[2].Reading , new Vector2 (0, font.LineSpacing * (j+player.PieSliceSensors.Count)), Color.Black);

			// Use a lowercase "o" as a marker for the end of the rangefinder
			Vector2 markerSize = font.MeasureString ("o");
			foreach (Rangefinder r in player.Rangefinders)
			{
				spriteBatch.DrawString (font, "o", new Vector2 (r.FoundPoint.X - markerSize.X / 2, r.FoundPoint.Y - markerSize.Y / 2), Color.Red);
			}

			// Draw labels on pie slice sensor detections.
			foreach (PieSliceSensor p in player.PieSliceSensors)
			{
				foreach (Agent a in p.DetectedAgents)
				{
					markerSize = font.MeasureString (p.Marker);
					spriteBatch.DrawString (font, p.Marker, a.Position, Color.Green);
				}
			}
				
			//more debug
			//spriteBatch.Draw(debugTex, player.BoundingBox, Color.White);
			spriteBatch.End ();

			base.Draw (gameTime);
		}			
	}
}

