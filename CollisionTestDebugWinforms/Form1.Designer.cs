namespace CollisionTest
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.timeUpDown = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).BeginInit();
			this.SuspendLayout();
			// 
			// timeUpDown
			// 
			this.timeUpDown.DecimalPlaces = 3;
			this.timeUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
			this.timeUpDown.Location = new System.Drawing.Point(12, 12);
			this.timeUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.timeUpDown.Name = "timeUpDown";
			this.timeUpDown.Size = new System.Drawing.Size(240, 39);
			this.timeUpDown.TabIndex = 0;
			this.timeUpDown.ValueChanged += new System.EventHandler(this.timeUpDown_ValueChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.LightGray;
			this.ClientSize = new System.Drawing.Size(800, 800);
			this.Controls.Add(this.timeUpDown);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.timeUpDown)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NumericUpDown timeUpDown;
	}
}

