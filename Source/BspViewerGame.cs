using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HL1BspReader
{
	public class BspViewerGame : Game
	{
		#region Fields

		private BspViewerForm bspViewerForm;

		private readonly float cameraFastSpeed = 10.0f;
		private readonly float cameraSlowSpeed = 4.0f;
		private readonly float mouseLookScale = 0.17f;

		private Vector3 cameraPosition;
		private Vector3 cameraRotation;
		private Matrix cameraProjection;
		private Matrix cameraView;

		#endregion Fields

		#region Constructors

		public BspViewerGame()
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

			this.bspViewerForm = new BspViewerForm();
			this.bspViewerForm.DockGameWindow(this);
			this.bspViewerForm.Show();
		}

		protected override void Update(GameTime gameTime)
		{
			KeyboardState keyboardState = Keyboard.GetState();
			MouseState mouseState = Mouse.GetState();

			if (mouseState.RightButton == ButtonState.Pressed)
			{
				this.updateCameraLook(mouseState);
				this.updateMovement(keyboardState, mouseState);
				this.updateView();
			}

			this.bspViewerForm.Text = this.cameraPosition.ToString();

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDeviceManager.GraphicsDevice.Clear(Color.AliceBlue);

			base.Draw(gameTime);
		}

		private void updateCameraLook(MouseState mouseState)
		{
			int midX = GraphicsDevice.Viewport.Width / 2;
			int midY = GraphicsDevice.Viewport.Height / 2;

			float deltaX = MathHelper.ToRadians((midX - mouseState.X) * this.mouseLookScale);
			float deltaY = MathHelper.ToRadians(-(midY - mouseState.Y) * this.mouseLookScale);

			Vector3 vectorRotation = this.cameraRotation + new Vector3(deltaX, deltaY, 0);
			if (vectorRotation.Y > MathHelper.PiOver2 - 0.01f)
			{
				vectorRotation.Y = MathHelper.PiOver2 - 0.01f;
			}
			else if (vectorRotation.Y < -MathHelper.PiOver2 + 0.01f)
			{
				vectorRotation.Y = -MathHelper.PiOver2 + 0.01f;
			}

			this.cameraRotation = vectorRotation;
			Mouse.SetPosition(midX, midY);
		}

		private void updateMovement(KeyboardState keyboardState, MouseState mouseState)
		{
			Quaternion rotation = Quaternion.CreateFromYawPitchRoll(this.cameraRotation.X, this.cameraRotation.Y, this.cameraRotation.Z);
			float speed = (mouseState.RightButton == ButtonState.Pressed) ? this.cameraSlowSpeed : this.cameraFastSpeed;
			Vector3 vectorMovement = Vector3.Zero;

			if (keyboardState.IsKeyDown(Keys.W))
			{
				vectorMovement.Z = 1;
			}
			else if (keyboardState.IsKeyDown(Keys.S))
			{
				vectorMovement.Z = -1;
			}

			if (keyboardState.IsKeyDown(Keys.A))
			{
				vectorMovement.X = 1;
			}
			else if (keyboardState.IsKeyDown(Keys.D))
			{
				vectorMovement.X = -1;
			}

			if (vectorMovement != Vector3.Zero)
			{
				vectorMovement.Normalize();
				vectorMovement = Vector3.Transform(vectorMovement * speed, rotation);
				this.cameraPosition = this.cameraPosition + vectorMovement;
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
