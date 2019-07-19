using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Revitalize.Framework.Minigame.SeasideScrambleMinigame;
using StardustCore.UIUtilities;
namespace Revitalize.Framework.Minigame.SeasideScrambleMinigame
{
    /// <summary>
    /// TODO: Finish character select screen. Ensure that camera snapping doesn't happen until game starts.
    ///     -Make boxes per character
    ///     -Make prompt to click for p1, press a for p2,3,4
    ///     -Make Sound effects happen
    ///     -make prompt for color selection
    ///         -a,d for keyboard
    ///         -dpad for p2-4
    /// Also add interface for game entity for camera to consistently have a focus target.
    /// </summary>
    public class SeasideScramble : StardewValley.Minigames.IMinigame
    {

        public static SeasideScramble self;

        SeasideScrambleMap currentMap;
        public Dictionary<string, SeasideScrambleMap> SeasideScrambleMaps;

        public int currentNumberOfPlayers
        {
            get
            {
                return this.players.Count;
            }
        }
        public const int maxPlayers = 4;
        public Dictionary<SSCEnums.PlayerID, SSCPlayer> players;

        public bool quitGame;
        public Vector2 topLeftScreenCoordinate;

        public SSCTextureUtilities textureUtils;

        public SSCCamera camera;

        public SSCMenus.SSCMenuManager menuManager;

        public SeasideScramble()
        {
            self = this;
            this.camera = new SSCCamera();
            //this.viewport = new xTile.Dimensions.Rectangle(StardewValley.Game1.viewport);
            this.topLeftScreenCoordinate = new Vector2((float)(this.camera.viewport.Width / 2 - 384), (float)(this.camera.viewport.Height / 2 - 384));

            this.LoadTextures();

            this.LoadMaps();
            this.loadStartingMap();
            this.quitGame = false;

            this.players = new Dictionary<SSCEnums.PlayerID, SSCPlayer>();
            //this.players.Add(SSCEnums.PlayerID.One, new SSCPlayer(SSCEnums.PlayerID.One));
            //this.getPlayer(SSCEnums.PlayerID.One).setColor(Color.PaleVioletRed);


            this.menuManager = new SSCMenus.SSCMenuManager();

            this.menuManager.addNewMenu(new SSCMenus.TitleScreen(this.camera.viewport));

        }

        public SSCPlayer getPlayer(SSCEnums.PlayerID id)
        {
            if (this.players.ContainsKey(id))
            {
                return this.players[id];
            }
            else return null;
        }

        private void LoadTextures()
        {
            this.textureUtils = new SSCTextureUtilities();
            TextureManager playerManager = new TextureManager("SSCPlayer");
            playerManager.searchForTextures(ModCore.ModHelper, ModCore.Manifest, Path.Combine("Content", "Minigames", "SeasideScramble", "Graphics", "Player"));
            TextureManager mapTextureManager = new TextureManager("SSCMaps");
            mapTextureManager.searchForTextures(ModCore.ModHelper, ModCore.Manifest, Path.Combine("Content", "Minigames", "SeasideScramble", "Maps", "Backgrounds"));
            this.textureUtils.addTextureManager(playerManager);
            this.textureUtils.addTextureManager(mapTextureManager);
        }

        private void LoadMaps()
        {
            this.SeasideScrambleMaps = new Dictionary<string, SeasideScrambleMap>();
            this.SeasideScrambleMaps.Add("TestRoom", new SeasideScrambleMap(SeasideScrambleMap.LoadMap("TestRoom.tbin").Value));
        }
        private void loadStartingMap()
        {
            this.currentMap = this.SeasideScrambleMaps["TestRoom"];
        }

