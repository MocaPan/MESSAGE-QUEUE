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
        /// Agrega un nuevo elemento al final de la lista (alias de Agregar).
        /// </summary>
        /// <param name="valor">El valor del elemento a agregar.</param>
        public void AddLast(T valor)
        {
            Agregar(valor);
        }

        /// <summary>
        /// Agrega un nuevo elemento al principio de la lista.
        /// </summary>
        /// <param name="valor">El valor del elemento a agregar.</param>
        public void AddFirst(T valor)
        {
            NodoDoble<T> nuevoNodo = new NodoDoble<T>(valor);

            if (cabeza == null)
            {
                cabeza = cola = nuevoNodo;
            }
            else
            {
                nuevoNodo.Siguiente = cabeza;
                cabeza.Anterior = nuevoNodo;
                cabeza = nuevoNodo;
            }

            tamaño++;
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
        /// Elimina el primer elemento de la lista.
        /// </summary>
        public void RemoveFirst()
        {
            if (cabeza == null)
                throw new InvalidOperationException("La lista está vacía");

            cabeza = cabeza.Siguiente;
            if (cabeza != null)
                cabeza.Anterior = null;
            else
                cola = null; // Si la lista queda vacía, también actualizamos cola

            tamaño--;
        }

        /// <summary>
        /// Elimina el último elemento de la lista.
        /// </summary>
        public void RemoveLast()
        {
            if (cola == null)
                throw new InvalidOperationException("La lista está vacía");

            if (cabeza == cola)
            {
                cabeza = cola = null; // Si hay un solo elemento
            }
            else
            {
                cola = cola.Anterior;
                cola.Siguiente = null;
            }

            tamaño--;
        }

        /// <summary>
        /// Obtiene el valor almacenado en una posición específica de la lista.
        /// Utiliza un recorrido desde <c>cabeza</c> o desde <c>cola</c> según la cercanía al índice.
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
        /// Implementación del método abstracto para obtener el siguiente nodo en una lista doblemente enlazada.
        /// </summary>
        /// <param name="nodo">Nodo actual.</param>
        /// <returns>El siguiente nodo en la lista.</returns>
        protected override NodoDoble<T> ObtenerSiguiente(NodoDoble<T> nodo)
        {
            return nodo.Siguiente;
        }

        /// <summary>
        /// Verifica si el valor existe en la lista.
        /// </summary>
        /// <param name="valor">Valor a verificar en la lista.</param>
        /// <returns>True si el valor está presente en la lista, de lo contrario false.</returns>
        public bool Contiene(T valor)
        {
            NodoDoble<T> actual = cabeza;
            while (actual != null)
            {
                if (actual.Valor.Equals(valor))
                    return true;
                actual = actual.Siguiente;
            }
            return false;
        }

        /// <summary>
        /// Limpia todos los elementos de la lista.
        /// </summary>
        public void Limpiar()
        {
            cabeza = cola = null;
            tamaño = 0;
        }

        /// <summary>
        /// Devuelve el primer elemento de la lista.
        /// </summary>
        public T PrimerElemento()
        {
            if (cabeza == null)
                throw new InvalidOperationException("La lista está vacía");
            return cabeza.Valor;
        }

        /// <summary>
        /// Devuelve el último elemento de la lista.
        /// </summary>
        public T UltimoElemento()
        {
            if (cola == null)
                throw new InvalidOperationException("La lista está vacía");
            return cola.Valor;
        }

        /// <summary>
        /// Muestra todos los elementos de la lista en orden.
        /// </summary>
        public void Mostrar()
        {
            NodoDoble<T> actual = cabeza;
            while (actual != null)
            {
                Console.WriteLine(actual.Valor);
                actual = actual.Siguiente;
            }
        }
    }
}
