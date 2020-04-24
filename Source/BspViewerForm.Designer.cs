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
			this.leftSplitContainer = new System.Windows.Forms.SplitContainer();
			this.leftTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.hullsComboBox = new System.Windows.Forms.ComboBox();
			this.clipnodesLabel = new System.Windows.Forms.Label();
			this.modelLabel = new System.Windows.Forms.Label();
			this.modelComboBox = new System.Windows.Forms.ComboBox();
			this.bspTreeLabel = new System.Windows.Forms.Label();
			this.bspTreeView = new System.Windows.Forms.TreeView();
			this.clipnodesTreeView = new System.Windows.Forms.TreeView();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.rightSplitContainer = new System.Windows.Forms.SplitContainer();
			((System.ComponentModel.ISupportInitialize)(this.leftSplitContainer)).BeginInit();
			this.leftSplitContainer.Panel1.SuspendLayout();
			this.leftSplitContainer.SuspendLayout();
			this.leftTableLayoutPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.rightSplitContainer)).BeginInit();
			this.rightSplitContainer.Panel1.SuspendLayout();
			this.rightSplitContainer.Panel2.SuspendLayout();
			this.rightSplitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// leftSplitContainer
			// 
			this.leftSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.leftSplitContainer.Location = new System.Drawing.Point(0, 0);
			this.leftSplitContainer.Name = "leftSplitContainer";
			// 
			// leftSplitContainer.Panel1
			// 
			this.leftSplitContainer.Panel1.Controls.Add(this.leftTableLayoutPanel);
			this.leftSplitContainer.Size = new System.Drawing.Size(1202, 805);
			this.leftSplitContainer.SplitterDistance = 321;
			this.leftSplitContainer.TabIndex = 0;
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
			this.leftTableLayoutPanel.Size = new System.Drawing.Size(321, 805);
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
			this.hullsComboBox.Location = new System.Drawing.Point(65, 412);
			this.hullsComboBox.Name = "hullsComboBox";
			this.hullsComboBox.Size = new System.Drawing.Size(253, 21);
			this.hullsComboBox.TabIndex = 6;
			this.hullsComboBox.SelectedIndexChanged += new System.EventHandler(this.hullsComboBox_SelectedIndexChanged);
			// 
			// clipnodesLabel
			// 
			this.clipnodesLabel.AutoSize = true;
			this.clipnodesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.clipnodesLabel.Location = new System.Drawing.Point(3, 409);
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
			this.modelComboBox.Size = new System.Drawing.Size(253, 21);
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
			this.bspTreeLabel.Size = new System.Drawing.Size(315, 13);
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
			this.bspTreeView.Size = new System.Drawing.Size(315, 363);
			this.bspTreeView.TabIndex = 4;
			this.bspTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.bspTreeView_AfterSelect);
			// 
			// clipnodesTreeView
			// 
			this.leftTableLayoutPanel.SetColumnSpan(this.clipnodesTreeView, 2);
			this.clipnodesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.clipnodesTreeView.Location = new System.Drawing.Point(3, 439);
			this.clipnodesTreeView.Name = "clipnodesTreeView";
			this.clipnodesTreeView.Size = new System.Drawing.Size(315, 363);
			this.clipnodesTreeView.TabIndex = 5;
			this.clipnodesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.clipnodesTreeView_AfterSelect);
			// 
			// propertyGrid
			// 
			this.propertyGrid.CommandsVisibleIfAvailable = false;
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.HelpVisible = false;
			this.propertyGrid.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
			this.propertyGrid.Size = new System.Drawing.Size(363, 805);
			this.propertyGrid.TabIndex = 1;
			// 
			// rightSplitContainer
			// 
			this.rightSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rightSplitContainer.Location = new System.Drawing.Point(0, 0);
			this.rightSplitContainer.Name = "rightSplitContainer";
			// 
			// rightSplitContainer.Panel1
			// 
			this.rightSplitContainer.Panel1.Controls.Add(this.leftSplitContainer);
			// 
			// rightSplitContainer.Panel2
			// 
			this.rightSplitContainer.Panel2.Controls.Add(this.propertyGrid);
			this.rightSplitContainer.Size = new System.Drawing.Size(1569, 805);
			this.rightSplitContainer.SplitterDistance = 1202;
			this.rightSplitContainer.TabIndex = 2;
			// 
			// BspViewerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1569, 805);
			this.Controls.Add(this.rightSplitContainer);
			this.Name = "BspViewerForm";
			this.Text = "Bsp Viewer";
			this.leftSplitContainer.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.leftSplitContainer)).EndInit();
			this.leftSplitContainer.ResumeLayout(false);
			this.leftTableLayoutPanel.ResumeLayout(false);
			this.leftTableLayoutPanel.PerformLayout();
			this.rightSplitContainer.Panel1.ResumeLayout(false);
			this.rightSplitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.rightSplitContainer)).EndInit();
			this.rightSplitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer leftSplitContainer;
		private System.Windows.Forms.TableLayoutPanel leftTableLayoutPanel;
		private System.Windows.Forms.Label modelLabel;
		private System.Windows.Forms.ComboBox modelComboBox;
		private System.Windows.Forms.Label clipnodesLabel;
		private System.Windows.Forms.Label bspTreeLabel;
		private System.Windows.Forms.TreeView bspTreeView;
		private System.Windows.Forms.TreeView clipnodesTreeView;
		private System.Windows.Forms.ComboBox hullsComboBox;
		internal System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.SplitContainer rightSplitContainer;
	}
}

