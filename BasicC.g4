grammar BasicC;

prog : line+
     ;

line : atrib EOL
     | input EOL
     | output EOL
     | controlflow
     | loop
     | function
     ;

atrib: VAR '=' expr #VariavelExistente
     | TYPE=(INTEGER|DOUBLE|BOOLEAN) VAR '=' expr  #VariavelNova
     | STRING VAR '=' STR  #VariavelNovaString
     | VAR '=' STR  #VariavelExistenteString
     ;

input: READ '(' VAR ')'
     ;

output: PRINT '(' STR ')'
      | PRINT '(' expr ')'
      ;

expr : expr '+' expr
     | expr '-' expr
     | term
     ;

term : term '*' term
     | term '/' term
     | term '%' term
     | factor
     ;

factor: '(' expr ')'
      | VAR
      | NUM
      ;

controlflow: IF '(' bexpr ')' block
            | IF '(' bexpr ')' block ELSE block
            ;

loop: WHILE '(' bexpr ')' block
    ;


block: '{' line+ '}'
     ;

// rblock: '{' line+ RETURN expr EOL '}'
//      ;

rblock:
     '{' rbody '}'                # fnBlockLine
     ;

rbody:
     line                      # fnBodyLine
     | line rbody               # fnBodyLineMore
     | RETURN expr EOL           # fnReturnExprLine
     | RETURN EOL                # fnReturnLine
     ;
    
bexpr: expr RELOP=(EQ|NE|LT|GT|LE|GE) expr
     | expr
     ;
    
function: TYPE=(INTEGER|DOUBLE|BOOLEAN|STRING) VAR '('params')' rblock
          | VOID VAR '('params')' block
          ;

params: TYPE=(INTEGER|DOUBLE|BOOLEAN|STRING) VAR
     | TYPE=(INTEGER|DOUBLE|BOOLEAN|STRING) VAR SEP params
     | //vazio
     ;

//TOKENS
WHILE : 'while';
IF : 'if';
ELSE : 'else';
EOL : ';';
PRINT : 'print';
READ : 'read';
NUM : [0-9]+;
INTEGER : 'int';
DOUBLE : 'double';
BOOLEAN : 'bool';
STRING : 'str';
VOID : 'void';
VAR : [a-zA-Z]+;
SUM : '+';
SUB : '-';
DIV : '/';
MULT : '*';
MOD : '%';
ASSIGN : '=';
RETURN : 'return';
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