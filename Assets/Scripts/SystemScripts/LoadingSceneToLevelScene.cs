using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SystemScripts
{
    public class LoadingSceneToLevelScene : MonoBehaviour
    {
        public GameObject liveStat;
        public GameObject gameOverPopup;
        private AudioSource _loadSceneAudio;
        public AudioClip gameOverSound;

        private void Start()
        {
            _loadSceneAudio = GetComponent<AudioSource>();
            if (GameStatusController.Live < 1)
            {
                GameStatusController.IsGameOver = true;
            }

            if (!GameStatusController.IsGameOver)
            {
                if (GameStatusController.IsDead)
                {
                    StartCoroutine(RepeatLevelScene());
                }
                else
                {
                    StartCoroutine(LevelScene());
                }
            }
            else
            {
                liveStat.SetActive(false);
                gameOverPopup.SetActive(true);
                _loadSceneAudio.PlayOneShot(gameOverSound);
                StartCoroutine(StartingScene());
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private static IEnumerator LevelScene()
        {
            yield return new WaitForSeconds(2);
    
            // 获取当前需要加载的场景索引
            int targetLevelIndex = GameStatusController.CurrentLevel;

            // 检查目标索引是否超出了 Build Settings 中已添加的场景总数
            if (targetLevelIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(targetLevelIndex);
                GameStatusController.CurrentLevel += 1;
            }
            else
            {
                // 如果已经没有下一关，强制返回主菜单（假设主菜单的 Build Index 是 0）
                Debug.LogWarning("已通关所有关卡，返回主菜单！");
                SceneManager.LoadScene(0); 
            }
        }

        private static IEnumerator RepeatLevelScene()
        {
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(GameStatusController.CurrentLevel - 1);
            GameStatusController.IsDead = false;
        }

        private static IEnumerator StartingScene()
        {
            yield return new WaitForSeconds(4.5f);
            SceneManager.LoadScene(0);
            GameStatusController.Live = 3;
            GameStatusController.Score = 0;
            GameStatusController.CollectedCoin = 0;
            GameStatusController.IsGameOver = false;
            GameStatusController.IsDead = false;
        }
    }
}