# Compilador ANTLR

## Introdução

Bem-vindo ao README do projeto final da matéria de Compiladores! Neste documento, você encontrará informações essenciais sobre o funcionamento e o propósito do compilador que desenvolvemos.

Este compilador é composto por diversos componentes essenciais, como o analisador léxico, analisador sintático, gerador de código e otimizador de código. Cada um desses componentes desempenha um papel crucial no processo de transformação do código fonte em um programa executável, garantindo não apenas a correção do programa, mas também sua eficiência de execução.

## Requisitos

Para compilar e executar o compilador, é necessário ter instalado o Java Development Kit (JDK) versão 8 ou superior e o Ant (um gerenciador de compilação de software livre) versão 1.9.3 ou superior.

### Compilação do Projeto

Para compilar o compilador, siga os seguintes passos:
- Clone o repositório do Github joaodelrio/TF_Compiladores: [Repositório para o trabalho final de compiladores](https://github.com/joaodelrio/TF_Compiladores)
- Abra um terminal e navegue até o diretório raiz do projeto.
- Digite o comando `ant jar` para gerar o arquivo JAR do compilador.
- Para executar o compilador, digite o comando `dotnet run` no terminal. Onde `Lang.g4` é o nome do arquivo que contém o código fonte da linguagem definida pela gramática do compilador.

## Exemplos

## Comandos de entrada e saída
```csharp
print("Oi");
read(a);
print(a);
```
## Declaração de variáveis
```csharp
int b=20;
bool c=true;
str d="teste";
print(b);
print(c);
print(d);
b=1;
print(b);
(bool)b;
print(b);
```
## Operações matemáticas;
```csharp
print((20+48)/8*2);
```
## Controle de fluxo;
```csharp
if(10>2){
    print("Entrou no If");
}
else{
    print("Entrou no else");
}

if(10<2){
    print("Entrou no If");
}
else{
    print("Entrou no else");
}
```
## Laços de repetição;
```csharp
int i=1;
while(i<10){
    print(i);
    i=i+1;
}
```
## Funções
```csharp
void function(){
    print("Entrou na funcao");
}
print("Antes da funcao");
function();

void function(){
    int a =20;
    print(a);
}

function();
print(a);
```
## Coisas mais
```csharp
int valor=20;
typeof(valor);
(str) valor;
typeof(valor);


10>5 ? {
    print("é maior");
} : {
    print("é menor");
};
```

## Conclusão
Em suma, desenvolver este compilador foi uma jornada desafiadora e gratificante. Ao longo do processo, aprendemos muito sobre compiladores, linguagens de programação e trabalho em equipe. Enfrentamos diversos obstáculos, mas conseguimos superá-los com determinação e colaboração. Este projeto nos proporcionou uma compreensão mais profunda dos conceitos fundamentais da computação
e nos preparou para enfrentar futuros desafios com confiança. Estamos orgulhosos do trabalho realizado e ansiosos para aplicar o que aprendemos em projetos futuros. Agradecemos a todos os envolvidos neste processo e esperamos que nosso compilador seja útil para a comunidade de desenvolvedores.
