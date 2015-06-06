
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
using System.Windows.Forms;

namespace TypedQuery {

	static class Program {
		
		[STAThread]
		private static void Main() {
			Application.ThreadException += Application_ThreadException;
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new TableGeneratorForm());

			Npgsql.NpgsqlConnection.ClearAllPools();	//Fixes issue with NpgSql throwing an exception
		}

		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e) {
			MessageBox.Show(e.Exception.Message, "Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}