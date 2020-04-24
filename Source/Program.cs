using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL1BspReader
{
	public static class Program
	{
		#region Methods

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			using (BspViewerGame bspViewerGame = new BspViewerGame())
			{
				bspViewerGame.Run();
			}
		}

		#endregion Methods
	}
}
