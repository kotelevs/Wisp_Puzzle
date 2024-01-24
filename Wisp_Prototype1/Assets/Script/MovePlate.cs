using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;
    //this is how the gameobkect piece that spawned the moveplate has reference to the moveplate? or how moveplate has access to game piece..
    GameObject reference = null;

    //board positions not world positions
    int matrixX;
    int matrixY;

    //a unit can move to location, or move to location and attack. differentiate. 
    //false is movement, true is attacking 
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //change colour of sprite to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<GameControl>().GetPosition(matrixX, matrixY);

            Destroy(cp);
        }
        //after moving by tapping moveplate, set the original referenced position to empty
        controller.GetComponent<GameControl>().SetPositionEmpty(reference.GetComponent<Pieces>().GetXboard(),
            reference.GetComponent<Pieces>().GetYboard());

        reference.GetComponent<Pieces>().SetXBoard(matrixX);
        reference.GetComponent<Pieces>().SetYBoard(matrixY);
        reference.GetComponent<Pieces>().SetCoords();

        // After moving the piece
       // reference.GetComponent<Pieces>().SetXBoard(matrixX);
       // reference.GetComponent<Pieces>().SetYBoard(matrixY);
       // reference.GetComponent<Pieces>().SetCoords();
        reference.GetComponent<Pieces>().ChangeColorBasedOnTile(); // Add this line

        //allows the game controller to also keep track of whats going on here.. keep it in the know fr
        controller.GetComponent<GameControl>().SetPosition(reference);

        //switching from one player to the next.. which i dont need
        // controller.GetComponent<GameControl>().NextTurn();

        reference.GetComponent<Pieces>().DestroyMovePlates();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}