using System.Collections;
using _Project.Code.Enemy;
using _Project.Code.ScriptableObjects;
using Unity.Cinemachine;
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
        CinemachinePanTilt _panTilt;
        
        float _lastFireTime;
        float _recoilAmount;
        float _recoilSpeed;
        int _currentWeapon;
        float _currentTilt;
        float _targetTilt;
        
        void Awake()
        {
            _enemyLayer = LayerMask.GetMask("Enemy");
            _animator = GetComponentInChildren<Animator>();
            _panTilt = GetComponentInChildren<CinemachinePanTilt>();
            
            _currentWeapon = 0;
            _recoilAmount = _weaponsSO[_currentWeapon].recoilAmount;
            _recoilSpeed =  _weaponsSO[_currentWeapon].recoilSpeed;
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
                _gunRenderer.transform.localScale = _weaponsSO[_currentWeapon].spriteScale;
                _animator.runtimeAnimatorController = _weaponsSO[_currentWeapon].weaponAnimator;
                _recoilAmount = _weaponsSO[_currentWeapon].recoilAmount;
                _recoilSpeed =  _weaponsSO[_currentWeapon].recoilSpeed;
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                _currentWeapon = 0;
                _gunRenderer.transform.localScale = _weaponsSO[_currentWeapon].spriteScale;
                _animator.runtimeAnimatorController = _weaponsSO[_currentWeapon].weaponAnimator;
                _recoilAmount = _weaponsSO[_currentWeapon].recoilAmount;
                _recoilSpeed =  _weaponsSO[_currentWeapon].recoilSpeed;
            }
        }
        
        public void Shoot()
        {
            if (RateOfFire()) return;

            StartCoroutine(Recoil());
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

        IEnumerator Recoil()
        {
            _currentTilt = _panTilt.TiltAxis.Value;
            _targetTilt = _currentTilt - _recoilAmount;
            
            while (Mathf.Abs(_panTilt.TiltAxis.Value - _targetTilt) > 0.01f)
            { 
                _panTilt.TiltAxis.Value = Mathf.MoveTowards
                    (_panTilt.TiltAxis.Value, _targetTilt , _recoilSpeed);
                
                yield return null;
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