        /// <summary>
        /// What happens when the screen changes size.
        /// </summary>
        public void changeScreenSize()
        {
            Viewport viewport = StardewValley.Game1.graphics.GraphicsDevice.Viewport;
            double num1 = (double)(viewport.Width / 2 - 384);
            viewport = StardewValley.Game1.graphics.GraphicsDevice.Viewport;
            double num2 = (double)(viewport.Height / 2 - 384);
            this.topLeftScreenCoordinate = new Vector2((float)num1, (float)num2);
            this.camera.viewport = new xTile.Dimensions.Rectangle(StardewValley.Game1.viewport);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Used to update Stardew Valley while this minigame runs. True means SDV updates false means the SDV pauses all update ticks.
        /// </summary>
        /// <returns></returns>
        public bool doMainGameUpdates()
        {
            return false;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Draws all game aspects to the screen.
        /// </summary>
        /// <param name="b"></param>
        public void draw(SpriteBatch b)
        {
            if (this.currentMap != null)
            {
                this.currentMap.draw(b);
            }
            b.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, (DepthStencilState)null, (RasterizerState)null);

            foreach(SSCPlayer p in this.players.Values) {
                p.draw(b);
            }

            /*
            if (this.menuManager.activeMenu != null)
            {
                this.menuManager.activeMenu.draw(b);
            }
            */
            this.menuManager.drawAll(b);
            b.End();
        }

        /// <summary>
        /// What happens when the left click is held.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void leftClickHeld(int x, int y)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// The id of the minigame???
        /// </summary>
        /// <returns></returns>
        public string minigameId()
        {
            return "Seaside Scramble Stardew Lite Edition";
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Does this override free mous emovements?
        /// </summary>
        /// <returns></returns>
        public bool overrideFreeMouseMovement()
        {
            return true;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// ??? Undocumended.
        /// </summary>
        /// <param name="data"></param>
        public void receiveEventPoke(int data)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// What happens when a key is pressed.
        /// </summary>
        /// <param name="k"></param>
        public void receiveKeyPress(Keys k)
        {
            //throw new NotImplementedException();
            if (k == Keys.Escape)
            {
                this.quitGame = true;
            }

            foreach(SSCPlayer player in this.players.Values)
            {
                player.receiveKeyPress(k);
            }

            if (this.menuManager.isMenuUp)
            {
                this.menuManager.activeMenu.receiveKeyPress(k);
            }

        }

        /// <summary>
        /// Gets a gamepad state.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public GamePadState getGamepadState(PlayerIndex index)
        {
            return Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// Called when the minigame registeres a key on the keyboard being released.
        /// </summary>
        /// <param name="K"></param>
        public void receiveKeyRelease(Keys K)
        {
            foreach (SSCPlayer player in this.players.Values)
            {
                player.receiveKeyRelease(K);
            }
        }

        /// <summary>
        /// Called when the minigame receives a left click.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="playSound"></param>
        public void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (this.menuManager.activeMenu != null)
            {
                this.menuManager.activeMenu.receiveLeftClick(x, y, playSound);
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when the minigame receives a right click.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="playSound"></param>
        public void receiveRightClick(int x, int y, bool playSound = true)
        {
            if (this.menuManager.activeMenu != null)
            {
                this.menuManager.activeMenu.receiveRightClick(x, y, playSound);
            }
        }

        public void releaseLeftClick(int x, int y)
        {
            //throw new NotImplementedException();
        }

        public void releaseRightClick(int x, int y)
        {
            //throw new NotImplementedException();
        }


        private void receiveGamepadInput(GamePadState state,SSCEnums.PlayerID ID)
        {
            if (state == null) return;
            else
            {
                if (this.players.ContainsKey(ID))
                {
                    this.players[ID].receiveGamepadInput(state);
                }
            }
        }

        /// <summary>
        /// Called every update frame.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool tick(GameTime time)
        {
            KeyboardState kState = Keyboard.GetState();

            foreach (Keys k in kState.GetPressedKeys())
            {
                this.receiveKeyPress(k);
            }
            for (int i = 0; i < 4; i++)
            {
                GamePadState state = this.getGamepadState((PlayerIndex)i);
                this.receiveGamepadInput(state,(SSCEnums.PlayerID)i);
            }


            if (this.quitGame)
            {
                return true;
            }
            if (this.currentMap != null)
            {
                this.currentMap.update(time);
            }


            if (this.menuManager.activeMenu != null)
            {
                this.menuManager.activeMenu.update(time);
                if (this.menuManager.activeMenu.readyToClose())
                {
                    this.menuManager.closeActiveMenu();
                }
            }
            else
            {
                foreach (SSCPlayer player in this.players.Values)
                {
                    if (player.playerID == SSCEnums.PlayerID.One) this.camera.centerOnPosition(player.position);
                    player.update(time);
                }
            }

            return false;
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Called when the minigame is quit upon.
        /// </summary>
        public void unload()
        {
            //throw new NotImplementedException();
            ModCore.log("Exit the game!");
        }

        public static Vector2 GlobalToLocal(xTile.Dimensions.Rectangle viewport, Vector2 globalPosition)
        {
            return new Vector2(globalPosition.X - (float)viewport.X, globalPosition.Y - (float)viewport.Y);
        }

        public static Vector2 GlobalToLocal(Vector2 globalPosition)
        {
            return new Vector2(globalPosition.X - (float)SeasideScramble.self.camera.viewport.X, globalPosition.Y - (float)SeasideScramble.self.camera.viewport.Y);
        }
    }
}