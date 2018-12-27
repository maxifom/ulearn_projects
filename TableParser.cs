using System.Collections.Generic;
using System.Text;
namespace TableParser
{

    public class FieldsParserTask
    {
        const char DOUBLE_QUOTE = '\"';
        const char SINGLE_QUOTE = '\'';
        const char SLASH = '\\';
        const char WHITESPACE = ' ';

        //Метод для парсинга и сохранения одного простого поля
        static Token ParseEasyField(string line, int i)
        {
            int startIndex = i;
            while (i < line.Length && line[i] != DOUBLE_QUOTE && line[i] != SINGLE_QUOTE && line[i] != WHITESPACE)
            {
                i++;
            }
            return new Token(line, startIndex, i - startIndex);
        }

        //Метод для парсинга и сохранения одного поля с кавычками
        static Token ParseQuoteField(string line, int i, char quoteType)
        {
            int startIndex = i;
            StringBuilder currentString = new StringBuilder();
            while (i < line.Length && line[i] != quoteType)
            {
                if (line[i] == SLASH)
                {
                    i++;
                }
                currentString.Append(line[i]);
                i++;
            }
            return new Token(currentString.ToString(), startIndex, i - startIndex + 1);
        }

        // Метод для парсинга строки в набор простых полей и полей с кавычками
        public static List<string> ParseLine(string line)
        {
            List<string> fields = new List<string>();
            int i = 0;
            while (i < line.Length)
            {
                if (line[i] == DOUBLE_QUOTE || line[i] == SINGLE_QUOTE)
                {
                    Token tokenQuote = ParseQuoteField(line, i + 1, line[i]);
                    fields.Add(tokenQuote.Value);
                    i = tokenQuote.GetIndexNextToToken();
                }
                else if (line[i] != ' ')
                {
                    Token tokenEasy = ParseEasyField(line, i);
                    fields.Add(tokenEasy.Value.Substring(tokenEasy.StartIndex, tokenEasy.Length));
                    i = tokenEasy.GetIndexNextToToken();
                }
                else
                {
                    i++;
                }
            }
            return fields;
        }
    }
}