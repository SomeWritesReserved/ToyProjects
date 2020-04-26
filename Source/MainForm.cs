using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HL1BspReader
{
	public partial class MainForm : Form
	{
		#region Constructors

		public MainForm()
		{
			this.InitializeComponent();
		}

		#endregion Constructors

		#region Properties

		public Bsp Bsp { get; set; }

		#endregion Properties

		#region Events

		private void modelComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.populateClipnodesTreeView();
		}

		private void hullsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.populateClipnodesTreeView();
		}

		private void bspTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.bspTreeView.SelectedNode != null)
			{
				this.propertyGrid.SelectedObject = this.bspTreeView.SelectedNode.Tag;
			}
		}

		private void clipnodesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.clipnodesTreeView.SelectedNode != null)
			{
				this.propertyGrid.SelectedObject = this.clipnodesTreeView.SelectedNode.Tag;
			}
		}

		#endregion Events

		#region Methods

		internal void DockGameWindow(GameView gameView)
		{
			Form gameWindow = (Form)Control.FromHandle(gameView.Window.Handle);
			gameWindow.FormBorderStyle = FormBorderStyle.None;
			gameWindow.TopLevel = false;
			gameWindow.Dock = DockStyle.Fill;

			// This ugly just makes sure the XNA viewport gets updates if the user resizes the hosting form
			MethodInfo gameWindowClientSizeChangedMethod = gameView.GraphicsDeviceManager.GetType().GetMethod("GameWindowClientSizeChanged", BindingFlags.Instance | BindingFlags.NonPublic);
			this.ResizeEnd += (sender, args) =>
			{
				gameWindowClientSizeChangedMethod.Invoke(gameView.GraphicsDeviceManager, new object[] { null, null });
			};

			this.leftSplitContainer.Panel2.Controls.Add(gameWindow);
			gameWindow.BeginInvoke(new Action(() => gameWindowClientSizeChangedMethod.Invoke(gameView.GraphicsDeviceManager, new object[] { null, null })));

			this.loadBsp();
		}

		private void loadBsp()
		{
			this.Bsp = BspReader.ReadFromFile(@"C:\Users\dexter\Desktop\Valve Hammer Editor\maps\bsptest.bsp");

			this.modelComboBox.Items.Clear();
			this.modelComboBox.Items.AddRange(this.Bsp.Models.ToArray());
			this.populateBspTreeView();
			this.populateClipnodesTreeView();
		}

		private void populateBspTreeView()
		{
			this.bspTreeView.Nodes.Clear();

			void populate(TreeNodeCollection uiCollection, BspNode parentNode)
			{
				TreeNode uiNode = new TreeNode(parentNode.ToString())
				{
					Tag = parentNode,
				};
				uiCollection.Add(uiNode);
				if (parentNode.ChildANode != null)
				{
					populate(uiNode.Nodes, parentNode.ChildANode);
				}
				else
				{
					uiNode.Nodes.Add(new TreeNode(parentNode.ChildALeaf.ToString())
					{
						Tag = parentNode.ChildALeaf,
					});
				}
				if (parentNode.ChildBNode != null)
				{
					populate(uiNode.Nodes, parentNode.ChildBNode);
				}
				else
				{
					uiNode.Nodes.Add(new TreeNode(parentNode.ChildBLeaf.ToString())
					{
						Tag = parentNode.ChildBLeaf,
					});
				}
			}

			populate(this.bspTreeView.Nodes, this.Bsp.RootNode);
			this.bspTreeView.ExpandAll();
		}

		private void populateClipnodesTreeView()
		{
			this.clipnodesTreeView.Nodes.Clear();

			if (this.modelComboBox.SelectedItem == null) { return; }
			if (this.hullsComboBox.SelectedItem == null) { return; }
			BspModel selectedModel = (BspModel)this.modelComboBox.SelectedItem;

			void populate(TreeNodeCollection uiCollection, BspClipnode parentClipnode)
			{
				TreeNode uiNode = new TreeNode(parentClipnode.ToString())
				{
					Tag = parentClipnode,
				};
				uiCollection.Add(uiNode);
				if (parentClipnode.ChildAClipnode != null)
				{
					populate(uiNode.Nodes, parentClipnode.ChildAClipnode);
				}
				else
				{
					uiNode.Nodes.Add(new TreeNode(parentClipnode.ChildAContents.ToString())
					{
						Tag = parentClipnode.ChildAContents,
					});
				}
				if (parentClipnode.ChildBClipnode != null)
				{
					populate(uiNode.Nodes, parentClipnode.ChildBClipnode);
				}
				else
				{
					uiNode.Nodes.Add(new TreeNode(parentClipnode.ChildBContents.ToString())
					{
						Tag = parentClipnode.ChildBContents,
					});
				}
			}

			BspClipnode clipnode = null;
			if (this.hullsComboBox.SelectedIndex == 0)
			{
				clipnode = selectedModel.Clipnode0;
			}
			if (this.hullsComboBox.SelectedIndex == 1)
			{
				clipnode = selectedModel.Clipnode1;
			}
			if (this.hullsComboBox.SelectedIndex == 2)
			{
				clipnode = selectedModel.Clipnode2;
			}
			if (this.hullsComboBox.SelectedIndex == 3)
			{
				clipnode = selectedModel.Clipnode3;
			}

			populate(this.clipnodesTreeView.Nodes, clipnode);
			this.clipnodesTreeView.ExpandAll();
		}

		#endregion Methods
	}
}
