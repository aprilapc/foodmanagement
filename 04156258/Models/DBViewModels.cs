using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _04156258.Models
{
    public class DBViewModels
    {
        public IEnumerable<Member> Member { get; set; }
        public IEnumerable<Restaurant> Restaurant { get; set; }
        public IEnumerable<MealsAndDiscount> MealsAndDiscount { get; set; }
        public IEnumerable<CollectMeals> CollectMeals { get; set; }
        public IEnumerable<Satisfaction> Satisfaction { get; set; }
    }
}