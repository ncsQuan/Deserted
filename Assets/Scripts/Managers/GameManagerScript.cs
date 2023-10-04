using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


public class GameManagerScript : MonoBehaviour
{
    public Slider healthBar;
    public GameObject gameOverUI;
    private bool isDead;
    public GameObject Player;
    public UIManager uiManager;

    private PlayerInterface playerInterface;
    private PlayerController playerController;

    #region Event Subscriptions
    private UnityAction<float> FoodPickupEventListener;
    private UnityAction<int> SpellCastEventListener;
    private UnityAction<float> PlayerDamageEventListener;
    #endregion

    #region Resource Management
    [Header("Resource Management")]
    [Range(1f, 5f)]
    public float energyDecreasePerSecond = 1f;
    #endregion

    private void Awake()
    {
        playerInterface = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInterface>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        if (playerInterface == null)
        {
            Debug.LogError("Missing PlayerInterface component must be attached to an object with the player tag");
        }
        FoodPickupEventListener = new UnityAction<float>(FoodPickupEventHandler);
        SpellCastEventListener = new UnityAction<int>(SpellCastEventHandler);
        PlayerDamageEventListener = new UnityAction<float>(PlayerDamageEventHandler);
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverUI.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        /*if (healthBar.value <= 0 && !isDead)
        {
            isDead = true;
            Player.SetActive(false);
            gameOver();
            Debug.Log("Dead");
        }*/
        decreaseEnergyOverTime();

        // If we're sprinting, consume mana
        if (playerController.GetComponent<Animator>().GetBool("sprint")) {
            decreaseManaOverTime();
        }
        else
        {
            regenerateManaOverTime();
        }
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void restart()
    {
        SceneManager.LoadScene("Tutorial Scene");
        SceneManager.LoadScene("PlayerCameras", LoadSceneMode.Additive);
        SceneManager.LoadScene("RockScene", LoadSceneMode.Additive);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Intro Scene");

    }

    public void quit()
    {
        Application.Quit();

    }

    private void OnEnable()
    {
        EventManager.StartListening<FoodPickupEvent, float>(FoodPickupEventListener);
        EventManager.StartListening<SpellCastEvent, int>(SpellCastEventListener);
        EventManager.StartListening<PlayerDamageEvent, float>(PlayerDamageEventListener);
    }

    private void OnDisable()
    {
        EventManager.StopListening<FoodPickupEvent, float>(FoodPickupEventListener);
        EventManager.StopListening<SpellCastEvent, int>(SpellCastEventListener);
        EventManager.StopListening<PlayerDamageEvent, float>(PlayerDamageEventListener);
    }

    #region Helper Update Functions
    private void decreaseEnergyOverTime() {
        float decreaseRate = -1 * Time.deltaTime * (energyDecreasePerSecond * 60f) / 60f;
        playerInterface.setEnergy(decreaseRate);
        uiManager.updateEnergy(playerInterface.getCurrentEnergy());
    }

    private void decreaseManaOverTime()
    {
        float decreaseRate = -1 * Time.deltaTime * ((energyDecreasePerSecond * 1.8f) * 60f) / 60f;
        playerInterface.setMana(decreaseRate);
        uiManager.updateMana(playerInterface.getCurrentMana());
    }

    private void regenerateManaOverTime()
    {
        float regenerateRate = 1 * Time.deltaTime * (energyDecreasePerSecond * 60f) / 60f;
        playerInterface.setMana(regenerateRate);
        uiManager.updateMana(playerInterface.getCurrentMana());
    }
    #endregion

    #region Event Handlers
    private void FoodPickupEventHandler(float energyRestored) {
        playerInterface.setEnergy(energyRestored);
    }

    private void SpellCastEventHandler(int manaCost)
    {
        playerInterface.setMana(manaCost);
    }

    private void PlayerDamageEventHandler(float damage)
    {
        playerInterface.takeDamage((int)damage);
    }
    #endregion
}

