using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsDiary
{
    class AdditionalResourcesHelper
    {
        private List<string> fullListOfAdditionalExercises = new List<string> { "Pilka Nozna", "Koszykowka", "Siatkowka", "Muzyka", "Spiew", "Aktorstwo", "Szachy", "Malarstwo", "Grafika", "Programowanie" }.OrderBy(x => x).ToList();

        private List<string> studentGroupList = new List<string> { "1A", "1B", "1C", "1D", "2A", "2B", "2C", "2D", "3A", "3B", "3C", "3D", "4A", "4B", "4C", "4D", };

        public List<string> FullListOfAdditionalExercises
        {
            get
            {
                return fullListOfAdditionalExercises;
            }

        }

        public List<string> StudentGroupList
        {
            get
            {
                return studentGroupList;
            }
        }
    }
}
