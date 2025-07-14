using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Button attackButton;
    private Animator animator;

    private PlayerController playerController;
    private float attackCooldown = .25f;
    private bool canAttack;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();

        attackButton.onClick.AddListener(AttackMethod);
        canAttack = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AttackMethod();
        }
    }
    public void AttackMethod()
    {
        if (!canAttack) return;

        StartCoroutine(AttackAnimation());
    }

    private IEnumerator AttackAnimation()
    {
        playerController.canMove = false;
        canAttack = false;
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(attackCooldown);

        playerController.canMove = true;
        canAttack = true;
    }
}
