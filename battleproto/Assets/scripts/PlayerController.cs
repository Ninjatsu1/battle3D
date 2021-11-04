using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class PlayerController : MonoBehaviour
    {
        private Animator _animator;
        private StarterAssetsInputs _input;
        private CharacterController _controller;
        public int baseDamage = 3;
        public Transform attackPoint;
        public float attackRange = 0.5f;
        public LayerMask enemyLayers;

        public float dashSpeed;
        public float dashTime;

        private int attackHash;
        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _input = GetComponent<StarterAssetsInputs>();
            _controller = GetComponent<CharacterController>();
            attackHash = Animator.StringToHash("Attack");
        }

        // Update is called once per frame
        void Update()
        {
            MeleeAttack();

           Sidestep();
        }

        //Create hit detection
        private void DetectHit()
        {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);
            for (int i = 0; i < hitEnemies.Length; i++)
            {
                if (hitEnemies[i].CompareTag("Enemy"))
                {
                    Debug.Log("Enemy hit");
                    EnemyStats enemyStats = hitEnemies[i].GetComponent<EnemyStats>();
                    if (enemyStats != null)
                    {
                        Debug.Log("Enemy damaged");
                        enemyStats.TakeDamage(baseDamage);
                    }
                }
            }
        }
        //Check if player pressed button, then animete 
        private void MeleeAttack()
        {
            if (_input.attack)
            {
                DetectHit();
                Debug.Log("Play attack");
                _animator.SetTrigger(attackHash);
            }
            _input.attack = false;
        }


        private void Sidestep()
        {
           if (_input.sideStep)
           {
                StartCoroutine(Dash());
           }
             _input.sideStep = false;
        }

        IEnumerator Dash()
        {
            Debug.Log("Dashing");
            float startTime = Time.time;
            while (Time.time < startTime + dashTime)
            {
                _controller.Move(transform.forward * dashSpeed * Time.deltaTime);
                yield return null;
            }
            
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}