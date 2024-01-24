using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Pieces : MonoBehaviour
{
    //references

    public GameObject controller;
    public GameObject movePlate;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //variabe to keep track of red or green player
    ///  i dont think i need the above at allllll 
    private string player;

    // references for all the sprites the chess piece can be 
    public Sprite Piece, Piece_Green, Piece_Blue;


    //starting

    public void Activate()
    {
        //access controller
        controller = GameObject.FindGameObjectWithTag("GameController");

        //take the instantiated location and adjust transform (place the pieces where they need to go)
        SetCoords();
        switch (this.name)
        {
            //get unity to search for name of the piece, and then spawn itself in place of the object (as far as i understand)
            case "Piece_Green": this.GetComponent<SpriteRenderer>().sprite = Piece_Green; break;
            case "Piece_Blue": this.GetComponent<SpriteRenderer>().sprite = Piece_Blue; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 2f;
        y *= 2f;

        //seems to control the mass of the pieces
        x += -2f;
        y += -2f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }
    //apparently lots of whats below is just to help with privacy in the code and not really needed.. 
    public int GetXboard()
    {
        return xBoard;
    }

    public int GetYboard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }
    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        DestroyMovePlates();

        InitiateMovePlates();
    }

    public void DestroyMovePlates()
    {
        //search for moveplate object in unity
        //apparently just created an array for a moveplate below!!?
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        //created a ... four loop
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }

    }

    public void InitiateMovePlates()
    {
        //this switch statement is setting up the different movements of chess piecces.. meanwhile i only have one1 type of movement for my pieces, and only one group as well, no black and white. its funny, because although technically speaking my game is "easier" to make, you need to know What to Delete, to Delete it. lmao
        switch (this.name)
        {
            //..this is for the black and white teams in chess. making the following code accesible to both teams.. so they both folllow through wiht it..
            // now idk wtf the case and break is for. i may not even need it cause all my damn pieces move the damn same!
            case "Piece_Green":
            case "Piece_Blue":
            case "Piece":
                SurroundMovePlate();
                //moving in a line like a bishop/queen would.. 
                //LineMovePlate(1, 1);
                // LineMovePlate(-1, -1);
                // LineMovePlate(-1, 1);
                // LineMovePlate(1, -1);
                break;
        }
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        // PointMovePlate(xBoard - 3, yBoard - 3);
        PointMovePlate(xBoard - 1, yBoard - 0);
        // PointMovePlate(xBoard - 3, yBoard + 3);
        // PointMovePlate(xBoard + 3, yBoard - 3);
        PointMovePlate(xBoard + 1, yBoard - 0);
        // PointMovePlate(xBoard + 3, yBoard + 3);
    }

    public void PointMovePlate(int x, int y)

    {

        GameControl sc = controller.GetComponent<GameControl>();

        if (sc.PositionOnBoard(x, y))
        {

            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Pieces>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 2f;
        y *= 2f;

        x += -2f;
        y += -2f;

        // i think the numbers below are what makes the array? spawning the map array above the map already made?
        // this is to display in unity...
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        // this is for us to keep track of? maybe the matrix thing is
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixY, matrixX);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        // i think the numbers below are what makes the array? spawning the map array above the map already made?
        // this is to display in unity...
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        // this is for us to keep track of? maybe the matrix thing is
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixY, matrixX);
    }

    public void ChangeColorBasedOnTile()
    {
        // Get the current tile's sprite renderer to access its color
        SpriteRenderer tileSpriteRenderer = GetCurrentTileSpriteRenderer();
        if (tileSpriteRenderer != null)
        {
            Color pieceColor = GetComponent<SpriteRenderer>().color;
            Color tileColor = tileSpriteRenderer.color;

            // Check if the colors are the same or different
            // Assuming color comparison is done based on the name or another identifier since Color comparison might not be exact
            if (pieceColor == tileColor)
            {
                // Change piece to white if it's the same color
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                // Change piece to black if it's a different color
                GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }

    // Method to get the SpriteRenderer of the current tile
    private SpriteRenderer GetCurrentTileSpriteRenderer()
    {
        // Implement logic to obtain the SpriteRenderer of the tile the piece is on
        // This could involve raycasting down or looking up the grid based on the piece's position
        // For example:
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        if (hit.collider != null && hit.collider.gameObject.CompareTag("Tile"))
        {
            return hit.collider.gameObject.GetComponent<SpriteRenderer>();
        }
        return null;
    }


}
