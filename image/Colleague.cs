using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image
{
    class Colleague
    {
        private string monster1;
        private int amount;

        public Colleague(string monster1, int amount)
        {
            this.monster1 = monster1;
            this.amount = amount;
        }

        public string Monster1 { get => monster1; set => monster1 = value; }
        public int Amount { get => amount; set => amount = value; }
    }
}
