
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

namespace TypedQuery {

	public partial class TableGeneratorForm : Form {

		public TableGeneratorForm() {
			
			InitializeComponent();

			Sql.Settings.UseParameters = false;

			LoadConnections();
		}

		private void LoadConnections() {

			cboConnections.Items.Clear();
			cboConnections.SelectedItem = null;
			cboConnections.Text = string.Empty;
			lblConnections.BackColor = Color.Gold;

			btnLoad.Enabled = false;

			List<Connection.Connection> connections = new Connection.ConnectionsFile().Load();

			foreach(Connection.Connection connection in connections) {
				cboConnections.Items.Add(connection);
			}
		}

		private void btnLoad_Click(object sender, EventArgs e) {

			if(cboConnections.SelectedItem == null) {
				MessageBox.Show("Please select a connection");
				return;
			}

			Cursor.Current = Cursors.WaitCursor;

			Connection.Connection connection = (Connection.Connection) cboConnections.SelectedItem;

			IList<Logic.ITable> tableList;

			if(connection.DatabaseType == Sql.DatabaseType.PostgreSql) {
				Postgresql.PgDatabase.Instance.SetConnectionString(connection.ConnectionString);
				tableList = new Logic.PostgreSqlSchema().GetTableList();
			}
			else if(connection.DatabaseType == Sql.DatabaseType.Mssql) {
				SqlServer.SqlServerDatabase.Instance.SetConnectionString(connection.ConnectionString);
				tableList = new Logic.SqlServerSchema().GetTableList(SqlServer.SqlServerDatabase.Instance);
			}
			else
				throw new Exception("Unknown database type: " + connection.DatabaseType.ToString());

			Dictionary<string, SchemaNode> schemaNodeLookup = new Dictionary<string, SchemaNode>();

			List<SchemaNode> schemaNodes = new List<SchemaNode>();

			foreach(Logic.ITable table in tableList) {

				string schemaKey = table.Schema.ToLower();

				SchemaNode schemaNode;

				if(!schemaNodeLookup.TryGetValue(schemaKey, out schemaNode)) {
					schemaNode = new SchemaNode(table.Schema);
					schemaNodeLookup.Add(schemaKey, schemaNode);
					schemaNodes.Add(schemaNode);
				}

				schemaNode.Nodes.Add(new TableNode(table));
			}

			List<Logic.IStoredProcedureDetail> storedProcedures = null;

			if(connection.DatabaseType == Sql.DatabaseType.PostgreSql) {
				//Do nothing
			}
			else if(connection.DatabaseType == Sql.DatabaseType.Mssql) {
				using(System.Data.Common.DbConnection dbConnection = SqlServer.SqlServerDatabase.Instance.GetConnection(true)) {
					storedProcedures = Logic.SqlServerSchema.GetStoredProcedures(dbConnection);
				}
			}
			else
				throw new Exception("Unknown database type: " + connection.DatabaseType.ToString());			

			if(storedProcedures != null && storedProcedures.Count > 0){

				foreach(Logic.IStoredProcedureDetail spDetail in storedProcedures) {

					string schemaKey = spDetail.Schema.ToLower();

					SchemaNode schemaNode;

					if(!schemaNodeLookup.TryGetValue(schemaKey, out schemaNode)) {
						schemaNode = new SchemaNode(spDetail.Schema);
						schemaNodeLookup.Add(schemaKey, schemaNode);
						schemaNodes.Add(schemaNode);
					}

					schemaNode.Nodes.Add(new StoredProcedureNode(spDetail));
				}
			}

			tvwSchema.Nodes.Clear();
			tvwSchema.Nodes.AddRange(schemaNodes.ToArray());

			if(tvwSchema.Nodes.Count == 1)
				tvwSchema.Nodes[0].Expand();

			foreach(SchemaNode schemaNode in schemaNodes) {

				if(schemaNode.Text == "public") {
					schemaNode.Expand();
					break;
				}
			}
			Cursor.Current = Cursors.Arrow;
		}

		private class SchemaNode : TreeNode {

			public SchemaNode(string pSchemaName) : base(pSchemaName) {

			}
		}

		public class TableNode : TreeNode {

			public Logic.ITable Table { get; private set; }

			public TableNode(Logic.ITable pTable) {
				Table = pTable;
				Text = pTable.TableName + (pTable.IsView ? " (View)" : string.Empty);
			}
		}

		public class StoredProcedureNode : TreeNode {

			public Logic.IStoredProcedureDetail StoredProcedure { get; private set; }

			public StoredProcedureNode(Logic.IStoredProcedureDetail pStoredProcedure) {
				StoredProcedure = pStoredProcedure;
				Text = pStoredProcedure.Name + " (Stored Procedure)";
			}
		}

		private void tvwSchema_AfterSelect(object sender, TreeViewEventArgs e) {
			GenerateCodeDefintion(false);
		}

		private void btnClose_Click(object sender, EventArgs e) {
			Close();
		}

