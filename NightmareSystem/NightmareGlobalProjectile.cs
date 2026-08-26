using NightmareMode.NightmareSystem;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Roguelike.Common.Global;
internal class NightmareGlobalProjectile : GlobalProjectile {
	public override bool InstancePerEntity => true;

	public bool IsFromBoss = false;
	public bool IsFromNPC = false;

	public int OnKill_ScatterShot = -1;

	public int NPC_WhoAmI = -1;
	public override void OnSpawn(Projectile projectile, IEntitySource source) {
		if (source is null) {
			return;
		}
		
		if (source is EntitySource_Parent parent3) {
			if (parent3.Entity is NPC npc) {
				if (npc.boss) {
					IsFromBoss = true;
				}
				IsFromNPC = true;
				NPC_WhoAmI = npc.whoAmI;
			}
		}
	}
	public override void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers) {
		if (IsFromNPC) {
			if (NPC_WhoAmI <= -1 && NPC_WhoAmI >= Main.npc.Length) {
				if (Main.npc[NPC_WhoAmI].TryGetGlobalNPC(out NightmareGlobalNPC global)) {
					modifiers.SourceDamage = global.DamageIncrease.CombineWith(modifiers.SourceDamage);
				}
			}
		}
	}
}
