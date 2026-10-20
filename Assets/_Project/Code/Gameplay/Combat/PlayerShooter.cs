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
        CinemachinePanTilt _tiltAxis;
        
        float _lastFireTime;
        float _recoilAmount;
        float _recoilSpeed;
        int _currentWeapon;
        float _targetTilt;
        float _recoveryTilt;
        
        void Awake()
        {
            _enemyLayer = LayerMask.GetMask("Enemy");
            _animator = GetComponentInChildren<Animator>();
            _tiltAxis = GetComponentInChildren<CinemachinePanTilt>();
            
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
            
            if (Physics.SphereCast(ray, fireRadius, out RaycastHit hit, 70f, _enemyLayer))
            { 
                if (hit.collider.TryGetComponent<IEnemy>(out var enemy))
                {
                    float damage = _weaponsSO[_currentWeapon].damage;
                    
                    switch (hit.distance)
                    {
                        case <= 5f:
                            break;  
                        case <= 12f:
                            damage *= 0.8f;
                            break;
                        default:
                            damage *= 0.5f;
                            break;
                    }
                   
                    enemy.TakeDamage(damage);
                    enemy.HitVFX(hit.point, hit.normal);
                }
            }
        }

        IEnumerator Recoil()
        {
            _targetTilt = _tiltAxis.TiltAxis.Value - _recoilAmount;
                                                            
            while (Mathf.Abs(_tiltAxis.TiltAxis.Value - _targetTilt) > 0.01f)
            { 
                _tiltAxis.TiltAxis.Value = Mathf.MoveTowards
                    (_tiltAxis.TiltAxis.Value, _targetTilt , _recoilSpeed);
                
                yield return null;
            }
            
            _recoveryTilt = _tiltAxis.TiltAxis.Value + _recoilAmount / 3;

            while (Mathf.Abs(_tiltAxis.TiltAxis.Value - _recoveryTilt ) > 0.01f)
            {
                    _tiltAxis.TiltAxis.Value = Mathf.MoveTowards
                        (_tiltAxis.TiltAxis.Value, _recoveryTilt, _recoilSpeed / 3);
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
