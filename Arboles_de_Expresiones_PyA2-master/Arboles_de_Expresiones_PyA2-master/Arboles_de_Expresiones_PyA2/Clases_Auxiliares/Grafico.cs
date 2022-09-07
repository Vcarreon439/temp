using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Diagnostics;
using System.Reflection;


namespace Arboles_de_Expresiones_PyA2.Clases_Auxiliares
{
    public class Grafico
    {
        #region Campos

        Nodo arbol;
        private string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        private string comand = @"/c Batch.bat";
        private int i, j;

        #endregion

        #region Constructores

        public Grafico() 
        {
            this.arbol = arbol;
        }

        #endregion

        #region Funciones

        public void DrawTree() 
        {
            CreateFileDot();
            ExecuteDot();
        }

        private string CreateFileDot() 
        {
            string cadenaDot = "";
            StartFileDot(arbol, ref cadenaDot);
            using (StreamWriter archivo = new StreamWriter(path + @"\Arbol.dot"))
            {
                archivo.WriteLine(cadenaDot);
                archivo.Close();
            }
            return cadenaDot;
        }

        private void StartFileDot(Nodo arbol,  ref string cadenaDot)
        {
            if (arbol!=null)
            {
                cadenaDot += "digraph Grafico {\nnode [style=bold, fillcolor=gray];\n";
                Recorrido(arbol, ref cadenaDot);
                cadenaDot += "\n}";
            }
        }

        private void Recorrido(Nodo arbol, ref string cadenaDot)
        {
            if (arbol!=null)
            {
                cadenaDot += $"{arbol.Datos}\n";
                if (arbol.NodoIzquierdo!=null)
                {
                    i = arbol.Datos.ToString().IndexOf("[");
                    i = arbol.NodoIzquierdo.ToString().IndexOf("[");
                    cadenaDot += $"{arbol.Datos.ToString().Remove(i)}->{arbol.NodoIzquierdo.Datos.ToString().Remove(j)}\n";
                }

                if (arbol.NodoDerecho != null)
                {
                    i = arbol.Datos.ToString().IndexOf("[");
                    i = arbol.NodoDerecho.ToString().IndexOf("[");
                    cadenaDot += $"{arbol.Datos.ToString().Remove(i)}->{arbol.NodoDerecho.Datos.ToString().Remove(j)}\n";
                }
                Recorrido(arbol.NodoIzquierdo, ref cadenaDot);
                Recorrido(arbol.NodoDerecho, ref cadenaDot);
            }
        }

        private void ExecuteDot()
        {
            Directory.SetCurrentDirectory(path);
            using (Process pr = new Process())
            {
                ProcessStartInfo Infopr = new ProcessStartInfo("cmd", comand);
                Infopr.CreateNoWindow = true;
                pr.Start();
                pr.WaitForExit();
                pr.Close();
            }
        }


        #endregion
    }
}
