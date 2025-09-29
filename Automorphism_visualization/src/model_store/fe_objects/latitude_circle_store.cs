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
    public class latitude_circle_store
    {

        private Vector2 centerpt = new Vector2(0.0f, 0.0f);
        private double centerradius = 20.0d;
        private int num_circles = 10;

        public List<meshdata_store> latitude_circles = new List<meshdata_store>();
        public List<double> latitude_circle_radius = new List<double>();

        private int pt_count = 30;

        private double unitcircleradius = 1000.0;




        public latitude_circle_store(double centerradius)
        {
            // center radius
            this.centerradius = centerradius;

            // Lattidue circles radius
            double radius_interval = ((unitcircleradius - centerradius) / (double)(num_circles + 1));

            for (int i = 0; i < num_circles; i++)
            {
                // Add the circle radius
                double temp_latitude_circle_radius = centerradius + ((i + 1) * radius_interval);
                latitude_circle_radius.Add(temp_latitude_circle_radius);


                // initialize the temporary mesh data store to add to the list
                meshdata_store temp_latitude_circle = new meshdata_store();

                //__________________________________________________________________________
                // Create the Latitude circle
                // Add the boundary points for Latidue Circle

                for (int j = 0; j < pt_count; j++)
                {
                    // Create the points for circle
                    double angle = (2.0 * Math.PI * j) / (double)pt_count;
                    double x = centerpt.X + (temp_latitude_circle_radius * Math.Cos(angle));
                    double y = centerpt.Y + (temp_latitude_circle_radius * Math.Sin(angle));

                    temp_latitude_circle.add_mesh_point(j, x, y, 0.0, -1);

                    if (j < pt_count - 1)
                    {
                        temp_latitude_circle.add_mesh_lines(j, j, j + 1, 2);

                    }

                }

                temp_latitude_circle.add_mesh_lines(pt_count - 1, pt_count - 1, 0, 2);
                // Create the shaders and buffers

                temp_latitude_circle.set_shader();
                temp_latitude_circle.set_buffer();

                // Add to the latitude circle list
                latitude_circles.Add(temp_latitude_circle);

            }

        }


        public void paint_latitude_circles()
        {
            // Paint the latitude circles
            gvariables_static.LineWidth = 2.0f;

            for (int i = 0; i < latitude_circles.Count; i++)
            {
                latitude_circles[i].paint_dynamic_mesh_lines();

            }
            

        }


        public void update_openTK_uniforms(bool set_modelmatrix, bool set_viewmatrix, bool set_transparency,
            drawing_events graphic_events_control)
        {

            // Update the buffer of latitude circles

            for (int i = 0; i < latitude_circles.Count; i++)
            {
                latitude_circles[i].update_openTK_uniforms(set_modelmatrix, set_viewmatrix, set_transparency, 
                    graphic_events_control.projectionMatrix,
             graphic_events_control.modelMatrix, graphic_events_control.viewMatrix,
             graphic_events_control.geom_transparency);


            }

        }


        public void update_centerpt(Vector2 new_CenterPt)
        {

            // Lattidue circles radius
            double radius_interval = ((unitcircleradius - centerradius) / (double)(num_circles + 1));

            for (int i = 0; i < num_circles; i++)
            {
                // Add the circle radius
                double temp_latitude_circle_radius = centerradius + ((i + 1) * radius_interval);
                latitude_circle_radius.Add(temp_latitude_circle_radius);


                // initialize the temporary mesh data store to add to the list
                meshdata_store temp_latitude_circle = new meshdata_store();

                //__________________________________________________________________________
                // Create the Latitude circle
                // Add the boundary points for Latidue Circle

                for (int j = 0; j < pt_count; j++)
                {
                    // Create the points for circle
                    double angle = (2.0 * Math.PI * j) / (double)pt_count;
                    double z_x = 0.0f  + (temp_latitude_circle_radius * Math.Cos(angle)) / 1000.0d;
                    double z_y = 0.0f + (temp_latitude_circle_radius * Math.Sin(angle)) / 1000.0d;

                    double c_x = new_CenterPt.X / 1000.0d;
                    double c_y = new_CenterPt.Y / 1000.0d ;


                    // Numerator
                    double a = z_x + c_x;
                    double b = z_y + c_y;

                    // Denominator
                    double u = 1 + (c_x * z_x) + (c_y * z_y);
                    double v = (c_x * z_y) - (c_y * z_x);
                    double denom = (u * u) + (v * v);

                    // Final transformed coordinates
                    double fz_x = ((a * u) + (b * v)) / denom;
                    double fz_y = ((b * u) - (a * v)) / denom;


                    // Update the mesh point
                    latitude_circles[i].update_mesh_point(j, fz_x * 1000.0, fz_y * 1000.0, 0.0, -1);

                }


                // Update the buffers
                // latitude_circles[i].set_buffer();

            }


        }



    }
}