		private void GenerateCodeDefintion(bool pRegenerate) {

			Cursor.Current = Cursors.WaitCursor;

			if(tvwSchema.SelectedNode != null && tvwSchema.SelectedNode is TableNode) {

				Logic.ITable table = ((TableNode)tvwSchema.SelectedNode).Table;

				Logic.ITableDetails tableDetails;
				string columnPrefix = pRegenerate ? txtColumnPrefix.Text : string.Empty;

				bool guessPrefix = string.IsNullOrEmpty(txtColumnPrefix.Text);
				
				if(table.DatabaseType == Sql.DatabaseType.PostgreSql) {

					string errorText;
					new Logic.PostgreSqlSchema().GetTableDetails(table.TableName, table.Schema, out tableDetails, out errorText);	//TODO: Show any errors

					txtTableDefinition.Text = Logic.CodeGenerator.GenerateTableAndRowCode(tableDetails, ref columnPrefix, chkIncludeSchema.Checked, guessPrefix, chkGenerateCommentMetaData.Checked, chkRemoveUnderscores.Checked);
					txtClassCode.Text = Logic.CodeGenerator.GenerateClassCode(tableDetails, columnPrefix, chkRemoveUnderscores.Checked);
				}
				else if(table.DatabaseType == Sql.DatabaseType.Mssql) {
					
					string errorText;
					new Logic.SqlServerSchema().GetTableDetails(SqlServer.SqlServerDatabase.Instance, table.TableName, table.Schema, out tableDetails, out errorText);	//TODO: Show any errors
					txtTableDefinition.Text = Logic.CodeGenerator.GenerateTableAndRowCode(tableDetails, ref columnPrefix, chkIncludeSchema.Checked, guessPrefix, chkGenerateCommentMetaData.Checked, chkRemoveUnderscores.Checked);
					txtClassCode.Text = Logic.CodeGenerator.GenerateClassCode(tableDetails, columnPrefix, chkRemoveUnderscores.Checked);
				}
				else
					throw new Exception("Unnknown database type: " + table.DatabaseType.ToString());

				if(!pRegenerate || guessPrefix)
					txtColumnPrefix.Text = columnPrefix;
			}
			else if(tvwSchema.SelectedNode != null && tvwSchema.SelectedNode is StoredProcedureNode) {

				Logic.IStoredProcedureDetail spDetail = ((StoredProcedureNode)tvwSchema.SelectedNode).StoredProcedure;

				string columnPrefix = string.Empty;
				txtTableDefinition.Text = Logic.CodeGenerator.GenerateStoredProcedureCode(spDetail, txtColumnPrefix.Text, chkIncludeSchema.Checked);
				txtClassCode.Text = "Note: Class code is not generated for stored procedures";
				txtColumnPrefix.Text = string.Empty;
			}
			else {
				txtTableDefinition.Text = string.Empty;
				txtColumnPrefix.Text = string.Empty;
			}
			Cursor.Current = Cursors.Arrow;	
		}

		private bool mIgnoreColumnPrefixEvent = false;

		private void txtColumnPrefix_TextChanged(object sender, EventArgs e) {

			if(mIgnoreColumnPrefixEvent)
				return;

			if(tvwSchema.SelectedNode != null && tvwSchema.SelectedNode is TableNode) {
				GenerateCodeDefintion(true);
			}

			if(!string.IsNullOrEmpty(txtColumnPrefix.Text))
				txtColumnPrefix.BackColor = Color.Gold;
			else
				txtColumnPrefix.BackColor = System.Drawing.SystemColors.Window;
		}				

		private void chkIncludeSchema_CheckedChanged(object sender, EventArgs e) {

			if(tvwSchema.SelectedNode != null && tvwSchema.SelectedNode is TableNode) {
				GenerateCodeDefintion(true);
			}
		}

		private void txtTableDefinition_KeyDown(object sender, KeyEventArgs e) {
			if(e.Control && e.KeyCode == Keys.A)
				txtTableDefinition.SelectAll();
		}

		private void chkGenerateCommentsMetaData_CheckedChanged(object sender, EventArgs e) {

			if(tvwSchema.SelectedNode != null && tvwSchema.SelectedNode is TableNode) {
				GenerateCodeDefintion(true);
			}
		}

		private void button1_Click(object sender, EventArgs e) {
			Cursor.Current = Cursors.WaitCursor;
			TypedQuery.Connection.ConnectionsForm connectionsForm = new Connection.ConnectionsForm();
			connectionsForm.ShowDialog();
			LoadConnections();
			Cursor.Current = Cursors.Arrow;
		}

		private void cboConnections_SelectedIndexChanged(object sender, EventArgs e) {

			btnLoad.Enabled = cboConnections.SelectedItem != null;

			if(cboConnections.SelectedItem == null)
				lblConnections.BackColor = Color.Gold;
			else
				lblConnections.BackColor = System.Drawing.SystemColors.Control;
		}

        private void chkRemoveUnderscores_CheckedChanged(object sender, EventArgs e) {
            if (tvwSchema.SelectedNode != null && tvwSchema.SelectedNode is TableNode) {
                GenerateCodeDefintion(true);
            }
        }
    }
}