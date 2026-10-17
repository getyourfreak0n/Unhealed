using _Project.Code.Enemy;
using _Project.Code.ScriptableObjects;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Code.Gameplay.Combat
{
    public class PlayerShooter : MonoBehaviour
    {
        //serialized fields
        [SerializeField] Transform crosshair;
        [SerializeField] float fireRadius = 0.4f;
        [SerializeField] float nextFireTime = 0.4f;
        [SerializeField] WeaponsSO[] _weaponsSO;
        [SerializeField] private SpriteRenderer _gunRenderer;
        
        //private fields
        LayerMask _enemyLayer;
        Animator _animator;
        
        float _lastFireTime;
        int _currentWeapon;
        
        void Awake()
        {
            _enemyLayer = LayerMask.GetMask("Enemy");
            _animator = GetComponentInChildren<Animator>();
            
            _currentWeapon = 0;
        }

        void Update()
        {
            WeaponSwitcher();    
        }

        void WeaponSwitcher()
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                _currentWeapon = 1;
                _animator.runtimeAnimatorController = _weaponsSO[_currentWeapon].weaponAnimator;
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                _currentWeapon = 0;
                _animator.runtimeAnimatorController = _weaponsSO[_currentWeapon].weaponAnimator;
                
            }
        }
        
        public void Shoot()
        {
            if (RateOfFire()) return;

            _animator.SetTrigger("isShooting");
            AudioManager.Instance.PlaySound(_weaponsSO[_currentWeapon].clip);
            
            Ray ray = new Ray(crosshair.position, crosshair.forward);
            
            if (Physics.SphereCast(ray, fireRadius, out RaycastHit hit, 100f, _enemyLayer))
            { 
                if (hit.collider.TryGetComponent<IEnemy>(out var enemy))
                {
                        enemy.TakeDamage(_weaponsSO[_currentWeapon].damage);
                }
            }
        }

        bool RateOfFire()
        {
            if (Time.time - _lastFireTime <= _weaponsSO[_currentWeapon].nextFireTime)
            {
                return true;
            }

            _lastFireTime = Time.time;
            return false;
        }
    }
}
