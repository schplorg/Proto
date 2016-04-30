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
                if (!(me.inventories[i].TryRemoveInventory(mine[i]) && you.inventories[i].TryAddInventory(mine[i]))) {
                    return false;
                }
            }
            for (int i = 0; i < mine.Length; i++) {
                if (!(you.inventories[i].TryRemoveInventory(yours[i]) && me.inventories[i].TryAddInventory(yours[i]))) {
                    return false;
                }
            }
            return true;
        }

        public void Apply() {
            //// for each inventory in my offer
            for (int i = 0; i < mine.Length; i++) {
                if (me.inventories[i].TryRemoveInventory(mine[i]) && you.inventories[i].TryAddInventory(mine[i])) {                    
                    me.inventories[i].RemoveInventory(mine[i]);
                    Console.WriteLine("Removed items from " + me.name + "'s inventory.");
                    you.inventories[i].AddInventory(mine[i]);
                    Console.WriteLine("Added items to " + you.name + "'s inventory.");
                } else {
                    throw new Exception();
                }
            }
            // for each inventory in your offer
            for (int i = 0; i < mine.Length; i++) {
                if (you.inventories[i].TryRemoveInventory(yours[i]) && me.inventories[i].TryAddInventory(yours[i])) {
                    you.inventories[i].RemoveInventory(yours[i]);
                    Console.WriteLine("Removed items from " + you.name + "'s inventory.");
                    me.inventories[i].AddInventory(yours[i]);
                    Console.WriteLine("Added items to " + me.name + "'s inventory.");
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
