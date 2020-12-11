using System;
using System.Collections.Generic;
using Wisteria.Effects.VertexStrips.Shaders;
using Wisteria.Effects.VertexStrips.Colors;
using Wisteria.Effects.VertexStrips.Caps;
using Wisteria.Effects.VertexStrips.Positions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Wisteria.Effects.VertexStrips
{
    public class TrailManager
    {
        /// <summary>
        /// List of Active Trails
        /// </summary>
        private List<Trail> _trails;

        /// <summary>
        /// Base Shader that is applied to all trails
        /// </summary>
        private Effect _effect;

        /// <summary>
        /// This mod
        /// </summary>
        private Mod _mod;

        /// <summary>
        /// Factory for constructing trails
        /// </summary>
        private TrailFactory _factory;

        public TrailManager()
        {
            _mod = Wisteria.Instance;
            _factory = new TrailFactory();
            _trails = new List<Trail>();
            _effect = _mod.GetEffect("Effects/trailShaders");
        }

        /// <summary>
        /// Instantiates a trail based on the projectile passed.
        /// This is mod based, so is expected to check if type == ModContent.ProjectileType
        /// </summary>
        /// <param name="projectile"></param>
        public void InstantiateModTrail(Projectile projectile)
        {
            /*if (projectile.type == ModContent.ProjectileType<ApeBeamBlast>())
            {
                //Probably isn't a good trail but threw it together for the example
                /*_trails.Add(
                    _factory.Create()
                        .WithProjectile(projectile)
                        .WithColor(new StandardTrailColor(new Color(120, 217, 255)))
                        .WithCap(new RoundTrailCap())
                        .WithPosition(new DefaultTrailPosition())
                        .SetWidth(8f)
                        .SetLength(250f)
                        .Build()
                );
            }
            if (projectile.type == ModContent.ProjectileType<KiBlastProjectile>())
            {
                _trails.Add(
                    _factory.Create()
                        .WithProjectile(projectile)
                        .WithColor(new GradientTrailColor(new Color(52, 183, 235), new Color(38, 141, 181)))
                        .WithCap(new RoundTrailCap())
                        .WithPosition(new DefaultTrailPosition())
                        .SetWidth(15f)
                        .SetLength(200f)
                        .Build()
                );
            }
            if (projectile.type == ModContent.ProjectileType<KiVertexOrb>())
            {
                _trails.Add(
                    _factory.Create()
                        .WithProjectile(projectile)
                        .WithColor(new GradientTrailColor(new Color(52, 183, 235), new Color(33, 123, 158)))
                        .WithCap(new RoundTrailCap())
                        .WithPosition(new DefaultTrailPosition())
                        .SetWidth(15f)
                        .SetLength(500f)
                        .Build()
                );
            }*/
        }

        /// <summary>
        /// Instantiates a trail to be drawn to a vanilla projectile
        /// If a trail is desired for vanilla anyway.
        /// </summary>
        /// <param name="projectile"></param>
        public void InstantiateVanillaTrail(Projectile projectile)
        {
            switch (projectile.type)
            {
                case ProjectileID.WoodenArrowFriendly:
                case ProjectileID.WoodenArrowHostile:
                    break;
            }
        }

        /// <summary>
        /// Will forcibly kill a trail
        /// </summary>
        /// <param name="projectile"></param>
        public void Kill(Projectile projectile)
        {
            EndTrail(projectile, Math.Max(1.0f, projectile.velocity.Length() * 6.0f));
        }

        /// <summary>
        /// Updates the trails in the local list
        /// If the trail is dead it is removed from the list
        /// </summary>
        public void UpdateTrails()
        {
            for (int i = _trails.Count - 1; i >= 0; i--)
            {
                Trail t = _trails[i];

                t.Update();
                if (t.Dead)
                {
                    _trails.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Calls the Trails draw method
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawTrails(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _trails.Count; i++)
            {
                _trails[i].Draw(_effect, spriteBatch.GraphicsDevice);
            }
        }

        /// <summary>
        /// Ends the trail upto current point
        /// dissolve is the speed in which to fade away
        /// </summary>
        /// <param name="p"></param>
        /// <param name="dissolve"></param>
        private void EndTrail(Projectile p, float dissolve)
        {
            for (int i = 0; i < _trails.Count; i++)
            {
                Trail t = _trails[i];
                if (t.Projectile.whoAmI == p.whoAmI)
                {
                    t.StartDissolve(dissolve);
                    return;
                }
            }
        }

        /// <summary>
        /// Trail Factory, a fluent builder
        /// </summary>
        private class TrailFactory
        {
            /// <summary>
            /// List of factory actions, these are applied on Build
            /// </summary>
            private List<Action<Trail>> _factoryActions;

            /// <summary>
            /// Used for single checking to make sure the factory isn't reconstructed
            /// </summary>
            private bool isBuilding;

            public TrailFactory()
            {
                _factoryActions = new List<Action<Trail>>();
                isBuilding = false;
            }

            /// <summary>
            /// Simply states that the factory is building, used to make sure another can't be created
            /// Even if the With methods are called they will just overwrite whats there anyway
            /// </summary>
            /// <returns></returns>
            public TrailFactory Create()
            {
                if (isBuilding)
                {
                    Wisteria.Instance.Logger.Info($"Factory told to create but is currently building");
                    return this;
                }

                isBuilding = true;
                return this;
            }

            /// <summary>
            /// Sets the trails projectile behaviour
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public TrailFactory WithProjectile(Projectile p)
            {
                if (!isBuilding)
                {
                    Wisteria.Instance.Logger.Info($"WithProjectile called before Create");
                    return this;
                }

                _factoryActions.Add(n => n.SetProjectile(p));
                return this;
            }

            /// <summary>
            /// Sets the trails color behaviour
            /// </summary>
            /// <param name="trailColor"></param>
            /// <returns></returns>
            public TrailFactory WithColor(ITrailColor trailColor)
            {
                if (!isBuilding)
                {
                    Wisteria.Instance.Logger.Info($"WithColor called before Create");
                    return this;
                }

                _factoryActions.Add(n => n.SetTrailColor(trailColor));
                return this;
            }

            /// <summary>
            /// Sets the trails cap behaviour
            /// </summary>
            /// <param name="trailCap"></param>
            /// <returns></returns>
            public TrailFactory WithCap(ITrailCap trailCap)
            {
                if (!isBuilding)
                {
                    Wisteria.Instance.Logger.Info($"WithCap called before Create");
                    return this;
                }

                _factoryActions.Add(n => n.SetTrailCap(trailCap));
                return this;
            }

            /// <summary>
            /// Sets the trails position behaviour
            /// </summary>
            /// <param name="trailPosition"></param>
            /// <returns></returns>
            public TrailFactory WithPosition(ITrailPosition trailPosition)
            {
                if (!isBuilding)
                {
                    Wisteria.Instance.Logger.Info($"WithPosition called before Create");
                    return this;
                }

                _factoryActions.Add(n => n.SetTrailPosition(trailPosition));
                return this;
            }

            /// <summary>
            /// Sets the trails shader behaviour
            /// </summary>
            /// <param name="trailShader"></param>
            /// <returns></returns>
            public TrailFactory WithShader(ITrailShader trailShader)
            {
                if (!isBuilding)
                {
                    Wisteria.Instance.Logger.Info($"WithShader called before Create");
                    return this;
                }

                _factoryActions.Add(n => n.SetTrailShader(trailShader));
                return this;
            }

            /// <summary>
            /// Sets the start width of the trail
            /// </summary>
            /// <param name="width"></param>
            /// <returns></returns>
            public TrailFactory SetWidth(float width)
            {
                if (!isBuilding)
                {
                    Wisteria.Instance.Logger.Info($"SetWidth called before Create");
                    return this;
                }

                _factoryActions.Add(n => n.SetTrailWidth(width));
                return this;
            }

            /// <summary>
            /// Sets the max length of the trail
            /// </summary>
            /// <param name="length"></param>
            /// <returns></returns>
            public TrailFactory SetLength(float length)
            {
                if (!isBuilding)
                {
                    Wisteria.Instance.Logger.Info($"SetLength called before Create");
                    return this;
                }

                _factoryActions.Add(n => n.SetTrailLength(length));
                return this;
            }

            /// <summary>
            /// Builds the trail
            /// </summary>
            /// <returns></returns>
            public Trail Build()
            {
                if (!isBuilding)
                {
                    Wisteria.Instance.Logger.Info($"Trail Build called but nothing being built");
                    return null;
                }
                
                Trail t = new Trail();
                isBuilding = false;
                _factoryActions.ForEach(x => x(t));
                return t;
            }
        }
    }
}