
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

using System;
using System.Collections.Generic;

namespace Postgresql.Columns {

	public sealed class Table : Sql.ATable {

		public readonly static Table Instance = new Table();

		public Sql.Column.StringColumn Table_catalog { get; private set; }
		public Sql.Column.StringColumn Table_schema { get; private set; }
		public Sql.Column.StringColumn Table_name { get; private set; }
		public Sql.Column.StringColumn Column_name { get; private set; }
		public Sql.Column.NIntegerColumn Ordinal_position { get; private set; }
		public Sql.Column.StringColumn Column_default { get; private set; }
		public Sql.Column.StringColumn Is_nullable { get; private set; }
		public Sql.Column.StringColumn Data_type { get; private set; }
		public Sql.Column.NIntegerColumn Character_maximum_length { get; private set; }
		public Sql.Column.NIntegerColumn Character_octet_length { get; private set; }
		public Sql.Column.NIntegerColumn Numeric_precision { get; private set; }
		public Sql.Column.NIntegerColumn Numeric_precision_radix { get; private set; }
		public Sql.Column.NIntegerColumn Numeric_scale { get; private set; }
		public Sql.Column.NIntegerColumn Datetime_precision { get; private set; }
		public Sql.Column.StringColumn Interval_type { get; private set; }
		public Sql.Column.StringColumn Interval_precision { get; private set; }
		public Sql.Column.StringColumn Character_set_catalog { get; private set; }
		public Sql.Column.StringColumn Character_set_schema { get; private set; }
		public Sql.Column.StringColumn Character_set_name { get; private set; }
		public Sql.Column.StringColumn Collation_catalog { get; private set; }
		public Sql.Column.StringColumn Collation_schema { get; private set; }
		public Sql.Column.StringColumn Collation_name { get; private set; }
		public Sql.Column.StringColumn Domain_catalog { get; private set; }
		public Sql.Column.StringColumn Domain_schema { get; private set; }
		public Sql.Column.StringColumn Domain_name { get; private set; }
		public Sql.Column.StringColumn Udt_catalog { get; private set; }
		public Sql.Column.StringColumn Udt_schema { get; private set; }
		public Sql.Column.StringColumn Udt_name { get; private set; }
		public Sql.Column.StringColumn Scope_catalog { get; private set; }
		public Sql.Column.StringColumn Scope_schema { get; private set; }
		public Sql.Column.StringColumn Scope_name { get; private set; }
		public Sql.Column.NIntegerColumn Maximum_cardinality { get; private set; }
		public Sql.Column.StringColumn Dtd_identifier { get; private set; }
		public Sql.Column.StringColumn Is_self_referencing { get; private set; }
		public Sql.Column.StringColumn Is_identity { get; private set; }
		public Sql.Column.StringColumn Identity_generation { get; private set; }
		public Sql.Column.StringColumn Identity_start { get; private set; }
		public Sql.Column.StringColumn Identity_increment { get; private set; }
		public Sql.Column.StringColumn Identity_maximum { get; private set; }
		public Sql.Column.StringColumn Identity_minimum { get; private set; }
		public Sql.Column.StringColumn Identity_cycle { get; private set; }
		public Sql.Column.StringColumn Is_generated { get; private set; }
		public Sql.Column.StringColumn Generation_expression { get; private set; }
		public Sql.Column.StringColumn Is_updatable { get; private set; }

