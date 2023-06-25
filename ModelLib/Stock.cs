using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ModelLib
{
    public static class Stock
    {
        public static AsyncObservableCollection<Pallet> Pallets = new AsyncObservableCollection<Pallet>();
        public static IEnumerable<Pallet> PalletsByDate;

        public static void RandInit()
        {
            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                var id = GetPalletId();
                Pallets.Add(new Pallet(id, rand.Next(200, 300), rand.Next(150, 200), 15));
                for (int j = 0; j < rand.Next(1, 20); j++)
                {
                    var box_id = GetBoxId(Pallets[i]);
                    var date = DateOnly.FromDateTime(DateTime.Now).AddDays(rand.Next(-100, 0));
                    Pallets[i].AddBox(new Box(box_id, rand.Next(10, 200), rand.Next(10, 150), rand.Next(10, 200), rand.Next(5, 500) / 10, date));
                }
            }
        }

        public static void AddPallet(double xlen, double ylen, double zlen)
        {
            var id = GetPalletId();

            Pallets.Add(new Pallet(id, xlen, ylen, zlen));
        }

        public static void AddBox(double xlen, double ylen, double zlen, double weight, DateOnly dateOnly, ref Pallet pallet)
        {
            var id = GetBoxId(pallet);

            pallet.AddBox(new Box(id, xlen, zlen, ylen, weight, dateOnly));
        }

        private static int GetPalletId()
        {
            if (Pallets.Count != 0)
            {
                int[] nums = Pallets.Select(x => x.Id).ToArray();
                int[] missingNums = Enumerable.Range(nums[0], nums[nums.Length - 1]).Except(nums).ToArray();
                return missingNums.Length == 0 ? nums.Max() + 1 : missingNums.FirstOrDefault();
            }
            else
            {
                return 0;
            }
        }

        private static int GetBoxId(Pallet p)
        {
            if (p.Count != 0)
            {
                int[] nums = p.Boxes.Select(x => x.Id).ToArray();
                int[] missingNums = Enumerable.Range(nums[0], nums[nums.Length - 1]).Except(nums).ToArray();
                return missingNums.Length == 0 ? nums.Max() + 1 : missingNums.FirstOrDefault();
            }
            else
            {
                return 0;
            }
        }

        public static void SortPallets(string sortParam, bool isReverse)
        {
            switch(sortParam)
            {
                case ("ID"):
                    Pallets = new AsyncObservableCollection<Pallet>(Pallets.OrderBy(i => i.Id));
                    break;
                case ("Ширина, см"):
                    Pallets = new AsyncObservableCollection<Pallet>(Pallets.OrderBy(i => i.XLen));
                    break;
                case ("Длина, см"):
                    Pallets = new AsyncObservableCollection<Pallet>(Pallets.OrderBy(i => i.ZLen));
                    break;
                case ("Коробки"):
                    Pallets = new AsyncObservableCollection<Pallet>(Pallets.OrderBy(i => i.Count));
                    break;
                case ("Вес, кг"):
                    Pallets = new AsyncObservableCollection<Pallet>(Pallets.OrderBy(i => i.Weight));
                    break;
                case ("Срок годности"):
                    Pallets = new AsyncObservableCollection<Pallet>(Pallets.OrderBy(i => i.EndDate));
                    break;
                case ("Объем, см"):
                    Pallets = new AsyncObservableCollection<Pallet>(Pallets.OrderBy(i => i.EndDate));
                    break;
                default:
                    break;
            }
            if (isReverse)
                Pallets = new AsyncObservableCollection<Pallet>(Pallets.Reverse());
        }

        public static void SaveToJson()
        {
            string jsonContent = JsonConvert.SerializeObject(Pallets, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            if (!File.Exists("data.json"))
                File.Create("data.json").Close();

            File.WriteAllText("data.json", jsonContent);
        }

        public static void LoadFromJson()
        {
            if (!File.Exists("data.json"))
                return;

            string jsonString = File.ReadAllText("data.json");

            Pallets = JsonConvert.DeserializeObject<AsyncObservableCollection<Pallet>>(jsonString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }

        public static IEnumerable<DateOnly> GetUniqDates()
        {
            HashSet<DateOnly> uniqDates = new HashSet<DateOnly>();
            foreach (var p in Pallets)
            {
                uniqDates.Add(p.EndDate);
            }
            return uniqDates;
        }

        public static void SelectByDate(DateOnly date)
        {
            PalletsByDate = Pallets.Where(x => x.EndDate == date);
        }
    }
}
