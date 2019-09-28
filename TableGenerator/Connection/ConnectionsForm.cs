
/*
 * 
 * Copyright (C) 2009-2019 JFo.nz
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
using System.Windows.Forms;

namespace TypedQuery.Connection {

    public partial class ConnectionsForm : Form {

        private List<Connection> mConnections;

        public ConnectionsForm() {

            InitializeComponent();

            ConnectionsFile connectionsFile = new ConnectionsFile();

            mConnections = connectionsFile.Load();

            LoadListView();
        }

        private void LoadListView() {

            lvwConnections.Items.Clear();

            foreach(Connection connection in mConnections) {
                ListViewItem item = new ListViewItem(connection.DatabaseType.ToString() + " -> " + connection.ConnectionString);
                item.Tag = connection;
                lvwConnections.Items.Add(item);
            }
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e) {
            Close();
        }

        private void btnAdd_Click(object sender, EventArgs e) {

            Cursor.Current = Cursors.WaitCursor;

            AddEditConnectionForm form = new AddEditConnectionForm();

            if(form.AddConnection() == DialogResult.OK) {

                mConnections.Add(form.Connection!);
                new ConnectionsFile().Save(mConnections);
                LoadListView();
            }
        }

        private void lvwConnections_SelectedIndexChanged(object sender, EventArgs e) {

            btnEdit.Enabled = lvwConnections.SelectedItems.Count > 0;
            btnDelete.Enabled = lvwConnections.SelectedItems.Count > 0;
        }

        private void btnEdit_Click(object sender, EventArgs e) {

            Cursor.Current = Cursors.WaitCursor;

            Connection connection = (Connection)lvwConnections.SelectedItems[0].Tag;

            Connection connectionCopy = new Connection(connection);

            AddEditConnectionForm form = new AddEditConnectionForm();

            if(form.EditConnection(connectionCopy) == DialogResult.OK) {

                connection.DatabaseType = form.Connection!.DatabaseType;
                connection.ConnectionString = form.Connection.ConnectionString;

                new ConnectionsFile().Save(mConnections);
                LoadListView();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e) {

            Cursor.Current = Cursors.WaitCursor;

            Connection connection = (Connection)lvwConnections.SelectedItems[0].Tag;

            if(MessageBox.Show("Are you sure you want to delete this connection?", "Delete Connection?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) {
                mConnections.Remove(connection);
                new ConnectionsFile().Save(mConnections);
                LoadListView();
            }
        }
    }
}