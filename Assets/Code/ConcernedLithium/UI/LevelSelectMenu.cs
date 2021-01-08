using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ConcernedLithium
{
    public class LevelSelectMenu : Menu
    {
        public SceneReference LevelContainer;
        public SceneReference HUD;
        public SceneReference Dan00;
        public SceneReference Level00;

        public void OnDan00Pressed() {
            LoadLevel(Dan00);
        }

        public void OnLevel00Pressed() {
            LoadLevel(Level00);
        }

        private void LoadLevel(SceneReference level) {
            SceneOrchestrator.LoadScene(UnityEngine.SceneManagement.LoadSceneMode.Single, LevelContainer).ConfigureAwait(false);
            SceneOrchestrator.LoadScene(UnityEngine.SceneManagement.LoadSceneMode.Additive, HUD).ConfigureAwait(false);
            SceneOrchestrator.LoadScene(UnityEngine.SceneManagement.LoadSceneMode.Additive, level).ConfigureAwait(false);
        }
    }
}
