using _2DHelmholtz_solver.src.model_store.geom_objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automorphism_visualization.src.model_store.fe_objects
{
    public class verticalgrid_line_store
    {

        private Vector2 centerpt = new Vector2(0.0f, 0.0f);
        private double centerradius = 20.0d;

        public List<meshdata_store> vertical_grid_lines = new List<meshdata_store>();

        private int segment_count = 60;

        const double unitcircleradius = 1000.0;
        const float boundary_size = 4000.0f;








    }
}
