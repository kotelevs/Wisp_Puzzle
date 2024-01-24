using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;
//using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public GameObject Piece;

    //positions of everything
    //is this the array. tell me someone 
    private GameObject[,] positions = new GameObject[3, 3];
    private GameObject[] player = new GameObject[6];

    //private string currentPlayer = "white";

    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        // spawn
        //Instantiate(Piece, new Vector3(0, 0), Quaternion.identity);

        player = new GameObject[] {
            Create("Piece_Green", 1, 1),
           // Create("Piece_Green", 5, 2),
            Create("Piece_Blue", 1, 0),
           // Create("Piece_Blue", 2, 8),
           // Create("Piece", 2, 1),
            Create("Piece", 1, 2)
        };

        // set all place positions on the position board
        for (int i = 0; i < player.Length; i++)
        {
            SetPosition(player[i]);
        }

    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(Piece, new Vector3(0, 0), Quaternion.identity);
        Pieces cm = obj.GetComponent<Pieces>();
        //  generalcontrol cm = obj.GetComponent<generalcontrol>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();
        // SetPosition(obj);
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Pieces cm = obj.GetComponent<Pieces>();

        positions[cm.GetXboard(), cm.GetYboard()] = obj;
    }

    // to allow pieces to "move", need to make their original location to be empty
    public void SetPositionEmpty(int x, int y)
    {
        //access ~positions array~ and set it to null so theres nothing there
        positions[x, y] = null;
    }
    //return the game piece object to a given position
    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    // see if the position is on the array board...?
    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            //code along with differnet chess players
            //SceneManager.LoadScene("Prototype");
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("WinnerTag").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerTag").GetComponent<Text>().text = playerWinner + " is the winner";

        GameObject.FindGameObjectWithTag("RestartTag").GetComponent<Text>().enabled = true;
    }
}
