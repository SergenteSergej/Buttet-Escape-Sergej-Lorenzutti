using UnityEngine;

public class Game_Over_On_Collision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Mouve_Bullets>())
        {
            GameMenager gameMenager = FindFirstObjectByType<GameMenager>();

            gameMenager.GameOver();
        }
    }
}