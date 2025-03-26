using System;

namespace EstructurasPersonalizadas
{
    /// <summary>
    /// Clase base para nodos en listas enlazadas.
    /// </summary>
    /// <typeparam name="T">Tipo de dato almacenado en el nodo.</typeparam>
    abstract class NodoBase<T>
    {
        /// <summary>
        /// Valor almacenado en el nodo.
        /// </summary>
        public T Valor { get; set; }

        /// <summary>
        /// Constructor que inicializa el nodo con un valor.
        /// </summary>
        /// <param name="valor">Valor del nodo.</param>
        public NodoBase(T valor)
        {
            Valor = valor;
        }
    }

    /// <summary>
    /// Nodo para listas simplemente enlazadas.
    /// </summary>
    /// <typeparam name="T">Tipo de dato almacenado en el nodo.</typeparam>
    class NodoSimple<T> : NodoBase<T>
    {
        /// <summary>
        /// Referencia al siguiente nodo en la lista.
        /// </summary>
        public NodoSimple<T> Siguiente { get; set; }

        /// <summary>
        /// Constructor que inicializa el nodo con un valor y deja la referencia <c>Siguiente</c> como <c>null</c>.
        /// </summary>
        /// <param name="valor">Valor del nodo.</param>
        public NodoSimple(T valor) : base(valor)
        {
            Siguiente = null;
        }
    }

    /// <summary>
    /// Clase base para listas enlazadas.
    /// </summary>
    /// <typeparam name="TNodo">Tipo de nodo que usa la lista.</typeparam>
    /// <typeparam name="T">Tipo de datos almacenados en la lista.</typeparam>
    abstract class ListaBase<TNodo, T> where TNodo : NodoBase<T>
    {
        /// <summary>
        /// Nodo inicial de la lista.
        /// </summary>
        protected TNodo cabeza;

        /// <summary>
        /// Tamaño de la lista (cantidad de elementos almacenados).
        /// </summary>
        protected int tamaño;

        /// <summary>
        /// Constructor que inicializa la lista vacía.
        /// </summary>
        public ListaBase()
        {
            cabeza = null;
            tamaño = 0;
        }

        /// <summary>
        /// Verifica si la lista contiene un valor específico.
        /// </summary>
        /// <param name="valor">Valor a buscar en la lista.</param>
        /// <returns><c>true</c> si el valor está en la lista, <c>false</c> en caso contrario.</returns>
        public bool Contiene(T valor)
        {
            TNodo actual = cabeza;
            while (actual != null)
            {
                if (actual.Valor.Equals(valor))
                    return true;

                actual = ObtenerSiguiente(actual); // Ahora usamos el método abstracto
            }
            return false;
        }

        /// <summary>
        /// Devuelve el número de elementos en la lista.
        /// </summary>
        /// <returns>Número de elementos en la lista.</returns>
        public int Tamaño()
        {
            return tamaño;
        }

        /// <summary>
        /// Método abstracto que obtiene el siguiente nodo en la lista.
        /// </summary>
        /// <param name="nodo">Nodo actual.</param>
        /// <returns>El siguiente nodo en la lista.</returns>
        protected abstract TNodo ObtenerSiguiente(TNodo nodo);

        /// <summary>
        /// Método abstracto que obtiene un elemento según su índice (debe ser implementado en subclases).
        /// </summary>
        /// <param name="indice">Índice del elemento a obtener.</param>
        /// <returns>El elemento en la posición indicada.</returns>
        public abstract T Obtener(int indice);
    }

    /// <summary>
    /// Implementación de una lista simplemente enlazada.
    /// </summary>
    /// <typeparam name="T">Tipo de datos almacenados en la lista.</typeparam>
    class ListaSimple<T> : ListaBase<NodoSimple<T>, T>
    {
        /// <summary>
        /// Agrega un nuevo elemento al final de la lista.
        /// </summary>
        /// <param name="valor">Valor del elemento a agregar.</param>
        public void Agregar(T valor)
        {
            NodoSimple<T> nuevoNodo = new NodoSimple<T>(valor);

            if (cabeza == null)
            {
                cabeza = nuevoNodo;
            }
            else
            {
                NodoSimple<T> actual = cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }
            tamaño++;
        }

