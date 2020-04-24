using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HL1BspReader
{
	public class BspViewerGame : Game
	{
		#region Fields

		private BspViewerForm bspViewerForm;

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
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
			this.GraphicsDeviceManager.GraphicsDevice.Clear(Color.AliceBlue);
		}

		#endregion Methods
	}
}
