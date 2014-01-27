using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    public class Entity
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public Vector2 position { get; set; }
        public Vector2 motion { get; set; }
        public Rectangle rect
        {
            get { return new Rectangle((int)position.X, (int)position.Y, (int)Width, (int)Height); }
        }
    }

    public class Paddle : Entity { }
    public class Ball : Entity { }
}
