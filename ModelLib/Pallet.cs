using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ModelLib
{
    [Serializable]
    public class Pallet : INotifyPropertyChanged
    {
        public static event Action<Pallet, string>? Log;
        public event PropertyChangedEventHandler? PropertyChanged;

        private int id;
        public int Id 
        { 
            get { return id; } 
            init { id = value; }
        }

        private double x_len;
        public double XLen 
        { 
            get { return x_len; } 
            init { x_len = value; }
        }

        private double y_len;
        public double YLen 
        { 
            get { return y_len; } 
            init { y_len = value; }
        }

        private double z_len;
        public double ZLen 
        { 
            get { return z_len; } 
            init { z_len = value; }
        }

        [JsonIgnore]
        public double Volume { get { return Math.Round(boxes.Sum(x => x.Volume) + XLen*ZLen*YLen, 2); } }

        [JsonIgnore]
        public DateOnly EndDate 
        { 
            get 
            {
                if (boxes.Count == 0)
                    return DateOnly.MaxValue;
                return boxes.Min(x => x.EndDate); 
            } 
        }

        [JsonIgnore]
        public double Weight { get { return boxes.Sum(x => x.Weight) + 30; } }

        [JsonIgnore]
        public int Count { get { return boxes.Count; } }

        private ObservableCollection<Box> boxes;
        public ObservableCollection<Box> Boxes 
        { 
            get { return boxes; } 
            set
            {
                boxes = value;
                OnPropertyChanged(nameof(Boxes));
                OnPropertyChanged(nameof(this.Weight));
                OnPropertyChanged(nameof(this.EndDate));
                OnPropertyChanged(nameof(this.Volume));
                OnPropertyChanged(nameof(this.Count));
            }
        }

        public Box this[int index]
        {
            get { return this.boxes[index]; }
        }

        public Pallet(int id, double x_len, double z_len, double y_len)
        {
            this.id = id;
            this.x_len = x_len;
            this.z_len = z_len;
            this.y_len = y_len;
            this.boxes = new ObservableCollection<Box>();
        }

        public void AddBox(Box box)
        {
            if (box.XLen <= this.x_len && box.ZLen <= this.z_len)
            {
                boxes.Add(box);
                OnPropertyChanged(nameof(this.Weight));
                OnPropertyChanged(nameof(this.EndDate));
                OnPropertyChanged(nameof(this.Volume));
                OnPropertyChanged(nameof(this.Count));
            }
            else
            {
                Log?.Invoke(this, $"Ширина или длина коробки больше палета!");
            }
        }

        public void RemoveBox(Box box)
        {
            boxes.Remove(box);
            OnPropertyChanged(nameof(this.Weight));
            OnPropertyChanged(nameof(this.EndDate));
            OnPropertyChanged(nameof(this.Volume));
            OnPropertyChanged(nameof(this.Count));
        }

        public void SortBoxes(string sortParam, bool isReverse)
        {
            switch (sortParam)
            {
                case ("ID"):
                    Boxes = new ObservableCollection<Box>(Boxes.OrderBy(x => x.Id));
                    break;
                case ("Вес, кг"):
                    Boxes = new ObservableCollection<Box>(Boxes.OrderBy(x => x.Weight));
                    break;
                case ("Срок годности"):
                    Boxes = new ObservableCollection<Box>(Boxes.OrderBy(x => x.EndDate));
                    break;
                case ("Объем, см"):
                    Boxes = new ObservableCollection<Box>(Boxes.OrderBy(x => x.Volume));
                    break;
                default:
                    break;
            }
            if (isReverse)
                Boxes = new ObservableCollection<Box>(Boxes.Reverse());
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
