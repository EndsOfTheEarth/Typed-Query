
/*
 * 
 * Copyright (C) 2009-2015 JFo.nz
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
	}
}