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
	public partial class Form2 : Form
	{
		BoundingBox2D boxA = new BoundingBox2D() { Min = new Vector2(300, 300), Max = new Vector2(500, 400) };
		BoundingBox2D boxB = new BoundingBox2D() { Min = new Vector2(550, 470), Max = new Vector2(650, 500) };
		Vector2 velocityB = new Vector2(-100, -100);

		public Form2()
		{
			this.InitializeComponent();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Vector2 velocity = this.velocityB * (float)this.timeUpDown.Value;

			e.Graphics.FillRectangle(Brushes.AliceBlue, this.boxB.ToRectangleF());
			e.Graphics.FillRectangle(Brushes.IndianRed, this.boxA.ToRectangleF());

			e.Graphics.FillRectangle(Brushes.Black, this.boxB.ToCenteredCircle());
			e.Graphics.FillRectangle(Brushes.Black, this.boxA.ToCenteredCircle());

			if (Helpers.CheckCollideSwept(this.boxA.GetCenter(), this.boxA.GetExtents(), Vector2.Zero,
				this.boxB.GetCenter(), this.boxB.GetExtents(), velocity,
				out float firstCollideTime, out Vector2 normal, out Vector2 edgeDir))
			{
				//this.Text = edgeDir.ToString();
				e.Graphics.FillRectangle(Brushes.AliceBlue, this.boxB.Translate(velocity * firstCollideTime).ToRectangleF());
				e.Graphics.FillRectangle(Brushes.Black, this.boxB.Translate(velocity).ToCenteredCircle());
				if (SAT.DoesPenetrateWithDepth(this.boxA, this.boxB.Translate(velocity), out Vector2 mtv, out float depth))
				{
					e.Graphics.FillRectangle(Brushes.Aqua, this.boxB.Translate(velocity).Translate(mtv * -depth).ToRectangleF());
				}
			}
			else
			{
				//this.Text = "Nope";
				e.Graphics.FillRectangle(Brushes.AliceBlue, this.boxB.Translate(velocity).ToRectangleF());
				e.Graphics.FillRectangle(Brushes.Black, this.boxB.Translate(velocity).ToCenteredCircle());
			}

			if (Helpers.CheckCollideRay(this.boxA.Expand(this.boxB), this.boxB.GetCenter(), velocity, out float firstCollideTimeRay))
			{
				this.Text = $"Hit {firstCollideTimeRay:0.00}";
				e.Graphics.FillRectangle(Brushes.DarkGray, this.boxB.Translate(velocity * firstCollideTimeRay).ToRectangleF());
			}
			else
			{
				this.Text = "Nope";
			}

			e.Graphics.DrawLine(Pens.Black, this.boxB.GetCenter().ToPointF(), this.boxB.Translate(velocity).GetCenter().ToPointF());
		}

		private void timeUpDown_ValueChanged(object sender, EventArgs e)
		{
			this.Refresh();
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			Vector2 clickPosition = new Vector2(e.X, e.Y);
			if (Control.ModifierKeys.HasFlag(Keys.Control))
			{
				this.velocityB = clickPosition - this.boxB.GetCenter();
			}
			else
			{
				this.boxB = new BoundingBox2D() { Min = new Vector2(-50, -15), Max = new Vector2(50, 15) }
					.Translate(clickPosition);
			}
			this.Refresh();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Vector2 clickPosition = new Vector2(e.X, e.Y);
				if (Control.ModifierKeys.HasFlag(Keys.Control))
				{
					this.velocityB = clickPosition - this.boxB.GetCenter();
				}
				else
				{
					this.boxB = new BoundingBox2D() { Min = new Vector2(-50, -15), Max = new Vector2(50, 15) }
						.Translate(clickPosition);
				}
				this.Refresh();
			}
		}
	}
}
