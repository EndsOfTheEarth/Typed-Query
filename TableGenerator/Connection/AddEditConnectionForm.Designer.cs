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
			this.txtDatabaseName = new System.Windows.Forms.TextBox();
			this.lblDatabaseName = new System.Windows.Forms.Label();
			this.grpGenerateConnectionString = new System.Windows.Forms.GroupBox();
			this.txtPort = new System.Windows.Forms.TextBox();
			this.lblPort = new System.Windows.Forms.Label();
			this.txtServer = new System.Windows.Forms.TextBox();
			this.lblServer = new System.Windows.Forms.Label();
			this.chkTrustCertificate = new System.Windows.Forms.CheckBox();
			this.chkEncrypt = new System.Windows.Forms.CheckBox();
			this.btnGenerate = new System.Windows.Forms.Button();
			this.chkIntegratedSecurity = new System.Windows.Forms.CheckBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblPassword = new System.Windows.Forms.Label();
			this.txtUserName = new System.Windows.Forms.TextBox();
			this.lblUserName = new System.Windows.Forms.Label();
			this.btnTestConnection = new System.Windows.Forms.Button();
			this.grpGenerateConnectionString.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtConnectionString
			// 
			this.txtConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtConnectionString.Location = new System.Drawing.Point(112, 32);
			this.txtConnectionString.Name = "txtConnectionString";
			this.txtConnectionString.Size = new System.Drawing.Size(548, 20);
			this.txtConnectionString.TabIndex = 3;
			this.txtConnectionString.TextChanged += new System.EventHandler(this.txtConnectionString_TextChanged);
			// 
			// lblConnectionString
			// 
			this.lblConnectionString.AutoSize = true;
			this.lblConnectionString.Location = new System.Drawing.Point(4, 36);
			this.lblConnectionString.Name = "lblConnectionString";
			this.lblConnectionString.Size = new System.Drawing.Size(91, 13);
			this.lblConnectionString.TabIndex = 2;
			this.lblConnectionString.Text = "Connection String";
			// 
			// cboDatabaseType
			// 
			this.cboDatabaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDatabaseType.FormattingEnabled = true;
			this.cboDatabaseType.Location = new System.Drawing.Point(112, 8);
			this.cboDatabaseType.Name = "cboDatabaseType";
			this.cboDatabaseType.Size = new System.Drawing.Size(136, 21);
			this.cboDatabaseType.TabIndex = 1;
			this.cboDatabaseType.SelectedIndexChanged += new System.EventHandler(this.cboDatabaseType_SelectedIndexChanged);
			// 
			// lblDatabaseType
			// 
			this.lblDatabaseType.AutoSize = true;
			this.lblDatabaseType.Location = new System.Drawing.Point(4, 12);
			this.lblDatabaseType.Name = "lblDatabaseType";
			this.lblDatabaseType.Size = new System.Drawing.Size(80, 13);
			this.lblDatabaseType.TabIndex = 0;
			this.lblDatabaseType.Text = "Database Type";
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point(508, 281);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 10;
			this.btnSave.Text = "&Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(584, 281);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 11;
			this.btnCancel.Text = "&Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// txtPostgreSqlExample
			// 
			this.txtPostgreSqlExample.Location = new System.Drawing.Point(112, 256);
			this.txtPostgreSqlExample.Name = "txtPostgreSqlExample";
			this.txtPostgreSqlExample.ReadOnly = true;
			this.txtPostgreSqlExample.Size = new System.Drawing.Size(544, 20);
			this.txtPostgreSqlExample.TabIndex = 9;
			this.txtPostgreSqlExample.Text = "Server=127.0.0.1;Port=5432;Database=database_name;User Id=login_name;Password=pwd" +
    "";
			// 
			// lblPostgreSqlExample
			// 
			this.lblPostgreSqlExample.AutoSize = true;
			this.lblPostgreSqlExample.Location = new System.Drawing.Point(4, 260);
			this.lblPostgreSqlExample.Name = "lblPostgreSqlExample";
			this.lblPostgreSqlExample.Size = new System.Drawing.Size(101, 13);
			this.lblPostgreSqlExample.TabIndex = 8;
			this.lblPostgreSqlExample.Text = "PostgreSql Example";
			// 
			// txtSqlServerExample
			// 
			this.txtSqlServerExample.Location = new System.Drawing.Point(112, 232);
			this.txtSqlServerExample.Name = "txtSqlServerExample";
			this.txtSqlServerExample.ReadOnly = true;
			this.txtSqlServerExample.Size = new System.Drawing.Size(544, 20);
			this.txtSqlServerExample.TabIndex = 7;
			this.txtSqlServerExample.Text = "Data Source=PC_NAME\\SQLEXPRESS;User Id=login_name;Password=pwd;database=database_" +
    "name;";
			// 
			// lblSqlServerExample
			// 
			this.lblSqlServerExample.AutoSize = true;
			this.lblSqlServerExample.Location = new System.Drawing.Point(4, 236);
			this.lblSqlServerExample.Name = "lblSqlServerExample";
			this.lblSqlServerExample.Size = new System.Drawing.Size(99, 13);
			this.lblSqlServerExample.TabIndex = 6;
			this.lblSqlServerExample.Text = "Sql Server Example";
			// 
			// txtDatabaseName
			// 
			this.txtDatabaseName.Location = new System.Drawing.Point(116, 64);
			this.txtDatabaseName.Name = "txtDatabaseName";
			this.txtDatabaseName.Size = new System.Drawing.Size(236, 20);
			this.txtDatabaseName.TabIndex = 5;
			// 
			// lblDatabaseName
			// 
			this.lblDatabaseName.AutoSize = true;
			this.lblDatabaseName.Location = new System.Drawing.Point(8, 68);
			this.lblDatabaseName.Name = "lblDatabaseName";
			this.lblDatabaseName.Size = new System.Drawing.Size(84, 13);
			this.lblDatabaseName.TabIndex = 4;
			this.lblDatabaseName.Text = "Database Name";
			// 
			// grpGenerateConnectionString
			// 
			this.grpGenerateConnectionString.Controls.Add(this.txtPort);
			this.grpGenerateConnectionString.Controls.Add(this.lblPort);
			this.grpGenerateConnectionString.Controls.Add(this.txtServer);
			this.grpGenerateConnectionString.Controls.Add(this.lblServer);
			this.grpGenerateConnectionString.Controls.Add(this.chkTrustCertificate);
			this.grpGenerateConnectionString.Controls.Add(this.chkEncrypt);
			this.grpGenerateConnectionString.Controls.Add(this.btnGenerate);
			this.grpGenerateConnectionString.Controls.Add(this.chkIntegratedSecurity);
			this.grpGenerateConnectionString.Controls.Add(this.txtPassword);
			this.grpGenerateConnectionString.Controls.Add(this.lblPassword);
			this.grpGenerateConnectionString.Controls.Add(this.txtUserName);
			this.grpGenerateConnectionString.Controls.Add(this.lblUserName);
			this.grpGenerateConnectionString.Controls.Add(this.txtDatabaseName);
			this.grpGenerateConnectionString.Controls.Add(this.lblDatabaseName);
			this.grpGenerateConnectionString.Location = new System.Drawing.Point(8, 60);
			this.grpGenerateConnectionString.Name = "grpGenerateConnectionString";
			this.grpGenerateConnectionString.Size = new System.Drawing.Size(476, 168);
			this.grpGenerateConnectionString.TabIndex = 5;
			this.grpGenerateConnectionString.TabStop = false;
			this.grpGenerateConnectionString.Text = "Generate Connection String";
			// 
			// txtPort
			// 
			this.txtPort.Location = new System.Drawing.Point(116, 40);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new System.Drawing.Size(44, 20);
			this.txtPort.TabIndex = 3;
			// 
			// lblPort
			// 
			this.lblPort.AutoSize = true;
			this.lblPort.Location = new System.Drawing.Point(8, 44);
			this.lblPort.Name = "lblPort";
			this.lblPort.Size = new System.Drawing.Size(26, 13);
			this.lblPort.TabIndex = 2;
			this.lblPort.Text = "Port";
			// 
			// txtServer
			// 
			this.txtServer.Location = new System.Drawing.Point(116, 16);
			this.txtServer.Name = "txtServer";
			this.txtServer.Size = new System.Drawing.Size(236, 20);
			this.txtServer.TabIndex = 1;
			// 
			// lblServer
			// 
			this.lblServer.AutoSize = true;
			this.lblServer.Location = new System.Drawing.Point(8, 20);
			this.lblServer.Name = "lblServer";
			this.lblServer.Size = new System.Drawing.Size(38, 13);
			this.lblServer.TabIndex = 0;
			this.lblServer.Text = "Server";
			// 
			// chkTrustCertificate
			// 
			this.chkTrustCertificate.AutoSize = true;
			this.chkTrustCertificate.Location = new System.Drawing.Point(280, 132);
			this.chkTrustCertificate.Name = "chkTrustCertificate";
			this.chkTrustCertificate.Size = new System.Drawing.Size(100, 17);
			this.chkTrustCertificate.TabIndex = 12;
			this.chkTrustCertificate.Text = "Trust Certificate";
			this.chkTrustCertificate.UseVisualStyleBackColor = true;
			// 
			// chkEncrypt
			// 
			this.chkEncrypt.AutoSize = true;
			this.chkEncrypt.Location = new System.Drawing.Point(280, 112);
			this.chkEncrypt.Name = "chkEncrypt";
			this.chkEncrypt.Size = new System.Drawing.Size(62, 17);
			this.chkEncrypt.TabIndex = 11;
			this.chkEncrypt.Text = "Encrypt";
			this.chkEncrypt.UseVisualStyleBackColor = true;
			// 
			// btnGenerate
			// 
			this.btnGenerate.Location = new System.Drawing.Point(392, 128);
			this.btnGenerate.Name = "btnGenerate";
			this.btnGenerate.Size = new System.Drawing.Size(75, 23);
			this.btnGenerate.TabIndex = 13;
			this.btnGenerate.Text = "&Generate";
			this.btnGenerate.UseVisualStyleBackColor = true;
			this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
			// 
			// chkIntegratedSecurity
			// 
			this.chkIntegratedSecurity.AutoSize = true;
			this.chkIntegratedSecurity.Location = new System.Drawing.Point(120, 88);
			this.chkIntegratedSecurity.Name = "chkIntegratedSecurity";
			this.chkIntegratedSecurity.Size = new System.Drawing.Size(115, 17);
			this.chkIntegratedSecurity.TabIndex = 6;
			this.chkIntegratedSecurity.Text = "Integrated Security";
			this.chkIntegratedSecurity.UseVisualStyleBackColor = true;
			this.chkIntegratedSecurity.CheckedChanged += new System.EventHandler(this.chkIsTrusted_CheckedChanged);
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(116, 132);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(152, 20);
			this.txtPassword.TabIndex = 10;
			// 
			// lblPassword
			// 
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(8, 136);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(53, 13);
			this.lblPassword.TabIndex = 9;
			this.lblPassword.Text = "Password";
			// 
			// txtUserName
			// 
			this.txtUserName.Location = new System.Drawing.Point(116, 108);
			this.txtUserName.Name = "txtUserName";
			this.txtUserName.Size = new System.Drawing.Size(152, 20);
			this.txtUserName.TabIndex = 8;
			// 
			// lblUserName
			// 
			this.lblUserName.AutoSize = true;
			this.lblUserName.Location = new System.Drawing.Point(8, 112);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(60, 13);
			this.lblUserName.TabIndex = 7;
			this.lblUserName.Text = "User Name";
			// 
			// btnTestConnection
			// 
			this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTestConnection.Location = new System.Drawing.Point(556, 56);
			this.btnTestConnection.Name = "btnTestConnection";
			this.btnTestConnection.Size = new System.Drawing.Size(104, 23);
			this.btnTestConnection.TabIndex = 4;
			this.btnTestConnection.Text = "Test Connection";
			this.btnTestConnection.UseVisualStyleBackColor = true;
			this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
			// 
			// AddEditConnectionForm
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(663, 308);
			this.Controls.Add(this.btnTestConnection);
			this.Controls.Add(this.grpGenerateConnectionString);
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
			this.Name = "AddEditConnectionForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AddEditConnectionForm";
			this.grpGenerateConnectionString.ResumeLayout(false);
			this.grpGenerateConnectionString.PerformLayout();
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
        private System.Windows.Forms.TextBox txtDatabaseName;
        private System.Windows.Forms.Label lblDatabaseName;
        private System.Windows.Forms.GroupBox grpGenerateConnectionString;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.CheckBox chkIntegratedSecurity;
		private System.Windows.Forms.Button btnGenerate;
		private System.Windows.Forms.CheckBox chkTrustCertificate;
		private System.Windows.Forms.CheckBox chkEncrypt;
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.Label lblServer;
		private System.Windows.Forms.Label lblPort;
		private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Button btnTestConnection;
    }
}