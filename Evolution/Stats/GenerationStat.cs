using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution
{
    /// <summary>
    /// This class is used to keep track of example brains from each generation for display and playback later
    /// </summary>
    public class GenerationStat
    {
        private int[] graphData;

        public int[] GraphData
        {
            get { return graphData; }
            set { graphData = value; }
        }

        public Brain Winner { get; set; }
        public Brain Median { get; set; }


    }
}
