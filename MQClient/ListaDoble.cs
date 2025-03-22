using System;

namespace EstructurasPersonalizadas
{
    /// <summary>
    /// Nodo para listas doblemente enlazadas.
    /// </summary>
    /// <typeparam name="T">Tipo de dato almacenado en el nodo.</typeparam>
    class NodoDoble<T> : NodoBase<T>
    {
        /// <summary>
        /// Referencia al siguiente nodo en la lista.
        /// </summary>
        public NodoDoble<T> Siguiente { get; set; }

        /// <summary>
        /// Referencia al nodo anterior en la lista.
        /// </summary>
        public NodoDoble<T> Anterior { get; set; }

        /// <summary>
        /// Constructor que inicializa el nodo con un valor.
        /// </summary>
        /// <param name="valor">Valor del nodo.</param>
        public NodoDoble(T valor) : base(valor)
        {
            Siguiente = null;
            Anterior = null;
        }
    }

    /// <summary>
    /// Implementación de una lista doblemente enlazada que permite almacenar y manipular elementos de tipo <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Tipo de los elementos almacenados en la lista.</typeparam>
    class ListaDoble<T> : ListaBase<NodoDoble<T>, T>
    {
        private NodoDoble<T> cola; // Nodo final de la lista

        /// <summary>
        /// Agrega un nuevo elemento al final de la lista.
        /// </summary>
        /// <param name="valor">El valor del elemento a agregar.</param>
        public void Agregar(T valor)
        {
            NodoDoble<T> nuevoNodo = new NodoDoble<T>(valor);

            if (cabeza == null)
            {
                cabeza = cola = nuevoNodo;
            }
            else
            {
                cola.Siguiente = nuevoNodo;
                nuevoNodo.Anterior = cola;
                cola = nuevoNodo;
            }

            tamaño++;
        }

        /// <summary>
        /// Obtiene el valor almacenado en una posición específica de la lista.
        /// Utiliza un recorrido desde <c>cabeza</c> o <c>cola</c> según la cercanía al índice.
        /// </summary>
        /// <param name="indice">Posición del elemento a obtener (basado en 0).</param>
        /// <returns>El valor almacenado en la posición indicada.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Se lanza si el índice está fuera del rango de la lista.
        /// </exception>
        public override T Obtener(int indice)
        {
            if (indice < 0 || indice >= tamaño)
                throw new IndexOutOfRangeException("Índice fuera de rango");

            NodoDoble<T> actual;

            // Determinar si recorrer desde cabeza o desde cola
            if (indice < tamaño / 2)
            {
                actual = cabeza;
                for (int i = 0; i < indice; i++)
                    actual = actual.Siguiente;
            }
            else
            {
                actual = cola;
                for (int i = tamaño - 1; i > indice; i--)
                    actual = actual.Anterior;
            }

            return actual.Valor;
        }

        /// <summary>
        /// Elimina un elemento de la lista según su valor.
        /// </summary>
        /// <param name="valor">El valor del elemento a eliminar.</param>
        public void Eliminar(T valor)
        {
            if (cabeza == null)
                return;

            if (cabeza.Valor.Equals(valor))
            {
                cabeza = cabeza.Siguiente;
                if (cabeza != null)
                    cabeza.Anterior = null;
                else
                    cola = null; // Si la lista queda vacía, también actualizamos cola

                tamaño--;
                return;
            }

            NodoDoble<T> actual = cabeza;
            while (actual != null && !actual.Valor.Equals(valor))
                actual = actual.Siguiente;

            if (actual != null)
            {
                if (actual.Siguiente != null)
                    actual.Siguiente.Anterior = actual.Anterior;
                else
                    cola = actual.Anterior; // Si eliminamos el último nodo, actualizamos cola

                if (actual.Anterior != null)
                    actual.Anterior.Siguiente = actual.Siguiente;

                tamaño--;
            }
        }

        /// <summary>
        /// Implementación del método abstracto para obtener el siguiente nodo en una lista doblemente enlazada.
        /// </summary>
        /// <param name="nodo">Nodo actual.</param>
        /// <returns>El siguiente nodo en la lista.</returns>
        protected override NodoDoble<T> ObtenerSiguiente(NodoDoble<T> nodo)
        {
            return nodo.Siguiente;
        }
    }
}
