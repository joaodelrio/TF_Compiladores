using Antlr4.Runtime;

namespace Lang
{
    public class LangErrorListener : BaseErrorListener
    {
        public Boolean HasErrors { get; private set; } = false;
        public List<string> ErrorMessages  { get; private set; } = new List<string>();
        public override void SyntaxError(TextWriter output, 
                                        IRecognizer recognizer, 
                                        IToken offendingSymbol, 
                                        int line, 
                                        int charPositionInLine, 
                                        string msg, 
                                        RecognitionException e)
        {
            HasErrors = true;
            ErrorMessages.Add($"Syntax error at line {line}, position {charPositionInLine}: {msg}");
        }
    }
}