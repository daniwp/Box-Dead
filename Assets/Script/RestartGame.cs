using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public Button yourButton;
    public GameObject player;
    PlayerHealth playerHealth;

    void Start()
    {
        yourButton.onClick.AddListener(TaskOnClick);
        yourButton.gameObject.SetActive(false);
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void FixedUpdate()
    {
        if (playerHealth.getIsDead())
        {
            yourButton.gameObject.SetActive(true);
        }
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("MenuScene");
    }
}