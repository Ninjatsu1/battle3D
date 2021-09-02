using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarterAssets
{
    public class Attack : MonoBehaviour
    {
        private Animator _animator;
        private StarterAssetsInputs _input;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _input = GetComponent<StarterAssetsInputs>();
        }

        // Update is called once per frame
        void Update()
        {
            MeleeAttack();
        }
        private void MeleeAttack()
        {
            if (_input.attack)
            {
                Debug.Log("Play attack");
                _animator.SetTrigger("Attack");
            }
            _input.attack = false;
        }
    }
}

