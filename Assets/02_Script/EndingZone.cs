using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌감지");
            Player player = other.GetComponent<Player>();

            if (player != null && player.HasItem("Doll"))
            {
                Debug.Log("클리어");
                SceneManager.LoadScene("Ending");
            }
        }
    }

}
