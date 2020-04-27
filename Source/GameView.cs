﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HL1BspReader
{
	public class GameView : Game
	{
		#region Fields

		private MainForm parentForm;

		private readonly float cameraFastSpeed = 10.0f;
		private readonly float cameraSlowSpeed = 4.0f;
		private readonly float mouseLookScale = 0.17f;

		private KeyboardState currentKeyboardState;
		private KeyboardState previousKeyboardState;
		private MouseState currentMouseState;
		private MouseState previousMouseState;
		private Point mouseDownPoint;
		private bool isDragging;
		private Vector3 cameraPosition = new Vector3(0, 0, 0);
		private Vector3 cameraRotation = new Vector3(MathHelper.PiOver4, -0.616F, 0);
		private Matrix cameraView;

		private BasicEffect basicEffect;

		#endregion Fields

		#region Constructors

		public GameView()
		{
			this.GraphicsDeviceManager = new GraphicsDeviceManager(this);
			this.GraphicsDeviceManager.GraphicsProfile = GraphicsProfile.HiDef;
			this.Window.AllowUserResizing = true;
			this.IsMouseVisible = true;
		}

		#endregion Constructors

		#region Properties

		public GraphicsDeviceManager GraphicsDeviceManager { get; }

		#endregion Properties

		#region Methods

		protected override void LoadContent()
		{
			base.LoadContent();

			this.parentForm = new MainForm();
			this.parentForm.DockGameWindow(this);
			this.parentForm.Show();

			this.basicEffect = new BasicEffect(this.GraphicsDevice);
			this.basicEffect.LightingEnabled = true;
			this.basicEffect.TextureEnabled = false;
			this.basicEffect.EnableDefaultLighting();
		}

		protected override void Update(GameTime gameTime)
		{
			this.currentKeyboardState = Keyboard.GetState();
			this.currentMouseState = Mouse.GetState();

			if (this.currentMouseState.RightButton == ButtonState.Pressed && this.previousMouseState.RightButton == ButtonState.Released)
			{
				this.isDragging = true;
				this.mouseDownPoint = new Point(this.currentMouseState.X, this.currentMouseState.Y);
			}
			else if (this.currentMouseState.RightButton == ButtonState.Released)
			{
				this.isDragging = false;
			}

			if (this.isDragging)
			{
				this.updateCameraLook();
				this.updateMovement();
				this.updateView();
			}

			this.previousKeyboardState = this.currentKeyboardState;
			this.previousMouseState = this.currentMouseState;

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDeviceManager.GraphicsDevice.Clear(Color.Black);

			this.basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90.0f), this.GraphicsDevice.Viewport.AspectRatio, 0.1f, 10000.0f);
			this.basicEffect.View = this.cameraView;
			this.basicEffect.World = Matrix.Identity;

			ShapeRenderHelper.RenderBox(this.GraphicsDevice, this.basicEffect, new Vector3(0, 36, 0), new Vector3(16, 36, 16), Quaternion.Identity);
			BspRender.RenderBspModel(this.GraphicsDevice, this.basicEffect, this.parentForm.Bsp.Models[0]);

			{
				this.GraphicsDevice.BlendState = BlendState.AlphaBlend;
				this.basicEffect.LightingEnabled = false;
				this.basicEffect.Alpha = 0.5f;

				Plane selectedPlane = new Plane(new Vector3(1, 1, 1), 72);
				this.basicEffect.DiffuseColor = Color.Blue.ToVector3();
				ShapeRenderHelper.RenderPlane(this.GraphicsDevice, this.basicEffect, selectedPlane);
				this.basicEffect.DiffuseColor = Color.Orchid.ToVector3();
				ShapeRenderHelper.RenderPlane(this.GraphicsDevice, this.basicEffect, new Plane(-selectedPlane.Normal, -selectedPlane.D));

				this.GraphicsDevice.BlendState = BlendState.Opaque;
				this.basicEffect.LightingEnabled = true;
				this.basicEffect.DiffuseColor = Color.White.ToVector3();
				this.basicEffect.Alpha = 1.0f;
			}
			{
				this.basicEffect.DiffuseColor = Color.Black.ToVector3();
				this.basicEffect.LightingEnabled = false;
				this.GraphicsDevice.RasterizerState = new RasterizerState() { FillMode = FillMode.WireFrame };
				this.basicEffect.World = Matrix.Identity;
				this.basicEffect.CurrentTechnique.Passes[0].Apply();
				this.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, new VertexPositionColor[]
				{
					new VertexPositionColor(Vector3.Zero, Color.Black),
					new VertexPositionColor(new Vector3(1, 1, 1) * 72, Color.Black),
				}, 0, 1);
				this.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
				this.basicEffect.LightingEnabled = true;
				this.basicEffect.DiffuseColor = Color.White.ToVector3();
			}


			base.Draw(gameTime);
		}

		private void updateCameraLook()
		{
			float deltaX = MathHelper.ToRadians((this.mouseDownPoint.X - this.currentMouseState.X) * this.mouseLookScale);
			float deltaY = MathHelper.ToRadians((this.mouseDownPoint.Y - this.currentMouseState.Y) * this.mouseLookScale);

			Vector3 vectorRotation = this.cameraRotation + new Vector3(deltaX, -deltaY, 0);
			if (vectorRotation.Y > MathHelper.PiOver2 - 0.01f)
			{
				vectorRotation.Y = MathHelper.PiOver2 - 0.01f;
			}
			else if (vectorRotation.Y < -MathHelper.PiOver2 + 0.01f)
			{
				vectorRotation.Y = -MathHelper.PiOver2 + 0.01f;
			}

			this.cameraRotation = vectorRotation;
			Mouse.SetPosition(this.mouseDownPoint.X, this.mouseDownPoint.Y);
		}

		private bool moved = false;
		private void updateMovement()
		{
			//Quaternion rotation = Quaternion.CreateFromYawPitchRoll(this.cameraRotation.X, this.cameraRotation.Y, this.cameraRotation.Z);
			Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, this.cameraRotation.X)
				* Quaternion.CreateFromAxisAngle(Vector3.UnitX, this.cameraRotation.Y);
			Vector3 vectorMovement = Vector3.Zero;

			if (this.currentKeyboardState.IsKeyDown(Keys.W))
			{
				vectorMovement.Z = 1;
			}
			if (this.currentKeyboardState.IsKeyDown(Keys.S))
			{
				vectorMovement.Z = -1;
			}

			if (this.currentKeyboardState.IsKeyDown(Keys.A))
			{
				vectorMovement.X = 1;
			}
			if (this.currentKeyboardState.IsKeyDown(Keys.D))
			{
				vectorMovement.X = -1;
			}

			if (vectorMovement != Vector3.Zero)
			{

				if (moved && !this.currentKeyboardState.IsKeyDown(Keys.X)) { return; }
				moved = true;
				vectorMovement.Normalize();
				float cameraSpeed = this.currentKeyboardState.IsKeyDown(Keys.LeftShift) ? this.cameraFastSpeed : this.cameraSlowSpeed;
				if (!this.currentKeyboardState.IsKeyDown(Keys.X)) { cameraSpeed = 124.70765f; }
				vectorMovement = Vector3.Transform(vectorMovement, rotation) * cameraSpeed;
				this.cameraPosition += vectorMovement;
			}
		}

		private static Vector3 projectVectorToPlane(Vector3 vector, Plane plane)
		{
			return vector - Vector3.Dot(vector, plane.Normal) * plane.Normal;
		}

		private void updateView()
		{
			Vector3 eyePosition = this.cameraPosition;
			Matrix rotation = Matrix.CreateFromYawPitchRoll(this.cameraRotation.X, this.cameraRotation.Y, this.cameraRotation.Z);
			Vector3 target = eyePosition + Vector3.Transform(Vector3.Backward, rotation);
			this.cameraView = Matrix.CreateLookAt(eyePosition, target, Vector3.Up);
		}

		#endregion Methods
	}
}
