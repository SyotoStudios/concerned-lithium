using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConcernedLithium
{
    public class MenuController : MonoBehaviour
    {
        public List<Menu> Menus = new List<Menu>();

        private void Awake() {
            Menus.ForEach(x => {
                x.Visible = x.ShowOnStart;
                x.Controller = this;
            });
        }

        public void ShowMenu(string menuName, bool additive = false) {
            Menu entry = Menus.Find(x => x.MenuName.ToLower().Equals(menuName.ToLower()));
            // Ensure menu with name and game object exists.
            if (entry == null) {
                Debug.Log($"Could not find menu with name {menuName}");
                return;
            }

            // Hide other menus
            if (!additive) {
                Menus.FindAll(x => x.Visible).ForEach(x => x.Visible = false);
            }

            // Show menu
            entry.Visible = true;
        }

        public void HideMenu(string menuName) {
            Menu entry = Menus.Find(x => x.MenuName.ToLower().Equals(menuName.ToLower()));
            // Ensure menu with name and game object exists.
            if (entry == null) {
                Debug.Log($"Could not find menu with name {menuName}");
                return;
            }

            entry.Visible = false;
        }
    }
}
