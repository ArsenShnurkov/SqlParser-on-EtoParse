using System;
using System.IO;
using Eto.Parse;
using Eto.Parse.Grammars;
using System.Diagnostics;

partial class Globals {
	static void Main(string[] args) {
		var fileContent = LoadFromResource("test1", "grammars", "test-grammar-2.ebnf");

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
			int token = 0;
			foreach (var m in match.Matches) {
				Console.WriteLine ("{0}: {1}, {2}", ++token, m.Text, m.Name);
			}
		}
	}
}

/*
Parsed as:
1: SELECT, reserved_word
2: *, asterisk
3: FROM, reserved_word
4: table1, regular_identifier
5: AS, reserved_word
6: a, regular_identifier
7: INNER, reserved_word
8: JOIN, reserved_word
9: table2, regular_identifier
10: AS, reserved_word
11: B, regular_identifier
12: ON, reserved_word
13: a, regular_identifier
14: ., period
15: id, regular_identifier
16: =, equals_operator
17: b, regular_identifier
18: ., period
19: a_id, regular_identifier
20: WHERE, reserved_word
21: a, regular_identifier
22: ., period
23: name, regular_identifier
24: =, equals_operator
25: 'test', character_string_literal
*/