using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using BasicInt.Grammar;
using Lang;

namespace BasicInt;

class Program
{
    static void Main(string[] args)
    {
        string text = File.ReadAllText("C://Users//devma//Downloads//Projetos//Compilador TRABALHO FINAL//input.txt");
        AntlrInputStream inputStream = new AntlrInputStream(text.ToString());
        BasicCLexer lexer = new BasicCLexer(inputStream);
        CommonTokenStream stream = new CommonTokenStream(lexer);
        BasicCParser parser = new BasicCParser(stream);
        
        //error listener
        LangErrorListener errorListener = new LangErrorListener();
        parser.RemoveErrorListeners();
        parser.AddErrorListener(errorListener);

        //listener
        LangListener langListener = new LangListener();
        parser.RemoveParseListeners();
        parser.AddParseListener(langListener);

        IParseTree? tree = null;
        try
        {
            tree = parser.prog();
            if (errorListener.HasErrors){
                Console.WriteLine("Errors!");
                errorListener.ErrorMessages.ForEach(e => Console.WriteLine(e));
                tree = null;
            }
            if (langListener.HasErrors){
                Console.WriteLine("Semantic Errors!");
                langListener.ErrorMessages.ForEach(e => Console.WriteLine(e));
                tree = null;
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        if (tree != null)
        {
            var langVisitor = new LangVisitor();
            langVisitor.Visit(tree);
        }
    }
}
