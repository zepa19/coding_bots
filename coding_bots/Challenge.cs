using System;
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
        List<Slide> slides = new List<Slide>();

        class Slide
        {
            public bool isHorizontal;
            public string IDs;
            public Photo photo1;
            public Photo photo2;
            public List<int> TAGS;

            public Slide()
            { }

            public Slide(Photo p)
            {
                isHorizontal = true;
                IDs = p.ID.ToString();
                photo1 = p;
            }

            public Slide(Photo p1, Photo p2)
            {
                isHorizontal = false;
                IDs = $"{p1.ID} {p2.ID}";
                photo1 = p1;
                photo2 = p2;
            }

            public void SetTags(List<int> tags)
            {
                this.TAGS = tags;
            }
        }

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
                for (int i = 0; i < 1; i++)
                {
                    if (TagsIds.ContainsKey(data[i + 2]))
                    {
                        Tags.Add(TagsIds[data[i + 2]]);
                    }
                    else
                    {
                        TagsIds.Add(data[i + 2], tagCount);
                        Tags.Add(tagCount);
                        tagCount++;
                    }
                }
            }

            public Photo(Photo p)
            {
                ID = p.ID;
                Tags = p.Tags;

            }
        }

        public bool PrepareData(List<string> lines)
        {
            if (lines == null)
            {
                return false;
            }

            for (int i = 1; i < lines.Count() - 1; i++)
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
            Slide last;
            if (photosH.Count() == 0)
            {
                last = new Slide(photosV[0], photosV[1]);
                photosV.RemoveRange(0, 2);
            }
            else
            {
                last = new Slide(photosH[0]);
                photosH.RemoveAt(0);
            }

            slides.Add(last);

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
            int prod = 0;
            bool found = false;
            foreach (string tag1 in tags1)
            {
                found = false;
                foreach (string tag2 in tags2)
                {
                    if (tag1.Equals(tag2))
                    {
                        found = true;
                        prod++;
                        break;
                    }
                }

                if (!found)
                {
                    dif1++;
                }
            }

            int dif2 = tags2.Count() - prod;
            return Math.Min(dif1, Math.Min(dif2, prod));
        }

        public List<string> GetSaveData()
        {
            List<string> savedData = new List<string>();
            int count = 0;
            foreach (Slide element in slides)
                count++;
            savedData.Add(count.ToString());
            foreach (Slide element in slides)
                savedData.Add(element.IDs);
            return savedData;
        }
    }
}
