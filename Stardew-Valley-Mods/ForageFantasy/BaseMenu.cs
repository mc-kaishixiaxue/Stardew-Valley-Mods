﻿namespace ForageFantasy
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using StardewValley;
    using StardewValley.Menus;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    public abstract class BaseMenu : IClickableMenu
    {
        private const int OkayButtonID = 101;

        private const int TopWidth = Game1.tileSize * 6;
        private const int Width = Game1.tileSize * 8;
        private const int Height = Game1.tileSize * 8;

        private readonly TextBox textBox;
        private readonly ClickableTextureComponent okayButton;

        public BaseMenu(string textBoxText)
          : base((Game1.uiViewport.Width / 2) - (Width / 2), (Game1.uiViewport.Height / 2) - (Height / 2), Width, Height, false)
        {
            Game1.player.Halt();

            textBox = new TextBox(null, null, Game1.dialogueFont, Game1.textColor)
            {
                X = (Game1.uiViewport.Width / 2) - (TopWidth / 2) - 12,
                Y = yPositionOnScreen - 4 + (Game1.tileSize * 2),
                Width = TopWidth,
                Height = Game1.tileSize * 3,

                Text = textBoxText,

                Selected = false
            };

            var yPos = yPositionOnScreen + Height - Game1.tileSize - borderWidth;

            okayButton = new ClickableTextureComponent(new Rectangle(xPositionOnScreen + Width + 4, yPos, Game1.tileSize, Game1.tileSize), Game1.mouseCursors, Game1.getSourceRectForStandardTileSheet(Game1.mouseCursors, 46, -1, -1), 1f, false)
            {
                myID = OkayButtonID
            };

            if (!Game1.options.SnappyMenus)
            {
                return;
            }

            populateClickableComponentList();
            snapToDefaultClickableComponent();
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This is an override, no choice")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "The above suppress is necessary")]
        public override void snapToDefaultClickableComponent()
        {
            currentlySnappedComponent = getComponentWithID(OkayButtonID);
            snapCursorToCurrentSnappedComponent();
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This is an override, no choice")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "The above suppress is necessary")]
        public override void receiveKeyPress(Keys key)
        {
            if (Game1.globalFade)
            {
                return;
            }

            if (((IEnumerable<InputButton>)Game1.options.menuButton).Contains(new InputButton(key)) && (textBox == null || !textBox.Selected))
            {
                Game1.playSound("smallSelect");
                if (readyToClose())
                {
                    Game1.exitActiveMenu();
                    if (textBox.Text.Length <= 0)
                    {
                        return;
                    }
                }
            }
            else
            {
                if (!Game1.options.SnappyMenus || (((IEnumerable<InputButton>)Game1.options.menuButton).Contains(new InputButton(key)) && textBox != null && textBox.Selected))
                {
                    return;
                }

                base.receiveKeyPress(key);
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This is an override, no choice")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "The above suppress is necessary")]
        public override void update(GameTime time)
        {
            base.update(time);
            int num1 = Game1.getOldMouseX() + Game1.uiViewport.X;
            int num2 = Game1.getOldMouseY() + Game1.uiViewport.Y;

            if (num1 - Game1.uiViewport.X < Game1.tileSize)
            {
                Game1.panScreen(-8, 0);
            }
            else if (num1 - (Game1.uiViewport.X + Game1.uiViewport.Width) >= -Game1.tileSize)
            {
                Game1.panScreen(8, 0);
            }

            if (num2 - Game1.uiViewport.Y < Game1.tileSize)
            {
                Game1.panScreen(0, -8);
            }
            else if (num2 - (Game1.uiViewport.Y + Game1.uiViewport.Height) >= -Game1.tileSize)
            {
                Game1.panScreen(0, 8);
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This is an override, no choice")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "The above suppress is necessary")]
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (Game1.globalFade)
            {
                return;
            }

            if (okayButton != null)
            {
                if (okayButton.containsPoint(x, y))
                {
                    Game1.exitActiveMenu();
                }
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This is an override, no choice")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "The above suppress is necessary")]
        public override bool readyToClose()
        {
            textBox.Selected = false;
            if (base.readyToClose())
            {
                return !Game1.globalFade;
            }

            return false;
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This is an override, no choice")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "The above suppress is necessary")]
        public override void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (Game1.globalFade)
            {
                return;
            }

            if (okayButton != null)
            {
                if (okayButton.containsPoint(x, y))
                {
                    Game1.exitActiveMenu();
                }
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This is an override, no choice")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "The above suppress is necessary")]
        public override void performHoverAction(int x, int y)
        {
            if (okayButton != null)
            {
                if (okayButton.containsPoint(x, y))
                {
                    okayButton.scale = Math.Min(1.1f, okayButton.scale + 0.05f);
                }
                else
                {
                    okayButton.scale = Math.Max(1f, okayButton.scale - 0.05f);
                }
            }
        }

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "This is an override, no choice")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "The above suppress is necessary")]
        public override void draw(SpriteBatch b)
        {
            if (!Game1.globalFade)
            {
                b.Draw(Game1.fadeToBlackRect, Game1.graphics.GraphicsDevice.Viewport.Bounds, Color.Black * 0.75f);
                textBox.Draw(b);
                Game1.drawDialogueBox(xPositionOnScreen, yPositionOnScreen + (Game1.tileSize * 2), Width, Height - (Game1.tileSize * 2), false, true, null, false);

                var yPos1 = (float)(yPositionOnScreen + spaceToClearTopBorder + (Game1.tileSize / 2) + (Game1.tileSize * 2));

                string status = GetStatusMessage();
                Utility.drawTextWithShadow(b, status, Game1.smallFont, new Vector2(xPositionOnScreen + IClickableMenu.spaceToClearSideBorder + (Game1.tileSize / 2), yPos1), Game1.textColor, 1f, -1f, -1, -1, 1f, 3);

                okayButton.draw(b);
            }

            drawMouse(b);
        }

        public abstract string GetStatusMessage();
    }
}