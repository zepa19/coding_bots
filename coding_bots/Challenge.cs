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
                TAGS = p.Tags;
            }

            public Slide(Photo p1, Photo p2)
            {
                isHorizontal = false;
                IDs = $"{p1.ID} {p2.ID}";
                photo1 = p1;
                photo2 = p2;
                TAGS = p1.Tags;
                p2.Tags.ForEach(tag => {
                    if(!TAGS.Contains(tag))
                    {
                        TAGS.Add(tag);
                    }
                });
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
            Photo.count = 0;
            Photo.tagCount = 0;
            Dictionary<string, int> TagsIds = new Dictionary<string, int>();
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

            while (photosH.Count() > 0 || photosV.Count() > 1)
            {
                int bestInterestH = 0;
                int bestIndexH = -1;
                for (int i = 0; i < photosH.Count(); i++)
                {
                    int currentInterest = CalculateInterest(last.TAGS, photosH[i].Tags);
                    if (currentInterest > bestInterestH)
                    {
                        bestInterestH = currentInterest;
                        bestIndexH = i;
                    }
                }

                int bestInterestV1 = 0;
                int bestInterestV2 = 0;
                int bestIndexV1 = -1;
                int bestIndexV2 = -1;
                for (int i = 0; i < photosV.Count(); i++)
                {
                    int currentInterest = CalculateInterest(last.TAGS, photosV[i].Tags);
                    if (currentInterest > bestInterestV1)
                    {
                        bestInterestV1 = currentInterest;
                        bestIndexV1 = i;
                    }
                }

                for (int i = 0; i < photosV.Count(); i++)
                {
                    if (i == bestIndexV1)
                    {
                        continue;
                    }
                    int currentInterest = CalculateInterest(last.TAGS, photosV[i].Tags);
                    if (currentInterest > bestInterestV2)
                    {
                        bestInterestV2 = currentInterest;
                        bestIndexV2 = i;
                    }
                }

                double bestinterestV = (bestInterestV1 + bestInterestV2) / 2.0;
                if (bestInterestH > bestinterestV)
                {
                    Slide bestSlide = new Slide(photosH[bestIndexH]);
                    slides.Add(bestSlide);
                    photosH.RemoveAt(bestIndexH);
                    last = bestSlide;
                }
                else
                {
                    Slide bestSlide = new Slide(photosV[bestIndexV1], photosV[bestIndexV2]);
                    slides.Add(bestSlide);
                    photosV.RemoveAt(bestIndexV1);
                    photosV.RemoveAt(bestIndexV2);
                    last = bestSlide;
                }
            }
        }

        public int CalculateInterest(List<int> tags1, List<int> tags2)
        {
            int dif1 = 0;
            int prod = 0;
            bool found = false;
            foreach (int tag1 in tags1)
            {
                found = false;
                foreach (int tag2 in tags2)
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
            savedData.Add(slides.Count().ToString());
            foreach (Slide element in slides)
                savedData.Add(element.IDs);
            return savedData;
        }

        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
    }
}
