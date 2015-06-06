namespace TypedQuery.Connection {
	partial class AddEditConnectionForm {
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
			this.txtConnectionString = new System.Windows.Forms.TextBox();
			this.lblConnectionString = new System.Windows.Forms.Label();
			this.cboDatabaseType = new System.Windows.Forms.ComboBox();
			this.lblDatabaseType = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.txtPostgreSqlExample = new System.Windows.Forms.TextBox();
			this.lblPostgreSqlExample = new System.Windows.Forms.Label();
			this.txtSqlServerExample = new System.Windows.Forms.TextBox();
			this.lblSqlServerExample = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtConnectionString
			// 
			this.txtConnectionString.Location = new System.Drawing.Point(112, 32);
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.Size = new System.Drawing.Size(544, 20);
			this.txtConnectionString.TabIndex = 7;
			// 
			// lblConnectionString
			// 
			this.lblConnectionString.AutoSize = true;
			this.lblConnectionString.Location = new System.Drawing.Point(4, 36);
			this.lblConnectionString.Name = "lblConnectionString";
			this.lblConnectionString.Size = new System.Drawing.Size(91, 13);
			this.lblConnectionString.TabIndex = 6;
			this.lblConnectionString.Text = "Connection String";
			// 
			// cboDatabaseType
			// 
			this.cboDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDatabaseType.FormattingEnabled = true;
			this.cboDatabaseType.Location = new System.Drawing.Point(112, 8);
			this.cboDatabaseType.Name = "cboDatabaseType";
			this.cboDatabaseType.Size = new System.Drawing.Size(136, 21);
			this.cboDatabaseType.TabIndex = 5;
			// 
			// lblDatabaseType
			// 
			this.lblDatabaseType.AutoSize = true;
			this.lblDatabaseType.Location = new System.Drawing.Point(4, 12);
			this.lblDatabaseType.Name = "lblDatabaseType";
			this.lblDatabaseType.Size = new System.Drawing.Size(80, 13);
			this.lblDatabaseType.TabIndex = 4;
			this.lblDatabaseType.Text = "Database Type";
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(504, 106);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 8;
			this.btnSave.Text = "&Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(580, 106);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtPostgreSqlExample
			// 
			this.txtPostgreSqlExample.Location = new System.Drawing.Point(112, 80);
			this.txtPostgreSqlExample.Name = "txtPostgreSqlExample";
			this.txtPostgreSqlExample.ReadOnly = true;
			this.txtPostgreSqlExample.Size = new System.Drawing.Size(544, 20);
			this.txtPostgreSqlExample.TabIndex = 13;
			this.txtPostgreSqlExample.Text = "Server=127.0.0.1;Port=5432;Database=database_name;User Id=login_name;Password=pwd" +
    "";
			// 
			// lblPostgreSqlExample
			// 
			this.lblPostgreSqlExample.AutoSize = true;
			this.lblPostgreSqlExample.Location = new System.Drawing.Point(4, 84);
			this.lblPostgreSqlExample.Name = "lblPostgreSqlExample";
			this.lblPostgreSqlExample.Size = new System.Drawing.Size(101, 13);
			this.lblPostgreSqlExample.TabIndex = 12;
			this.lblPostgreSqlExample.Text = "PostgreSql Example";
			// 
			// txtSqlServerExample
			// 
			this.txtSqlServerExample.Location = new System.Drawing.Point(112, 56);
			this.txtSqlServerExample.Name = "txtSqlServerExample";
			this.txtSqlServerExample.ReadOnly = true;
			this.txtSqlServerExample.Size = new System.Drawing.Size(544, 20);
			this.txtSqlServerExample.TabIndex = 11;
			this.txtSqlServerExample.Text = "Data Source=PC_NAME\\SQLEXPRESS;User Id=login_name;Password=pwd;database=database_" +
    "name;";
			// 
			// lblSqlServerExample
			// 
			this.lblSqlServerExample.AutoSize = true;
			this.lblSqlServerExample.Location = new System.Drawing.Point(4, 60);
			this.lblSqlServerExample.Name = "lblSqlServerExample";
			this.lblSqlServerExample.Size = new System.Drawing.Size(99, 13);
			this.lblSqlServerExample.TabIndex = 10;
			this.lblSqlServerExample.Text = "Sql Server Example";
			// 
			// AddEditConnectionForm
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(659, 133);
			this.Controls.Add(this.txtPostgreSqlExample);
			this.Controls.Add(this.lblPostgreSqlExample);
			this.Controls.Add(this.txtSqlServerExample);
			this.Controls.Add(this.lblSqlServerExample);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.txtConnectionString);
			this.Controls.Add(this.lblConnectionString);
			this.Controls.Add(this.cboDatabaseType);
			this.Controls.Add(this.lblDatabaseType);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "AddEditConnectionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AddEditConnectionForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtConnectionString;
		private System.Windows.Forms.Label lblConnectionString;
		private System.Windows.Forms.ComboBox cboDatabaseType;
		private System.Windows.Forms.Label lblDatabaseType;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtPostgreSqlExample;
		private System.Windows.Forms.Label lblPostgreSqlExample;
		private System.Windows.Forms.TextBox txtSqlServerExample;
		private System.Windows.Forms.Label lblSqlServerExample;
	}
}