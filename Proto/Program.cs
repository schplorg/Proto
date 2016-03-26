﻿using Proto.Entities;
using Proto.Misc;
using Proto.Storables;
using System;
using System.Collections.Generic;

namespace Proto
{
    class Program
    {
        static int gtime = 0;
        static int lastId = 0;
        static List<Entity> entities;
        static Hero player;
        static Random ran = new Random();

        static void Main(string[] args)
        {
            entities = new List<Entity>();


            Hero[] heroes = new Hero[6];
            for (int i = 0; i < heroes.Length; i++) {
                heroes[i] = new Hero("Lord " + NumberToWords(lastId + 1), 3, new Vector2());
            }
            entities.AddRange(heroes);
            player = new Hero("Lord Hosenschlitz", 3, new Vector2(0, 1));
            entities.Add(player);

            Town[] towns = new Town[64];
            for (int i = 0; i < towns.Length; i++) {
                double rand = ran.NextDouble() - .5d;
                towns[i] = new Town("St. " + NumberToWords(lastId+1),new Vector2((int)(100d*rand),GetId()));
            }
            entities.AddRange(towns);            
            while (true) {
                Console.WriteLine("Tick.");
                foreach (Hero h in entities.FindAll(h=>typeof(Hero) == h.GetType())) {
                    Console.WriteLine(h.name + " is at " + h.position.ToString());
                    //Console.WriteLine("And has ID " + h.id);
                    Console.WriteLine("And has " + h.inventory.Count + " food.");
                }
                foreach (Town t in entities.FindAll(t => typeof(Town) == t.GetType()))
                {
                    if (gtime == 0) {
                        Console.WriteLine(t.name + " is at " + t.position.ToString());
                        //Console.WriteLine("And has ID " + t.id);
                    } 
                }
                string input = "";
                do {
                    Console.WriteLine("Input:");
                    input = Console.ReadLine();
                    PlayerInput(input);
                } while (!string.IsNullOrEmpty(input));
                gtime++;
            }
        }

        public static void PlayerInput(string input) {
            // MOVE
            if (input.Contains("move")) {
                bool validx = false, validy = false;
                int x = 0, y = 0;
                foreach (string s in input.Split(' ')) {
                    if (!validx) {
                        if (Int32.TryParse(s, out x)) {
                            validx = true;
                        }
                    } else {
                        if (Int32.TryParse(s, out y)) {
                            validy = true;
                        }
                    }

                }
                if (validx && validy) {
                    player.Move(new Vector2(x, y));
                } else {
                    Console.WriteLine("No valid move");
                }
            }
            // ATTACK
            else if (input.Contains("attack")) {
                Console.WriteLine("Todo: Attack");
            }
            // TRADE
            else if (input.Contains("trade")) {
                Console.WriteLine("Trade.");
                Inventory[] mine = { new Inventory(1000), new Inventory(10000), new Inventory(1000), new Inventory(10) };
                Inventory[] yours = { new Inventory(1000), new Inventory(10000), new Inventory(1000), new Inventory(10) };

                try {
                    Entity you = ChooseEntity();

                    try {
                        mine[Storable.ID.Food.GetHashCode()].Add(new Food());
                    } catch {
                        throw new InvalidInputException();
                    }

                    Offer offer = new Offer(player, you, mine, yours);
                    if (you.TestOffer(offer)) {
                        offer.Apply();
                        Console.WriteLine("Offer accepted.");
                    } else {
                        Console.WriteLine("Offer denied.");
                    }
                } catch (InvalidInputException ex) {
                    Console.WriteLine("Invalid Input.");
                }
            } 
            // ADD FOOD
            else if (input.Contains("addfood")) {
                player.inventory.Add(new Food());
            }
        }

        public static Entity ChooseEntity() {
            // choose partner
            Console.WriteLine("Who do you want to trade with?");
            int id = 0;
            string partner = Console.ReadLine();
            if (Int32.TryParse(partner, out id) || entities[id] != null) {
                return entities[id];
            } else {
                throw new InvalidInputException(); 
            }
        }

        public static int GetId() {
            return lastId++;
        }

        public static void Kill(Entity e) {
            if (e.GetType() == typeof(Hero)) {
                entities[entities.IndexOf(e)] = new DeadHero((Hero)e);
            } else if (e.GetType() == typeof(Town)) {
                entities[entities.IndexOf(e)] = new DeadTown((Town)e);
            }
            
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }
    }        
}
