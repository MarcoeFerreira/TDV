using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FirstProject
{
    public class Player
    {
        private Point position;
        private Game1 game;
        private bool keysReleased = true;

        public Point Position => position;

        public Player(Game1 game1, int x, int y) //constructor que dada a as posições guarda a sua posição
        {
            position = new Point(x, y);
        }
        public void Update(GameTime gametime)
         {
            KeyboardState kState = Keyboard.GetState();
            if (keysReleased)
            {
                keysReleased = false;
                if (kState.IsKeyDown(Keys.A)) position.X--;
                else if (kState.IsKeyDown(Keys.W)) position.Y--;
                else if (kState.IsKeyDown(Keys.S)) position.Y++;
                else if (kState.IsKeyDown(Keys.D)) position.X++;
                else keysReleased = true;
            }
            else
            {
                if (kState.IsKeyUp(Keys.A) && kState.IsKeyUp(Keys.W) &&
                kState.IsKeyUp(Keys.S) && kState.IsKeyUp(Keys.D))
                {
                    keysReleased = true;
                }
            }




        }



    }

   
}
