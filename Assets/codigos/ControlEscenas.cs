using UnityEngine;

public class ControlEscenas:MonoBehaviour
{
    public string QueEscena;
    public string EscenaActual;

    public void CargarEscena(string queEscena)
    {
        print("Escena cargada");
    }

    public void EscenaSiguiente()
    {
        print("Ir a escena siguiente");
    }

    public void EscenaAnterior()
    {
        print("Ir a escena anterior");
    }

}