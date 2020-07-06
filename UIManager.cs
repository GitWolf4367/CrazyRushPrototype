using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /* UIManager script does the following:
     * 1.Show/Hide UI elements (Buttons, Menus)
     * 2.Update UI elements (Live, coins amount)
     */

    public Text cointAmountText; //on-screen coins amount

    public GameObject[] uiHeartsarr = new GameObject[3]; //array for amount of hearts displayed on screen
    public GameObject mainMenuButtons; //main menu buttons
    public GameObject topHeartUIButtons; //hearts
    public GameObject debugMenuPopUp; //debug menu
    public GameObject tutorialMenuPopUp; //tutorial menu
    public GameObject topGoldHomeUIButtons; //home button  

    PlayerData playerData;
    GameManager gameManager;
    Animator mainCharacterAnimator; 
    MainCharacterController mainCharacterController;

    // Start is called before the first frame update
    void Start()
    {
        playerData = GetComponent<PlayerData>();
        gameManager = GetComponent<GameManager>();
        mainCharacterAnimator = GetComponent<Animator>(); 
        mainCharacterController = GetComponent<MainCharacterController>();

        topHeartUIButtons.SetActive(false); //turns off hearts 
        topGoldHomeUIButtons.SetActive(false); //turns off gold home button
    }

    void Update()
    {
            cointAmountText.text = playerData.coinsAmount.ToString(); //sets the coins text in UI to coin Amount every frame
    }


    public void ClickOnPlayButton() //when play button is clicked
    {
        mainMenuButtons.SetActive(false);
        tutorialMenuPopUp.SetActive(false);
        debugMenuPopUp.SetActive(false);
        topHeartUIButtons.SetActive(true);
        topGoldHomeUIButtons.SetActive(true);  
        mainCharacterController.StartRunningOntrack();
    }

    public void ShowDebugMenu()
    {
        tutorialMenuPopUp.SetActive(false);
        debugMenuPopUp.SetActive(true);
    }

    public void ShowTutorialMenu()
    {
        debugMenuPopUp.SetActive(false);
        tutorialMenuPopUp.SetActive(true);
    }

    public void HideTutorialMenu()
    {
        tutorialMenuPopUp.SetActive(false);
    }

    public void HideDebugMenu()
    {
        debugMenuPopUp.SetActive(false);
    }

}
