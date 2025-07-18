using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MasonteVision.Halcon
{
    public class ToolsController
    {
        private IVisionTool myVisionTool;
        private ArrayList _visionToolList;

        public ArrayList VisionToolList
        {
            get { return _visionToolList; }
            set { _visionToolList = value; }
        }

        public ToolsController()
        {
            VisionToolList = new ArrayList();
        }


        public void AddVisionTool(IVisionTool NewTool)
        {
            myVisionTool = NewTool;
            _visionToolList.Add(myVisionTool);
        }
    }
}
