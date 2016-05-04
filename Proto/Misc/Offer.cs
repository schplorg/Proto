﻿using Proto.Entities;
using Proto.Storables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proto.Misc
{
    class Offer {
        public Entity me, you;
        public Inventory[] mine, yours;

        public Offer(Entity me, Entity you, Inventory[] mine, Inventory[] yours) {
            this.me = me;
            this.you = you;
            this.mine = mine;
            this.yours = yours;
        }

        public bool TryApply() {
            // check if the offer could be applied
            for (int i = 0; i < mine.Length; i++) {
                if (!(me.inventories[i].TryRemove(mine[i]) && you.inventories[i].TryAdd(mine[i]))) {
                    return false;
                }
            }
            for (int i = 0; i < mine.Length; i++) {
                if (!(you.inventories[i].TryRemove(yours[i]) && me.inventories[i].TryAdd(yours[i]))) {
                    return false;
                }
            }
            return true;
        }

        public void Apply(){
            // for each inventory in my offer
            for (int i = 0; i < mine.Length; i++) {
                if (me.inventories[i].TryRemove(mine[i]) && you.inventories[i].TryAdd(mine[i])) {           
                    you.inventories[i].Add(mine[i]);
                    Console.WriteLine("Added " + mine[i].Count + " items to " + you.name + "'s inventory[" + i + "].");
                    me.inventories[i].Remove(mine[i]);
                    Console.WriteLine("Removed " + mine[i].Count + " items from " + me.name + "'s inventory[" + i + "].");
                } else {
                    throw new Exception();
                }
            }
            // for each inventory in your offer
            for (int i = 0; i < yours.Length; i++) {
                if (you.inventories[i].TryRemove(yours[i]) && me.inventories[i].TryAdd(yours[i])) {
                    me.inventories[i].Add(yours[i]);
                    Console.WriteLine("Added " + yours[i].Count + " items to " + me.name + "'s inventory[" + i + "].");
                    you.inventories[i].Remove(yours[i]);
                    Console.WriteLine("Removed " + yours[i].Count + " items from " + you.name + "'s inventory[" + i + "].");
                } else {
                    throw new Exception();
                }
            }
        }

        public void Reject() {
            Console.WriteLine("Offer from " + me.name + " to " + you.name + " rejected!");
            you.offers.Remove(this);
        }

    }
}
