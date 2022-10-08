using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Variablels needed for gameplay. 
    [SerializeField] int playerTurn; // 0 = X player turn,,, 1 = O player turn 
    [SerializeField] int turnCount; // counts the turns played 
    [SerializeField] GameObject[] turnIcons; // Displays the players turn through the indicator
    [SerializeField] Sprite[] playerIcons; // Holds player sprite icons  
    [SerializeField] Button[] boardSpaces; //Spaces playable on the board 
    [SerializeField] int[] buttonsPressed; // Checks which space was marked by which player. 
    [SerializeField] Text winnerText; // Text for winning text
    [SerializeField] GameObject[] winningLines; // All the winning lines; 
    [SerializeField] GameObject WinnerPanel;
    [SerializeField] int xScore; // Player X Score
    [SerializeField] int oScore; //Player O Score
    [SerializeField] Text xScoreText; // Text for x players score
    [SerializeField] Text oScoreText; // Text for o players score
    [SerializeField] Button xPlayerButton; // Button for x player to go first 
    [SerializeField] Button oPlayerButton; // Button for o player to go first 
    [SerializeField] GameObject catImage; // Image for cat (tieing in the game)
    [SerializeField] AudioSource buttonClickedAudio;
    [SerializeField] AudioSource restartAudio;

    void Start()
    {
        ResetGame(); 
    }

    void ResetGame()
    {
        //Setting the turn to the x player in the beginning of the game.
        playerTurn = 0;  
        turnCount = 0;
        // X player icon is set to true O player icon set to false at the beginning of the game.  
        turnIcons[0].SetActive(true);
        turnIcons[1].SetActive(false); 
        //Reset the buttons on the board spaces 
        for(int i = 0; i < boardSpaces.Length; i++)
        {
            // Make spaces interactable
            boardSpaces[i].interactable = true;
            boardSpaces[i].GetComponent<Image>().sprite = null; 
        }
        for(int i = 0; i < buttonsPressed.Length; i++)
        {
            buttonsPressed[i] = -100; 
        }
    }

    public void GridButtons(int buttonClicked)
    {
        //Make sure players can't change turns mid game. 
        xPlayerButton.interactable = false;
        oPlayerButton.interactable = false; 
        //Check which button was clicked 
        boardSpaces[buttonClicked].image.sprite = playerIcons[playerTurn];
        //Button clicked can't be clicked again 
        boardSpaces[buttonClicked].interactable = false;
        //Checks which player picked which space 
        buttonsPressed[buttonClicked] = playerTurn+1;
        turnCount++; 
        if(turnCount > 4)
        {
            bool isWinner = CheckForWinner();
            if(turnCount == 9 && isWinner == false)
            {
                Cat(); 
            }
        }

        //Change players turn 
        if(playerTurn == 0)
        {
            playerTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true);
        }
        else
        {
            playerTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);
        }
    }

    bool CheckForWinner()
    {
        //All solutions a player can have to win (three in a row)
        int condition1 = buttonsPressed[0] + buttonsPressed[1] + buttonsPressed[2];
        int condition2 = buttonsPressed[3] + buttonsPressed[4] + buttonsPressed[5];
        int condition3 = buttonsPressed[6] + buttonsPressed[7] + buttonsPressed[8];
        int condition4 = buttonsPressed[0] + buttonsPressed[3] + buttonsPressed[6];
        int condition5 = buttonsPressed[1] + buttonsPressed[4] + buttonsPressed[7];
        int condition6 = buttonsPressed[2] + buttonsPressed[5] + buttonsPressed[8];
        int condition7 = buttonsPressed[0] + buttonsPressed[4] + buttonsPressed[8];
        int condition8 = buttonsPressed[2] + buttonsPressed[4] + buttonsPressed[6];

        var solutions = new int[] { condition1, condition2, condition3, condition4, condition5, condition6, condition7, condition8 }; 
        
        for(int i = 0; i < solutions.Length; i++)
        {
            //Using math :(
            //Comparing the solution to the number in the player turn to check who won the game. 
            if (solutions[i] == 3 * (playerTurn + 1))
            {
                //Display the winner. 
                Debug.Log("Player " + playerTurn + " won!");
                DisplayWinner(i);
                return true;  
            }
        }
        return false;
    }
    
    void DisplayWinner(int indexIn)
    {
        WinnerPanel.gameObject.SetActive(true); 
        if(playerTurn == 0)
        {
            xScore++;
            xScoreText.text = xScore.ToString(); 
            winnerText.text = "Player X Won In " + turnCount + " turns!"; 
        }else if(playerTurn == 1)
        {
            oScore++; 
            oScoreText.text = oScore.ToString();
            winnerText.text = "Player O Won In " + turnCount + " turns!"; 
        }
        winningLines[indexIn].SetActive(true);
    }

    public void Rematch()
    {
        ResetGame(); 
        for(int i = 0; i < winningLines.Length; i++)
        {
            winningLines[i].SetActive(false);
        }
        WinnerPanel.SetActive(false);
        xPlayerButton.interactable = true;
        oPlayerButton.interactable = true;
        catImage.SetActive(false);
    }

    public void Restart()
    {
        Rematch();
        xScore = 0;
        oScore = 0;
        xScoreText.text = "0";
        oScoreText.text = "0"; 
    }

    public void SwitchPlayer(int selectPlayer)
    {
        if (selectPlayer == 0)
        {
            playerTurn = 0;
            turnIcons[0].SetActive(true);
            turnIcons[1].SetActive(false);   
        } else if (selectPlayer == 1)
        {
            playerTurn = 1;
            turnIcons[0].SetActive(false);
            turnIcons[1].SetActive(true); 
        }
    }

    void Cat()
    {
        WinnerPanel.SetActive(true); 
        catImage.SetActive(true);
        winnerText.text = "Tie Game!"; 
    }

    public void PlayButtonClickedAudio()
    {
        buttonClickedAudio.Play(); 
    }
    public void RestartRematchAudio()
    {
        restartAudio.Play();
    }

}
