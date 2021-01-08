using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ConcernedLithium
{
    public class MainMenu : Menu
    {
        public void OnPlayPressed() {
            Controller.ShowMenu("level-select", true);
        }

        public void OnQuitPressed() {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