		public Table()
			: base(PgDatabase.Instance, "columns", "information_schema", true, typeof(Row)) {

			Table_catalog = new Sql.Column.StringColumn(this, "table_catalog", false, int.MaxValue);
			Table_schema = new Sql.Column.StringColumn(this, "table_schema", false, int.MaxValue);
			Table_name = new Sql.Column.StringColumn(this, "table_name", false, int.MaxValue);
			Column_name = new Sql.Column.StringColumn(this, "column_name", false, int.MaxValue);
			Ordinal_position = new Sql.Column.NIntegerColumn(this, "ordinal_position", false);
			Column_default = new Sql.Column.StringColumn(this, "column_default", false, int.MaxValue);
			Is_nullable = new Sql.Column.StringColumn(this, "is_nullable", false, int.MaxValue);
			Data_type = new Sql.Column.StringColumn(this, "data_type", false, int.MaxValue);
			Character_maximum_length = new Sql.Column.NIntegerColumn(this, "character_maximum_length", false);
			Character_octet_length = new Sql.Column.NIntegerColumn(this, "character_octet_length", false);
			Numeric_precision = new Sql.Column.NIntegerColumn(this, "numeric_precision", false);
			Numeric_precision_radix = new Sql.Column.NIntegerColumn(this, "numeric_precision_radix", false);
			Numeric_scale = new Sql.Column.NIntegerColumn(this, "numeric_scale", false);
			Datetime_precision = new Sql.Column.NIntegerColumn(this, "datetime_precision", false);
			Interval_type = new Sql.Column.StringColumn(this, "interval_type", false, int.MaxValue);
			Interval_precision = new Sql.Column.StringColumn(this, "interval_precision", false, int.MaxValue);
			Character_set_catalog = new Sql.Column.StringColumn(this, "character_set_catalog", false, int.MaxValue);
			Character_set_schema = new Sql.Column.StringColumn(this, "character_set_schema", false, int.MaxValue);
			Character_set_name = new Sql.Column.StringColumn(this, "character_set_name", false, int.MaxValue);
			Collation_catalog = new Sql.Column.StringColumn(this, "collation_catalog", false, int.MaxValue);
			Collation_schema = new Sql.Column.StringColumn(this, "collation_schema", false, int.MaxValue);
			Collation_name = new Sql.Column.StringColumn(this, "collation_name", false, int.MaxValue);
			Domain_catalog = new Sql.Column.StringColumn(this, "domain_catalog", false, int.MaxValue);
			Domain_schema = new Sql.Column.StringColumn(this, "domain_schema", false, int.MaxValue);
			Domain_name = new Sql.Column.StringColumn(this, "domain_name", false, int.MaxValue);
			Udt_catalog = new Sql.Column.StringColumn(this, "udt_catalog", false, int.MaxValue);
			Udt_schema = new Sql.Column.StringColumn(this, "udt_schema", false, int.MaxValue);
			Udt_name = new Sql.Column.StringColumn(this, "udt_name", false, int.MaxValue);
			Scope_catalog = new Sql.Column.StringColumn(this, "scope_catalog", false, int.MaxValue);
			Scope_schema = new Sql.Column.StringColumn(this, "scope_schema", false, int.MaxValue);
			Scope_name = new Sql.Column.StringColumn(this, "scope_name", false, int.MaxValue);
			Maximum_cardinality = new Sql.Column.NIntegerColumn(this, "maximum_cardinality", false);
			Dtd_identifier = new Sql.Column.StringColumn(this, "dtd_identifier", false, int.MaxValue);
			Is_self_referencing = new Sql.Column.StringColumn(this, "is_self_referencing", false, int.MaxValue);
			Is_identity = new Sql.Column.StringColumn(this, "is_identity", false, int.MaxValue);
			Identity_generation = new Sql.Column.StringColumn(this, "identity_generation", false, int.MaxValue);
			Identity_start = new Sql.Column.StringColumn(this, "identity_start", false, int.MaxValue);
			Identity_increment = new Sql.Column.StringColumn(this, "identity_increment", false, int.MaxValue);
			Identity_maximum = new Sql.Column.StringColumn(this, "identity_maximum", false, int.MaxValue);
			Identity_minimum = new Sql.Column.StringColumn(this, "identity_minimum", false, int.MaxValue);
			Identity_cycle = new Sql.Column.StringColumn(this, "identity_cycle", false, int.MaxValue);
			Is_generated = new Sql.Column.StringColumn(this, "is_generated", false, int.MaxValue);
			Generation_expression = new Sql.Column.StringColumn(this, "generation_expression", false, int.MaxValue);
			Is_updatable = new Sql.Column.StringColumn(this, "is_updatable", false, int.MaxValue);

			AddColumns(Table_catalog, Table_schema, Table_name, Column_name, Ordinal_position, Column_default, Is_nullable, Data_type, Character_maximum_length, Character_octet_length, Numeric_precision, Numeric_precision_radix, Numeric_scale, Datetime_precision, Interval_type, Interval_precision, Character_set_catalog, Character_set_schema, Character_set_name, Collation_catalog, Collation_schema, Collation_name, Domain_catalog, Domain_schema, Domain_name, Udt_catalog, Udt_schema, Udt_name, Scope_catalog, Scope_schema, Scope_name, Maximum_cardinality, Dtd_identifier, Is_self_referencing, Is_identity, Identity_generation, Identity_start, Identity_increment, Identity_maximum, Identity_minimum, Identity_cycle, Is_generated, Generation_expression, Is_updatable);
		}

		public Row this[int pIndex, Sql.IResult pResult] {
			get { return (Row)pResult.GetRow(this, pIndex); }
		}
	}

	public sealed class Row : Sql.ARow {

		private new Table Tbl {
			get { return (Table)base.Tbl; }
		}

