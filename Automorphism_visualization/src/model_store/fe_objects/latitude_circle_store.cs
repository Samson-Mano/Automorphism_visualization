using _2DHelmholtz_solver.src.model_store.geom_objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automorphism_visualization.src.model_store.fe_objects
{
    public class latitude_circle_store
    {

        private Vector2 centerpt = new Vector2(0.0f, 0.0f);
        private double centerradius = 20.0d;
        private int num_circles = 10;

        public List<meshdata_store> lattidude_circle = new List<meshdata_store>();
        public List<double> lattidude_circle_radius = new List<double>();

        private int pt_count = 30;

        private double unitcircleradius = 1000.0;




        public latitude_circle_store(double centerradius)
        {
            // center radius
            this.centerradius = centerradius;

            // Lattidue circles radius
            double radius_interval = ((unitcircleradius - centerradius) / (num_circles + 1));

            for (int i = 0; i < pt_count; i++)
            {
                double circle_radius = centerradius + ((i + 1) * radius_interval);
                lattidude_circle_radius.Add(circle_radius);

            }




        }


    }
}
