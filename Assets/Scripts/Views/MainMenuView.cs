using Com.MyCompany.MyGame;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class MainMenuView : View
    {
        public override void Initialize()
        {
            
        }

        public override void Show()
        {
            base.Show();
            ViewManager.Show<MainView>();
        }
    }
}