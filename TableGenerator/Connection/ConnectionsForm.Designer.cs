namespace TypedQuery.Connection {
	partial class ConnectionsForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.btnClose = new System.Windows.Forms.Button();
			this.lvwConnections = new System.Windows.Forms.ListView();
			this.colConnectionDetails = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lblConnections = new System.Windows.Forms.Label();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(760, 332);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// lvwConnections
			// 
			this.lvwConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lvwConnections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colConnectionDetails});
			this.lvwConnections.FullRowSelect = true;
			this.lvwConnections.GridLines = true;
			this.lvwConnections.HideSelection = false;
			this.lvwConnections.Location = new System.Drawing.Point(4, 24);
			this.lvwConnections.MultiSelect = false;
			this.lvwConnections.Name = "lvwConnections";
			this.lvwConnections.ShowGroups = false;
			this.lvwConnections.Size = new System.Drawing.Size(828, 304);
			this.lvwConnections.TabIndex = 5;
			this.lvwConnections.UseCompatibleStateImageBehavior = false;
			this.lvwConnections.View = System.Windows.Forms.View.Details;
			this.lvwConnections.SelectedIndexChanged += new System.EventHandler(this.lvwConnections_SelectedIndexChanged);
			this.lvwConnections.DoubleClick += new System.EventHandler(this.btnEdit_Click);
			// 
			// colConnectionDetails
			// 
			this.colConnectionDetails.Text = "Connection Details";
			this.colConnectionDetails.Width = 798;
			// 
			// lblConnections
			// 
			this.lblConnections.AutoSize = true;
			this.lblConnections.Location = new System.Drawing.Point(8, 8);
			this.lblConnections.Name = "lblConnections";
			this.lblConnections.Size = new System.Drawing.Size(66, 13);
			this.lblConnections.TabIndex = 6;
			this.lblConnections.Text = "Connections";
			// 
			// btnAdd
			// 
			this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnAdd.Location = new System.Drawing.Point(4, 332);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(75, 23);
			this.btnAdd.TabIndex = 7;
			this.btnAdd.Text = "&Add";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnEdit.Enabled = false;
			this.btnEdit.Location = new System.Drawing.Point(80, 332);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(75, 23);
			this.btnEdit.TabIndex = 8;
			this.btnEdit.Text = "&Edit";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnDelete.Enabled = false;
			this.btnDelete.Location = new System.Drawing.Point(156, 332);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(75, 23);
			this.btnDelete.TabIndex = 9;
			this.btnDelete.Text = "&Delete";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// ConnectionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(837, 358);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.lblConnections);
			this.Controls.Add(this.lvwConnections);
			this.Controls.Add(this.btnClose);
			this.MinimumSize = new System.Drawing.Size(643, 191);
			this.Name = "ConnectionsForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Maintain Connections";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ListView lvwConnections;
		private System.Windows.Forms.Label lblConnections;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ColumnHeader colConnectionDetails;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnDelete;
	}
}