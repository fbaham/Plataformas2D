using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Plataformas2D
{
    public class SpriteSheet
    {
        private readonly string _assetName;
        private readonly int _frameHeight;
        private readonly int _frameWidth;
        private readonly int _framesInX;
        private readonly Texture2D[] _frameTextures;
        private readonly int _totalFrames;

        private bool _loop;
        private bool _isFinished;

        private Texture2D _spriteSheetTexture;

        public SpriteSheet(string assetName, int frameWidth, int frameHeigth, int frameInX, int totalFrames, bool loop = true)
        {
            _assetName = assetName;
            _frameHeight = frameHeigth;
            _frameWidth = frameWidth;
            _framesInX = frameInX;
            _totalFrames = totalFrames;
            _loop = loop;

            _isFinished = false;

            _frameTextures = new Texture2D[totalFrames];

            Frame = 0;
        }
        public int Frame { get; private set; }

        public Texture2D CurrentFrame => _frameTextures[Frame];

        public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice)
        {
            if (string.IsNullOrEmpty(_assetName)) throw new NullReferenceException("_assetName no puede ser null o vacía");

            _spriteSheetTexture = content.Load<Texture2D>(_assetName);
            BreakSheetIntoFrames(graphicsDevice);
        }

        private void BreakSheetIntoFrames(GraphicsDevice graphicsDevice)
        {
            Color[,] spriteSheetTextureData = new Color[_spriteSheetTexture.Width, _spriteSheetTexture.Height];
            _spriteSheetTexture.GetData(spriteSheetTextureData);

            for(int currentFrame = 0; currentFrame < _totalFrames; currentFrame++)
            {
                _frameTextures[currentFrame] = GetTextureFromFrame(spriteSheetTextureData, currentFrame, graphicsDevice);
            }
        }

        private Texture2D GetTextureFromFrame(Color[,] spriteSheetTextureData, int currentFrame, GraphicsDevice graphicsDevice)
        {
            Color[] frameColorData = new Color[_frameWidth * _frameHeight];
            Texture2D frameTexture = new Texture2D(graphicsDevice, _frameWidth, _frameHeight);

            int fX = 0, fY = 0;

            int x = currentFrame % _framesInX * _frameWidth;
            int y = currentFrame / _framesInX * _frameHeight;

            int endX = x + _frameWidth;
            int endY = y + _frameHeight;

            for(int colorY = y; colorY < endY; colorY++)
            {
                for(int colorX = x; colorX < endX; colorX++)
                {
                    frameColorData[fX + fY * _frameWidth] = spriteSheetTextureData[colorX, colorY];
                    fX++;
                }
                fY++;
                fX = 0;
            }

            frameTexture.SetData(frameColorData);

            return frameTexture;
        }

        public void Update(GameTime gameTime)
        {
            if (_isFinished) return;

            Frame++;

            if(Frame >= _totalFrames)
            {
                if(_loop)
                {
                    Frame = 0;
                }
                else
                {
                    Frame = _totalFrames - 1;
                    _isFinished = true;
                }
            }

        }

    }
}
