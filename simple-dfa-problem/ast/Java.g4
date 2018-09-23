grammar Java;

class_definition: VISIBILITY CLASS ID LEFT_BRACE method RIGHT_BRACE;
method: VISIBILITY STATIC TYPE ID LEFT_PAREN VARARG ID RIGHT_PAREN body;
body: statement
    | LEFT_BRACE statement* RIGHT_BRACE;
statement: var_definition
         | assignment
         | if_op
		 | method_call;
method_call: ID (DOT ID)* LEFT_PAREN ID RIGHT_PAREN SEMICOLON;
if_op: IF LEFT_PAREN expr RIGHT_PAREN body;
var_definition: TYPE ID SEMICOLON;
assignment: ID EQUALS expr SEMICOLON;
expr: NUM | array_access;
array_access: ID LEFT_SQ_BRACKET NUM RIGHT_SQ_BRACKET;

VARARG: TYPE '...';
DOT: '.';
SEMICOLON: ';';
EQUALS: '=';
IF: 'if';
LEFT_BRACE: '{';
RIGHT_BRACE: '}';
LEFT_PAREN: '(';
RIGHT_PAREN: ')';
LEFT_SQ_BRACKET: '[';
RIGHT_SQ_BRACKET: ']';
TYPE: 'int' | 'boolean' | 'void';
CLASS: 'class';
VISIBILITY: 'public';
STATIC: 'static';
NUM: [0-9]+;
ID: [a-zA-Z]+;
WS: [ \t\r\n]+ -> skip;