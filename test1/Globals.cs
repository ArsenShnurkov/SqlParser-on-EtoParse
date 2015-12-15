using System;
using System.IO;
using Eto.Parse;
using Eto.Parse.Grammars;
using System.Diagnostics;
using System.Collections.Generic;

partial class Globals {
	const string nameOfTheStartingRule = "SQL_procedure_statement";
	static void Main(string[] args) {
		var fileContent = LoadFromResource("test1", "grammars", "test-grammar-3.ebnf");

		EbnfStyle style = (EbnfStyle)(
			(uint)EbnfStyle.Iso14977 
			//& ~(uint) EbnfStyle.WhitespaceSeparator	
			| (uint) EbnfStyle.EscapeTerminalStrings);

		EbnfGrammar grammar;
		Grammar parser;
		try
		{
			grammar = new EbnfGrammar(style);
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
			DumpAllMatches (match, nameOfTheStartingRule);
		}
	}
	public static void DumpAllMatches(Match m, string name)
	{
		for (int pos = 0; pos < m.Matches.Count; pos++)
		{
			Match nm = m.Matches [pos];
			var full_name = nm.Name + " <- " + name;

			bool low = IsTooLow (nm.Name);
			if (nm.Text != m.Text) {
				string fmt = "\"{0}\"" + Environment.NewLine + "\t" + "{1}";
				Console.WriteLine (fmt, nm.Text, full_name);
			}
			if (!low) {
				DumpAllMatches (nm, full_name);
			}
		}
	}
	static bool IsTooLow(string rule)
	{
		if (rule == "identifier") return true;
		if (rule == "character_string_literal") return true;
		return false;
	}
}

/*
"SELECT"
	SELECT <- query_specification <- SQL_procedure_statement
"*"
	select_list <- query_specification <- SQL_procedure_statement
"FROM table1 AS a INNER JOIN table2 AS B ON a.id = b.a_id WHERE a.name = 'test"
	table_expression <- query_specification <- SQL_procedure_statement
"FROM table1 AS a INNER JOIN table2 AS B ON a.id = b.a_id"
	from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"table1 AS a INNER JOIN table2 AS B ON a.id = b.a_id"
	table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"table1 AS a"
	table_primary <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"table1"
	table_or_query_name <- table_primary <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"a"
	correlation_name <- table_primary <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
" INNER JOIN table2 AS B ON a.id = b.a_id"
	joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"INNER"
	join_type <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"table2 AS B"
	table_reference <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"table2"
	table_or_query_name <- table_primary <- table_primary_or_joined_table <- table_reference <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"B"
	correlation_name <- table_primary <- table_primary_or_joined_table <- table_reference <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"ON a.id = b.a_id"
	join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"a.id = b.a_id"
	search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"a.id"
	row_value_predicand <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"a"
	identifier <- identifier_chain <- basic_identifier_chain <- column_reference <- nonparenthesized_value_expression_primary <- row_value_special_case <- row_value_predicand <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"."
	period <- identifier_chain <- basic_identifier_chain <- column_reference <- nonparenthesized_value_expression_primary <- row_value_special_case <- row_value_predicand <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"id"
	identifier <- identifier_chain <- basic_identifier_chain <- column_reference <- nonparenthesized_value_expression_primary <- row_value_special_case <- row_value_predicand <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"= b.a_id"
	comparison_predicate_part_2 <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"="
	comp_op <- comparison_predicate_part_2 <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"b.a_id"
	row_value_predicand <- comparison_predicate_part_2 <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"b"
	identifier <- identifier_chain <- basic_identifier_chain <- column_reference <- nonparenthesized_value_expression_primary <- row_value_special_case <- row_value_predicand <- comparison_predicate_part_2 <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"."
	period <- identifier_chain <- basic_identifier_chain <- column_reference <- nonparenthesized_value_expression_primary <- row_value_special_case <- row_value_predicand <- comparison_predicate_part_2 <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"a_id"
	identifier <- identifier_chain <- basic_identifier_chain <- column_reference <- nonparenthesized_value_expression_primary <- row_value_special_case <- row_value_predicand <- comparison_predicate_part_2 <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- join_condition <- join_specification <- qualified_join <- joined_table <- table_primary_or_joined_table <- table_reference <- table_reference_list <- from_clause <- table_expression <- query_specification <- SQL_procedure_statement
"WHERE a.name = 'test'"
	where_clause <- table_expression <- query_specification <- SQL_procedure_statement
"a.name = 'test'"
	search_condition <- where_clause <- table_expression <- query_specification <- SQL_procedure_statement
"a.name"
	row_value_predicand <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- where_clause <- table_expression <- query_specification <- SQL_procedure_statement
"a"
	identifier <- identifier_chain <- basic_identifier_chain <- column_reference <- nonparenthesized_value_expression_primary <- row_value_special_case <- row_value_predicand <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- where_clause <- table_expression <- query_specification <- SQL_procedure_statement
"."
	period <- identifier_chain <- basic_identifier_chain <- column_reference <- nonparenthesized_value_expression_primary <- row_value_special_case <- row_value_predicand <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- where_clause <- table_expression <- query_specification <- SQL_procedure_statement
"name"
	identifier <- identifier_chain <- basic_identifier_chain <- column_reference <- nonparenthesized_value_expression_primary <- row_value_special_case <- row_value_predicand <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- where_clause <- table_expression <- query_specification <- SQL_procedure_statement
"= 'test'"
	comparison_predicate_part_2 <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- where_clause <- table_expression <- query_specification <- SQL_procedure_statement
"="
	comp_op <- comparison_predicate_part_2 <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- where_clause <- table_expression <- query_specification <- SQL_procedure_statement
"'test'"
	row_value_predicand <- comparison_predicate_part_2 <- comparison_predicate <- predicate <- boolean_primary <- boolean_test <- boolean_factor <- boolean_term <- boolean_value_expression <- search_condition <- where_clause <- table_expression <- query_specification <- SQL_procedure_statement

*/