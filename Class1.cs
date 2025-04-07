while (!finPartida && !Comprueba(tab, out pos))
{

    if (!Comprueba(tab, out pos))
    {
        // Leemos de la consola el numero que toque.
        char ch = LeeInput();

        // En el caso de que sea una 'q', deberemos de terminar la partida.
        if (ch == 'q') finPartida = true;

        // Actualiza el estado de la casilla en el tablero.
        ActualizaEstado(ch, ref tab);

        if (ch != ' ' && ch != 'c') Render(tab);
    }

    else
    {
        Console.Clear();
        Console.WriteLine("Fin de partida. Has ganado!!");
        finPartida = true;
    }

}


static bool Comprueba(Tablero tab, out Posicion pos)
{
    // Esta variable marca si se ha rellenado todo el tablero.
    bool completo = false;

    // Devuelve donde está en 1 en el tablero.
    pos = Busca1(tab);

    // Devuelve el último numero colocado en el tablero.
    int maxNum = Ultimo(tab);

    //
    Posicion pos1;

    // Iteraremos por todos los números encontrando si todos están relacionados con la casilla de al lado.
    int i = 0;

    while (i < maxNum)
    {
        // Pos1 nos devuelve la posición de la casilla de siguiente número.
        if (Siguiente(tab, pos, out pos1))
        {
            pos = pos1;
            i++;
        }
    }

    if (i == maxNum) completo = true;

    return completo;
}

/// ALEXIA

static bool Comprueba(Tablero tab, out Posicion pos)
{
    // Esta variable marca si se ha rellenado todo el tablero.
    bool completo = false;

    // Devuelve donde está en 1 en el tablero.
    pos = Busca1(tab);


    int maxNum = Ultimo(tab);
    Posicion pos1;

    int i = 0;
    while (Siguiente(tab, pos, out pos1) && i < maxNum)
    {
        pos = pos1;
        i++;
    }

    if (i == maxNum) completo = true;

    return completo;
}