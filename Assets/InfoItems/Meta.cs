using Assets.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.InfoItems
{
    class Meta
    {       
        bool target;
        bool previousTarget;
        bool expanded;
        int targetNum;
        protected DataType dataType;
        protected DisplayArea displayArea;

        public Meta(bool target, DataType dataType, DisplayArea displayArea)
        {
            this.target = target;
            this.dataType = dataType;
            this.displayArea = displayArea;
            this.previousTarget = target;
            this.expanded = false;
        }

        public int TargetNum
        {
            get { return this.targetNum; }
            set { this.targetNum = value; }
        }

        public bool Expanded
        {
            get { return this.expanded; }
            set { this.expanded = value; }
        }

        public bool Target 
        { 
            get { return this.target; } 
            set { this.target = value; }
        }

        public DisplayArea DisplayArea
        {
            get { return this.displayArea; }
        }

        public DataType DataType
        {
            get { return this.dataType; }
        }

        public bool PreviousTarget
        {
            get { return this.previousTarget; }
            set { this.previousTarget = value; }
        }

        // Called on the new Meta object
        public void Merge(Meta oldMeta)
        {
            // Maybe we need to do something here
        }
    }
}