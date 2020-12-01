using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Wisteria.Utilities;

namespace Wisteria.UI
{
    public class BreathUI : UIState
    {
        public UIPanel BackPanel { get; set; }

        public Vector2 DrawPosition { get; set; }

        public static bool Visible { get; set;} = true;

        //public const float PADDING_X = -6, PADDING_Y = PADDING_X;

        public override void OnInitialize()
        {
            BackPanel = new UIPanel()
            {
                BackgroundColor = new Color(0, 0, 0, 0),
                BorderColor = new Color(0, 0, 0, 0)
            };
            BackPanel.Width.Set(40, 0f);
            BackPanel.Height.Set(124, 0f);
            BackPanel.Left.Set(Main.screenWidth / 2f - BackPanel.Width.Pixels / 2f, 0f);
            BackPanel.Top.Set(Main.screenHeight / 12f, 0f);
            Append(BackPanel);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch) => DrawBreathHex(spriteBatch, 1, 3f, Main.LocalPlayer.GetWisteriaPlayer().breath / 500);

        public void DrawBreathHex(SpriteBatch spriteBatch, float drawPosX = 1f, float drawPosY = 1f, float scale = 0.5f)
        {
            Texture2D texture = Wisteria.Instance.GetTexture("UI/BreathHex");

            DrawPosition = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 + 45);

            spriteBatch.Draw(texture, DrawPosition, texture.Bounds, Color.Cyan, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, DrawPosition, texture.Bounds, Color.Black, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.21f, SpriteEffects.None, 0);
        }
    }
}