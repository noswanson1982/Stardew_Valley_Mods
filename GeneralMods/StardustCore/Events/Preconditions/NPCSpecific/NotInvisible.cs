using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley;

namespace StardustCore.Events.Preconditions.NPCSpecific
{
    public class NotInvisible:EventPrecondition
    {

        public NPC npc;
        public NotInvisible()
        {

        }
        public NotInvisible(NPC npc)
        {
            this.npc = npc;
        }

        public override string ToString()
        {
            return this.precondition_npcNotInvisible();
        }

        /// <summary>
        /// Creates a precondition where the npc is not invisible. (Probably that you can find them in the game world.
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        public string precondition_npcNotInvisible()
        {
            StringBuilder b = new StringBuilder();
            b.Append("v ");
            b.Append(this.npc.Name);
            return b.ToString();
        }

        public override bool meetsCondition()
        {
            return this.npc.IsInvisible == false;
        }
    }
}