		public Row()
			: base(Table.Instance) {
		}

		public string Table_catalog {
			get { return Tbl.Table_catalog.ValueOf(this); }
		}

		public string Table_schema {
			get { return Tbl.Table_schema.ValueOf(this); }
		}

		public string Table_name {
			get { return Tbl.Table_name.ValueOf(this); }
		}

		public string Column_name {
			get { return Tbl.Column_name.ValueOf(this); }
		}

		public int? Ordinal_position {
			get { return Tbl.Ordinal_position.ValueOf(this); }
		}

		public string Column_default {
			get { return Tbl.Column_default.ValueOf(this); }
		}

		public string Is_nullable {
			get { return Tbl.Is_nullable.ValueOf(this); }
		}

		public string Data_type {
			get { return Tbl.Data_type.ValueOf(this); }
		}

		public int? Character_maximum_length {
			get { return Tbl.Character_maximum_length.ValueOf(this); }
		}

		public int? Character_octet_length {
			get { return Tbl.Character_octet_length.ValueOf(this); }
		}

		public int? Numeric_precision {
			get { return Tbl.Numeric_precision.ValueOf(this); }
		}

		public int? Numeric_precision_radix {
			get { return Tbl.Numeric_precision_radix.ValueOf(this); }
		}

		public int? Numeric_scale {
			get { return Tbl.Numeric_scale.ValueOf(this); }
		}

		public int? Datetime_precision {
			get { return Tbl.Datetime_precision.ValueOf(this); }
		}

		public string Interval_type {
			get { return Tbl.Interval_type.ValueOf(this); }
		}

		public string Interval_precision {
			get { return Tbl.Interval_precision.ValueOf(this); }
		}

		public string Character_set_catalog {
			get { return Tbl.Character_set_catalog.ValueOf(this); }
		}

		public string Character_set_schema {
			get { return Tbl.Character_set_schema.ValueOf(this); }
		}

		public string Character_set_name {
			get { return Tbl.Character_set_name.ValueOf(this); }
		}

		public string Collation_catalog {
			get { return Tbl.Collation_catalog.ValueOf(this); }
		}

		public string Collation_schema {
			get { return Tbl.Collation_schema.ValueOf(this); }
		}

		public string Collation_name {
			get { return Tbl.Collation_name.ValueOf(this); }
		}

		public string Domain_catalog {
			get { return Tbl.Domain_catalog.ValueOf(this); }
		}

		public string Domain_schema {
			get { return Tbl.Domain_schema.ValueOf(this); }
		}

		public string Domain_name {
			get { return Tbl.Domain_name.ValueOf(this); }
		}

		public string Udt_catalog {
			get { return Tbl.Udt_catalog.ValueOf(this); }
		}

		public string Udt_schema {
			get { return Tbl.Udt_schema.ValueOf(this); }
		}

		public string Udt_name {
			get { return Tbl.Udt_name.ValueOf(this); }
		}

		public string Scope_catalog {
			get { return Tbl.Scope_catalog.ValueOf(this); }
		}

		public string Scope_schema {
			get { return Tbl.Scope_schema.ValueOf(this); }
		}

		public string Scope_name {
			get { return Tbl.Scope_name.ValueOf(this); }
		}

		public int? Maximum_cardinality {
			get { return Tbl.Maximum_cardinality.ValueOf(this); }
		}

		public string Dtd_identifier {
			get { return Tbl.Dtd_identifier.ValueOf(this); }
		}

		public string Is_self_referencing {
			get { return Tbl.Is_self_referencing.ValueOf(this); }
		}

		public string Is_identity {
			get { return Tbl.Is_identity.ValueOf(this); }
		}

		public string Identity_generation {
			get { return Tbl.Identity_generation.ValueOf(this); }
		}

		public string Identity_start {
			get { return Tbl.Identity_start.ValueOf(this); }
		}

		public string Identity_increment {
			get { return Tbl.Identity_increment.ValueOf(this); }
		}

		public string Identity_maximum {
			get { return Tbl.Identity_maximum.ValueOf(this); }
		}

		public string Identity_minimum {
			get { return Tbl.Identity_minimum.ValueOf(this); }
		}

		public string Identity_cycle {
			get { return Tbl.Identity_cycle.ValueOf(this); }
		}

		public string Is_generated {
			get { return Tbl.Is_generated.ValueOf(this); }
		}

		public string Generation_expression {
			get { return Tbl.Generation_expression.ValueOf(this); }
		}

		public string Is_updatable {
			get { return Tbl.Is_updatable.ValueOf(this); }
		}
	}
}