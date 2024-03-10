using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMarketPlace
{
    public class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Rating { get; set; }

        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value;
                    // TBD change amount in the database

                    // Changes the display value in connected View
                    OnPropertyChanged(nameof(Amount));
                }
            }
        }

        private void OnPropertyChanged(string v)
        {
            throw new NotImplementedException();
        }
    }
}
