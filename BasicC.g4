
grammar BasicC;

// TYPECAST

prog : line+
     ;

line : atrib EOL
     | input EOL
     | output EOL
     | controlflow
     | loop
     | function
     | callFunction EOL
     | lib
     | typecast EOL
     | typeof EOL
     | ternary EOL
     ;

atrib : VAR '=' STR  #VariavelExistenteString
     | STRING VAR '=' STR  #VariavelNovaString
     | TYPE=(INTEGER|DOUBLE) VAR '=' expr  #VariavelNova
     | BOOLEAN VAR '=' BOOL  #VariavelNovaBoolean
     | VAR '=' expr #VariavelExistente 
     | VAR '=' BOOL #VariavelExistenteBoolean
     ;

input : READ '(' VAR ')'   #inputVar
     ;

output: PRINT '(' STR ')'     #outputStr
      | PRINT '(' VAR ')'    #outputVar
      | PRINT '(' expr ')'    #outputExpr
      ;

expr : term '+' expr          #exprSum
     | term '-' expr          #exprSub
     | term                   #exprTerm
     ;

term : factor '*' term          #termMult
     | factor '/' term          #termDiv
     | factor '%' term          #termMod
     | factor                 #termFactor
     ;

factor: '(' expr ')'          #factorExpr
      | VAR                   #factorVar
      | NUM                   #factorNum
      | STR                   #factorStr
      ;

controlflow: IF '(' bexpr ')' block    #ifBlock
            | IF '(' bexpr ')' block ELSE block  #ifElseBlock
            ;

loop: WHILE '(' bexpr ')' block    #loopBlock
    ;


block: '{' line+ '}'  #blockLine
     ;

// rblock: '{' line+ RETURN expr EOL '}'
//      ;

rblock:
     '{' rbody '}'                # fnBlockLine
     ;

rbody:
     line                      # fnBodyLine
     | line rbody               # fnBodyLineMore
     | RETURN BOOL EOL       # fnReturnBoolLine
     | RETURN STR EOL           # fnReturnStrLine
     | RETURN expr EOL          # fnReturnExprLine
     | RETURN EOL               # fnReturnLine
     ;
    
bexpr: expr RELOP=(EQ|NE|LT|GT|LE|GE) expr #bexprRelop
     | expr #bexprExpr
     ;
    
function: TYPE=(INTEGER|DOUBLE|BOOLEAN|STRING) VAR '('params')' rblock     # fnWithReturn
          | VOID VAR '('params')' rblock                                    # fnWithoutReturn
          ;

params: TYPE=(INTEGER|DOUBLE|BOOLEAN|STRING) VAR      #uniqueParam         
     | TYPE=(INTEGER|DOUBLE|BOOLEAN|STRING) VAR SEP params  #multipleParams
     |                                       # noneParam          
     ;

paramsCall: factor         # uniqueParamCall
          | factor SEP paramsCall   # multipleParamCall
          |               # noneParamCall
          ;

callFunction: VAR '('paramsCall')'         #funcInvoc
            ;

lib: IMPORT VAR EOL
   ;

typecast: '(' TYPE=(INTEGER|BOOLEAN|STRING) ')' VAR  # typeCast
        ;

typeof: TYPEOF '(' VAR ')'    # typeOfVar
     ;

ternary:
        bexpr '?' e1=block ':' e2=block # ternaryCond
        ;

//TOKENS
WHILE : 'while';
RETURN : 'return';
IMPORT : '#import';
TYPEOF : 'typeof';
IF : 'if';
ELSE : 'else';
EOL : ';';
PRINT : 'print';
READ : 'read';
INTEGER : 'int';
DOUBLE : 'double';
BOOLEAN : 'bool';
STRING : 'str';
VOID : 'void';
SUM : '+';
SUB : '-';
DIV : '/';
MULT : '*';
MOD : '%';
ASSIGN : '=';
SEP : ',';
LP : '(';
RP : ')';
LB : '{';
RB : '}';
LE : '<=';
LT : '<';
GT : '>';
GE : '>=';
EQ : '==';
NE : '!=';
NUMD : [0-9]+ '.' [0-9]+;
BOOL : 'true' | 'false';
NUM : [0-9]+;
VAR : [a-zA-Z]+;
STR : '"' ~[\n"]*'"';
COMMENT: '//' ~[\r\n]* -> skip;
WS : [ \t\n\r]+ -> skip;


// ------------------
// int b = 20;
// b = b + 10;
// read(a);
// if(a+b > 50){
//     print("hello world");
// }
// else{
//     print("hello world2");
// }
// while(a > 0){
//    print(a);
// }


// -----------------
// print("numero1 :");
// read(n1);
// print("numero2 :");
// read(n2);
// soma = n1+n2;
// print("soma :");
// print(soma);

// -----------------
// float CalculaGay(float a, float b){
//     float c;
//     c = a + b;
//     return c;
// }

// -----------------
// #import a;
// #import b;
//  
// int(b);
// 