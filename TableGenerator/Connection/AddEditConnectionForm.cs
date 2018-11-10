
/*
 * 
 * Copyright (C) 2009-2016 JFo.nz
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, version 3 of the License.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program.  If not, see http://www.gnu.org/licenses/.
 **/

using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TypedQuery.Connection {

	public partial class AddEditConnectionForm : Form {

		private Connection mConnection;

		public Connection Connection {
			get { return mConnection; }
		}

		public AddEditConnectionForm() {
			InitializeComponent();
			SetFieldStates();
		}

		private void LoadForm() {

			cboDatabaseType.Items.Add(Sql.DatabaseType.Mssql);
			cboDatabaseType.Items.Add(Sql.DatabaseType.PostgreSql);

			cboDatabaseType.SelectedItem = Sql.DatabaseType.Mssql;
		}

		public DialogResult AddConnection() {

			Text = "Add Connection";

			LoadForm();

			mConnection = new Connection();

			ShowDialog();

			return DialogResult;
		}

		public DialogResult EditConnection(Connection pConnection) {

			if(pConnection == null)
				throw new NullReferenceException("pConnection cannot be null");

			Text = "Edit Connection";

			LoadForm();

			mConnection = pConnection;

			cboDatabaseType.SelectedItem = mConnection.DatabaseType;
			txtConnectionString.Text = mConnection.ConnectionString;

			if(mConnection.DatabaseType == Sql.DatabaseType.Mssql) {

				SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(mConnection.ConnectionString);

				txtServer.Text = builder.DataSource;
				txtDatabaseName.Text = builder.InitialCatalog;

				chkIntegratedSecurity.Checked = builder.IntegratedSecurity;
				txtUserName.Text = builder.UserID;
				txtPassword.Text = builder.Password;

				chkEncrypt.Checked = builder.Encrypt;
				chkTrustCertificate.Checked = builder.TrustServerCertificate;
			}
			else if(mConnection.DatabaseType == Sql.DatabaseType.PostgreSql) {

				NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder(mConnection.ConnectionString);

				txtServer.Text = builder.Host;
				txtDatabaseName.Text = builder.Database;

				chkIntegratedSecurity.Checked = builder.IntegratedSecurity;

				txtUserName.Text = builder.UserName;
				txtPassword.Text = builder.Password;

				chkEncrypt.Checked = builder.SSL;
				//chkTrustCertificate.Checked - Not supportted it seems;
			}
			else {
				throw new Exception($"Unknown database type. Type = '{ mConnection.DatabaseType.ToString() }'");
			}
			SetFieldStates();

			ShowDialog();

			return DialogResult;
		}

		private void btnSave_Click(object sender, EventArgs e) {

			mConnection.DatabaseType = (Sql.DatabaseType)cboDatabaseType.SelectedItem;
			mConnection.ConnectionString = txtConnectionString.Text;

			DialogResult = System.Windows.Forms.DialogResult.OK;

			Close();
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			Close();
		}

		private void chkIsTrusted_CheckedChanged(object sender, EventArgs e) {
			SetFieldStates();
		}

		private void SetFieldStates() {
			btnTestConnection.Enabled = cboDatabaseType.SelectedItem != null && !string.IsNullOrEmpty(txtConnectionString.Text);
			txtUserName.Enabled = !chkIntegratedSecurity.Checked;
			txtPassword.Enabled = !chkIntegratedSecurity.Checked;
			chkTrustCertificate.Enabled = cboDatabaseType.SelectedItem != null && ((Sql.DatabaseType)cboDatabaseType.SelectedItem) == Sql.DatabaseType.Mssql;
		}

		private void btnGenerate_Click(object sender, EventArgs e) {

			Sql.DatabaseType databaseType = (Sql.DatabaseType)cboDatabaseType.SelectedItem;

			string connString;

			if(databaseType == Sql.DatabaseType.Mssql) {

				SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

				builder.DataSource = txtServer.Text;
				if(!string.IsNullOrEmpty(txtPort.Text)) {
					int port;
					if(!int.TryParse(txtPort.Text, out port)) {
						MessageBox.Show("Invalid port number");
						return;
					}
					builder.DataSource += ", " + port.ToString();
				}
				builder.InitialCatalog = txtDatabaseName.Text;

				if(chkIntegratedSecurity.Checked) {
					builder.IntegratedSecurity = chkIntegratedSecurity.Checked;
				}
				else {
					builder.UserID = txtUserName.Text;
					builder.Password = txtPassword.Text;
				}

				builder.Encrypt = chkEncrypt.Checked;
				builder.TrustServerCertificate = chkTrustCertificate.Checked;

				connString = builder.ConnectionString;
			}
			else if(databaseType == Sql.DatabaseType.PostgreSql) {

				NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder();

				builder.Host = txtServer.Text;
				builder.Database = txtDatabaseName.Text;

				if(!string.IsNullOrEmpty(txtPort.Text)) {
					int port;
					if(!int.TryParse(txtPort.Text, out port)) {
						MessageBox.Show("Invalid port number");
						return;
					}
					builder.Port = port;
				}

				if(chkIntegratedSecurity.Checked) {
					builder.IntegratedSecurity = chkIntegratedSecurity.Checked;
				}
				else {
					builder.UserName = txtUserName.Text;
					builder.Password = txtPassword.Text;
				}

				builder.SSL = chkEncrypt.Checked;
				builder.SslMode = chkEncrypt.Checked ? SslMode.Require : SslMode.Allow;
				//chkTrustCertificate.Checked - Not supportted it seems;

				connString = builder.ConnectionString;
			}
			else {
				throw new Exception($"Unknown database type. Type = '{ databaseType.ToString() }'");
			}
			txtConnectionString.Text = connString;
		}

		private void cboDatabaseType_SelectedIndexChanged(object sender, EventArgs e) {
			SetFieldStates();
		}

		private void btnTestConnection_Click(object sender, EventArgs e) {

			Cursor.Current = Cursors.WaitCursor;

			try {

				if(cboDatabaseType.SelectedItem == null || string.IsNullOrEmpty(txtConnectionString.Text)) {
					return;
				}
				Sql.DatabaseType dbType = (Sql.DatabaseType)cboDatabaseType.SelectedItem;

				if(dbType == Sql.DatabaseType.PostgreSql) {
					Postgresql.PgDatabase.Instance.SetConnectionString(txtConnectionString.Text);
					new Logic.PostgreSqlSchema().TestConnection(Postgresql.PgDatabase.Instance);
					MessageBox.Show(this, "Connection Worked!", "", MessageBoxButtons.OK);
				}
				else if(dbType == Sql.DatabaseType.Mssql) {
					SqlServer.SqlServerDatabase.Instance.SetConnectionString(txtConnectionString.Text);
					new Logic.SqlServerSchema().TestConnection(SqlServer.SqlServerDatabase.Instance);
					MessageBox.Show(this, "Connection Worked!", "", MessageBoxButtons.OK);
				}
				else {
					throw new Exception($"Unknown database type: {dbType.ToString()}");
				}
			}
			finally {
				Cursor.Current = Cursors.WaitCursor;
			}
		}

		private void txtConnectionString_TextChanged(object sender, EventArgs e) {
			SetFieldStates();
		}
    }
}