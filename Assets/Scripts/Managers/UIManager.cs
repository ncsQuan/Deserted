using Lean.Transition;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [Header("UI Components")]
    public Slider energySlider;
    public Slider manaSlider;
    public Slider healthSlider;
    public Gradient healthGradient;
    public Image healthImage;

    public GameObject pauseMenu;
    public PlayerInput playerInput;
    public GameObject gameOverMenu;
    public GameObject spellTipWindow;
    public Text spellTipText;
    private PlayerInterface playerInterface;

    #region Event Listeners
    private UnityAction<float> foodPickupEventListener;
    private UnityAction<int> spellCastEventListener;
    private UnityAction playerDeathEventListener;
    private UnityAction playerWinEventListener;
    private UnityAction<float> playerDamageEventListener;
    //private UnityAction spellPickUpEventListener;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {

        #region Initializing event handlers
        foodPickupEventListener = new UnityAction<float>(foodPickupEventHandler);
        spellCastEventListener = new UnityAction<int>(spellCastEventHandler);
        playerDeathEventListener = new UnityAction(playerDeathEventHandler);
        playerWinEventListener = new UnityAction(playerWinEventHandler);
        playerDamageEventListener = new UnityAction<float> (playerDamageEventHandler);
        //spellPickUpEventListener = new UnityAction(spellPickUpEventHandler);
        #endregion
    }

    

    private void Start()
    {
        playerInterface = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInterface>();
        if (playerInterface == null)
        {
            Debug.LogError("Missing PlayerInterface component must be attached to an object with the player tag");
        }
        Time.timeScale = 1.0f;
        energySlider.maxValue = playerInterface.getMaxEnergy();
        manaSlider.maxValue = playerInterface.getMaxMana();
        healthSlider.maxValue = playerInterface.getMaxHealth();
    }


    private void OnEnable()
    {
        EventManager.StartListening<FoodPickupEvent, float>(foodPickupEventListener);
        EventManager.StartListening<SpellCastEvent, int>(spellCastEventListener);
        EventManager.StartListening<PlayerDeathEvent>(playerDeathEventListener);
        EventManager.StartListening<PlayerWinEvent>(playerWinEventListener);
        EventManager.StartListening<PlayerDamageEvent, float>(playerDamageEventListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening<FoodPickupEvent, float>(foodPickupEventListener);
        EventManager.StopListening<SpellCastEvent, int>(spellCastEventListener);
        EventManager.StopListening<PlayerDeathEvent>(playerDeathEventListener);
        EventManager.StopListening<PlayerWinEvent>(playerWinEventListener);
        EventManager.StopListening<PlayerDamageEvent, float>(playerDamageEventListener);
    }

    #region Helper Functions
    public void SwitchScene(string sceneName)
    {
        int playerCamerasIndex = 2;
        int rockSceneIndex = 3;
        int numberOfScenes = SceneManager.sceneCount;
        Debug.Log(numberOfScenes);
        if (numberOfScenes > 2)
        {
            SceneManager.UnloadSceneAsync(playerCamerasIndex);
            SceneManager.UnloadSceneAsync(rockSceneIndex);
        }
        SceneManager.LoadScene(sceneName);
        SceneManager.LoadScene(playerCamerasIndex, LoadSceneMode.Additive);
        SceneManager.LoadScene(rockSceneIndex, LoadSceneMode.Additive);

    }

    public void PauseGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Intro Scene");
    }

    public void ContinueGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false; ;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CloseSpellTip() {
        Cursor.visible = false; ;
        Cursor.lockState = CursorLockMode.Locked;
        spellTipWindow.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
    }

    public void updateEnergy(float newEnergy) {
        energySlider.value = newEnergy;
    }

    public void updateMana(float newMana)
    {
        manaSlider.value = newMana;
    }

    #endregion

    #region Event Handlers
    private void foodPickupEventHandler(float energyRestored)
    {
        energySlider.value += energyRestored;
    }

    private void playerDamageEventHandler(float damage)
    {
        healthSlider.value -= damage;
        healthImage.color = healthGradient.Evaluate(healthSlider.normalizedValue);
    }
    private void spellCastEventHandler(int manaCost)
    {
        manaSlider.value += manaCost;
    }

    private void playerDeathEventHandler()
    {
        GameOver();
    }

    private void playerWinEventHandler()
    {
        Cursor.visible = true;
        Cursor.lockState= CursorLockMode.None;
        SceneManager.LoadScene("WinScene");
    }

    public void spellPickUpEventHandler(SpellScriptableObject spell)
    {
        spellTipText.text = spell.tip;
        spellTipText.alignment = TextAnchor.UpperCenter;
        spellTipText.horizontalOverflow = HorizontalWrapMode.Wrap;
        spellTipText.verticalOverflow = VerticalWrapMode.Overflow;
        spellTipText.lineSpacing = 1;
        spellTipText.fontSize = 30;
        spellTipWindow.GetComponent<LeanManualAnimation>().BeginTransitions();
    }
    #endregion
}
