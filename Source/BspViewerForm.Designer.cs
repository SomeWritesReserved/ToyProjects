namespace HL1BspReader
{
	partial class BspViewerForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
			this.leftTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.hullsComboBox = new System.Windows.Forms.ComboBox();
			this.clipnodesLabel = new System.Windows.Forms.Label();
			this.modelLabel = new System.Windows.Forms.Label();
			this.modelComboBox = new System.Windows.Forms.ComboBox();
			this.bspTreeLabel = new System.Windows.Forms.Label();
			this.bspTreeView = new System.Windows.Forms.TreeView();
			this.clipnodesTreeView = new System.Windows.Forms.TreeView();
			((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
			this.mainSplitContainer.Panel1.SuspendLayout();
			this.mainSplitContainer.SuspendLayout();
			this.leftTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainSplitContainer
			// 
			this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
			this.mainSplitContainer.Name = "mainSplitContainer";
			// 
			// mainSplitContainer.Panel1
			// 
			this.mainSplitContainer.Panel1.Controls.Add(this.leftTableLayoutPanel);
			this.mainSplitContainer.Size = new System.Drawing.Size(963, 604);
			this.mainSplitContainer.SplitterDistance = 258;
			this.mainSplitContainer.TabIndex = 0;
			// 
			// leftTableLayoutPanel
			// 
			this.leftTableLayoutPanel.ColumnCount = 2;
			this.leftTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.leftTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.leftTableLayoutPanel.Controls.Add(this.hullsComboBox, 1, 3);
			this.leftTableLayoutPanel.Controls.Add(this.clipnodesLabel, 0, 3);
			this.leftTableLayoutPanel.Controls.Add(this.modelLabel, 0, 0);
			this.leftTableLayoutPanel.Controls.Add(this.modelComboBox, 1, 0);
			this.leftTableLayoutPanel.Controls.Add(this.bspTreeLabel, 0, 1);
			this.leftTableLayoutPanel.Controls.Add(this.bspTreeView, 0, 2);
			this.leftTableLayoutPanel.Controls.Add(this.clipnodesTreeView, 0, 4);
			this.leftTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.leftTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.leftTableLayoutPanel.Name = "leftTableLayoutPanel";
			this.leftTableLayoutPanel.RowCount = 5;
			this.leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.leftTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.leftTableLayoutPanel.Size = new System.Drawing.Size(258, 604);
			this.leftTableLayoutPanel.TabIndex = 0;
			// 
			// hullsComboBox
			// 
			this.hullsComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.hullsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.hullsComboBox.FormattingEnabled = true;
			this.hullsComboBox.Items.AddRange(new object[] {
            "Hull 0",
            "Hull 1",
            "Hull 2",
            "Hull 3"});
			this.hullsComboBox.Location = new System.Drawing.Point(65, 311);
			this.hullsComboBox.Name = "hullsComboBox";
			this.hullsComboBox.Size = new System.Drawing.Size(190, 21);
			this.hullsComboBox.TabIndex = 6;
			this.hullsComboBox.SelectedIndexChanged += new System.EventHandler(this.hullsComboBox_SelectedIndexChanged);
			// 
			// clipnodesLabel
			// 
			this.clipnodesLabel.AutoSize = true;
			this.clipnodesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.clipnodesLabel.Location = new System.Drawing.Point(3, 308);
			this.clipnodesLabel.Name = "clipnodesLabel";
			this.clipnodesLabel.Size = new System.Drawing.Size(56, 27);
			this.clipnodesLabel.TabIndex = 3;
			this.clipnodesLabel.Text = "Clipnodes:";
			this.clipnodesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// modelLabel
			// 
			this.modelLabel.AutoSize = true;
			this.modelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modelLabel.Location = new System.Drawing.Point(3, 0);
			this.modelLabel.Name = "modelLabel";
			this.modelLabel.Size = new System.Drawing.Size(56, 27);
			this.modelLabel.TabIndex = 0;
			this.modelLabel.Text = "Model:";
			this.modelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// modelComboBox
			// 
			this.modelComboBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.modelComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.modelComboBox.FormattingEnabled = true;
			this.modelComboBox.Location = new System.Drawing.Point(65, 3);
			this.modelComboBox.Name = "modelComboBox";
			this.modelComboBox.Size = new System.Drawing.Size(190, 21);
			this.modelComboBox.TabIndex = 1;
			this.modelComboBox.SelectedIndexChanged += new System.EventHandler(this.modelComboBox_SelectedIndexChanged);
			// 
			// bspTreeLabel
			// 
			this.bspTreeLabel.AutoSize = true;
			this.leftTableLayoutPanel.SetColumnSpan(this.bspTreeLabel, 2);
			this.bspTreeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bspTreeLabel.Location = new System.Drawing.Point(3, 27);
			this.bspTreeLabel.Name = "bspTreeLabel";
			this.bspTreeLabel.Size = new System.Drawing.Size(252, 13);
			this.bspTreeLabel.TabIndex = 2;
			this.bspTreeLabel.Text = "BSP Tree:";
			this.bspTreeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// bspTreeView
			// 
			this.leftTableLayoutPanel.SetColumnSpan(this.bspTreeView, 2);
			this.bspTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.bspTreeView.Location = new System.Drawing.Point(3, 43);
			this.bspTreeView.Name = "bspTreeView";
			this.bspTreeView.Size = new System.Drawing.Size(252, 262);
			this.bspTreeView.TabIndex = 4;
			// 
			// clipnodesTreeView
			// 
			this.leftTableLayoutPanel.SetColumnSpan(this.clipnodesTreeView, 2);
			this.clipnodesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.clipnodesTreeView.Location = new System.Drawing.Point(3, 338);
			this.clipnodesTreeView.Name = "clipnodesTreeView";
			this.clipnodesTreeView.Size = new System.Drawing.Size(252, 263);
			this.clipnodesTreeView.TabIndex = 5;
			// 
			// BspViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(963, 604);
			this.Controls.Add(this.mainSplitContainer);
			this.Name = "BspViewerForm";
			this.Text = "Bsp Viewer";
			this.mainSplitContainer.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
			this.mainSplitContainer.ResumeLayout(false);
			this.leftTableLayoutPanel.ResumeLayout(false);
			this.leftTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer mainSplitContainer;
		private System.Windows.Forms.TableLayoutPanel leftTableLayoutPanel;
		private System.Windows.Forms.Label modelLabel;
		private System.Windows.Forms.ComboBox modelComboBox;
		private System.Windows.Forms.Label clipnodesLabel;
		private System.Windows.Forms.Label bspTreeLabel;
		private System.Windows.Forms.TreeView bspTreeView;
		private System.Windows.Forms.TreeView clipnodesTreeView;
		private System.Windows.Forms.ComboBox hullsComboBox;
	}
}

