using EstructurasPersonalizadas;

public class HistorialMensajes
{
    private Arreglo<ListaSimple<string>> Historial;
    private ListaSimple<string> Suscripciones;

    public HistorialMensajes(int capacidad)
    {
        this.Historial = new Arreglo<ListaSimple<string>>(capacidad);
        this.Suscripciones = new ListaSimple<string>();

        // Inicializa cada índice del historial con una lista vacía
        for (int i = 0; i < capacidad; i++)
        {
            this.Historial.Agregar(new ListaSimple<string>());
        }
    }

    // Método para agregar un mensaje con su tema
    public void AgregarMensaje(string tema, string mensaje)
    {
        int index = ObtenerIndiceTema(tema);

        if (index == -1) // Si el tema no existe, agrégalo
        {
            index = Suscripciones.Tamaño();
            Suscripciones.Agregar(tema);
            Historial.Agregar(new ListaSimple<string>());
        }

        Historial.Obtener(index).Agregar($"[{tema}]: {mensaje}");
    }

    // Método para obtener el índice de un tema en la lista de suscripciones
    private int ObtenerIndiceTema(string tema)
    {
        for (int i = 0; i < Suscripciones.Tamaño(); i++)
        {
            if (Suscripciones.Obtener(i) == tema)
                return i;
        }
        return -1; // No encontrado
    }

    // Método para actualizar los TextBox con el último mensaje de un tema
    public void ActualizarTextBox(string tema, TextBox txtTema, TextBox txtMensaje)
    {
        int index = ObtenerIndiceTema(tema);
        if (index != -1 && Historial.Obtener(index).Tamaño() > 0)
        {
            txtTema.Text = tema;
            txtMensaje.Text = Historial.Obtener(index).Obtener(Historial.Obtener(index).Tamaño() - 1);
        }
        else
        {
            txtTema.Text = tema;
            txtMensaje.Text = "No hay mensajes para este tema.";
        }
    }
}
