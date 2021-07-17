using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CollisionTest
{
	public partial class Form1 : Form
	{
		BoundingBox2D boxA = new BoundingBox2D() { Min = new Vector2(300, 300), Max = new Vector2(500, 400) };
		BoundingBox2D boxB = new BoundingBox2D() { Min = new Vector2(550, 470), Max = new Vector2(650, 500) };
		Vector2 velocityB_side = new Vector2(-100, -150);
		Vector2 velocityB_bottom = new Vector2(-120, -80);

		public Form1()
		{
			this.InitializeComponent();
		}

		protected override void OnShown(EventArgs e)
		{
			new Form2().Show();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Vector2 corner2corner = this.boxA.Max - this.boxB.Min;

			//Vector2 velocity = this.velocityB_side * (float)this.timeUpDown.Value;
			//Vector2 velocity = this.velocityB_bottom * (float)this.timeUpDown.Value;
			Vector2 velocity = corner2corner * (float)this.timeUpDown.Value;
			//Vector2 velocity = (this.boxA.Max - this.boxB.GetCenter()) * (float)this.timeUpDown.Value;
			//Vector2 velocity = (this.boxA.GetCenter() - this.boxB.GetCenter()) * (float)this.timeUpDown.Value;

			float dprime = Vector2.Dot(corner2corner, corner2corner);
			float dside = Vector2.Dot(corner2corner, velocityB_side);
			float dbottom = Vector2.Dot(corner2corner, velocityB_bottom);

			e.Graphics.FillRectangle(Brushes.AliceBlue, this.boxB.ToRectangleF());
			e.Graphics.FillRectangle(Brushes.AliceBlue, this.boxB.Translate(velocity).ToRectangleF());
			e.Graphics.FillRectangle(Brushes.IndianRed, this.boxA.ToRectangleF());

			e.Graphics.FillRectangle(Brushes.Black, this.boxB.Translate(velocity).ToCenteredCircle());
			e.Graphics.FillRectangle(Brushes.Black, this.boxB.ToCenteredCircle());
			e.Graphics.FillRectangle(Brushes.Black, this.boxA.ToCenteredCircle());

			e.Graphics.DrawLine(Pens.Gray, this.boxA.Max.ToPointF(), this.boxB.Min.ToPointF());
			e.Graphics.DrawLine(Pens.Black, this.boxB.GetCenter().ToPointF(), this.boxB.Translate(velocity).GetCenter().ToPointF());
		}

		private void timeUpDown_ValueChanged(object sender, EventArgs e)
		{
			this.Refresh();
		}
	}

	public struct BoundingBox2D
	{
		public Vector2 Min;
		public Vector2 Max;
	}
}
