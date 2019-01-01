﻿using Assets.Code.Game.ClientPlayer;
using Client.Net;
using Common.Entity;
using Common.Networking.Packets;
using CommonCode.EventBus;
using CommonCode.Networking.Packets;
using UnityEngine;

namespace Assets.Code.Game.PacketListeners
{
    public class EntityListener : IEventListener
    {
        [EventMethod]
        public void OnEntityAttack(EntityAttackPacket packet)
        {
            var attacker = UnityExtensions.GetEntity(packet.AttackerUID);
            var defender = UnityExtensions.GetEntity(packet.DefenderUID);
            attacker.PerformAttackAnimation(defender.Entity);
        }

        [EventMethod]
        public void OnEntityMove(EntityMovePacket packet)
        {
            var entityObj = GameObject.Find(packet.UID);
            if (entityObj != null)
            {
                var livingEntityBhv = entityObj.GetComponent<LivingEntityBehaviour>();
                if (livingEntityBhv != null)
                {
                    livingEntityBhv.Route.Add(packet.To);
                    livingEntityBhv.ForceUpdate(); // to make sure its moved right away

                    if (UnityClient.Player.Target != null && UnityClient.Player.Target == livingEntityBhv.Entity)
                    {
                        UnityClient.Player.FindPathTo(livingEntityBhv.Entity.Position);
                    }
                }
            }
        }
    }
}
