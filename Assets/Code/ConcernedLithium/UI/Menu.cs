using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConcernedLithium
{
    public class Menu : MonoBehaviour
    {
        [HideInInspector]
        public MenuController Controller;

        public string MenuName;
        public bool ShowOnStart;

        public bool Visible {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }
    }
}
