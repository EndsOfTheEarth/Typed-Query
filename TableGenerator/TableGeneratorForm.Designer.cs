namespace TypedQuery {
	partial class TableGeneratorForm {
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
            this.tvwSchema = new System.Windows.Forms.TreeView();
            this.txtTableDefinition = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabDefinitions = new System.Windows.Forms.TabControl();
            this.tabPageTableDefinition = new System.Windows.Forms.TabPage();
            this.chkGenerateCommentMetaData = new System.Windows.Forms.CheckBox();
            this.chkIncludeSchema = new System.Windows.Forms.CheckBox();
            this.txtColumnPrefix = new System.Windows.Forms.TextBox();
            this.lblColumnPrefix = new System.Windows.Forms.Label();
            this.tabClassCode = new System.Windows.Forms.TabPage();
            this.txtClassCode = new System.Windows.Forms.TextBox();
            this.splitter = new System.Windows.Forms.SplitContainer();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnMaintainConnections = new System.Windows.Forms.Button();
            this.cboConnections = new System.Windows.Forms.ComboBox();
            this.lblConnections = new System.Windows.Forms.Label();
            this.chkRemoveUnderscores = new System.Windows.Forms.CheckBox();
            this.tabDefinitions.SuspendLayout();
            this.tabPageTableDefinition.SuspendLayout();
            this.tabClassCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // tvwSchema
            // 
            this.tvwSchema.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvwSchema.HideSelection = false;
            this.tvwSchema.Location = new System.Drawing.Point(0, 0);
            this.tvwSchema.Name = "tvwSchema";
            this.tvwSchema.Size = new System.Drawing.Size(213, 631);
            this.tvwSchema.TabIndex = 1;
            this.tvwSchema.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwSchema_AfterSelect);
            // 
            // txtTableDefinition
            // 
            this.txtTableDefinition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTableDefinition.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTableDefinition.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtTableDefinition.HideSelection = false;
            this.txtTableDefinition.Location = new System.Drawing.Point(4, 28);
            this.txtTableDefinition.Multiline = true;
            this.txtTableDefinition.Name = "txtTableDefinition";
            this.txtTableDefinition.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTableDefinition.Size = new System.Drawing.Size(802, 575);
            this.txtTableDefinition.TabIndex = 3;
            this.txtTableDefinition.WordWrap = false;
            this.txtTableDefinition.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTableDefinition_KeyDown);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(959, 683);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabDefinitions
            // 
            this.tabDefinitions.Controls.Add(this.tabPageTableDefinition);
            this.tabDefinitions.Controls.Add(this.tabClassCode);
            this.tabDefinitions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDefinitions.Location = new System.Drawing.Point(0, 0);
            this.tabDefinitions.Name = "tabDefinitions";
            this.tabDefinitions.SelectedIndex = 0;
            this.tabDefinitions.Size = new System.Drawing.Size(818, 631);
            this.tabDefinitions.TabIndex = 5;
            // 
            // tabPageTableDefinition
            // 
            this.tabPageTableDefinition.Controls.Add(this.chkRemoveUnderscores);
            this.tabPageTableDefinition.Controls.Add(this.chkGenerateCommentMetaData);
            this.tabPageTableDefinition.Controls.Add(this.chkIncludeSchema);
            this.tabPageTableDefinition.Controls.Add(this.txtColumnPrefix);
            this.tabPageTableDefinition.Controls.Add(this.lblColumnPrefix);
            this.tabPageTableDefinition.Controls.Add(this.txtTableDefinition);
            this.tabPageTableDefinition.Location = new System.Drawing.Point(4, 22);
            this.tabPageTableDefinition.Name = "tabPageTableDefinition";
            this.tabPageTableDefinition.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTableDefinition.Size = new System.Drawing.Size(810, 605);
            this.tabPageTableDefinition.TabIndex = 0;
            this.tabPageTableDefinition.Text = "Table Definition";
            // 
            // chkGenerateCommentMetaData
            // 
            this.chkGenerateCommentMetaData.AutoSize = true;
            this.chkGenerateCommentMetaData.Checked = true;
            this.chkGenerateCommentMetaData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGenerateCommentMetaData.Location = new System.Drawing.Point(296, 8);
            this.chkGenerateCommentMetaData.Name = "chkGenerateCommentMetaData";
            this.chkGenerateCommentMetaData.Size = new System.Drawing.Size(170, 17);
            this.chkGenerateCommentMetaData.TabIndex = 7;
            this.chkGenerateCommentMetaData.Text = "Generate Comment Meta Data";
            this.chkGenerateCommentMetaData.UseVisualStyleBackColor = true;
            this.chkGenerateCommentMetaData.CheckedChanged += new System.EventHandler(this.chkGenerateCommentsMetaData_CheckedChanged);
            // 
            // chkIncludeSchema
            // 
            this.chkIncludeSchema.AutoSize = true;
            this.chkIncludeSchema.Checked = true;
            this.chkIncludeSchema.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeSchema.Location = new System.Drawing.Point(184, 8);
            this.chkIncludeSchema.Name = "chkIncludeSchema";
            this.chkIncludeSchema.Size = new System.Drawing.Size(103, 17);
            this.chkIncludeSchema.TabIndex = 6;
            this.chkIncludeSchema.Text = "Include Schema";
            this.chkIncludeSchema.UseVisualStyleBackColor = true;
            this.chkIncludeSchema.CheckedChanged += new System.EventHandler(this.chkIncludeSchema_CheckedChanged);
            // 
            // txtColumnPrefix
            // 
            this.txtColumnPrefix.Location = new System.Drawing.Point(76, 4);
            this.txtColumnPrefix.Name = "txtColumnPrefix";
            this.txtColumnPrefix.Size = new System.Drawing.Size(100, 20);
            this.txtColumnPrefix.TabIndex = 5;
            this.txtColumnPrefix.TextChanged += new System.EventHandler(this.txtColumnPrefix_TextChanged);
            // 
            // lblColumnPrefix
            // 
            this.lblColumnPrefix.AutoSize = true;
            this.lblColumnPrefix.Location = new System.Drawing.Point(4, 8);
            this.lblColumnPrefix.Name = "lblColumnPrefix";
            this.lblColumnPrefix.Size = new System.Drawing.Size(71, 13);
            this.lblColumnPrefix.TabIndex = 4;
            this.lblColumnPrefix.Text = "Column Prefix";
            // 
            // tabClassCode
            // 
            this.tabClassCode.Controls.Add(this.txtClassCode);
            this.tabClassCode.Location = new System.Drawing.Point(4, 22);
            this.tabClassCode.Name = "tabClassCode";
            this.tabClassCode.Padding = new System.Windows.Forms.Padding(3);
            this.tabClassCode.Size = new System.Drawing.Size(810, 605);
            this.tabClassCode.TabIndex = 1;
            this.tabClassCode.Text = "Class Code";
            this.tabClassCode.UseVisualStyleBackColor = true;
            // 
            // txtClassCode
            // 
            this.txtClassCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtClassCode.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.txtClassCode.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.txtClassCode.Location = new System.Drawing.Point(3, 3);
            this.txtClassCode.Multiline = true;
            this.txtClassCode.Name = "txtClassCode";
            this.txtClassCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtClassCode.Size = new System.Drawing.Size(804, 599);
            this.txtClassCode.TabIndex = 0;
            // 
            // splitter
            // 
            this.splitter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitter.Location = new System.Drawing.Point(0, 52);
            this.splitter.Name = "splitter";
            // 
            // splitter.Panel1
            // 
            this.splitter.Panel1.Controls.Add(this.tvwSchema);
            // 
            // splitter.Panel2
            // 
            this.splitter.Panel2.Controls.Add(this.tabDefinitions);
            this.splitter.Size = new System.Drawing.Size(1035, 631);
            this.splitter.SplitterDistance = 213;
            this.splitter.TabIndex = 6;
            // 
            // btnLoad
            // 
            this.btnLoad.Enabled = false;
            this.btnLoad.Location = new System.Drawing.Point(4, 8);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(116, 40);
            this.btnLoad.TabIndex = 2;
            this.btnLoad.Text = "&Load Schema";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnMaintainConnections
            // 
            this.btnMaintainConnections.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaintainConnections.Location = new System.Drawing.Point(905, 12);
            this.btnMaintainConnections.Name = "btnMaintainConnections";
            this.btnMaintainConnections.Size = new System.Drawing.Size(128, 23);
            this.btnMaintainConnections.TabIndex = 7;
            this.btnMaintainConnections.Text = "Maintain Connections";
            this.btnMaintainConnections.UseVisualStyleBackColor = true;
            this.btnMaintainConnections.Click += new System.EventHandler(this.button1_Click);
            // 
            // cboConnections
            // 
            this.cboConnections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboConnections.FormattingEnabled = true;
            this.cboConnections.Location = new System.Drawing.Point(200, 12);
            this.cboConnections.Name = "cboConnections";
            this.cboConnections.Size = new System.Drawing.Size(701, 21);
            this.cboConnections.TabIndex = 8;
            this.cboConnections.SelectedIndexChanged += new System.EventHandler(this.cboConnections_SelectedIndexChanged);
            // 
            // lblConnections
            // 
            this.lblConnections.AutoSize = true;
            this.lblConnections.Location = new System.Drawing.Point(128, 16);
            this.lblConnections.Name = "lblConnections";
            this.lblConnections.Size = new System.Drawing.Size(66, 13);
            this.lblConnections.TabIndex = 9;
            this.lblConnections.Text = "Connections";
            // 
            // chkRemoveUnderscores
            // 
            this.chkRemoveUnderscores.AutoSize = true;
            this.chkRemoveUnderscores.Checked = true;
            this.chkRemoveUnderscores.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRemoveUnderscores.Location = new System.Drawing.Point(476, 8);
            this.chkRemoveUnderscores.Name = "chkRemoveUnderscores";
            this.chkRemoveUnderscores.Size = new System.Drawing.Size(129, 17);
            this.chkRemoveUnderscores.TabIndex = 8;
            this.chkRemoveUnderscores.Text = "Remove Underscores";
            this.chkRemoveUnderscores.UseVisualStyleBackColor = true;
            this.chkRemoveUnderscores.CheckedChanged += new System.EventHandler(this.chkRemoveUnderscores_CheckedChanged);
            // 
            // TableGeneratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 707);
            this.Controls.Add(this.lblConnections);
            this.Controls.Add(this.cboConnections);
            this.Controls.Add(this.btnMaintainConnections);
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnLoad);
            this.MinimumSize = new System.Drawing.Size(697, 434);
            this.Name = "TableGeneratorForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "C# Typed Query";
            this.tabDefinitions.ResumeLayout(false);
            this.tabPageTableDefinition.ResumeLayout(false);
            this.tabPageTableDefinition.PerformLayout();
            this.tabClassCode.ResumeLayout(false);
            this.tabClassCode.PerformLayout();
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).EndInit();
            this.splitter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TreeView tvwSchema;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.TextBox txtTableDefinition;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TabControl tabDefinitions;
		private System.Windows.Forms.TabPage tabPageTableDefinition;
		private System.Windows.Forms.SplitContainer splitter;
		private System.Windows.Forms.TextBox txtColumnPrefix;
		private System.Windows.Forms.Label lblColumnPrefix;
		private System.Windows.Forms.CheckBox chkIncludeSchema;
		private System.Windows.Forms.CheckBox chkGenerateCommentMetaData;
		private System.Windows.Forms.Button btnMaintainConnections;
		private System.Windows.Forms.ComboBox cboConnections;
		private System.Windows.Forms.Label lblConnections;
		private System.Windows.Forms.TabPage tabClassCode;
		private System.Windows.Forms.TextBox txtClassCode;
        private System.Windows.Forms.CheckBox chkRemoveUnderscores;
    }
}

