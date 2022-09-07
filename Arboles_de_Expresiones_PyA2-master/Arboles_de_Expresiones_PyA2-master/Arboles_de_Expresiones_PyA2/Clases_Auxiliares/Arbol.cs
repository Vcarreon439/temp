using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace Arboles_de_Expresiones_PyA2.Clases_Auxiliares
{
    public class Arbol
    {
        #region Campos
        //Insercion de la cola
        private string precedencia = "+-*/^";
        private string[] delimitadores = { "+", "-", "*", "/", "^" };
        private string[] operandos;
        private string[] operadores;
        private Queue colaExpresion;

        //Creacion del arbol
        private string token;
        private string operadorTmp;
        private int i = 0;
        private Stack pilaOperadores;
        private Stack pilaOperandos;
        private Stack pilaDot;
        private Nodo raiz = null;

        public Nodo nodoDot { get; set; }

        //Propiedades para recorridos
        public string Pre { get; set; }
        public string In { get; set; }
        public string Post { get; set; }

        #endregion

        #region Constructores
        public Arbol()
        {
            pilaOperadores = new Stack();
            pilaOperandos = new Stack();
            pilaDot = new Stack();
            colaExpresion = new Queue();
        }

        #endregion

        #region Insercion_Cola

        public void Insertar_EnCola(string expresion) 
        {
            operandos = expresion.Split(delimitadores, StringSplitOptions.RemoveEmptyEntries);
            operadores = expresion.Split(operandos, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; colaExpresion.Count < operandos.Length + (operadores.Length - 1); i++)
            {
                colaExpresion.Enqueue(operandos[i]);
                colaExpresion.Enqueue(operadores[i]);
            }
            colaExpresion.Enqueue(operandos[operandos.Length - 1]);
        }
        #endregion

        #region Arbol

        public Nodo CrearArbol() 
        {
            while (colaExpresion.Count!=0)
            {
                token = (string)colaExpresion.Dequeue();
                if (precedencia.IndexOf(token)<0)
                {
                    pilaOperandos.Push(new Nodo(token));
                    pilaDot.Push(new Nodo($"nodo{++i}[label=\"{token}\"]"));
                }
                else
                {
                    if (pilaOperadores.Count!=0)
                    {
                        operadorTmp = (string)pilaOperadores.Peek();
                        while (pilaOperadores.Count!=0&&precedencia.IndexOf(operadorTmp)>=precedencia.IndexOf(token)) 
                        {
                            GuardarSubArbol();
                            if (pilaOperadores.Count!=0)
                            {
                                operadorTmp = (string)pilaOperadores.Peek();
                            }
                        }
                    }
                    pilaOperadores.Push(token);
                }
            }

            raiz = (Nodo)pilaOperandos.Peek();
            nodoDot = (Nodo)pilaDot.Peek();
            while (pilaOperadores.Count !=0)
            {
                GuardarSubArbol();
                raiz = (Nodo)pilaOperandos.Peek();
                nodoDot = (Nodo)pilaDot.Peek();
            }
            return raiz;
        }

        private void GuardarSubArbol()
        {
            Nodo derecho = (Nodo)pilaOperandos.Pop();
            Nodo izquierdo = (Nodo)pilaOperadores.Pop();
            pilaOperandos.Push(new Nodo(derecho, izquierdo, $"nodo{i++}[label=\"{pilaOperadores.Pop()}\"]"));
        }

        #endregion

        #region Recorridos

        public string InsertaPre(Nodo arbol) 
        {
            if (arbol!=null)
            {
                Pre += arbol.Datos + " ";
                InsertaPre(arbol.NodoIzquierdo);
                InsertaPre(arbol.NodoDerecho);
            }
            return Pre;
        }

        public string InsertaIn(Nodo arbol)
        {
            if (arbol != null)
            {
                InsertaIn(arbol.NodoIzquierdo);
                In += arbol.Datos + " ";
                InsertaIn(arbol.NodoDerecho);
            }
            return In;
        }

        public string InsertaPost(Nodo arbol)
        {
            if (arbol != null)
            {
                InsertaIn(arbol.NodoIzquierdo);
                InsertaIn(arbol.NodoDerecho);
                Post += arbol.Datos + " ";
            }
            return Post;
        }

        #endregion

        public void Limpiar() 
        {
            Post = Pre = In = "";
                

        }
    }
}