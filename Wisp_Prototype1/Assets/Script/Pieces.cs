using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PieceColor
{
    Red,
    Green,
    Blue,
    White,
    Black,
    None // Use this for default or error cases
}


public class Pieces : MonoBehaviour
{

    private Tilemap tilemap;
    private Material spriteMaterial;

    //references

    public GameObject controller;
    public GameObject movePlate;

    //Positions
    private int xBoard = -1;
    private int yBoard = -1;

    //variabe to keep track of red or green player
    ///  i dont think i need the above at allllll 
    private string player;

    /// old
     // references for all the sprites the chess piece can be 
    //public Sprite Piece, Piece_Green, Piece_Blue;

    // References for all the sprites the chess piece can be
    public Sprite Piece; // Let's assume this is red
    public Sprite Piece_Green;
    public Sprite Piece_Blue;

    // Current color of the piece
    public PieceColor pieceColor = PieceColor.None;


    private void Awake() // or Start, if you prefer
    {

        spriteMaterial = GetComponent<SpriteRenderer>().material;

    
        tilemap = FindObjectOfType<Tilemap>();

        // Assign the pieceColor based on the sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr.sprite == Piece) // Assuming 'Piece' is the red sprite
        {
            pieceColor = PieceColor.Red;
        }
        else if (sr.sprite == Piece_Green)
        {
            pieceColor = PieceColor.Green;
        }
        else if (sr.sprite == Piece_Blue)
        {
            pieceColor = PieceColor.Blue;
        }

        // Now your piece knows its color
    }

    public void ChangeColor(Color newColor)
    {
        if (spriteMaterial != null)
        {
            spriteMaterial.color = newColor;
        }
    }
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
        // Determine the piece's color based on its sprite
        if (this.GetComponent<SpriteRenderer>().sprite == Piece)
        {
            pieceColor = PieceColor.Red;
        }
        else if (this.GetComponent<SpriteRenderer>().sprite == Piece_Green)
        {
            pieceColor = PieceColor.Green;
        }
        else if (this.GetComponent<SpriteRenderer>().sprite == Piece_Blue)
        {
            pieceColor = PieceColor.Blue;
        }
    }


    public void SetCoords()
    {
        float x = xBoard * 2f - 2f;
        float y = yBoard * 2f - 2f;

        Debug.Log($"Setting coords for piece. Grid: ({xBoard}, {yBoard}), World: ({x}, {y})");

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
        TileBase currentTile = GetCurrentTile();
        if (currentTile != null)
        {
            Debug.Log($"Current tile found: {currentTile.name}");

            if (currentTile.name == "Tiles_Even_0" && pieceColor == PieceColor.Red)
            {
                Debug.Log("Current tile is 'Tiles_Even_0' (Red) and piece color is Red. Changing color to White.");
                spriteMaterial.color = Color.white; // Change color using material
            }
            else if (currentTile.name == "Tiles_Even_1" && pieceColor == PieceColor.Green)
            {
                Debug.Log("Current tile is 'Tiles_Even_1' (Green) and piece color is Green. Changing color to White.");
                spriteMaterial.color = Color.white; // Change color using material
            }
            else if (currentTile.name == "Tiles_Even_2" && pieceColor == PieceColor.Blue)
            {
                Debug.Log("Current tile is 'Tiles_Even_2' (Blue) and piece color is Blue. Changing color to White.");
                spriteMaterial.color = new Color(0f, 1f, 1f, 1f); // Change color using material
            }
            else
            {
                Debug.Log($"Current tile is '{currentTile.name}' and piece color is {pieceColor}. Changing color to Black.");
                spriteMaterial.color = Color.black; // Change color using material
            }
        }
        else
        {
            Debug.Log("No tile found under the piece.");
        }
    }


    // Method to get the SpriteRenderer of the current tile
    private TileBase GetCurrentTile()
    {
        Vector3Int cellPosition = tilemap.WorldToCell(transform.position);
        return tilemap.GetTile(cellPosition);
    }


}
