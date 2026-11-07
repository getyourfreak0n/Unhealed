using _Project.Code.Enemy;
using _Project.Code.Gameplay.StatePattern;
using UnityEngine;

namespace _Project.Code.Gameplay.Combat.WeaponStates
{
    public class ShotgunState : IState
    {
        readonly PlayerShooter _player;
        readonly RaycastHit[] hits = new RaycastHit[5];


        public ShotgunState(PlayerShooter player)
        {
            _player = player;
        }
        
        public void Enter()
        {
            _player.CurrentWeapon = 1;
            _player.LastFireTime = 0f;
            _player.gunRenderer.transform.localScale = _player._weaponsSO[_player.CurrentWeapon].spriteScale;
            _player.gunRenderer.gameObject.transform.localPosition = _player._weaponsSO[_player.CurrentWeapon].spriteTransform;
            _player.Animator.runtimeAnimatorController = _player._weaponsSO[_player.CurrentWeapon].weaponAnimator;
            _player.RecoilAmount = _player._weaponsSO[_player.CurrentWeapon].recoilAmount;
            _player.RecoilSpeed =  _player._weaponsSO[_player.CurrentWeapon].recoilSpeed;
            _player.SpreadAmount = _player._weaponsSO[_player.CurrentWeapon].spreadAmount;
            _player.FireDistance = _player._weaponsSO[_player.CurrentWeapon].fireDistance;   
        }

        public void FrameUpdate()
        {
        }

        public void Exit()
        {
        }
        public void DamageHitscan()
        {
            Ray ray = new Ray(_player.crosshair.position, _player.crosshair.forward);
            int numberOfHits = Physics.SphereCastNonAlloc(ray, _player.SpreadAmount, hits, _player.FireDistance, _player.EnemyLayer);

            for (int i = 0; i < numberOfHits; i++)
            {
                if (hits[i].collider.TryGetComponent(out IEnemy enemy))
                {
                    enemy.TakeDamage(_player._weaponsSO[_player.CurrentWeapon].damage);       
                }
            }
        }
    }
}
