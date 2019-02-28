using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_bots
{
    public class Challenge
    {
        List<Photo> photos;

        class Photo
        {
            public static int count = 0;

            public int ID;

            public bool isHorizontal;

            public List<string> Tags;

            public Photo(string line)
            {
                ID = count;
                count++;
                var data = line.Split(' ');

                isHorizontal = data[0] == "H";
                int TagAmount = Int32.Parse(data[1]);
                for(int i = 0; i < TagAmount; i++)
                {
                    Tags.Add(data[i + 2]);
                }
            }

        }

        public bool PrepareData(List<string> lines)
        {
            if(lines == null)
            {
                return false;
            }

            for (int i = 1; i < lines.Count(); i++)
            {
                photos.Add(new Photo(lines[i]));
            }

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
