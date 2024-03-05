using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using BasicInt.Grammar;

namespace Lang
{

    // durante a criação da arvore sintática, o parser chama o método Exit de cada regra
    // para validar se está semanticamente correto
    // ex: int x = "string"; //erro
    // ex: int x = 1; //correto
    // ex: int soma(){ return "string"} //erro
    // ex: int soma(){ return 4 } //correto
    // ex: if ("string") //erro
    // ex: if (boolean) //correto
    // ex: if (boolean) //Verificar se eh true ou false
    // ex: int a + float b = float c //retorna float e dar um WARNING de perda de precisao
    // ex: #import biblioteca //verifica se a biblioteca existe
    public class LangListener : BasicCBaseListener
    {
        public Dictionary<string, IParseTree> Functions { get; protected set; } = new Dictionary<string, IParseTree>();
        public HashSet<string> Variables { get; private set; } = new HashSet<string>();
        public Dictionary<string, object> LocalVariables { get; private set; } = new Dictionary<string, object>();

        public Boolean HasErrors { get; private set; } = false;
        public List<string> ErrorMessages { get; private set; } = new List<string>();

        struct LocalVariablesAttributs
        {
            public int type;
            public object? value;
            public string name;
            public LocalVariablesAttributs(int _type, object? _value, string _name)
            {
                type = _type;
                value = _value;
                name = _name;
            }

            public int GetType()
            {
                return type;
            }

            public object? GetValue()
            {
                return value;
            }

            public string GetName()
            {
                return name;
            }

            public void SetType(int _type)
            {
                type = _type;
            }

            public void SetValue(object? _value)
            {
                value = _value;
            }

            public void SetName(string _name)
            {
                name = _name;
            }
        }


        public override void ExitVariavelNova([NotNull] BasicCParser.VariavelNovaContext context) 
        {
            //verificar se existe e verificar tipo (lança erro) 
            var varName = context.VAR().GetText();

            if (Variables.Contains(varName)) {
                HasErrors = true;
                ErrorMessages.Add("Variável existente");
            } else 
            {
                Variables.Add(varName);
            }
            
        }

        public override void ExitVariavelNovaString([NotNull] BasicCParser.VariavelNovaStringContext context)
        {
            var varName = context.VAR().GetText();
            
            if (Variables.Contains(varName)) {
                HasErrors = true;
                ErrorMessages.Add("Variável existente");
            }
            else {
                Variables.Add(varName);
            }
        }

        public override void ExitVariavelNovaBoolean([NotNull] BasicCParser.VariavelNovaBooleanContext context)
        {
            var varName = context.VAR().GetText();
            
            if (Variables.Contains(varName)) {
                HasErrors = true;
                ErrorMessages.Add("Variável existente");
            }
            else {
                Variables.Add(varName);
            }
        }

        public override void ExitVariavelExistenteBoolean([NotNull] BasicCParser.VariavelExistenteBooleanContext context)
        {
            
            //verificar se existe (lança erro se não existir)
            var varName = context.VAR().GetText();
            var varNewValue = context.BOOL().GetText();

            if (!Variables.Contains(varName)) 
            {
                HasErrors = true;
                ErrorMessages.Add("Variável inexistente, crie ela primeiro");
            }
        }

        public override void ExitVariavelExistente([NotNull] BasicCParser.VariavelExistenteContext context)
        {
            //verificar se existe (lança erro se não existir)
            var varName = context.VAR().GetText();

            if (!Variables.Contains(varName)) 
            {
                HasErrors = true;
                ErrorMessages.Add("Variável inexistente");
            }
        }

        public override void ExitVariavelExistenteString([NotNull] BasicCParser.VariavelExistenteStringContext context)
        {
            //verificar se existe (lança erro se não existir) e atualizar valor 
            var varName = context.VAR().GetText();
            var varNewValue = context.STR().GetText();

            if (!Variables.Contains(varName))
            {
                HasErrors = true;
                ErrorMessages.Add("Variável inexistente, crie ela primeiro");
            }
        }

        public override void ExitOutputVar([NotNull] BasicCParser.OutputVarContext context)
        {
            var varName = context.VAR().GetText();

            if (!Variables.Contains(varName)) {
                HasErrors = true;
                ErrorMessages.Add("Variável inexistente, crie ela antes de utilizar.");
            }
        }

        public override void ExitInputVar([NotNull] BasicCParser.InputVarContext context)
        {
            var varName = context.VAR().GetText();
            
            if (!Variables.Contains(varName)) {
                Variables.Add(varName);
            }
        }

        public override void ExitFnWithReturn([NotNull] BasicCParser.FnWithReturnContext context)
        {
            //verificar se nome de função já existe (lança erro se existir) e cria função
            var fnName = context.VAR().GetText();
            var fnReturnType = context.TYPE.GetType();
            
            if (Functions.ContainsKey(fnName))
            {
                HasErrors = true;
                ErrorMessages.Add("Function " + fnName + " already defined");
            } 
            else 
            {
                Functions.Add(fnName, context);
            }
            
        }

        public override void ExitFnWithoutReturn([NotNull] BasicCParser.FnWithoutReturnContext context)
        {
            //verificar se nome de função já existe (lança erro se existir) e cria função
            var fnName = context.VAR().GetText();
            
            if (Functions.ContainsKey(fnName))
            {
                HasErrors = true;
                ErrorMessages.Add("Function " + fnName + " already defined");
            } 
            else 
            {
                Functions.Add(fnName, context);
            }
        }

        public override void ExitTypeCast([NotNull] BasicCParser.TypeCastContext context)
        {
            //verificar se variável existe (lança erro se não existir)
            var varName = context.VAR().GetText();

            if (!Variables.Contains(varName))
            {
                HasErrors = true;
                ErrorMessages.Add("Variável inexistente, crie ela primeiro");
            }
        }
    }
}