using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using Wisteria.Players;

namespace Wisteria.UI
{
    internal class BreathUI : UIState
    {
        public override void OnInitialize()
        {
            backPanel = new UIPanel();
            backPanel.Width.Set(40, 0f);
            backPanel.Height.Set(124, 0f);
            backPanel.Left.Set(Main.screenWidth / 2f - backPanel.Width.Pixels / 2f, 0f);
            backPanel.Top.Set(Main.screenHeight / 12f, 0f);
            backPanel.BackgroundColor = new Color(0, 0, 0, 0);
            backPanel.BorderColor = new Color(0, 0, 0, 0);
            Append(backPanel);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            WisteriaPlayer player = Main.LocalPlayer.GetModPlayer<WisteriaPlayer>();
            DrawBreathHex(spriteBatch, player, 1, 3f, player.Breath / 500);
        }

        public void DrawBreathHex(SpriteBatch spriteBatch, WisteriaPlayer player, float drawPosX = 1f, float drawPosY = 1f, float scale = 0.5f)
        {
            Texture2D texture = Wisteria.Instance.GetTexture("UI/BreathHex");
            _drawPosition = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2 + 45);
            spriteBatch.Draw(texture, _drawPosition, texture.Bounds, Color.Cyan, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, _drawPosition, texture.Bounds, Color.Black, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.21f, SpriteEffects.None, 0);
        }
        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        public UIPanel backPanel { get; set; }
        Vector2 _drawPosition { get; set; }
        public static bool visible { get; set; } = true;

        public const float
            PaddingX = -6,
            PaddingY = PaddingX;
    }
}
