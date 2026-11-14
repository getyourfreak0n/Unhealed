
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
        [SerializeField] private Transform firePoint;
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
            _agent.updateRotation = false;
        }

        void Start()
        {
            RandomizeStats();
        }

        void Update()
        {
            if (player == null) return;
            
            ChasePlayer();
            EnemyRotator();
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
            if (_renderer == null) yield break;
            
            WaitForSeconds wait = new WaitForSeconds(0.2f);
            
            _renderer.material.color = Color.white;
            yield return wait;
            _renderer.material.color = Color.red;
            yield return wait;
            _renderer.material.color = Color.white;
            yield return wait;
            _renderer.material.color = Color.red;
            yield return wait;
            
            _flickerCoroutine = null;
        }

        IEnumerator Shoot()
        {
            WaitForSeconds wait = new WaitForSeconds(Random.Range(1f, 3f));
            
            Quaternion spawnRotation = transform.localRotation * Quaternion.Euler(Random.Range(-1f, 1f), 0f, 0f);
            Instantiate(bulletPrefab, firePoint.position, spawnRotation);
            
            yield return wait;
            
            _shootCoroutine  = null;
        }

        void EnemyRotator()
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);    
        }
        
        void RandomizeStats()
        {
            _agent.speed = Random.Range(3.5f, 8f);
            _agent.angularSpeed = Random.Range(150f, 300f);
            _agent.stoppingDistance = Random.Range(5f,20f);
            _agent.avoidancePriority = Random.Range(1, 50);
            _agent.radius = Random.Range(0.6f, 10f);
        }
    }
}
