using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using BasicInt.Grammar;

namespace Lang
{
    //após a criação da arvore sintática, o visitor percorre a arvore e executa ações,
    //interpretanto o código
    public class LangVisitor : BasicCBaseVisitor<object?> 
    {
        public Dictionary<string, object> Functions { get; protected set; } = new Dictionary<string, object>();
        public Dictionary<string, object> Variables { get; private set; } = new Dictionary<string, object>();
        public Dictionary<string, object> LocalVariables { get; private set; } = new Dictionary<string, object>();
        public string caminho = "";
        public string isFunction = "";

        struct VariableAttributes
        {
            public int type;
            public object? value;

            public VariableAttributes(int _type, object? _value)
            {
                type = _type;
                value = _value;
            }

            public int GetType()
            {
                return type;
            }

            public object? GetValue()
            {
                return value;
            }

            public void SetType(int _type)
            {
                type = _type;
            }

            public void SetValue(object? _value)
            {
                value = _value;
            }
        }

        struct FunctionAttributes
        {
            public int type;
            public IParseTree value;

            public FunctionAttributes(int _type, IParseTree _value)
            {
                type = _type;
                value = _value;
            }

            public int GetType()
            {
                return type;
            }

            public IParseTree GetValue()
            {
                return value;
            }

            public void SetType(int _type)
            {
                type = _type;
            }

            public void SetValue(IParseTree _value)
            {
                value = _value;
            }
        }

        public struct LocalVariablesAttributes
        {
            public int type;
            public object? value;
            public string name;
            private Type type1;
            private object? result;

            public LocalVariablesAttributes(int _type, object? _value, string _name)
            {
                type = _type;
                value = _value;
                name = _name;
            }

