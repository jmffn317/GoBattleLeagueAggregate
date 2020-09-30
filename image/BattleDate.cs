using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image
{
    class BattleDate
    {
        private DateTime dateTime;

        private string season;

        private string league;

        private string rank;

        private string result;

        private string monster1;

        private string monster2;

        private string monster3;

        public BattleDate(DateTime dateTime, string season, string league, string rank, string result, string monster1, string monster2, string monster3)
        {
            this.DateTime = dateTime;
            this.Season = season;
            this.League = league;
            this.Rank = rank;
            this.Result = result;
            this.Monster1 = monster1;
            this.Monster2 = monster2;
            this.Monster3 = monster3;
        }

        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        public string Season { get => season; set => season = value; }
        public string League { get => league; set => league = value; }
        public string Rank { get => rank; set => rank = value; }
        public string Result { get => result; set => result = value; }
        public string Monster1 { get => monster1; set => monster1 = value; }
        public string Monster2 { get => monster2; set => monster2 = value; }
        public string Monster3 { get => monster3; set => monster3 = value; }
    }
}
