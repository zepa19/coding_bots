using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_bots
{
    public class Challenge
    {
        public bool PrepareData(List<string> lines)
        {
            if(lines == null)
            {
                return false;
            }

            // parsing input data

            return true;
        }

        public void Solve()
        {
            // solving challenge algorithm   
        }

        public List<string> GetSaveData()
        {
            // returning list of lines to output file

            return new List<string>()
            {
                "first line",
                "second line",
                "third line"
            };
        }
    }
}
