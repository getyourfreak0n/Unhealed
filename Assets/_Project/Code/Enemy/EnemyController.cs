using System.Collections;
using _Project.Code.Audio;
using _Project.Code.Gameplay.Combat;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Code.Enemy
{
    public class EnemyController : MonoBehaviour, IEnemy, IDamageable
    {
        [SerializeField] private GameObject vfxPrefab;
        [SerializeField] private AudioClip damageClip;
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] GameObject player;

        NavMeshAgent _agent;
        CinemachineImpulseSource _impulse;
        MeshRenderer _renderer;
        Coroutine _flickerCoroutine;
        Coroutine _shootCoroutine;
        float _health = 100f;

        void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
            _agent = GetComponent<NavMeshAgent>();
            _impulse = GetComponent<CinemachineImpulseSource>();
        }
        
        void Update()
        {
            if (player == null) return;
            
            ChasePlayer();
            ShootAtPlayer();
        }

        void ChasePlayer()
        {
            _agent.SetDestination(player.transform.position);
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            FlickerEffect();
            DamageSFX();
            Debug.Log($"Ouch! Took {damage} damage!");

            if (_health <= 0f)
            {
                Destroy(gameObject);
            }
        }

        public void CameraImpulse()
        {
            _impulse.GenerateImpulse();
        }

        public void FlickerEffect()
        {
            if (_flickerCoroutine == null)
            {
                _flickerCoroutine = StartCoroutine(Flicker());
            }
        }

        public void DamageSFX()
        {
            AudioManager.Instance.PlaySound(damageClip, 0.5f, 0.3f, 0.5f);
        }

        public void HitVFX(Vector3 hitPoint, Vector3 hitDirection)
        {
            GameObject instantiate =Instantiate(vfxPrefab, hitPoint, Quaternion.LookRotation(hitDirection));
            Destroy(instantiate, 1.5f);
        }

        void ShootAtPlayer()
        {
            if (_shootCoroutine == null)
            {
                _shootCoroutine = StartCoroutine(Shoot());
            }
        }

        IEnumerator Flicker()
        {
            WaitForSeconds wait = new WaitForSeconds(0.2f);
            
            _renderer.material.color = Color.white;
            yield return wait;
            _renderer.material.color = Color.red;
            yield return wait;
            _renderer.material.color = Color.white;
            yield return wait;
            _renderer.material.color = Color.red;
            
            _flickerCoroutine = null;
        }

        IEnumerator Shoot()
        {
            WaitForSeconds wait = new WaitForSeconds(Random.Range(1f, 3f));
           
            Vector3 spawnPosition = transform.position + (transform.forward * 1.5f);
            
            Vector3 directionToPlayer = (player.transform.position - spawnPosition).normalized;
            Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer);
    
            // Spawn the bullet facing the player
            Instantiate(bulletPrefab, spawnPosition, rotationToPlayer);
            yield return wait;
            
            _shootCoroutine  = null;
        }
    }
}