        /// <summary>
        /// Obtiene el valor de un nodo en la posición indicada.
        /// </summary>
        /// <param name="indice">Índice del nodo a obtener (basado en 0).</param>
        /// <returns>El valor del nodo en la posición indicada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se lanza si el índice está fuera del rango de la lista.
        /// </exception>
        public override T Obtener(int indice)
        {
            if (indice < 0 || indice >= tamaño)
                throw new IndexOutOfRangeException("Índice fuera de rango");

            NodoSimple<T> actual = cabeza;
            for (int i = 0; i < indice; i++)
            {
                actual = actual.Siguiente;
            }
            return actual.Valor;
        }

        /// <summary>
        /// Elimina un nodo de la lista según su valor.
        /// </summary>
        /// <param name="valor">El valor del nodo a eliminar.</param>
        public void Eliminar(T valor)
        {
            if (cabeza == null)
                return;

            if (cabeza.Valor.Equals(valor))
            {
                cabeza = cabeza.Siguiente;
                tamaño--;
                return;
            }

            NodoSimple<T> actual = cabeza;
            while (actual.Siguiente != null && !actual.Siguiente.Valor.Equals(valor))
            {
                actual = actual.Siguiente;
            }

            if (actual.Siguiente != null)
            {
                actual.Siguiente = actual.Siguiente.Siguiente;
                tamaño--;
            }
        }

        /// <summary>
        /// Implementación del método abstracto para obtener el siguiente nodo en una lista simplemente enlazada.
        /// </summary>
        /// <param name="nodo">Nodo actual.</param>
        /// <returns>El siguiente nodo en la lista.</returns>
        protected override NodoSimple<T> ObtenerSiguiente(NodoSimple<T> nodo)
        {
            return nodo.Siguiente;
        }

        /// <summary>
        /// Recorre la lista y retorna los elementos como un string.
        /// </summary>
        /// <returns>Un string con los elementos de la lista, separados por comas.</returns>
        public string Recorrer()
        {
            NodoSimple<T> actual = cabeza;
            string resultado = "";

            while (actual != null)
            {
                // Si no es el primer elemento, agregar una coma
                if (resultado != "")
                    resultado += ", ";

                resultado += actual.Valor.ToString();  // Convierte el valor del nodo a string
                actual = actual.Siguiente;
            }

            return resultado;
        }

        /// <summary>
        /// Recorre la lista y compara cada elemento con el string dado.
        /// </summary>
        /// <param name="comparar">El string con el que se compararán los elementos de la lista.</param>
        /// <returns>Un string con los elementos que coincidan con el string dado, separados por comas.</returns>
        /// <summary>
        /// Recorre la lista y compara cada elemento con el string dado.
        /// </summary>
        /// <param name="comparar">El string con el que se compararán los elementos de la lista.</param>
        /// <returns>Devuelve <c>true</c> si hay al menos un elemento que coincida, <c>false</c> si no hay coincidencias.</returns>
        public bool Contiene(string comparar)
        {
            NodoSimple<T> actual = cabeza;

            while (actual != null)
            {
                if (actual.Valor.ToString() == comparar)  // Compara el valor del nodo con el string dado
                {
                    return true;  // Si hay una coincidencia, retorna true
                }
                actual = actual.Siguiente;
            }

            return false;  // Si no se encuentra ninguna coincidencia, retorna false
        }


        public void RecorrerEscribe(RichTextBox caja)
        {
            NodoSimple<T> nodoActual = cabeza; // Usar 'cabeza' en lugar de 'Primero'

            // Recorrer la lista desde el primer nodo
            caja.Clear();
            while (nodoActual != null)
            {
                // Escribir el valor del nodo en el RichTextBox
                caja.Text += nodoActual.Valor.ToString() + Environment.NewLine;

                // Pasar al siguiente nodo
                nodoActual = nodoActual.Siguiente;
            }
        }


    }


}
