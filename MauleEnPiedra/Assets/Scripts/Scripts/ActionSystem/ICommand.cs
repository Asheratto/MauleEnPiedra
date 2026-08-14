using UnityEngine;

//Patron de diseño entonces ahora, se supone que es un sistema quien realiza estas acciones pero primero vamos a hacer unos comandos faciles
public interface ICommand
{
    public void Execute();
    public void Undo();
}
