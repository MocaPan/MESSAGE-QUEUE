using System;

namespace EstructurasPersonalizadas
{
    /// <summary>
    /// Clase que simula un arreglo dinámico sin usar List<T> ni Array de C#.
    /// </summary>
    /// <typeparam name="T">Tipo de datos a almacenar.</typeparam>
    public class Arreglo<T>
    {
        private T[] datos; // Arreglo interno para almacenar elementos
        private int capacidad; // Capacidad máxima antes de redimensionar
        private int conteo; // Número de elementos actualmente almacenados

        /// <summary>
        /// Constructor que inicializa el arreglo con una capacidad predeterminada.
        /// </summary>
        /// <param name="capacidadInicial">Tamaño inicial del arreglo.</param>
        public Arreglo(int capacidadInicial = 10)
        {
            if (capacidadInicial <= 0)
                throw new ArgumentException("La capacidad debe ser mayor que cero.");

            capacidad = capacidadInicial;
            datos = new T[capacidad]; // Inicializa el arreglo interno
            conteo = 0;
        }

        /// <summary>
        /// Constructor que permite inicializar el arreglo con datos preexistentes.
        /// </summary>
        /// <param name="datosIniciales">Arreglo con los datos a copiar.</param>
        public Arreglo(T[] datosIniciales)
        {
            if (datosIniciales == null || datosIniciales.Length == 0)
                throw new ArgumentException("El arreglo inicial no puede estar vacío.");

            capacidad = datosIniciales.Length;
            datos = new T[capacidad];
            Array.Copy(datosIniciales, datos, capacidad);
            conteo = capacidad;
        }

        /// <summary>
        /// Agrega un elemento al final del arreglo.
        /// </summary>
        /// <param name="valor">Elemento a agregar.</param>
        public void Agregar(T valor)
        {
            if (conteo == capacidad)
            {
                Redimensionar();
            }
            datos[conteo] = valor;
            conteo++;
        }

        /// <summary>
        /// Elimina un elemento en la posición especificada.
        /// </summary>
        /// <param name="index">Índice del elemento a eliminar.</param>
        public void Eliminar(int index)
        {
            if (index < 0 || index >= conteo)
            {
                throw new IndexOutOfRangeException("Índice fuera de rango.");
            }

            for (int i = index; i < conteo - 1; i++)
            {
                datos[i] = datos[i + 1]; // Desplaza los elementos hacia la izquierda
            }

            datos[conteo - 1] = default; // Limpia la última posición
            conteo--;
        }

        /// <summary>
        /// Obtiene el elemento en la posición especificada.
        /// </summary>
        /// <param name="index">Índice del elemento a obtener.</param>
        /// <returns>Elemento en la posición indicada.</returns>
        public T Obtener(int index)
        {
            if (index < 0 || index >= conteo)
            {
                throw new IndexOutOfRangeException("Índice fuera de rango.");
            }
            return datos[index];
        }

        /// <summary>
        /// Retorna el número de elementos almacenados.
        /// </summary>
        /// <returns>Cantidad de elementos en el arreglo.</returns>
        public int Tamaño()
        {
            return conteo;
        }

        /// <summary>
        /// Elimina todos los elementos del arreglo.
        /// </summary>
        public void Vaciar()
        {
            datos = new T[capacidad]; // Reinicia el arreglo interno
            conteo = 0;
        }

        /// <summary>
        /// Duplica la capacidad del arreglo cuando se llena.
        /// </summary>
        private void Redimensionar()
        {
            capacidad *= 2;
            T[] nuevoArreglo = new T[capacidad];

            for (int i = 0; i < conteo; i++)
            {
                nuevoArreglo[i] = datos[i];
            }

            datos = nuevoArreglo; // Reemplaza el arreglo viejo con el nuevo
        }

        /// <summary>
        /// Devuelve el contenido del arreglo como un array estándar de C#.
        /// </summary>
        /// <returns>Arreglo con los elementos almacenados.</returns>
        public T[] ObtenerDatos()
        {
            T[] copia = new T[conteo];
            Array.Copy(datos, copia, conteo);
            return copia;
        }
    }
}
