﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coding_bots
{
    public class Challenge
    {
        List<Photo> photosH = new List<Photo>();
        List<Photo> photosV = new List<Photo>();
        static Dictionary<string, int> TagsIds = new Dictionary<string, int>();
       
        class Photo
        {
            public static int count = 0;
            public static int tagCount = 0;

            public int ID;

            public bool isHorizontal;

            public List<int> Tags = new List<int>();

            public Photo(string line)
            {
                ID = count;
                count++;
                var data = line.Split(' ');

                isHorizontal = data[0] == "H";
                int TagAmount = Int32.Parse(data[1]);
                for(int i = 0; i <  1; i++)
                {
                    if(TagsIds.ContainsKey(data[i+2]))
                    {
                        Tags.Add(TagsIds[data[i+2]]);
                    }
                    else
                    {
                        TagsIds.Add(data[i + 2], tagCount);
                        Tags.Add(tagCount);
                        tagCount++;
                    }
                }
            }

            public Photo (Photo p)
            {
                ID = p.ID;
                Tags = p.Tags;

            }
        }

        public bool PrepareData(List<string> lines)
        {
            if(lines == null)
            {
                return false;
            }

            for (int i = 1; i < lines.Count()-1; i++)
            {
                Photo newPhoto = new Photo(lines[i]);
                if (newPhoto.isHorizontal)
                {
                    photosH.Add(newPhoto);
                }
                else
                {
                    photosV.Add(newPhoto);
                }
            }

            return true;
        }

        public void Solve()
        {
            // List<string> lastTags = new Photo(photos[0]);

            //while (photos.Count() != 0)
            //{
            //    int bestIndex = 0;
            //    int bestMatch = 0;
                
            //    for (int i = 1; i < photos.Count(); i++)
            //    {
            //        int currentMatch = 0;
            //        if (bestMatch < currentMatch)
            //        {
            //            bestMatch = currentMatch;
            //            bestIndex = i;
            //        }
            //    }
            //}
        }

        public double CalculateInterest(List<string> tags1, List<string> tags2)
        {
            int dif1 = 0;
            int dif2 = 0;
            int prod = 0;
            bool found = false;
            foreach(string tag1 in tags1)
            {
                foreach (string tag2 in tags2)
                {
                    if(tag1.Equals(tag2))
                    {
                        found = true;
                        dif1++;
                        break;
                    }

                }
                if (found)
                {
                    continue;
                }
            }
            return 0;
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
