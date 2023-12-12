# Funcionamiento del Intérprete del HULK

## Inicio
El programa comienza a ejecutarse recibiendo un string que sería el codigo que el usuario desea ejecutar.
Primeramente se comprueba si el código es o no vacío, pues una entrada vacía asume que es para cerrar el intérprete

Luego en caso de no ser vacía la entrada:
Se comienza a procesar haciendo primeramente un análisis léxico,
después un analisis sintáctico,
 y después el proceso de interpretación para devolver un resultado

## Analisis léxico

Recibe un string y va procesando caracter por caracter de este string para ir constituyendo los tokens, los tokens son la identificación que se le asigna a cada conjunto de caracteres para posteriormente según su clasificación poderlos procesar.

Los tokens son una estructura que guarda:
un tipo => Que es de tipo TokenType
un valor => Que es de tipo objeto y guarda el valor asociado a ese tipo
y dos valores enteros que posteriormente se explicará su utilidad

TokenType es una clase de enum que contiene todos los posible tipos,
enum porque permite agrupar un conjunto finito de valores que además no van a cambiar.

La clase Tokenizer recibe un string que es la línea de código escrita por el usuario,
Y con el método Start comienza a procesar el codigo clasificando para crear un conjunto de Tokens
