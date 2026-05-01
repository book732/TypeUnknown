using UnityEngine;

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
                GameManager.Instance.QuitGame();

            }
        }
    }

}
