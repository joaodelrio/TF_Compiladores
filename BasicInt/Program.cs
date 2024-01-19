using Antlr4.Runtime;

namespace BasicInt;

class Program
{
    static void Main(string[] args)
    {
        string text = File.ReadAllText("D:/Compilador TRABALHO FINAL/input.txt");
        AntlrInputStream inputStream = new AntlrInputStream(text.ToString());
        BasicCLexer lexer = new BasicCLexer(inputStream);
        CommonTokenStream stream = new CommonTokenStream(lexer);
        BasicCParser parser = new BasicCParser(stream);
        parser.prog();
    }
}
