using UnityEngine;

public class MonsterAttackScript : MonoBehaviour
{
    private UserSessionScript _sessionScript;
    private void Start()
    {
        _sessionScript = FindFirstObjectByType<UserSessionScript>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(_sessionScript != null)
            {
                //_sessionScript.currentHealth--;
                MainGameScript.instance.DecreaseHealth(1);
                this.gameObject.SetActive(false);
            }
        }
    }
}