            public LocalVariablesAttributes(Type type1, object? result, object value) : this()
            {
                this.type1 = type1;
                this.result = result;
                this.value = value;
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

        public Boolean VerificaTipo(int varType, string varValue)
        {
            switch(varType)
            {
                case BasicCLexer.INTEGER:
                    int valorInteiro = 0;
                    if (!int.TryParse(varValue, out valorInteiro))
                    {
                        Console.WriteLine("Entrada inválida para o tipo INT");
                        return false;
                    }
                    break;
                case BasicCLexer.DOUBLE:
                    double valorDouble = 0.0;
                    if (!double.TryParse(varValue, out valorDouble))
                    {
                        Console.WriteLine("Entrada inválida para o tipo DOUBLE");
                        return false;
                    }
                    break;
                case BasicCLexer.BOOLEAN:
                    bool valorBooleano = false;
                    if (!bool.TryParse(varValue, out valorBooleano))
                    {
                        Console.WriteLine("Entrada inválida para o tipo BOOLEAN");
                        return false;
                    }
                    break;
                //ARRUMAR
                case BasicCLexer.STRING:
                    if (varValue[0] != '"' || varValue[varValue.Length - 1] != '"')
                    {
                        Console.WriteLine("Entrada inválida para o tipo STRING");
                        return false;
                    }
                    break;
                default:
                    Console.WriteLine("Tipo de variável inválido");
                    return false;
            }
            return true;
        }


        #region I/O Statements
        public override object? VisitVariavelNova([NotNull] BasicCParser.VariavelNovaContext context)
        {
            var varName = context.VAR().GetText();
            var varType = context.TYPE.Type;
            var varValue = context.expr().GetText();
            
            bool result = VerificaTipo(varType, varValue);
            if (result)
            {
                if (isFunction == "")
                {
                    Variables.Add(varName, new VariableAttributes(varType, varValue));
                }
                else
                {
                    LocalVariables.Add(isFunction, new LocalVariablesAttributes(varType, varValue, varName));
                }
            }

            return null;
        }

        public override object? VisitVariavelExistente([NotNull] BasicCParser.VariavelExistenteContext context)
        {
            var varName = context.VAR().GetText();
            var varNewValue = Visit(context.expr());
            
            if (isFunction == "" && Variables.ContainsKey(varName))
            {
                var variableAttributes = (VariableAttributes)Variables[varName];
                bool result = VerificaTipo(variableAttributes.GetType(), varNewValue.ToString());
                if (result)
                    Variables[varName] = new VariableAttributes(variableAttributes.GetType(), varNewValue);
            }
            else
            {
                if(LocalVariables.ContainsKey(isFunction))
                {
                    var localVariablesAttributes = (LocalVariablesAttributes)LocalVariables[isFunction];
                    bool result = VerificaTipo(localVariablesAttributes.GetType(), varNewValue.ToString());
                    if (result)
                        LocalVariables[isFunction] = new LocalVariablesAttributes(localVariablesAttributes.GetType(), varNewValue, varName);
                }
                else{
                    Console.WriteLine("Variável '"+ varName + "' não foi encontrada no escopo atual");
                }
            }

            return null;
        }

        public override object? VisitVariavelNovaString([NotNull] BasicCParser.VariavelNovaStringContext context)
        {
            var varName = context.VAR().GetText();
            var varType = BasicCLexer.STRING;
            var varValue = context.STR().GetText();

            bool result = VerificaTipo(varType, varValue);
            if (result)
            {
                if (isFunction == "")
                {
                    Variables.Add(varName, new VariableAttributes(varType, varValue));
                }
                else
                {
                    LocalVariables.Add(isFunction, new LocalVariablesAttributes(varType, varValue, varName));
                }
            }

            return null;
        }

        public override object? VisitVariavelExistenteString([NotNull] BasicCParser.VariavelExistenteStringContext context)
        {
            var varName = context.VAR().GetText();
            var varNewValue = context.STR().GetText();
            var variableAttributes = (VariableAttributes)Variables[varName];
            
            bool result = VerificaTipo(BasicCLexer.STRING, varNewValue);
            if (result)
                Variables[varName] = new VariableAttributes(variableAttributes.GetType(), varNewValue);
            
            return null;
        }

        public override object VisitVariavelNovaBoolean([NotNull] BasicCParser.VariavelNovaBooleanContext context)
        {
            var varName = context.VAR().GetText();
            var varType = BasicCLexer.BOOLEAN;
            var varValue = context.BOOL().GetText();

            bool result = VerificaTipo(varType, varValue);
            if (result)
            {
                if (isFunction == "")
                {
                    Variables.Add(varName, new VariableAttributes(varType, varValue));
                }
                else
                {
                    LocalVariables.Add(isFunction, new LocalVariablesAttributes(varType, varValue, varName));
                }
            }

            return null;
        }

        public override object? VisitOutputVar([NotNull] BasicCParser.OutputVarContext context)
        {
            var varName = context.VAR().GetText();
            if (Variables.ContainsKey(varName))
            {
                var variableAttributes = (VariableAttributes)Variables[varName];
                Console.WriteLine(variableAttributes.GetValue());
            }
            else if (LocalVariables.ContainsKey(isFunction))
            {
                var localVariablesAttributes = (LocalVariablesAttributes)LocalVariables[isFunction];
                Console.WriteLine(localVariablesAttributes.GetValue());
                
            }
            else 
            {
                Console.WriteLine("Variável '"+ varName + "' não foi encontrada no escopo atual");
            }

            return null;
        }

        public override object? VisitOutputStr([NotNull] BasicCParser.OutputStrContext context)
        {
            var varText = context.STR().GetText();
            varText  = varText.Substring(1, varText.Length - 2);
            Console.WriteLine(varText);

            return null;
        }

        public override object? VisitOutputExpr([NotNull] BasicCParser.OutputExprContext context)
        {
            var exprValue = Visit(context.expr());
            Console.WriteLine(exprValue);

            return null;
        }

        public override object? VisitInputVar([NotNull] BasicCParser.InputVarContext context)
        {
            var varName = context.VAR().GetText();
            var varType = BasicCLexer.STRING;
            var varValue = Console.ReadLine();

            //Adiciona as aspas no varValue
            varValue = '"' + varValue + '"';

            bool result = VerificaTipo(varType, varValue);
            if (result)
                Variables.Add(varName, new VariableAttributes(varType, varValue));

            return null;
        }
        #endregion

        #region Operations
        protected (Double, Double) GetDoubles(IParseTree tree1, IParseTree tree2)
        {
            var t1 = Visit(tree1);
            var t2 = Visit(tree2);

            if (t1 is VariableAttributes && t2 is VariableAttributes)
            {
                Double.TryParse(((VariableAttributes)t1).GetValue().ToString(), out var d1);
                Double.TryParse(((VariableAttributes)t2).GetValue().ToString(), out var d2);
                return (d1, d2);
            }
            else if (t1 is VariableAttributes && t2 is not VariableAttributes)
            {
                Double.TryParse(((VariableAttributes)t1).GetValue().ToString(), out var d1);
                Double.TryParse(t2.ToString(), out var d2);
                return (d1, d2);
            }
            else if (t1 is not VariableAttributes && t2 is VariableAttributes)
            {
                Double.TryParse(t1.ToString(), out var d1);
                Double.TryParse(((VariableAttributes)t2).GetValue().ToString(), out var d2);
                return (d1, d2);
            }
            else
            {
                Double.TryParse(t1.ToString(), out var d1);
                Double.TryParse(t2.ToString(), out var d2);
                return (d1, d2);
            }
        }
        
        public override object VisitExprSum([NotNull] BasicCParser.ExprSumContext context)
        {

            var d = GetDoubles(context.term(), context.expr());
            return d.Item1 + d.Item2;
        }

        public override object VisitExprSub([NotNull] BasicCParser.ExprSubContext context)
        {
            var d = GetDoubles(context.term(), context.expr());
            return d.Item1 - d.Item2;
        }

        public override object? VisitExprTerm([NotNull] BasicCParser.ExprTermContext context)
        {
            return Visit(context.term());
        }

        public override object VisitTermMult([NotNull] BasicCParser.TermMultContext context)
        {
            var d = GetDoubles(context.factor(), context.term());
            return d.Item1 * d.Item2;
        }

        public override object VisitTermDiv([NotNull] BasicCParser.TermDivContext context)
        {
            var d = GetDoubles(context.factor(), context.term());
            return d.Item1 / d.Item2;
        }

        public override object VisitTermMod([NotNull] BasicCParser.TermModContext context)
        {
            var d = GetDoubles(context.factor(), context.term());
            return d.Item1 % d.Item2;
        }

        public override object? VisitTermFactor([NotNull] BasicCParser.TermFactorContext context)
        {
            return Visit(context.factor());
        }

        public override object VisitFactorVar([NotNull] BasicCParser.FactorVarContext context)
        {
            var varName = context.VAR().GetText();
            return Variables[varName];
        }

        public override object? VisitFactorNum([NotNull] BasicCParser.FactorNumContext context)
        {
            var txtNum = context.NUM().GetText();
            return Double.Parse(txtNum);
        }

        public override object? VisitFactorExpr([NotNull] BasicCParser.FactorExprContext context)
        {
            return Visit(context.expr());
        }
        #endregion

        #region function
        public override object? VisitFuncInvoc([NotNull] BasicCParser.FuncInvocContext context)
        {
            var fnName = context.VAR().GetText();
            var functionAttributes = (FunctionAttributes)Functions[fnName];
            

            //struct chave = nome da função, valor : variaveis função 

            var result = Visit(context.paramsCall());
            
            
            isFunction = fnName;
            //LocalVariables.Add(isFunction, new LocalVariablesAttributes(localVarType, locarVarValue, localVarName));
            Visit(functionAttributes.GetValue()); //chama function rblock
            
            isFunction = "";
            
            switch(functionAttributes.GetType())
            {
                case BasicCLexer.INTEGER:
                    if(caminho != "fnReturnExpr")
                        Console.WriteLine("Erro: Função com retorno inteiro deve retornar um valor inteiro");
                    break;
                case BasicCLexer.STRING:
                    if(caminho != "fnReturnStr")
                        Console.WriteLine("Erro: Função com retorno string deve retornar um valor string");
                    break;
                case BasicCLexer.BOOLEAN:
                    if(caminho != "fnReturnBool")
                        Console.WriteLine("Erro: Função com retorno booleano deve retornar um valor booleano");
                
                    break;
                default:
                    return null;
            }
           
            
            return null;
        }

        public override object? VisitUniqueParam([NotNull] BasicCParser.UniqueParamContext context)
        {
            var localVarType = context.TYPE.Type;
            var localVarName = context.VAR().GetText();
            var locarVarValue = "";
            return null;
        }

        public override object? VisitMultipleParams([NotNull] BasicCParser.MultipleParamsContext context)
        {
            
            return Visit(context.@params());
        }

        public override object? VisitUniqueParamCall([NotNull] BasicCParser.UniqueParamCallContext context)
        {
            return Visit(context.factor());
        }

        public override object? VisitMultipleParamCall([NotNull] BasicCParser.MultipleParamCallContext context)
        {
            return GetDoubles(context.factor(), context.paramsCall());
        }

        public override object? VisitFnWithReturn([NotNull] BasicCParser.FnWithReturnContext context)
        {
            var fnName = context.VAR().GetText();
            var fnType = context.TYPE.Type;
            Functions.Add(fnName, new FunctionAttributes(fnType, context.rblock()));
            
            return null;
        }

        public override object? VisitFnWithoutReturn([NotNull] BasicCParser.FnWithoutReturnContext context)
        {
            var fnName = context.VAR().GetText();
            var fnType = BasicCLexer.VOID;
            Functions.Add(fnName, new FunctionAttributes(fnType, context.rblock()));
            Visit(context.@params());
            return null;
        }

        public override object? VisitFnBlockLine([NotNull] BasicCParser.FnBlockLineContext context)
        {
            return Visit(context.rbody());
        }

        public override object? VisitFnBodyLine([NotNull] BasicCParser.FnBodyLineContext context)
        {
            return Visit(context.line());
        }

        public override object? VisitFnBodyLineMore([NotNull] BasicCParser.FnBodyLineMoreContext context)
        {
            Visit(context.line());
            return Visit(context.rbody());
        }

        public override object? VisitFnReturnExprLine([NotNull] BasicCParser.FnReturnExprLineContext context)
        {
            caminho = "fnReturnExpr";
            return Visit(context.expr());
        }

        public override object? VisitFnReturnStrLine([NotNull] BasicCParser.FnReturnStrLineContext context)
        {
            caminho = "fnReturnStr";
            return Visit(context.STR());
        }

        public override object? VisitFnReturnBoolLine([NotNull] BasicCParser.FnReturnBoolLineContext context)
        {
            caminho = "fnReturnBool";
            return Visit(context.BOOL());
        }
        #endregion


        #region controlflow
        public override object? VisitIfBlock([NotNull] BasicCParser.IfBlockContext context)
        {
            var condition = Visit(context.bexpr());
            
            
            if (condition != null && (bool)condition)
            {
                Visit(context.block());
            }
            return null;
        }

        public override object? VisitIfElseBlock([NotNull] BasicCParser.IfElseBlockContext context)
        {
            var condition = Visit(context.bexpr());
            
            if (condition != null && (bool)condition)
            {
                Visit(context.block(0));
            }
            else
            {
                Visit(context.block(1));
            }
            return null;
        }


        public override object? VisitBexprRelop([NotNull] BasicCParser.BexprRelopContext context)
        {
            var d = GetDoubles(context.expr(0), context.expr(1));
            switch (context.RELOP.Type)
            {
                case BasicCLexer.LT:
                    return d.Item1 < d.Item2;
                case BasicCLexer.LE:
                    return d.Item1 <= d.Item2;
                case BasicCLexer.GT:
                    return d.Item1 > d.Item2;
                case BasicCLexer.GE:
                    return d.Item1 >= d.Item2;
                case BasicCLexer.EQ:
                    return d.Item1 == d.Item2;
                case BasicCLexer.NE:
                    return d.Item1 != d.Item2;
                default:
                    return null;
            }
        }

        public override object VisitLoopBlock([NotNull] BasicCParser.LoopBlockContext context)
        {
            var cond = Visit(context.bexpr());
            while (cond != null && (bool)cond)
            {
                Visit(context.block());
                cond = Visit(context.bexpr());
            }
            return null;
        }

        #endregion

        #region typecast
        public override object? VisitTypeCast([NotNull] BasicCParser.TypeCastContext context)
        {
            var varName = context.VAR().GetText();
            var varType = context.TYPE.Type; //TIPO QUE EU QUERO CONVERTER

            var variableAttributes = (VariableAttributes)Variables[varName]; // PEGA O TIPO E VALOR ATUAL DA VARIAVEL

            //Verificar se o tipo de cast é válido
            switch(varType)
            {
                case BasicCLexer.INTEGER:
                    // String para Inteiro
                    if(variableAttributes.GetType() == BasicCLexer.STRING)
                    {
                        if(!int.TryParse(variableAttributes.GetValue().ToString().Substring(1, variableAttributes.GetValue().ToString().Length - 2), out int n))
                            Console.WriteLine("Erro: Não é possível fazer cast de string para inteiro");
                        else
                            Variables[varName] = new VariableAttributes(varType, '"'+n+'"');
                    }
                    // Boolean para Inteiro
                    else if(variableAttributes.GetType() == BasicCLexer.BOOLEAN)
                    {
                        if(variableAttributes.GetValue().ToString() == "true")
                            Variables[varName] = new VariableAttributes(varType, 1);
                        else if(variableAttributes.GetValue().ToString() == "false")
                            Variables[varName] = new VariableAttributes(varType, 0);
                        else
                            Console.WriteLine("Erro: Não é possível fazer cast de booleano para inteiro");
                    }
                    break;
                case BasicCLexer.STRING:
                    // Inteiro para String
                    if(variableAttributes.GetType() == BasicCLexer.INTEGER)
                        Variables[varName] = new VariableAttributes(varType, '"'+variableAttributes.GetValue().ToString()+'"');
                    // Boolean para String
                    else if(variableAttributes.GetType() == BasicCLexer.BOOLEAN)
                        Variables[varName] = new VariableAttributes(varType, '"'+variableAttributes.GetValue().ToString()+'"');
                    break;
                case BasicCLexer.BOOLEAN:
                    // Inteiro para Boolean
                    if(variableAttributes.GetType() == BasicCLexer.INTEGER)
                    {
                        if(variableAttributes.GetValue().ToString() == "0")
                            Variables[varName] = new VariableAttributes(varType, false);
                        else if(variableAttributes.GetValue().ToString() == "1")
                            Variables[varName] = new VariableAttributes(varType, true);
                        else
                            Console.WriteLine("Erro: Não é possível fazer cast de inteiro para booleano");
                    }
                    // String para Boolean
                    else if(variableAttributes.GetType() == BasicCLexer.STRING)
                    {
                        var value = variableAttributes.GetValue().ToString();
                        if(value.Substring(1, value.Length-2).Equals("true"))
                            Variables[varName] = new VariableAttributes(varType, true);
                        else if(value.Substring(1, value.Length-2).Equals("false"))
                            Variables[varName] = new VariableAttributes(varType, false);
                        else
                            Console.WriteLine("Erro: Não é possível fazer cast de string para booleano");
                    }
                    break;
                default:
                    return null;
            }
            return null;
        }


        #endregion

        #region lib

        public override object? VisitLib([NotNull] BasicCParser.LibContext context)
        {
            var libName = context.VAR().GetText();
            //Procura se a biblioteca existe na pasta bibliotecas
            if (File.Exists("bibliotecas/" + libName + ".txt"))
            {
                //Lê o arquivo da biblioteca
                string[] lines = File.ReadAllLines("bibliotecas/" + libName + ".txt");
                foreach (string line in lines)
                {
                    //Executa cada linha do arquivo
                    var input = new AntlrInputStream(line);
                    var lexer = new BasicCLexer(input);
                    var tokens = new CommonTokenStream(lexer);
                    var parser = new BasicCParser(tokens);
                    var libTree = parser.prog();
                    Visit(libTree);
                }
            }
            else
            {
                Console.WriteLine("Erro: Biblioteca não encontrada");
            }
            
            return null;
        }

        #endregion

    }
}