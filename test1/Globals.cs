using System;
using System.IO;
using Eto.Parse;
using Eto.Parse.Grammars;
using System.Diagnostics;

partial class Globals {
	static void Main(string[] args) {
		var fileContent = LoadFromResource("test1", "grammars", "test-grammar-3.ebnf");

		EbnfStyle style = (EbnfStyle)(
			(uint)EbnfStyle.Iso14977 
			//& ~(uint) EbnfStyle.WhitespaceSeparator	
			| (uint) EbnfStyle.EscapeTerminalStrings);

		Grammar parser;
		try
		{
			var grammar = new EbnfGrammar(style);
			var nameOfTheStartingRule = "SQL_procedure_statement";
			parser = grammar.Build(fileContent, nameOfTheStartingRule);
		}
		catch (Exception ex)
		{
			Trace.WriteLine (ex.ToString ());
/*
System.ArgumentException: the topParser specified is not found in this ebnf
  at Eto.Parse.Grammars.EbnfGrammar.Build (System.String bnf, System.String startParserName) [0x00048] in <filename unknown>:0 
  at Globals.Main (System.String[] args) [0x0002b] in /var/calculate/remote/distfiles/egit-src/SqlParser-on-EtoParse.git/test1/Program.cs:20 
*/
			throw;
		}

		var match = parser.Match("SELECT * FROM table1 AS a INNER JOIN table2 AS B ON a.id = b.a_id WHERE a.name = 'test'");
		if (match.Success == false) {
			Console.Out.WriteLine ("No luck!");
		}
		else {
			int token1 = 0;
			foreach (var m in match.Matches) {
				Console.WriteLine ("{0}: {1}, {2}", token1++, m.Text, m.Name);
			}
			Console.WriteLine ("Matches of query_specification");
			var query_specification = match.Matches[0];
			int token2 = 0;
			foreach (var m in query_specification.Matches) {
				Console.WriteLine ("{0}: {1}, {2}", token2++, m.Text, m.Name);
			}
			Console.WriteLine ("Matches of from_clause");
			var table_expression = query_specification.Matches [2];
			var from_clause = table_expression.Matches[0];
			var table_reference_list = from_clause.Matches [0];
			var table_reference = table_reference_list.Matches [0];
			var table_primary_or_joined_table = table_reference.Matches [0];
			int token3 = 0;
			foreach (var m in table_primary_or_joined_table.Matches) {
				Console.WriteLine ("{0}: {1}, {2}", token3++, m.Text, m.Name);
			}
			var joined_table = table_primary_or_joined_table.Matches [1];
			var qualified_join = joined_table.Matches [0];
			foreach (var m in qualified_join.Matches) {
				Console.WriteLine ("{0}: {1}, {2}", token3++, m.Text, m.Name);
			}
			// Now one more bug is in WHERE clause:
		}
	}
}
/*

0: SELECT * FROM table1 AS a INNER JOIN table2 AS B ON a.id = b.a_id WHERE a.nam, query_specification
1: =, equals_operator
2: 'test', character_string_literal
Matches of query_specification
0: SELECT, SELECT
1: *, select_list
2: FROM table1 AS a INNER JOIN table2 AS B ON a.id = b.a_id WHERE a.nam, table_expression
Matches of from_clause
0: table1 AS a, table_primary
1:  INNER JOIN table2 AS B ON a.id = b.a_id, joined_table
2: INNER, join_type
3: table2 AS B, table_reference
4: ON a.id = b.a_id, join_specification

*/