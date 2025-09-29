using _2DHelmholtz_solver.global_variables;
using _2DHelmholtz_solver.src.model_store.geom_objects;
using _2DHelmholtz_solver.src.opentk_control.opentk_bgdraw;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automorphism_visualization.src.model_store.fe_objects
{
    public class longitude_line_store
    {

        private Vector2 centerpt = new Vector2(0.0f, 0.0f);
        private double centerradius = 20.0d;
        private int num_lines = 16;

        public List<meshdata_store> longitude_lines = new List<meshdata_store>();

        private int pt_count = 30;

        private double unitcircleradius = 1000.0;




        public longitude_line_store(double centerradius)
        {
            // center radius
            this.centerradius = centerradius;

            // Longitude lines angle interval
            double angle_interval = (2.0 * Math.PI) / (double)num_lines;

            for (int i = 0; i < num_lines; i++)
            {
                // Temporary lines angle
                double temp_logitude_angle = i * angle_interval;


                // initialize the temporary mesh data store to add to the list
                meshdata_store temp_longitude_line = new meshdata_store();

                // Longitude line start and end point
                Vector2 temp_startpt = new Vector2(centerpt.X + (float)(centerradius * Math.Cos(temp_logitude_angle)),
                    centerpt.Y + (float)(centerradius * Math.Sin(temp_logitude_angle)));

                Vector2 temp_endpt = new Vector2(centerpt.X + (float)(unitcircleradius * Math.Cos(temp_logitude_angle)),
                    centerpt.Y + (float)(unitcircleradius * Math.Sin(temp_logitude_angle)));

                //__________________________________________________________________________
                // Create the Longitude lines
                // Add the boundary points for Longitue lines

                for (int j = 0; j < pt_count; j++)
                {
                    // Create the points for lines
                    double param_t = (j / (double)(pt_count - 1));
                    double x = (1.0 - param_t) * temp_startpt.X + (param_t * temp_endpt.X);
                    double y = (1.0 - param_t) * temp_startpt.Y + (param_t * temp_endpt.Y);

                    temp_longitude_line.add_mesh_point(j, x, y, 0.0, -1);

                    if (j < pt_count - 1)
                    {
                        temp_longitude_line.add_mesh_lines(j, j, j + 1, 2);

                    }

                }

                temp_longitude_line.add_mesh_lines(pt_count - 1, pt_count - 1, 0, 2);
                // Create the shaders and buffers

                temp_longitude_line.set_shader();
                temp_longitude_line.set_buffer();

                // Add to the list
                longitude_lines.Add(temp_longitude_line);

            }

        }


        public void paint_longitude_lines()
        {
            // Paint the center circle
            gvariables_static.LineWidth = 2.0f;

            for (int i = 0; i < longitude_lines.Count; i++)
            {
                longitude_lines[i].paint_static_mesh_lines();

            }


        }


        public void update_openTK_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency,
            drawing_events graphic_events_control)
        {

            // Update the buffer of center circle

            for (int i = 0; i < longitude_lines.Count; i++)
            {
                longitude_lines[i].update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency,
                    graphic_events_control.projectionMatrix,
             graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
             graphic_events_control.geom_transparency);


            }

        }


        public void update_centerpt(Vector2 new_CenterPt)
        {




        }

    }
}
