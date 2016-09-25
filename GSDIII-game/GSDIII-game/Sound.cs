using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GSDIII_game
{
    //Dylan Coats
    class Sound
    {
        Song MainMenu;
        public Song Game;
        Song GameOver;
        Song Shop;
        SoundEffect SwordPower;
        SoundEffect SwordAttack;
        SoundEffect BurstPower;
        SoundEffect Die;
        SoundEffect GunPickup;
        SoundEffect GunShoot;
        SoundEffect coin;
        SoundEffect shrink;
        SoundEffect door;
        SoundEffect bazookaLaunch;
        SoundEffect burstSound;
        public SoundEffect bazookaExplode;
        SoundEffect purchase;
        SoundEffect buttonClick;
        SoundEffect laugh;
        SoundEffect explosion2;
        SoundEffect enemyDie;
        bool isDead;
        Game1.StateMachine sM;

        public Sound(ContentManager c, Game1.StateMachine s)
        {
            MainMenu = c.Load<Song>("Theme2");
            Game = c.Load<Song>("Battle3");
            GameOver = c.Load<Song>("Gameover1");
            Shop = c.Load<Song>("117 PLANET ZERARD");
            SwordPower = c.Load<SoundEffect>("GetSword");
            SwordAttack = c.Load<SoundEffect>("SwordAttack");
            BurstPower = c.Load<SoundEffect>("BurstPower");
            Die = c.Load<SoundEffect>("Dead");
            GunPickup = c.Load<SoundEffect>("Equip1");
            GunShoot = c.Load<SoundEffect>("Gun1");
            coin = c.Load<SoundEffect>("CoinSound");
            shrink = c.Load<SoundEffect>("Shrink");
            door = c.Load<SoundEffect>("DoorSound");
            bazookaLaunch = c.Load<SoundEffect>("Launch");
            burstSound = c.Load<SoundEffect>("burstSound");
            bazookaExplode = c.Load<SoundEffect>("ExplosionSound");
            purchase = c.Load<SoundEffect>("Purchase");
            buttonClick = c.Load<SoundEffect>("ButtonNoise");
            laugh = c.Load<SoundEffect>("Laugh");
            explosion2 = c.Load<SoundEffect>("Explosion2");
            enemyDie = c.Load<SoundEffect>("EnemyDead");
            sM = s;
        }

        public void PlayMainMenu()
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(MainMenu);
                MediaPlayer.IsRepeating = true;
            }

        }

        public void PlayGame()
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(Game);
                MediaPlayer.IsRepeating = true;
            }
        }

        public void PlayGameOver()
        {
                MediaPlayer.Play(GameOver);
                MediaPlayer.IsRepeating = false;
        }

        public void PlayShop()
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(Shop);
                MediaPlayer.IsRepeating = false;
            }
        }

        public void PlaySwordPower()
        {
            SwordPower.Play();
        }

        public void PlayEnemyDead()
        {
            enemyDie.Play();
        }
        public void PlaySwordAttack()
        {
            SwordAttack.Play();
        }

        public void PlayBurstPower()
        {
            BurstPower.Play();
        }

        public void PlayDie()
        {
            if (isDead == false)
            {
                Die.Play();
            }
            isDead = true;
        }

        public void PlayGunPickup()
        {
            GunPickup.Play();
        }

        public void PlayGunShoot()
        {
            GunShoot.Play();
        }

        public void PlayCoin()
        {
            coin.Play();
        }

        public void PlayShrink()
        {
            shrink.Play();
        }

        public void PlayDoor()
        {
            door.Play();
        }

        public void PlayBazookaLaunch()
        {
            bazookaLaunch.Play();
        }

        public void PlayBazookaExplode()
        {
            bazookaExplode.Play();
        }

        public void PlayBurstSound()
        {
            burstSound.Play();
        }

        public void PlayPurchase()
        {
            purchase.Play();
        }

        public void PlayButton()
        {
            buttonClick.Play();
        }

        public void PlayLaugh()
        {
            laugh.Play();
        }

        public void PlayExplosion2()
        {
            explosion2.Play();
        }

        public void StopEverything()
        {
            MediaPlayer.Stop();
        }
    }
}
