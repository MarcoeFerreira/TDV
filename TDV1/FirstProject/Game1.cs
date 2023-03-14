using FirstProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;

namespace FirstProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont font;
        private int nrLinhas = 0;
        private int nrColunas = 0;
        private char[,] level;
        private Texture2D player, dot, box, wall; //Load images Texture
        private int tilesize = 64;//potencias de 2 (operações binarias)

        private Player sokoban;
        public List<Point> boxes;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            LoadLevel("level1.txt");//Carrega o ficheiro XXXXX.txt
            _graphics.PreferredBackBufferHeight = tilesize * level.GetLength(1); //definição da altura
            _graphics.PreferredBackBufferWidth = tilesize * level.GetLength(0); //definição da largura
            _graphics.ApplyChanges(); //aplica a atualização da janela
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player = Content.Load<Texture2D>("Character4");
            dot = Content.Load<Texture2D>("EndPoint_Blue");
            box = Content.Load<Texture2D>("CrateDark_Brown");
            wall = Content.Load<Texture2D>("WallRound_Brown");
            font = Content.Load<SpriteFont>("File");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            sokoban.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "O texto que quiser", new Vector2(0, 40), Color.Black);
            _spriteBatch.DrawString(font, $"Numero de Linhas = {nrLinhas} -- Numero de Colunas = {nrColunas}", new Vector2(0, 0), Color.Black);
            Rectangle position = new Rectangle(0, 0, tilesize, tilesize);
            for (int x = 0; x < level.GetLength(0); x++) //pega a primeira dimensão
            {
                for (int y = 0; y < level.GetLength(1); y++) //pega a segunda dimensão
                {
                    position.X = x * tilesize; //define a posiçao
                    position.Y = y * tilesize; //define a posiçao

                    switch (level[x, y])
                    {
                        /*case 'Y':
                            _spriteBatch.Draw(player, position, Color.White);
                            break;*/

                        //case '#':
                        //_spriteBatch.Draw(box, position, Color.White);
                        //break;
                        case '.':
                            _spriteBatch.Draw(dot, position, Color.White);
                            break;
                        case 'X':
                            _spriteBatch.Draw(wall, position, Color.White);
                            break;
                    }
                }
            }

            position.X = sokoban.Position.X * tilesize; //posição do Player
            position.Y = sokoban.Position.Y * tilesize; //posição do Player
            _spriteBatch.Draw(player, position, Color.White); //desenha o Player

            foreach (Point b in boxes)
            {
                position.X = b.X * tilesize;
                position.Y = b.Y * tilesize;
                _spriteBatch.Draw(box, position, Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        void LoadLevel(string levelFile)
        {
            string[] linhas = File.ReadAllLines($"Content/{levelFile}"); // "Content/" + level
            nrLinhas = linhas.Length;
            nrColunas = linhas[0].Length;
            level = new char[nrColunas, nrLinhas];
            boxes = new List<Point>();

            for (int x = 0; x < nrColunas; x++)
            {
                for (int y = 0; y < nrLinhas; y++)
                {
                    if (linhas[y][x] == '#')
                    {
                        boxes.Add(new Point(x, y));
                        level[x, y] = ' '; // put a blank instead of the box '#'
                    }
                    else if (linhas[y][x] == 'Y')
                    {
                        sokoban = new Player(this, x, y);
                        level[x, y] = ' '; // put a blank instead of the sokoban 'Y'
                    }
                    else
                    {
                        level[x, y] = linhas[y][x];
                    }
                }
            }

        }
        public bool HasBox(int x, int y)
        {
            foreach (Point b in boxes)
            {
                if (b.X == x && b.Y == y) return true; // se a caixa tiver a mesma posição do Player
            }
            return false;
        }
        public bool FreeTile(int x, int y)
        {
            if (level[x, y] == 'X') return false;           // se for uma parede está ocupada
            if (HasBox(x, y)) return false;                 // verifica se é uma caixa
            return true;
                                                             /* The same as: return level[x,y] != 'X' && !HasBox(x,y); */
        }
    }
}