using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // Sahne kontrolü için

public class WaveManager : MonoBehaviour
{
    public List<GameObject> waves = new List<GameObject>();
    public GameObject level2Panel;      // Inspector’da atayın. Başlangıçta SetActive(false) olsun.

    private List<EnemyGroupController> controllers = new List<EnemyGroupController>();
    private int currentIndex = 0;

    void Start()
    {
        // Başlangıçta Level2 paneli pasif yap:
        if (level2Panel != null)
            level2Panel.SetActive(false);

        foreach (var waveGO in waves)
        {
            var ctrl = waveGO.GetComponent<EnemyGroupController>();
            if (ctrl != null)
            {
                controllers.Add(ctrl);
                ctrl.OnWaveCompleted += OnWaveCompleted;
                ctrl.StopWave();
            }
            waveGO.SetActive(false);
        }

        if (controllers.Count > 0)
            ActivateWave(0);
    }

    private void ActivateWave(int index)
    {
        currentIndex = index;
        var waveGO = waves[index];
        waveGO.SetActive(true);
        controllers[index].StartWave();
    }

    private void OnWaveCompleted()
    {
        // Mevcut dalgayı durdur ve pasifleştir
        controllers[currentIndex].StopWave();
        waves[currentIndex].SetActive(false);

        // Eğer son dalgadaysak
        if (currentIndex == controllers.Count - 1)
        {
            // Hangi sahnedeyiz kontrol et:
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "GameScene")
            {
                // GameScene ise Level 2 panelini göster:
                if (level2Panel != null)
                {
                    Time.timeScale = 0f; // Oyun durdurulsun
                    level2Panel.SetActive(true);
                }

                // Burada istemiyorsanız dijital döngü başlamasın; sadece panel göster.
                    // Eğer panelde bir buton var ve o buton LoadScene("Level2") yapacaksa,
                    // bu aşamada WaveManager durur. İsterseniz bu objeyi pasif edebilirsiniz:
                    // this.enabled = false;
                    // veya event unsubscribe:
                    // foreach(var ctrl in controllers) ctrl.OnWaveCompleted -= OnWaveCompleted;
                }
                else if (sceneName == "Level2")
                {
                    // Level2 sahnesindeysek dalgaları başa döndür:
                    // currentIndex zaten son indeksti, yeni başa döneceğiz
                    ActivateWave(0);
                }
                else
                {
                    // Başka sahne isimleri varsa, ihtiyaca göre ekleyin.
                    // Örneğin başka bir Level3’e geçiş paneli vb.
                    // Şimdilik döngüsel davranmak isterseniz:
                    // ActivateWave(0);
                    // veya hiçbir şey yapmayıp durdurabilirsiniz.
                }
        }
        else
        {
            // Son dalga değilse, sıradaki dalgayı aktif et
            ActivateWave(currentIndex + 1);
        }
    }
}
