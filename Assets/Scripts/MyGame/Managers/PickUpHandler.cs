using System.Collections;
using System.Collections.Generic;
using Com.MyCompany.MyGame;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Utils;

namespace MyGame.Managers
{
    public class PickUpHandler : MonoBehaviour
    {
        [SerializeField] private GameObject awesomeSpritePrefab;
        [SerializeField] private List<Sprite> awesomeSprites;
        
        private global::Player _player;

        public void Init(global::Player player)
        {
            _player = player;
        }

        public void AddAwesomeTriggerScore(int score)
        {
            StartCoroutine(ShowAwesomeSprite());
            GameManager.Instance.localPlayer.score += score;
            SoundManager.Instance.PlayCoinPickUp();
        }
        
        private IEnumerator ShowAwesomeSprite()
        {
            //int index = Random.Range(0, awesomeSprites.Count);
            //awesomeSpritePrefab.GetComponent<Image>().sprite = awesomeSprites[index];
            awesomeSpritePrefab.SetActive(true);
            yield return new WaitForSeconds(1f);
            awesomeSpritePrefab.SetActive(false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            CheckForBulletBonus(other);
        }

        private void CheckForBulletBonus(Collider other)
        {
            if (other.CompareTag("Bullet"))
            {
                _player.weaponManager.SwitchWeapon("RPG7", 5f);

                _player.Ammo++;

                if (_player.Ammo > 30) 
                    _player.Ammo = 30;
            
                SoundManager.Instance.PlayPickUp();
                
                PoolManager.Return(other.gameObject.GetComponent<PoolItem>());
            }
        }
    }
}