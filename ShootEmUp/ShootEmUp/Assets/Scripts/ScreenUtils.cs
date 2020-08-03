using UnityEngine;
using Vector2 = UnityEngine.Vector2;


public class ScreenUtils : MonoBehaviour
    {
        public static float topBorder;
        public static float bottomBorder;
        public static float rightBorder;
        public static float leftBorder;
        
        
        private void Awake()
        {
            Vector2 viewPortTopBorder = new Vector2(0.5f,1);
            viewPortTopBorder = Camera.main.ViewportToWorldPoint(viewPortTopBorder);
            topBorder = viewPortTopBorder.y;
            
            Vector2 viewPortBottomBorder = new Vector2(0.5f,0);
            viewPortBottomBorder = Camera.main.ViewportToWorldPoint(viewPortBottomBorder);
            bottomBorder = viewPortBottomBorder.y;
            
            Vector2 viewPortRightBorder = new Vector2(1f, 1f);
            viewPortRightBorder = Camera.main.ViewportToWorldPoint(viewPortRightBorder);
            rightBorder = viewPortRightBorder.x;
            
            Vector2 viewPortLeftBorder = new Vector2(0f,0f);
            viewPortLeftBorder = Camera.main.ViewportToWorldPoint(viewPortLeftBorder);
            leftBorder = viewPortLeftBorder.x;
        }
    }
